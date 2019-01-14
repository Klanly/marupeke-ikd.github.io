using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 太陽（天候）管理人
//  内部時間で天候を変化させる

public class SunManager : MonoBehaviour {

    [SerializeField]
    Light light_;

    [SerializeField]
    Light[] spotLights_;

    [SerializeField]
    float secPerDay_ = 120.0f;  // 1日の実秒数

    [SerializeField]
    UnityEngine.UI.Text timeText_;

    public Gradient sunColor_;

    [Range(0,1)]
    public float debugTime_ = 0.0f;

    private void OnValidate()
    {
        light_.color = sunColor_.Evaluate( debugTime_ );
    }

    // Use this for initialization
    void Start () {
        state_ = new State_EarlyMorning( this );
	}
	
	// Update is called once per frame
	void Update () {
        float deltaSec = ( 86400 / secPerDay_ ) * Time.deltaTime;
        curGameSec_ += deltaSec;
        curElapsedSec_ += deltaSec;
        curGameSec_ %= 86400;
        // 現在の時刻に合わせた太陽の位置を設定
        float t = ( curGameSec_ ) / 86400.0f;
        float th = t * 360.0f * Mathf.Deg2Rad;
        float z = -Mathf.Cos( th );
        float r = Mathf.Sin( th );
        float x = -r * Mathf.Sin( 25.0f * Mathf.Deg2Rad );
        float y = r * Mathf.Cos( 25.0f * Mathf.Deg2Rad );

        light_.transform.localRotation = Quaternion.LookRotation( new Vector3(-x, -y, -z ) );
        light_.color = sunColor_.Evaluate( t + 0.25f );    // 6時を起点とする

        timeText_.text = string.Format( "{0:00}:{1:00}", getHour(), getMin() );

        if ( state_ != null )
            state_ = state_.update();
    }

    // 時間を取得
    public int getHour()
    {
        int sec = ( ( int )( curGameSec_ ) + 6 * 60 * 60 ) % 86400;     // 6時を起点とする
        return sec / 3600;
    }

    // 分を取得
    public int getMin()
    {
        int sec = ( ( int )( curGameSec_ ) + 6 * 60 * 60 ) % 86400;     // 6時を起点とする
        return ( sec % 3600 ) / 60;
    }

    // 経過日数を取得
    public int getDay()
    {
        // 6:00が起点なので
        return ( int )( ( curElapsedSec_ + 6.0f * 60.0f * 60.0f ) / 86400.0f );
    }

    // 経過秒を取得
    public float getElapsedSec()
    {
        return curElapsedSec_;
    }

    // スポットライトをスイッチング
    void turnSpotLight( bool isTurnOn )
    {
        foreach ( var s in spotLights_ ) {
            s.gameObject.SetActive( isTurnOn );
        }
    }

    class StateBase : State
    {
        public StateBase( SunManager parent )
        {
            parent_ = parent;
        }
        protected SunManager parent_;
    }

    // 早朝（6:00）
    class State_EarlyMorning : StateBase
    {
        public State_EarlyMorning( SunManager parent ) : base( parent ) {}

        // 内部初期化
        override protected void innerInit()
        {
            // スポットライトをオフ
            parent_.turnSpotLight( false );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            // 夕刻(17:00)になったら遷移
            if ( parent_.getHour() == 17 ) {
                return new State_Evening( parent_ );
            }
            return this;
        }
    }

    // 夕刻 (17:00)
    class State_Evening : StateBase
    {
        public State_Evening( SunManager parent ) : base( parent ) { }
        // 内部初期化
        override protected void innerInit()
        {
            // スポットライトをオン
            parent_.turnSpotLight( true );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            // 朝(7:00)になったら遷移
            if ( parent_.getHour() == 7 ) {
                return new State_EarlyMorning( parent_ );
            }
            return this;
        }
    }

    float curGameSec_ = 0.0f;
    float curElapsedSec_ = 0.0f;
    State state_;
}
