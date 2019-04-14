using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour {

    [SerializeField]
    string seName_;

    [SerializeField]
    Rigidbody rigidBody_;

    [SerializeField]
    BoxCollider collision_;

    public string getSEName() {
        return seName_;
    }

    public void setSE( string name ) {
        seName_ = name;
    }

    public void playSE() {
        if ( bPlaying_ == true )
            return;
        float sec = SoundAccessor.getInstance().playSE( seName_ );
        bPlaying_ = true;
        GlobalState.wait( sec, () => {
            bPlaying_ = false;
            return false;
        } );
    }

    public void removeAction( System.Action finishCallback ) {
        // コリジョン削除
        Destroy( rigidBody_ );
        Destroy( collision_ );
        removeFinishCallback_ = finishCallback;
        state_ = new RemoveAction( this );
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if ( state_ != null ) {
            state_.update();
        }
	}

    class RemoveAction : State< Speaker > {
        public RemoveAction( Speaker parent ) : base( parent ) {

        }
        protected override State innerInit() {
            // くるくる回って
            GlobalState.time( 1.75f, (sec, t) => {
                var q = parent_.transform.rotation;
                var dq = Quaternion.Euler( 0.0f, ( Time.deltaTime * ( 1.0f + t ) ) * 360.0f * 3.0f, 0.0f );
                parent_.transform.rotation = q * dq;
                return true;
            } ).finish( () => {
                parent_.removeFinishCallback_();
            } );
            // 飛んでく
            GlobalState.wait( 0.3f, () => {
                return false;
            } ).nextTime( 1.5f, (sec, t) => {
                float v = t * 0.1f;
                var pos = parent_.transform.position;
                pos.y += v;
                parent_.transform.position = pos;
                return true;
            } );
            return this;
        }
    }

    bool bPlaying_ = false;
    State state_;
    System.Action removeFinishCallback_;
}
