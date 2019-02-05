using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimicBoxCover : MonoBehaviour {

    [SerializeField]
    bool bDebugOpen_ = false;

    [SerializeField]
    Rot rot_ = Rot.MX;

    [SerializeField]
    GameObject[] ansPoses_;

    enum Rot
    {
        PX, PY, PZ, MX, MY, MZ
    }

    public void open()
    {
        if ( bOpened_ == true )
            return;
        bOpened_ = true;

        var initRot = transform.localRotation.eulerAngles;
        float sign = 1.0f;
        if ( rot_ == Rot.MX || rot_ == Rot.MY || rot_ == Rot.MZ )
            sign = -1.0f;
        float x = ( rot_ == Rot.PX || rot_ == Rot.MX ? 1.0f : 0.0f );
        float y = ( rot_ == Rot.PY || rot_ == Rot.MY ? 1.0f : 0.0f );
        float z = ( rot_ == Rot.PZ || rot_ == Rot.MZ ? 1.0f : 0.0f );
        GlobalState.time( 0.5f, (sec, t) => {
            var r = initRot + new Vector3( 1.0f * x, 1.0f * y, 1.0f * z ) * sign * 110.0f * t;
            transform.localRotation = Quaternion.Euler( r );
            return true;
        } );
    }

    // アンサー位置の数を取得
    public int getAnswerNodeNum()
    {
        return ansPoses_.Length;
    }

    // アンサー位置を取得
    public GameObject getAnswerPos( int id )
    {
        if ( id >= ansPoses_.Length )
            return null;
        return ansPoses_[ id ];
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if ( bDebugOpen_ == true ) {
            bDebugOpen_ = false;
            open();
        }
    }

    bool bOpened_ = false;
}
