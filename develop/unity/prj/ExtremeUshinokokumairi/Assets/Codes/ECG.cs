using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 心電図

public class ECG : MonoBehaviour
{
    [SerializeField]
    TrailRenderer[] points_;      // 動点(2点)交互に利用

    [SerializeField]
    float bpm_ = 72.0f;     // 心拍数

    [SerializeField]
    float beatSec_ = 0.35f;  // 鼓動開始から終了までの時間

    [SerializeField]
    float speed_ = 1.0f / 60.0f;    // 1フレームでの移動距離

    [SerializeField]
    float moveDistance_ = 5.0f;     // 最大移動距離

    [SerializeField]
    float power_ = 1.0f;    // 鼓動の強さ

    // 心拍数を設定
    public void setBeat( float beat ) {
        bpm_ = beat;
        if ( bpm_ >= 200 ) {
            bpm_ = 200;
        } else if ( bpm_ < 40 ) {
            bpm_ = 40;
        }
    }

    // 心拍数を追加
    public float addBeat( float beat ) {
        bpm_ += beat;
        if ( bpm_ >= 200 ) {
            bpm_ = 200;
        } else if ( bpm_ < 40 ) {
            bpm_ = 40;
        }
        return bpm_;
    }

    // 鼓動
    float beating() {
        float t = interval_;
        if ( t >= beatSec_ ) {
            t = beatSec_;
        }
        float h = power_ * Mathf.Sin( t / beatSec_ * 2.0f * Mathf.PI );
        if ( t >= beatSec_ ) {
            action_ = wait;
        }
        return h;
    }

    // 安静
    float wait() {
        if ( interval_ >= 60.0f / bpm_ ) {
            interval_ -= 60.0f / bpm_;
            action_ = beating;
            return action_();
        }
        return 0.0f;
    }

    private void Awake() {
        action_ = wait;
        points_[ 1 ].Clear();
        points_[ 1 ].gameObject.SetActive( false );
    }

    void coolTime() {
        if ( coolTimeIdx_ == -1 ) {
            return;
        }
        ct_ += Time.deltaTime;
        if ( ct_ >= 1.5f ) {
            points_[ coolTimeIdx_ ].Clear();
            points_[ coolTimeIdx_ ].gameObject.SetActive( false );
            coolTimeIdx_ = -1;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dist_ += Time.deltaTime * speed_;
        interval_ += Time.deltaTime;
        float h = action_();

        if ( dist_ >= moveDistance_ ) {
            // 最終まで到達したので切り替え
            points_[ curIdx_ ].transform.localPosition = new Vector3( moveDistance_, h, 0.0f );
            coolTimeIdx_ = curIdx_;

            // 交代
            curIdx_ = ( curIdx_ + 1 ) % 2;
            points_[ curIdx_ ].Clear();
            points_[ curIdx_ ].gameObject.SetActive( true );
            dist_ -= moveDistance_;
            points_[ curIdx_ ].transform.localPosition = new Vector3( dist_, h, 0.0f );
        } else {
            points_[ curIdx_ ].transform.localPosition = new Vector3( dist_, h, 0.0f );
        }

        coolTime();
    }

    float dist_;  // 現在の移動距離
    System.Func< float > action_;
    float interval_ = 0.0f;
    int curIdx_ = 0;    // 移動中の動点インデックス
    int coolTimeIdx_ = -1;  // 最大距離まで到達してクールタイム中の動点インデックス
    float ct_ = 0.0f;  // クールタイム経過時間
}
