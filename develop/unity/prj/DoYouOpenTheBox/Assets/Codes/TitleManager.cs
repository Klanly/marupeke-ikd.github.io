using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Button startBtn_;

    [SerializeField]
    Camera camera_;

    [SerializeField]
    UnityEngine.UI.Image fader_;


    public System.Action FinishCallback { set; get; }

    private void Awake() {
        fader_.gameObject.SetActive( true );
        fader_.color = Color.black;
    }

    void Start()
    {
        // カーソル解放
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        startBtn_.gameObject.SetActive( false );
        bool isClick = false;
        var f = camera_.transform.forward;
        var pos = camera_.transform.localPosition;

        GlobalState.time( 2.0f, (sec, t) => {
            fader_.color = new Color( 0.0f, 0.0f, 0.0f, 1.0f - t );
            return true;
        } ).oneFrame(()=> {
            startBtn_.gameObject.SetActive( true );
            fader_.gameObject.SetActive( false );
        } ).next( () => {
            startBtn_.onClick.AddListener( () => {
                isClick = true;
                startBtn_.gameObject.SetActive( false );
                fader_.gameObject.SetActive( true );
                GlobalState.time( 1.0f, (sec, t) => {
                    camera_.transform.localPosition = pos + t * f;
                    return true;
                } );
            } );
            return !isClick;
        } ).nextTime( 2.0f, (sec, t) => {
            fader_.color = new Color( 0.0f, 0.0f, 0.0f, t );
            return true;
        } ).finish( () => {
            FinishCallback();
        } );
    }

    void Update()
    {
        
    }
}
