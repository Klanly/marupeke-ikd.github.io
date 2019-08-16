using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ウィンドウフレーム
//  拡縮時にコーナー、フレームの位置やサイズを変更

public class WindowFrame : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer frame_;

    [SerializeField]
    Transform cornerLTOfs_;

    [SerializeField]
    Transform cornerRBOfs_;

    [SerializeField]
    SpriteRenderer cornerLT_;

    [SerializeField]
    SpriteRenderer cornerRB_;

    [SerializeField]
    Vector2 frameSize_ = Vector2.one;

    [SerializeField]
    Vector2 pivotRate_ = Vector2.zero;

    public void setPivotRate( float rx, float ry, bool keepRectPos ) {
        var prePR = pivotRate_;
        pivotRate_.x = rx;
        pivotRate_.y = ry;
        if ( keepRectPos == true ) {
            var pos = transform.localPosition;
            pos += new Vector3( ( rx - prePR.x ) * frameSize_.x, ( prePR.y - ry ) * frameSize_.y, 0.0f );
            transform.localPosition = pos;
        }
        updateState();
    }

    public void setSize( Vector2 size ) {
        frameSize_ = size;
    }

    void updateState() {
        // pivotに合わせて位置調整
        var LTofs = new Vector3( -pivotRate_.x * frameSize_.x, pivotRate_.y * frameSize_.y, 0.0f );
        var RBofs = new Vector3( LTofs.x + frameSize_.x, LTofs.y - frameSize_.y, 0.0f );
        cornerLTOfs_.localPosition = LTofs;
        cornerRBOfs_.localPosition = RBofs;
        frame_.size = frameSize_;
        frame_.transform.localPosition = new Vector3( LTofs.x + frameSize_.x * 0.5f, LTofs.y - frameSize_.y * 0.5f, 0.0f );
    }

    private void Awake() {
//        cornerLTPos_ = cornerLT_.transform.localPosition;
//        cornerRBPos_ = cornerRB_.transform.localPosition;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateState();
    }

//    Vector3 cornerLTPos_ = Vector3.zero;
//    Vector3 cornerRBPos_ = Vector3.zero;
}
