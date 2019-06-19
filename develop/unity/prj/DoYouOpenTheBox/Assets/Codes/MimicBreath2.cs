using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicBreath2 : MonoBehaviour
{
    [SerializeField]
    float waitSec_ = 5.0f;

    [SerializeField]
    float breathSec_ = 3.0f;

    [SerializeField]
    float breathAngle_ = 1.0f;

    [SerializeField, Range( 0.0f, 1.0f )]
    float tooth_ = 0.75f;

    [SerializeField]
    Transform breathHole_;

    private void Awake() {
        waitState_ = new Wait( this );
        breathState_ = new Breath( this );
        state_ = waitState_;

        mimic_ = GetComponent<Mimic>();
        mimic_.setToothVal( tooth_ );
    }
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if ( mimic_.isOpen() == true )
            return;

        if ( state_ != null )
            state_ = state_.update();
    }

    class Wait : State<MimicBreath2> {
        public Wait(MimicBreath2 parent) : base( parent ) { }
        protected override State innerInit() {
            GlobalState.wait( parent_.waitSec_, () => {
                if ( parent_ == null || parent_.mimic_.isOpen() == true )
                    return false;
                return true;
            } ).finish( () => {
                setNextState( parent_.breathState_, true );
            } );
            return this;
        }
    }

    class Breath : State<MimicBreath2> {
        public Breath(MimicBreath2 parent) : base( parent ) { }
        protected override State innerInit() {
            GlobalState.time( parent_.breathSec_, (sec, t) => {
                if ( parent_ == null || parent_.mimic_.isOpen() == true )
                    return false;

                parent_.breathHole_.localRotation = Quaternion.Euler( 0.0f, 0.0f, Mathf.Sin( t * Mathf.PI * 2.0f ) * parent_.breathAngle_ );
                return true;

            } ).finish( () => {
                setNextState( parent_.waitState_, true );
            } );
            return this;
        }
    }

    class Open : State<MimicBreath2> {
        public Open(MimicBreath2 parent) : base( parent ) { }
        protected override State innerInit() {
            return this;
        }
    }

    State waitState_;
    State breathState_;
    State state_;
    Mimic mimic_;
}
