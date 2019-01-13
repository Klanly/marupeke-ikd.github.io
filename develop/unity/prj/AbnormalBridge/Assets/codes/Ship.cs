using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : Passenger {

	// Use this for initialization
	void Start () {

        switch ( Random.Range( 0, 2 ) ) {
            case 0:
                // 上から下
                sp.addPos( new Vector3( -16.0f, 0.0f, 90.0f ) );
                sp.addPos( new Vector3( -16.0f, 0.0f, 59.2f ) );
                sp.addPos( new Vector3( -18.2f, 0.0f, 31.6f ) );
                sp.addPos( new Vector3( -25.1f, 0.0f, 13.6f ) );
                sp.addPos( new Vector3( -23.4f, 0.0f, -7.6f ) );
                sp.addPos( new Vector3( -25.0f, 0.0f, -41.3f ) );
                sp.addPos( new Vector3( -23.7f, 0.0f, -79.0f ) );
                sp.addPos( new Vector3( -23.9f, 0.0f, -104.0f ) );
                bUpper_ = true;
                warningTriggerZ_ = 65.0f;
                break;

            case 1:
                // 下から上
                sp.addPos( new Vector3( -7.9f, 0.0f, -99.7f ) );
                sp.addPos( new Vector3( -6.3f, 0.0f, -76.8f ) );
                sp.addPos( new Vector3( 7.0f, 0.0f, -55.6f ) );
                sp.addPos( new Vector3( 19.4f, 0.0f, -43.6f ) );
                sp.addPos( new Vector3( 23.8f, 0.0f, -18.1f ) );
                sp.addPos( new Vector3( 19.6f, 0.0f, 18.4f ) );
                sp.addPos( new Vector3( 4.0f, 0.0f, 37.8f ) );
                sp.addPos( new Vector3( -4.0f, 0.0f, 56.9f ) );
                sp.addPos( new Vector3( -4.0f, 0.0f, 76.6f ) );
                sp.addPos( new Vector3( -4.0f, 0.0f, 108.6f ) );
                bUpper_ = false;
                warningTriggerZ_ = -68.9f;
                break;
        }
        sp.setup();

        initY_ = transform.localPosition.y;
        float overDist = 0.0f;
        sp.getPos( 0.0f, out prePos_, out overDist );
        prePos_.y = initY_;
        transform.localPosition = prePos_;

        state_ = new State_Move( this );
    }

    // Update is called once per frame
    void Update () {
        if ( state_ != null )
            state_ = state_.update();
	}

    class StateBase : State
    {
        public StateBase(Ship parent)
        {
            parent_ = parent;
        }
        protected Ship parent_;
    }

    // 通常移動
    class State_Move : StateBase
    {
        public State_Move(Ship parent) : base( parent ) { }
        override protected State innerUpdate()
        {
            // パスに沿って移動しよう
            parent_.prePos_ = parent_.transform.localPosition;
            Vector3 pos;
            float overDist = 0.0f;
            if ( parent_.sp.getPos( parent_.speed_ * Time.deltaTime, out pos, out overDist ) == false ) {
                Destroy( this.parent_.gameObject );
            }
            pos.y = parent_.initY_;
            parent_.transform.localPosition = pos;
            parent_.transform.localRotation = Quaternion.LookRotation( pos - parent_.prePos_ );

            // 橋との衝突をチェック
            if ( checkCollision( pos ) == true ) {
                return new State_Collide( parent_ );
            }

            // トリガー位置を通過していたら警告を
            if ( ( parent_.prePos_.z - parent_.warningTriggerZ_ ) * ( pos.z - parent_.warningTriggerZ_ ) < 0.0f ) {
                parent_.manager_.warningShipApproaching( parent_.bUpper_ );
            }
            return this;
        }

        // 橋との衝突をチェック
        bool checkCollision( Vector3 pos )
        {
            // 衝突しないケース
            // ・橋が規定の高さ以上に上がっている
            var bridge = parent_.manager_.getCollideBridge( pos );
            if ( bridge == null )
                return false;
            float bridgeY = bridge.getYLevel();
            return ( bridgeY <= -7.5f );
        }
    }

    // 衝突しちゃった
    class State_Collide : StateBase
    {
        public State_Collide( Ship parent ) : base(parent) { }
        override protected State innerUpdate()
        {
            Destroy( this.parent_.gameObject );
            return null;
        }
    }

        SimplePath sp = new SimplePath();
    float initY_ = 0.0f;
    Vector3 prePos_ = Vector3.zero;
    State state_;
    float warningTriggerZ_ = 0.0f;
    bool bUpper_ = true;
}
