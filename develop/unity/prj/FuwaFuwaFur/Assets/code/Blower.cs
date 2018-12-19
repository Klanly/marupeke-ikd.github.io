using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ブロワー
//
//  クリック中に画面中央に対して風邪を起こす

public class Blower : MonoBehaviour {

    [SerializeField]
    StageManager stageManager_;

    [SerializeField]
    protected float blowPower_ = 1.0f;

    [SerializeField]
    protected float blowDecRate_ = 0.5f;

    protected class BlowTask
    {
        public BlowTask( Vector3 dir, float initPower, float decRate = 0.2f )
        {
            curPower_ = initPower;
            decRate_ = decRate >= 1.0f ? 0.5f : decRate;
            dir_ = dir.normalized;
        }

        public bool update( out Vector3 dir )
        {
            dir = dir_ * curPower_;
            curPower_ *= decRate_;
            return ( curPower_ <= 0.001f );
        }

        Vector3 dir_;
        float curPower_ = 0.0f;
        float decRate_ = 0.0f;
    }
	// Use this for initialization
	void Start () {
		
	}

    protected void updateTask()
    {
        if ( blowTasks_.Count > 0 ) {
            var blowTasks = new List<BlowTask>();
            foreach ( var t in blowTasks_ ) {
                Vector3 dir;
                if ( t.update( out dir ) == false ) {
                    stageManager_.addBlowDirect( Camera.main.ScreenToViewportPoint( dir ) );
                    blowTasks.Add( t );
                }
            }
            blowTasks_ = blowTasks;
        }
    }

    // Update is called once per frame
    void Update () {
        updateTask();
    }

    protected List<BlowTask> blowTasks_ = new List<BlowTask>();
}
