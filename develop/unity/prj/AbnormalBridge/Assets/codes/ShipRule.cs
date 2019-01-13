using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 船排出ルール
//
//  船は基本どの時間も往来する
//  夜間の方が若干往来頻度が高い
//  ただし少なくとも20分以上は間が空く

public class ShipRule : MonoBehaviour {

    [Range( 0, 1 )]
    public float[] densities_;

    [SerializeField]
    float numPerHour_;

    [SerializeField]
    float nextInterval_;

    public System.Action EmmitCallback { set { emmitCallback_ = value; } }

    public void setup(GameManager manager)
    {
        manager_ = manager;
    }

    // Use this for initialization
    void Start () {
        int hour = manager_.getSunManager().getHour();
        float r = densities_[ hour ] * numPerHour_;
        nextOutTime_ = RandomEmitter.exponentialNextEncountTime( r, 1.0f ) * 3600.0f;
    }

    // Update is called once per frame
    void Update () {
        int hour = manager_.getSunManager().getHour();
        float sec = manager_.getSunManager().getElapsedSec();
        float elapsed = sec - preSec_;
        preSec_ = sec;

        // 経過時間に達していたら排出
        if ( bValidateEmit_ == true ) {
            nextOutTime_ -= elapsed;
            nextInterval_ = nextOutTime_;

            if ( nextOutTime_ <= 0.0f ) {
                if ( emmitCallback_ != null )
                    emmitCallback_();
                float r = densities_[ hour ] * numPerHour_;
                if ( r > 0.0f )
                    nextOutTime_ = RandomEmitter.exponentialNextEncountTime( r, 1.0f ) * 3600.0f;
                else
                    bValidateEmit_ = false;
            }
            if ( densities_[ hour ] > 0.0f )
                bValidateEmit_ = true;
        }
    }

    GameManager manager_;
    float preSec_ = 0.0f;
    float nextOutTime_ = 0.0f;
    System.Action emmitCallback_;
    bool bValidateEmit_ = true;
}
