using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMissMark : MonoBehaviour {

    [SerializeField]
    float minZ_ = -1.2f;

    [SerializeField]
    float maxZ_ = -1.7f;

    [SerializeField]
    float speed_ = 10.0f;

    [SerializeField]
    GameObject[] missMarks_;

    public void show()
    {
        gameObject.SetActive( true );

        if ( state_ != null )
            state_.forceFinish();

        float unit = 8.0f;
        float sz = -3.0f;
        state_ = GlobalState.start( () => {
            sz += Time.deltaTime * unit;
            if ( sz >= minZ_ )
                sz = minZ_;
            foreach ( var m in missMarks_ ) {
                var p = m.transform.localPosition;
                p.z = sz;
                m.transform.localPosition = p;
            }
            if ( sz == minZ_ )
                return false;
            return true;
        } ).next( () => {
            time_ += Time.deltaTime;
            float z = minZ_ + ( maxZ_ - minZ_ ) * ( Mathf.Cos( speed_ * Mathf.PI * 2.0f * time_ ) + 1.0f ) * 0.5f;
            foreach ( var m in missMarks_ ) {
                var p = m.transform.localPosition;
                p.z = z;
                m.transform.localPosition = p;
            }
            return true;
        } );
    }

    public void hide()
    {
        gameObject.SetActive( false );

        if ( state_ != null )
            state_.forceFinish();

        state_ = null;
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    float time_ = 0.0f;
    GlobalStateBase state_ = null;
}
