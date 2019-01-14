using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerRule : MonoBehaviour {

    [Range( 0, 1 )]
    public float[] densities_;

    [SerializeField]
    float numPerHour_;

    [SerializeField]
    float nextInterval_;

    [SerializeField]
    float intensity_ = 1.0f;

    public System.Action EmmitCallback { set { emmitCallback_ = value; } }

    // 時間内排出量の倍率設定
    public void setNumPerHourIntensity( float intensity )
    {
        intensity_ = intensity;
    }

    public void setup(GameManager manager)
    {
        manager_ = manager;
    }

    void innerStart()
    {
        int hour = manager_.getSunManager().getHour();
        float r = densities_[ hour ] * numPerHour_;
        nextOutTime_ = RandomEmitter.exponentialNextEncountTime( r, 1.0f ) * 3600.0f;
    }

    void innerUpdate()
    {
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
                float r = densities_[ hour ] * numPerHour_ * intensity_;
                if ( r > 0.0f )
                    nextOutTime_ = RandomEmitter.exponentialNextEncountTime( r, 1.0f ) * 3600.0f;
                else
                    bValidateEmit_ = false;
            }
            if ( densities_[ hour ] > 0.0f )
                bValidateEmit_ = true;
        }
    }

    // Use this for initialization
    void Start()
    {
        innerStart();
    }

    // Update is called once per frame
    void Update()
    {
        innerUpdate();
    }

    GameManager manager_;
    float preSec_ = 0.0f;
    float nextOutTime_ = 0.0f;
    System.Action emmitCallback_;
    bool bValidateEmit_ = true;
}
