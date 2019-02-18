using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ピース
public class Piece : MonoBehaviour {

    // ピースが定位置にある？
    public bool isStayPosition()
    {
        var len = ( transform.localPosition - answerPos_ ).magnitude;
        var ang = Quaternion.Angle( transform.localRotation, answerRot_ );
        Debug.Log( "len: " + len + ", ang: " + ang );
        return false;
    }

    // ピースを嵌める
    public void stay()
    {
        transform.localPosition = answerPos_;
        transform.localRotation = answerRot_;
        Destroy( onCollideCallback_.gameObject );   // コライダーを消して無効に
    }

    private void Awake()
    {
        answerPos_ = transform.localPosition;
        answerRot_ = transform.localRotation;
        onCollideCallback_ = transform.GetComponentInChildren<OnCollideCallback>();
        if ( onCollideCallback_ != null ) {
        }
	}
	
	void Update () {
		
	}

    Vector3 answerPos_;
    Quaternion answerRot_;
    OnCollideCallback onCollideCallback_;
}
