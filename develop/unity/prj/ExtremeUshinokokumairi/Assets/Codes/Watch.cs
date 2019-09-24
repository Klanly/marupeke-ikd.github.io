using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watch : MonoBehaviour {
    [SerializeField]
    int hour_ = 0;

    [SerializeField]
    int minute_ = 0;

    [SerializeField]
    int sec_ = 0;

    [SerializeField]
    int curSecond_ = 0;

    [SerializeField]
    Transform long_;

    [SerializeField]
    Transform short_;

    [SerializeField]
    int times_ = 1;     // 秒の倍率

    [SerializeField]
    bool bActive_ = false;


    // 時刻を設定
    public void setTime(int hour, int minute, int sec) {
        hour_ = hour % 24;
        minute_ = minute % 60;
        sec_ = sec % 60;
        curSecond_ = hour_ * 3600 + minute_ * 60 + sec_;

        float longDeg = 6.0f * ( minute_ + ( curSecond_ % 60 ) /60.0f );
        float shortDeg = 30.0f * ( hour_ % 12 ) + 0.5f * minute_;

        long_.transform.localRotation = Quaternion.Euler( 0.0f, 0.0f, longDeg );
        short_.transform.localRotation = Quaternion.Euler( 0.0f, 0.0f, shortDeg );
    }

    // 時刻を進める
    public void addSec(int sec) {
        curSecond_ += sec;
        curSecond_ = curSecond_ % 86400;
        int h = ( curSecond_ / 3600 );
        int m = ( curSecond_ / 60 ) % 60;
        int s = curSecond_ % 60;
        setTime( h, m, s );
    }

    // 時計を動かす
    public void setActive( bool isActive ) {
        bActive_ = isActive;
    }

    private void Awake() {
        setTime( hour_, minute_, sec_ );
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if ( bActive_ == true ) {
            t_ += Time.deltaTime * times_;
            int sec = ( int )t_;
            if ( sec > 0 ) {
                addSec( sec );
                t_ -= sec;
            }
        }
    }
    float t_ = 0.0f;
}