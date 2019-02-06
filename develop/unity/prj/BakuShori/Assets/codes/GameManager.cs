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

    // 完全成功演出
    void allDiactiveDone()
    {
        Debug.Log( "All diactive effect on GameManager." );
    }

    // 失敗演出
    void failure()
    {
        Debug.Log( "Failure effect on GameManager." );
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
        enum Result
        {
            None,
            Success,
            Failure,
        }

        public Setup(GameManager parent) : base( parent ) {
        }

        protected override State innerInit()
        {
            // データ生成
            parent_.generator_.create( parent_.dataSet_.spec_, parent_.dataSet_.gimicSpec_, out parent_.dataSet_.bombBox_ );

            // 成功を受ける
            parent_.dataSet_.bombBox_.AllDiactiveDoneCallback = () => {
                result_ = Result.Success;
            };

            // 失敗を受ける
            parent_.dataSet_.bombBox_.FailureCallback = () => {
                result_ = Result.Failure;
            };

            return null;
        }

        protected override State innerUpdate()
        {
            if ( result_ == Result.Success ) {
                return new Success( parent_ );
            } else if ( result_ == Result.Failure ) {
                return new Failure( parent_ );
            }
            return this;
        }

        Result result_ = Result.None;
    }

    class Success : StateBase
    {
        public Success(GameManager parent) : base( parent ) { }
        protected override State innerInit()
        {
            parent_.allDiactiveDone();
            return null;
        }
    }

    class Failure : StateBase
    {
        public Failure(GameManager parent) : base( parent ) { }
        protected override State innerInit()
        {
            parent_.failure();
            return null;
        }
    }

    DataSet dataSet_ = new DataSet();
    State state_;
}
