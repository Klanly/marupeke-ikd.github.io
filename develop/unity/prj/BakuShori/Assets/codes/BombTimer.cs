using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTimer : MonoBehaviour {

    [SerializeField]
    TextMesh text_;

    [SerializeField]
    float sec_ = 180.0f;

    // 残り時間を設定
    public void setRemainTime( int remainSec )
    {
        sec_ = remainSec;
    }

    public void startTimer( int sec )
    {
        setTime( sec );
        bTimer_ = true;
        sec_ = sec;
    }

    public float getSec()
    {
        return sec_;
    }

    // タイムゼロ時に通知
    public void setNotifyZero( System.Action notifyZero )
    {
        notifyZero_ = notifyZero;
    }

    // タイマーを急激に減少
    public void advanceTimer( System.Action notifyZero )
    {
        notifyZero_ = notifyZero;
        speed_ = 35.0f;
    }

    // タイマーをストップ
    public void stopTimer()
    {
        bTimer_ = false;
    }

    void setTime( int sec )
    {
        if ( sec < 0 ) {
            if ( notifyZero_ != null ) {
                notifyZero_();
                notifyZero_ = null;
            }
            return;
        }

        int minute = sec / 60;
        int s = sec % 60;
        text_.text = string.Format( "{0:00}:{1:00}", minute, s );
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if ( bTimer_ == false )
            return;

        sec_ -= Time.deltaTime * speed_;
        setTime( ( int )sec_ );
    }

    bool bTimer_ = true;
    float speed_ = 1.0f;
    System.Action notifyZero_;
}
