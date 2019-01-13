using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour {

    [SerializeField]
    BridgeType type_;

    [SerializeField]
    float riseUpAcc_ = 1.0f;

    [SerializeField]
    float sinkAcc_ = 1.0f;

    [SerializeField]
    bool debugSwitchOn_;

    [SerializeField]
    float bridgeWidth_ = 20.0f;

    [SerializeField]
    float bridgeRoadWidth_ = 5.0f;

    [SerializeField]
    float curYLevel_ = 0.0f;

    public enum BridgeType
    {
        Bridge_One_Shot,    // スイッチを押すと自動的にライズアップ
        Bridge_Hold,        // スイッチを押している時だけライズアップ
    }

    // 橋の高さを取得
    public float getYLevel()
    {
        return bridge_.getCurPos().y;
    }

    // 橋と衝突している？
    public bool isCollide( Vector3 pos )
    {
        // 橋との衝突は
        // ・橋の道幅内に点が入っている
        // ・橋の幅に点が入っている
        // とします
        var bridgePos = transform.position;
        float hbw = bridgeWidth_ * 0.5f;
        float hbrw = bridgeRoadWidth_ * 0.5f;
        return (
            pos.x >= bridgePos.x - hbw && pos.x <= bridgePos.x + hbw &&
            pos.z >= bridgePos.z - hbrw && pos.z <= bridgePos.z + hbrw
        );
    }

    // スイッチを入れる
    public void switchOn()
    {
        bridge_.switchOn();
    }

    // Use this for initialization
    void Start () {
        if ( type_ == BridgeType.Bridge_One_Shot )
            bridge_ = new OneShotBridge();
        else
            bridge_ = new HoldBridge();

    }

    // Update is called once per frame
    void Update () {
        bridge_.setRiseUpAcc( riseUpAcc_ );
        bridge_.setSinkAcc( sinkAcc_ );

        transform.localPosition = bridge_.update( transform.localPosition );

        if ( debugSwitchOn_ == true ) {
            debugSwitchOn_ = false;
            bridge_.switchOn();
        }

        curYLevel_ = getYLevel();
    }

    BridgeBase bridge_;
}
