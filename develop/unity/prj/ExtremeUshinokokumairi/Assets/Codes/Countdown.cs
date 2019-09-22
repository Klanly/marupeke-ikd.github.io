using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    [SerializeField]
    TextMesh text_;

    [SerializeField]
    float zoomFactor_ = 2.0f;

    [SerializeField]
    float zoomTime_ = 0.5f;

    public System.Action FinishCallback { set { finishCallback_ = value; } }
    System.Action finishCallback_;

    private void Awake() {
        text_.text = "";
    }

    void animate( string text, System.Action finishCallback ) {
        text_.text = text;
        float curZoom = zoomFactor_;
        GlobalState.time( zoomTime_, (sec, t) => {
            curZoom = Lerps.Float.easeInOut( zoomFactor_, 1.0f, t );
            text_.transform.localScale = new Vector3( curZoom, curZoom, curZoom );
            return true;
        } ).nextTime( 1.0f - zoomTime_, (sec, t) => {
            return true;
        } ).finish( () => {
            finishCallback();
        } );
    }

    // Start is called before the first frame update
    void Start()
    {
        string[] c = new string[] { "参", "弐", "壱", "開打！" };
        animate( c[ 0 ], () => {
            animate( c[ 1 ], () => {
                animate( c[ 2 ], () => {
                    animate( c[ 3 ], () => {
                        if ( finishCallback_ != null ) {
                            finishCallback_();
                        }
                        Destroy( gameObject );
                    } );
                } );
            } );
        } );
    }
}
