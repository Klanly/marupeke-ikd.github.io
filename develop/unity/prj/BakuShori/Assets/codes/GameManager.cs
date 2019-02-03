using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ゲーム管理人
public class GameManager : MonoBehaviour {

    [SerializeField]
    GimicLayoutGenerator generator_ = null;

    [SerializeField]
    HandlerOperator handler_;

	void Start () {
        state_ = new Setup( this );
        handler_.setActive( true );
    }
	
	void Update () {
        if ( state_ != null )
            state_ = state_.update();
	}

    class DataSet
    {
        public BombBox bombBox_;
        public LayoutSpec spec_ = new LayoutSpec();
        public GimicSpec gimicSpec_ = new GimicSpec();
    }

    class StateBase : State
    {
        public StateBase( GameManager parent )
        {
            parent_ = parent;
        }
        protected GameManager parent_;
    }

    class Setup : StateBase
    {
        public Setup(GameManager parent) : base( parent ) {
        }

        protected override State innerInit()
        {
            // データ生成
            parent_.generator_.create( parent_.dataSet_.spec_, parent_.dataSet_.gimicSpec_, out parent_.dataSet_.bombBox_ );
            return null;
        }
    }

    DataSet dataSet_ = new DataSet();
    State state_;
}
