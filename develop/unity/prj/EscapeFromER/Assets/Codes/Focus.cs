using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// フォーカス演出
public class Focus : MonoBehaviour {

    [SerializeField]
    GameObject model1_;

    [SerializeField]
    GameObject model2_;

    // フォーカス開始
    public void active() {
        if ( bActive_ == true || bRequestDestroy_ == true )
            return;

        bActive_ = true;
        model1_.SetActive( true );
        model2_.SetActive( true );
        transform.localPosition = bound_.center;
        model1_.transform.localPosition = Vector3.zero;
        model2_.transform.localPosition = Vector3.zero;
        var zero = Vector3.zero;
        var e1 = -bound_.extents;
        var e2 = bound_.extents;
        GlobalState.time( 0.75f, (sec, t) => {
            if ( this == null )
                return false;
            model1_.transform.localPosition = Lerps.Vec3.easeOut( zero, e1, t );
            model2_.transform.localPosition = Lerps.Vec3.easeOut( zero, e2, t );
            return true;
        } );
    }

    // フォーカス終了
    public void destroy() {
        bRequestDestroy_ = true;
        var zero = Vector3.zero;
        var s1 = -bound_.extents;
        var s2 = bound_.extents;
        GlobalState.time( 0.40f, (sec, t) => {
            model1_.transform.localPosition = Lerps.Vec3.easeOut( s1, zero, t );
            model2_.transform.localPosition = Lerps.Vec3.easeOut( s2, zero, t );
            return true;
        } ).finish( () => {
            model1_.SetActive( false );
            model2_.SetActive( false );
            Destroy( gameObject );
        } );
    }

    // フォーカス対象物の大きさを設定
    public void setSize( Bounds bound ) {
        bound_ = bound;
    }

	// Use this for initialization
	void Awake () {
        model1_.SetActive( false );
        model2_.SetActive( false );
    }

    // Update is called once per frame
    void Update () {
		
	}

    Bounds bound_ = new Bounds( Vector3.zero, Vector3.one );
    bool bRequestDestroy_ = false;
    bool bActive_ = false;
}
