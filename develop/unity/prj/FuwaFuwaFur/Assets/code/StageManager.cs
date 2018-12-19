using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {

    [SerializeField]
    FurModel furModel_;

    [SerializeField]
    Bee beePrefab_;

    [SerializeField]
    Camera mainCamera_;

    [SerializeField]
    Fader fader_;

    [SerializeField]
    float beeDensity_ = 50.0f;     // 蜂密度。1m^2当たりの蜂の数

    [SerializeField]
    GameOverSprite gameOverSprite_;

    [SerializeField]
    Transform root_;

    class UnitRegion
    {
        public GameObject manageGameObj_;
        public UnitRegion()
        {

        }
    }
    public void addBlowDirect( Vector3 direction )
    {
        furModel_.addBlow( direction );
    }

    public bool isFinish()
    {
        return bFinish_;
    }

    class FinishWaitState : State
    {
        public FinishWaitState( StageManager parent, float waitTime, System.Action callback )
        {
            parent_ = parent;
            waitTime_ = waitTime;
            callback_ = callback;
        }
        // 内部状態
        override protected State innerUpdate()
        {
            t += Time.deltaTime;
            if ( t >= waitTime_ ) {
                callback_();
                return null;
            }
            return this;
        }
        StageManager parent_;
        float waitTime_ = 2.0f;
        float t = 0.0f;
        System.Action callback_;
    }

    // Use this for initialization
    void Start () {
        float mesureUnit = 0.001f;  // ワールドの1ユニット1mm

        // 空間管理人
        float lowHeightM = 0.5f;
        float hiHeightM = 100.0f;
        float leftM = -0.2f;
        float rightM = 0.2f;
        spaceManager_ = new Space2DManager( new Vector2( leftM / mesureUnit, lowHeightM / mesureUnit ), new Vector2( rightM / mesureUnit, hiHeightM / mesureUnit ), 1.0f / mesureUnit );

        // 空間ユニットインデックスに対応する範囲を管理するGameObject生成
        spaceRoot_ = new GameObject( "SpaceRoot" );
        spaceRoot_.transform.parent = root_;
        int unitNum = spaceManager_.getUnitNum();
        for ( int i = 0; i < unitNum; ++i ) {
            UnitRegion r = new UnitRegion();
            r.manageGameObj_ = new GameObject( "idx_" + i );
            r.manageGameObj_.transform.parent = spaceRoot_.transform;
            r.manageGameObj_.transform.localPosition = Vector3.zero;
            r.manageGameObj_.SetActive( false );
            unitRegions_.Add( r );
        }

        beeFactory_.beePrefab = beePrefab_;

        // 5m位から100mまで、幅-2mから2mの範囲に適当にハチを散りばめてみる
        int beeNum = ( int )( ( hiHeightM - lowHeightM ) * ( rightM - leftM ) * beeDensity_ );
        for ( int i = 0; i < beeNum; ++i ) {
            var bee = beeFactory_.create( Vector3.zero, 3.0f, 8.0f, 40.0f, 20.0f, 30.0f );
            bee.transform.localPosition = new Vector3( Random.Range( leftM, rightM ) / mesureUnit, Random.Range( lowHeightM, hiHeightM ) / mesureUnit, 0.0f );

            // 蜂の初期位置に該当する空間ユニットに蜂を登録
            int spaceIdx = spaceManager_.calcPointIndex( new Vector2( bee.transform.localPosition.x, bee.transform.localPosition.y ) );
            if ( spaceIdx >= unitRegions_.Count ) {
                spaceIdx = 0;
            }
            bee.transform.parent = unitRegions_[ spaceIdx ].manageGameObj_.transform;
        }

        // Mofに蜂が当たったらGameOver
        furModel_.ColliderCallback.onTriggerEnter_ = ( collision ) => {
            furModel_.toGameOver();
            furModel_.ColliderCallback.onTriggerEnter_ = null;
            bGameOver_ = true;
            furModel_.gameOverFinishCallback_ = () => {
                fader_.setFade( 0.5f );
                gameOverSprite_.gameObject.SetActive( true );
                finishWaitState_ = new FinishWaitState( this, 5.0f, () => {
                    bFinish_ = true;
                } );
            };
        };
	}
	
	// Update is called once per frame
	void Update () {
        if ( bGameOver_ == false ) {
            List<int> actives, noneActies;
            Vector3 furPos = furModel_.transform.localPosition;
            spaceManager_.setVisibleRegion( new Vector2( furPos.x, furPos.y ), 400.0f );
            spaceManager_.update( out actives, out noneActies );
            if ( actives.Count > 0 ) {
                foreach( var i in actives ) {
                    unitRegions_[ i ].manageGameObj_.SetActive( true );
                }
            }
            if ( noneActies.Count > 0 ) {
                foreach ( var i in noneActies ) {
                    unitRegions_[ i ].manageGameObj_.SetActive( false );
                }
            }
        }

        if ( finishWaitState_ != null )
            finishWaitState_ = finishWaitState_.update();
    }

    BeeFactory beeFactory_ = new BeeFactory();
    Space2DManager spaceManager_;
    GameObject spaceRoot_;
    List<UnitRegion> unitRegions_ = new List<UnitRegion>();
    bool bGameOver_ = false;
    bool bFinish_ = false;
    State finishWaitState_ = null;
}
