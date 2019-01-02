using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// どこでも使えるステート管理人

public class GlobalStateManager : MonoBehaviour {
    private void Update()
    {
        updater_.update();
    }

    public void setUpdater(GlobalStateUpdater updater)
    {
        updater_ = updater;
    }

    GlobalStateUpdater updater_;
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

    static GlobalStateUpdater instance_ = new GlobalStateUpdater();
    List<GlobalState> list_ = new List<GlobalState>();
}

// どこでも使えるステート
public class GlobalState
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
        float t = 0;
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

    // 次のステートを登録
    public GlobalState next( System.Func< bool > action, System.Action post = null )
    {
        nextState_ = new GlobalState( action, post );
        return nextState_;
    }
    public GlobalState next( System.Action init, System.Func<bool> action, System.Action post = null)
    {
        nextState_ = new GlobalState( init, action, post );
        return nextState_;
    }

    // 最終アクション
    public void finish( System.Action onFinish )
    {
        onFinish_ = onFinish;
    }

    // ステート更新
    public bool update()
    {
        if ( init_ != null ) {
            init_();
            init_ = null;
        }

        if ( action_ == null || action_() == false ) {
            if ( onPost_ != null ) {
                onPost_();
            }
            if ( nextState_ != null ) {
                nextState_.onFinish_ = onFinish_;
                GlobalStateUpdater.getInstance().add( nextState_ );
            } else if ( onFinish_ != null ) {
                onFinish_();
            }
            return false;   // このステート自体は終了
        }
        return true;
    }

    System.Action init_ = null;
    System.Func<bool> action_ = null;
    System.Action onPost_ = null;
    System.Action onFinish_ = null;
    GlobalState nextState_ = null;
}