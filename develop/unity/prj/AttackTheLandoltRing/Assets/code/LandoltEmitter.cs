using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandoltEmitter : MonoBehaviour {

    [SerializeField]
    LandoltEnemy enemy_;

    [SerializeField]
    float speed_ = 1.0f;

    [SerializeField]
    float ramda_ = 20.0f;

    [SerializeField]
    float speedUpRate_ = 0.5f;    // speed / sec.

    [SerializeField]
    float minSpaceDegree_ = 40.0f;

    [SerializeField]
    float maxSpaceDegree_ = 65.0f;

    [SerializeField]
    float minEnemyRadius_ = 10.0f;

    [SerializeField]
    float maxEnemyRadius_ = 20.0f;

    [SerializeField]
    float maxEnemyRadiusAcc_ = 0.01f;

    public System.Action<Transform, float> FieldOverExplodeCallback
    {
        set { fieldOverExplodeCallback_ = value; }
        get { return fieldOverExplodeCallback_; }
    }

    public System.Action<LandoltEnemy> ExplodeCallback
    {
        set { explodeCallback_ = value; }
        get { return explodeCallback_; }
    }


    // 初期化
    // numPerMin: 1分当たりの平均エミット数
    public void setup( float numPerMin, float fieldRadius )
    {
        fieldRadius_ = fieldRadius;
        setNumPerMin( numPerMin );
    }

    // エミット許可
    public void setEnableEmit( bool isEnable )
    {
        bEnable_ = isEnable;
        preEmitSec_ = Time.realtimeSinceStartup;
    }

    // 敵のベーススピードを変更
    public void setEnemySpeed( float speed )
    {
        speed_ = speed;
    }

    // 平均エミット数を変更
    public void setNumPerMin( float numPerMin )
    {
        ramda_ = numPerMin;
    }

    // 敵をエミット
    void emit()
    {
        float curSec = Time.realtimeSinceStartup;
        float interval = curSec - preEmitSec_;
        if ( interval >= nextEmitInterval_ ) {
            // エミット
            var obj = Instantiate<LandoltEnemy>( enemy_ );
            obj.setup(
                minEnemyRadius_ + Random.value * ( maxEnemyRadius_ - minEnemyRadius_ ),
                minSpaceDegree_ + Random.value * ( maxSpaceDegree_ - minSpaceDegree_ ),
                ( int )( Random.value * 24 ) * 15.0f,
                speed_ + speedAcc_, fieldRadius_
               );
            obj.FieldOverExplodeCallback = fieldOverExplodeCallback_;
            obj.ExploadCallback = explodeCallback_;

            // 次のエミットタイム算出
            float r = Random.value * 0.80f;
            nextEmitInterval_ = -Mathf.Log( 1.0f - r ) / ramda_ * 60.0f;   // sec

            preEmitSec_ = curSec;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ( bEnable_ == true )
            emit();

        // スピードを線形に増加
        speedAcc_ += speedUpRate_ * Time.deltaTime;

        // 最大径を線形に増加
        maxEnemyRadius_ += maxEnemyRadiusAcc_;
    }

    bool bEnable_ = false;
    float preEmitSec_ = 0.0f;
    float nextEmitInterval_ = 0.0f;
    float speedAcc_ = 0.0f;
    float fieldRadius_ = 10.0f;
    System.Action<Transform, float> fieldOverExplodeCallback_;
    System.Action<LandoltEnemy> explodeCallback_;
}
