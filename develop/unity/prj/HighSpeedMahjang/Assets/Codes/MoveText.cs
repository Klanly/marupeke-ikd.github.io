using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveText : MonoBehaviour
{
    [SerializeField]
    TextMesh text_;

    public void setup( Vector3 endPos, Vector3 startOffset, float time, float delaySec ) {
        endPos_ = endPos;
        transform.localPosition = endPos + startOffset;
        startPos_ = endPos + startOffset;
        time_ = time;
        gameObject.SetActive( false );
        GlobalState.wait( delaySec, () => {
            gameObject.SetActive( true );
            return false;
        } );
    }

    public void text( string str ) {
        text_.text = str;
    }

    // Start is called before the first frame update
    void Start()
    {
        t_ = time_;
    }

    // Update is called once per frame
    void Update()
    {
        if ( t_ <= 0.0f ) {
            return;
        }

        t_ -= Time.deltaTime;
        float t = t_ / time_;
        if ( t <= 0.0f ) {
            t = 0.0f;
        }

        transform.localPosition = Lerps.Vec3.easeIn( startPos_, endPos_, 1.0f - t );
    }

    Vector3 startPos_ = Vector3.zero;
    Vector3 endPos_ = Vector3.zero;
    float time_ = 1.0f;
    float t_ = 1.0f;
}
