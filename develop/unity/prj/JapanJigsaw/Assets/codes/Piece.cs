using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ピース
public class Piece : MonoBehaviour {

    // ピース名を登録
    public void setName( string name )
    {
        name_ = name;
    }

    // ピース名を取得
    public string getName()
    {
        return name_;
    }

    // ピースが定位置にある？
    public bool isStayPosition()
    {
        var len = ( transform.localPosition - answerPos_ ).magnitude;
        var ang = Quaternion.Angle( transform.localRotation, answerRot_ );
        Debug.Log( "len: " + len + ", ang: " + ang );
        if ( len <= 0.1f && ang <= 5.0 )
            return true;
        return false;
    }

    // ピースを嵌める
    public void stay()
    {
        Destroy( onCollideCallback_.gameObject );   // コライダーを消して無効に

        var curPos = transform.localPosition;
        var curQ = transform.localRotation;
        GlobalState.time( 0.15f, (sec, t) => {
            transform.localPosition = Lerps.Vec3.easeIn( curPos, answerPos_, t );
            transform.localRotation = Lerps.Quaternion.easeIn( curQ, answerRot_, t );
            return true;
        } ).finish( () => {
            transform.localPosition = answerPos_;
            transform.localRotation = answerRot_;
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            var mat = renderer.material;
            mat.color = new Color( 164 / 255.0f, 233 / 255.0f, 154 / 255.0f );
            renderer.material = mat;
        } );

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
    string name_;
}
