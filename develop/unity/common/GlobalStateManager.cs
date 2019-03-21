using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// どこでも使えるステート管理人

public class GlobalStateManager : MonoBehaviour {
    private void Awake()
    {
        DontDestroyOnLoad( this );
    }

    private void Update()
    {
        updater_.update();
        stateNum_ = updater_.getStateNum();
    }

    public void setUpdater(GlobalStateUpdater updater)
    {
        updater_ = updater;
    }

    GlobalStateUpdater updater_;
    int stateNum_ = 0;
}


public class GlobalStateUpdater
{
    GlobalStateUpdater()
    {
        var obj = new GameObject( "GlobalStateManager" );
        obj.AddComponent<GlobalStateManager>().setUpdater( this );
    }

    static public GlobalStateUpdater getInstance()
    {
        return instance_;
    }

    // 登録
    public void add(GlobalState state)
    {
        list_.Add( state );
    }

    // 更新
    public void update()
    {
        if ( list_.Count > 0 ) {
            for ( int i = 0; i < list_.Count; ++i ) {
                if ( list_[ i ].update() == false ) {
                    list_.RemoveAt( i );
                    continue;
                }
            }
        }
    }

    // 更新中のステート数を取得
    public int getStateNum()
    {
        return list_.Count;
    }

    static GlobalStateUpdater instance_ = new GlobalStateUpdater();
    List<GlobalState> list_ = new List<GlobalState>();
}

// どこでも使えるステート
public class GlobalStateBase
{
    public GlobalStateBase() { }

    // 強制終了する
    public virtual void forceFinish() { bForceStop_ = true;  }

    protected bool bForceStop_ = false;
}

public class GlobalState : GlobalStateBase
{
    GlobalState( System.Func< bool > action, System.Action post )
    {
        action_ = action;
        onPost_ = post;
    }
    GlobalState( System.Action init, System.Func<bool> action, System.Action post)
    {
        init_ = init;
        action_ = action;
        onPost_ = post;
    }

    // ステート開始
    static public GlobalState start( System.Func< bool > action, System.Action post = null )
    {
        var state = new GlobalState( action, post );
        GlobalStateUpdater.getInstance().add( state );
        return state;
    }
    static public GlobalState start( System.Action init, System.Func<bool> action, System.Action post = null)
    {
        var state = new GlobalState( init, action, post );
        GlobalStateUpdater.getInstance().add( state );
        return state;
    }

    // 間を置く
    static public GlobalState wait( float waitSec, System.Func< bool > action )
    {
        float t = 0.0f;
        var state = new GlobalState(
            () => {
                t += Time.deltaTime;
                if ( t >= waitSec )
                    return false;
                return true;
            },
            () => {
                start( action );
            }
        );
        GlobalStateUpdater.getInstance().add( state );
        return state;
    }

    // 指定時間だけループ
    static public GlobalState time( float sec, System.Func< float, float, bool > action )
    {
        float curSec = 0.0f;
        var state = new GlobalState(
            () => {
                curSec += Time.deltaTime;
                if ( curSec >= sec ) {
                    action( sec, 1.0f );
                    return false;
                }
                return action( curSec, curSec / sec );
            },
            null
        );
        GlobalStateUpdater.getInstance().add( state );
        return state;
    }

    // 指定時間だけループ
    public GlobalState nextTime( float sec, System.Func<float, float, bool> action )
    {
		float curSec = 0.0f;
		nextState_ = new GlobalState(
			() => {
				curSec += Time.deltaTime;
				if ( curSec >= sec ) {
					action( sec, 1.0f );
					return false;
				}
				return action( curSec, curSec / sec );
			},
			null
		);
		nextState_.preState_ = this;
        return nextState_;
    }

	// 待つ
	public GlobalState wait( float sec ) {
		float curSec = 0.0f;
		nextState_ = new GlobalState(
			() => {
				curSec += Time.deltaTime;
				return ( curSec < sec );
			},
			null
		);
		nextState_.preState_ = this;
		return nextState_;
	}

	// 1フレーム
	public GlobalState oneFrame(System.Action action) {
		nextState_ = new GlobalState(
			() => {
				action();
				return false;
			},
			null
		);
		nextState_.preState_ = this;
		return nextState_;
	}

	// 次のステートを登録
	public GlobalState next( System.Func< bool > action, System.Action post = null )
    {
        nextState_ = new GlobalState( action, post );
        nextState_.preState_ = this;
        return nextState_;
    }
    public GlobalState next( System.Action init, System.Func<bool> action, System.Action post = null)
    {
        nextState_ = new GlobalState( init, action, post );
        nextState_.preState_ = this;
        return nextState_;
    }

    // 最終アクション
    public void finish( System.Action onFinish )
    {
        nextState_ = new GlobalState( onFinish, () => { return false; }, () => { } );
    }

    // ステート更新
    public bool update()
    {
        if ( init_ != null ) {
            init_();
            init_ = null;
        }

        // 終わった？
        if ( action_ == null || action_() == false || bForceStop_ == true ) {
            if ( onPost_ != null ) {
                onPost_();
            }

            // 強制終了時は次のステートは実行しない
            if ( bForceStop_ == true )
                return false;

            if ( nextState_ != null ) {
                GlobalStateUpdater.getInstance().add( nextState_ );
            }
            return false;   // このステート自体は終了
        }
        return true;
    }

    // 強制終了する
    public override void forceFinish() {
        base.forceFinish();
        if ( preState_ != null )
            preState_.forceFinish();
    }

    System.Action init_ = null;
    System.Func<bool> action_ = null;
    System.Action onPost_ = null;
    GlobalState nextState_ = null;
    GlobalState preState_ = null;
}