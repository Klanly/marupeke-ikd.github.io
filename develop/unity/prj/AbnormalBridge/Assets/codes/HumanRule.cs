using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 人排出ルール
//  朝8～9時に出勤ラッシュで多く人を排出
//  9時以降は落ち着くが12時に少しだけ多くなる。
//  12～17時までは少ない。
//  17～20時まで長く帰宅ラッシュ（最大排出）
//  20時以降はパラパラと。

//  朝方はランニングしている人がちらほら
//  お昼にはランニングする人はいない
//  18時頃からランニングする人が増える。
//  夜中にランニングする人はちらちらといる。

public class HumanRule : MonoBehaviour {

    [Range( 0, 1 )]
    public float[] walkDensitys_;

    [Range( 0, 1 )]
    public float[] runDensitys_;

    [SerializeField]
    float walkerNumPerHour_;

    [SerializeField]
    float runnerNumPerHour_;

    [SerializeField]
    float nextWalkerInterval_;

    [SerializeField]
    float nextRunnerInterval_;

    public System.Action WalkerEmmitCallback { set { walkerEmmitCallback_ = value; } }
    public System.Action RunnerEmmitCallback { set { runnerEmmitCallback_ = value; } }

    public void setup( GameManager manager )
    {
        manager_ = manager;
    }

	// Use this for initialization
	void Start () {
        int hour = manager_.getSunManager().getHour();
        float walkerR = walkDensitys_[ hour ] * walkerNumPerHour_;
        float runnerR = runDensitys_[ hour ] * runnerNumPerHour_;
        nextWalkerOutTime_ = RandomEmitter.exponentialNextEncountTime( walkerR, 1.0f ) * 3600.0f;
        nextRunnerOutTime_ = RandomEmitter.exponentialNextEncountTime( runnerR, 1.0f ) * 3600.0f;
    }

    // Update is called once per frame
    void Update () {
        int hour = manager_.getSunManager().getHour();
        float sec = manager_.getSunManager().getElapsedSec();
        float elapsed = sec - preSec_;
        preSec_ = sec;

        // 経過時間に達していたら排出
        if ( bValidateWalkerEmit_ == true ) {
            nextWalkerOutTime_ -= elapsed;
            nextWalkerInterval_ = nextWalkerOutTime_;
            if ( nextWalkerOutTime_ <= 0.0f ) {
                if ( walkerEmmitCallback_ != null )
                    walkerEmmitCallback_();
                float walkerR = walkDensitys_[ hour ] * walkerNumPerHour_;
                if ( walkerR > 0.0f )
                    nextWalkerOutTime_ = RandomEmitter.exponentialNextEncountTime( walkerR, 1.0f ) * 3600.0f;
                else
                    bValidateWalkerEmit_ = false;
            }
        }
        if ( walkDensitys_[ hour ] > 0.0f )
            bValidateWalkerEmit_ = true;

        if ( bValidateRunnerEmit_ == true ) {
            nextRunnerOutTime_ -= elapsed;
            nextRunnerInterval_ = nextRunnerOutTime_;

            if ( nextRunnerOutTime_ <= 0.0f ) {
                if ( runnerEmmitCallback_ != null )
                    runnerEmmitCallback_();
                float runnerR = runDensitys_[ hour ] * runnerNumPerHour_;
                if ( runnerR > 0.0f )
                    nextRunnerOutTime_ = RandomEmitter.exponentialNextEncountTime( runnerR, 1.0f ) * 3600.0f;
                else
                    bValidateRunnerEmit_ = false;
            }
        }
        if ( runDensitys_[ hour ] > 0.0f )
            bValidateRunnerEmit_ = true;
    }

    GameManager manager_;
    float preSec_ = 0.0f;
    float nextWalkerOutTime_ = 0.0f;
    float nextRunnerOutTime_ = 0.0f;
    System.Action walkerEmmitCallback_;
    System.Action runnerEmmitCallback_;
    bool bValidateWalkerEmit_ = true;
    bool bValidateRunnerEmit_ = true;
}
