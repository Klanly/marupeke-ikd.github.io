using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBBreath : MonoBehaviour
{
    [SerializeField]
    float waitSec_ = 5.0f;

    [SerializeField]
    float breathSec_ = 3.0f;

    [SerializeField]
    float breathAngle_ = 1.0f;

    private void Awake() {
        waitState_ = new Wait( this );
        breathState_ = new Breath( this );
        state_ = waitState_;

        tb_ = GetComponent<Treasurebox>();
    }
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if ( tb_.isOpen() == true )
            return;

        if ( state_ != null )
            state_ = state_.update();
    }

    class Wait : State<TBBreath> {
        public Wait(TBBreath parent) : base( parent ) { }
        protected override State innerInit() {
            GlobalState.wait( parent_.waitSec_, () => {
                if ( parent_ == null || parent_.tb_.isOpen() == true )
                    return false;
                return true;
            } ).finish( () => {
                setNextState( parent_.breathState_, true );
            } );
            return this;
        }
    }

    class Breath : State<TBBreath> {
        public Breath(TBBreath parent) : base( parent ) { }
        protected override State innerInit() {
            GlobalState.time( parent_.breathSec_ * 0.5f, (sec, t) => {
                if ( parent_ == null || parent_.tb_.isOpen() == true )
                    return false;
                parent_.tb_.setFlapAngle( Lerps.Float.easeInOut01( t ) * parent_.breathAngle_ );
                return true;
            } ).nextTime( parent_.breathSec_ * 0.5f, (sec, t) => {
                if ( parent_ == null || parent_.tb_.isOpen() == true )
                    return false;
                parent_.tb_.setFlapAngle( ( 1.0f - Lerps.Float.easeInOut01( t ) ) * parent_.breathAngle_ );
                return true;
            } ).finish( () => {
                setNextState( parent_.waitState_, true );
            } );
            return this;
        }
    }

    class Open : State<TBBreath> {
        public Open(TBBreath parent) : base( parent ) { }
        protected override State innerInit() {
            return this;
        }
    }

    State waitState_;
    State breathState_;
    State state_;
    Treasurebox tb_;
}
