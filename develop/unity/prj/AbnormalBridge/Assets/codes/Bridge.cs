using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour {

    [SerializeField]
    UnityEngine.UI.Text enduranceText_;

    [SerializeField]
    BridgeType type_;

    [SerializeField]
    float riseUpAcc_ = 1.0f;

    [SerializeField]
    float sinkAcc_ = 1.0f;

    [SerializeField, Range(0, 1)]
    float endurance_ = 0.0f;   // 耐久度(0-1)

    [SerializeField]
    float enduranceUpPerFlame_ = 0.01f;

    [SerializeField]
    float enduranceDownPerFlame_ = 0.02f;

    [SerializeField]
    GameObject overheatSmorkPrefab_;

    [SerializeField]
    bool debugSwitchOn_;

    [SerializeField]
    float bridgeWidth_ = 20.0f;

    [SerializeField]
    float bridgeRoadWidth_ = 5.0f;

    [SerializeField]
    float curYLevel_ = 0.0f;

    [SerializeField]
    int index_;

    public System.Action< int > OnMiss { set { onMiss_ = value; } }

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

    // インデックスを取得
    public int getIndex()
    {
        return index_;
    }

    // Use this for initialization
    void Start () {
        renderer_ = GetComponent<MeshRenderer>();
        material_ = renderer_.material;

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

        if ( bOverheat_ == false ) {
            calcEndurance( curYLevel_ );
            updateColor();
        }
    }

    // 橋の耐久度を計算
    void calcEndurance( float h )
    {
        if ( endurance_ >= 1.0f )
            return;

        if ( h >= -0.01f ) {
            // 橋が完全に上がりきっている状態で耐久度の値が上がる
            endurance_ += enduranceUpPerFlame_;
        } else if ( h < 0.01f && h > -14.8f ) {
            // 橋が下がっている状態はヒートダウン
            endurance_ -= enduranceDownPerFlame_;
        } else {
            // 橋が完全に下がり切ったらクールダウン
            endurance_ = 0.0f;
        }
        endurance_ = Mathf.Clamp01( endurance_ );

        enduranceText_.text = string.Format( "{0}%", ( int )( endurance_ * 100 ) );
    }

    // 橋の耐久度に対応した色変化
    void updateColor()
    {
        // 耐久度が0.5以下の時は変化無し
        if ( endurance_ < 0.5f ) {
            material_.color = normal_;
            renderer_.material = material_;
        } else if ( endurance_ < 1.0f ) {
            // 耐久度が1.0に近付く程点滅周期を上げる
            float minW = 1.0f;
            float maxW = 10.0f;
            float et = ( endurance_ - 0.5f ) * 2.0f;
            enduranceVec_ = Mathf.Lerp( minW, maxW, et ) * 360.0f * Time.deltaTime;
            enduranceTh_ += enduranceVec_;
            enduranceTh_ %= 360;
            float t = ( Mathf.Sin( enduranceTh_ * Mathf.Deg2Rad ) + 1.0f ) * 0.5f;
            dangerColor_.g = dangerColor_.b = ( 1.0f - et );
            Color c = Color.Lerp( normal_, dangerColor_, t );
            material_.color = c;
            renderer_.material = material_;
        } else {
            // 耐久度が1.0はゲームオーバー
            material_.color = abnormal_;
            renderer_.material = material_;

            var overheat = Instantiate<GameObject>( overheatSmorkPrefab_ );
            overheat.transform.parent = transform;
            overheat.transform.localPosition = Vector3.zero;
            overheat.transform.localScale = Vector3.one;
            if ( onMiss_ != null )
                onMiss_( getIndex() );

            bOverheat_ = true;
        }
    }

    BridgeBase bridge_;
    MeshRenderer renderer_;
    float enduranceVec_ = 0.0f;
    float enduranceTh_ = 0.0f;
    Color normal_ = Color.white;
    Color abnormal_ = Color.red;
    Color dangerColor_ = Color.white;
    Material material_;
    bool bOverheat_ = false;
    System.Action<int> onMiss_;
}
