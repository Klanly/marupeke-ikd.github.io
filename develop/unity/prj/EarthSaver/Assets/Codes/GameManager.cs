using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// インゲーム管理人

public class GameManager : MonoBehaviour {

    [SerializeField]
    Transform siteManagerRoot_;

    [SerializeField]
    Transform uiRoot_;

    [SerializeField]
    SiteAccPanel siteAccessPanelPrefab_;

    [SerializeField]
    GameObject gameOverObj_;

    [SerializeField]
    UnityEngine.UI.Button gameOverBtn_;

    [SerializeField]
    UnityEngine.UI.Text missionTimeText_;

    [SerializeField]
    float aveSec_ = 10.0f;

    [SerializeField]
    float maxSec_ = 15.0f;

    [SerializeField]
    float halfRateSec_ = 30.0f;

    [SerializeField]
    float fallObjAvePowerMin_ = 200.0f;

    [SerializeField]
    float fallObjAvePowerMax_ = 600.0f;

    class Parameter {

    }

    private void Awake() {
        siteEmitter_ = new SiteEmitter();
        siteEmitter_.EmitCallback = emitSite;
        siteEmitter_.MaxSec = maxSec_;
        siteEmitter_.AveSec = aveSec_;
        siteEmitter_.AveHalfUnitSec = halfRateSec_;
        siteEmitter_.FallObjPowerMin = fallObjAvePowerMin_;
        siteEmitter_.FallObjPowerMax = fallObjAvePowerMax_;

        missionTimeText_.text = string.Format( "{0:0.00}", 0.0f );
    }

    // Use this for initialization
    void Start () {
        state_ = new GameStart( this );

        // TEST
        var pos = Vector3.zero;
        var q = Quaternion.identity;
        var aabb = new AABB();
        aabb.Min = new Vector3( -1.0f, -1.0f, -1.0f );
        aabb.Max = new Vector3(  1.0f,  1.0f,  1.0f );
        CameraUtil.fitAABB( Camera.main, Vector3.right, Vector3.up, aabb, out pos, out q );
	}
	
	// Update is called once per frame
	void Update () {
        if ( state_ != null )
            state_ = state_.update();
        if ( siteEmitter_ != null )
            siteEmitter_.update();
        if ( bGameOver_ == false ) {
            curMissionTime_ += Time.deltaTime;
            missionTimeText_.text = string.Format( "{0:0.00}", curMissionTime_ );
        }
    }

    // サイトエミット
    void emitSite( SiteManager siteManager ) {
        siteManager.transform.SetParent( siteManagerRoot_ );
        Debug.Log( "Emit!!" );

        // サイトアクセスパネルを追加
        var panel = Instantiate<SiteAccPanel>( siteAccessPanelPrefab_ );
        panel.transform.SetParent( uiRoot_ );
        panel.setCheck( false );
        panel.setup( siteManager );

        // 残り時間をボタン上に表示
        GlobalState.start( () => {
            if ( panel == null )
                return false;
            panel.setBtnText( string.Format( "{0:0.##}", siteManager.LookTime ) );
            if ( siteManager.LookTime <= 0.0f ) {
                return false;
            }
            return true;
        });

        panel.SiteAccBtn.onClick.AddListener( () => {
            // 現在のサイトを不活性に、選択サイトを活性に
            if ( curSelectSite_ != null )
                curSelectSite_.setActive( false );

            // カメラ位置を変更
            // ボタン操作は移動するまで不可に
            setAllSiteAccBtnActive( false );

            Vector3 endPos = Vector3.zero;
            Quaternion endRot = Quaternion.identity;
            siteManager.getCameraPose( out endPos, out endRot );
            Vector3 startPos = Camera.main.transform.position;
            Quaternion startRot = Camera.main.transform.rotation;
            float startRad = startPos.magnitude;
            float endRad = endPos.magnitude;
            GlobalState.time( 0.75f, (sec, t) => {
                var p = SphereSurfUtil.lerp( startPos, endPos, t ) * Lerps.Float.linear( startRad, endRad, t );
                var q = Quaternion.Lerp( startRot, endRot, t );
                Camera.main.transform.position = p;
                Camera.main.transform.rotation = q;
                return true;
            } ).finish(()=> {
                setAllSiteAccBtnActive( true );
                panel.SiteAccBtn.interactable = false;   // 現在のボタンは選択不可に

                // 移動後サイト活性
                siteManager.setActive( true );
                curSelectSite_ = siteManager;
            } );
        } );

        // シールド設定完了したらチェックマークON
        siteManager.CompleteCallback = () => {
            if ( panel != null )
                panel.setCheck( true );
        };

        // TODO:
        //  落下物破壊を確認
        siteManager.BrokenObjectCallback = () => {
            if ( panel == null )
                return;
            // ボタン・サイト削除
            siteAccPanels_.Remove( panel );
            Destroy( panel.gameObject );
            Destroy( siteManager.gameObject, 5.0f );

            // スコア追加
        };

        // 落下物が接地したらGameOver
        siteManager.ObjectContactedCallback = () => {
            siteEmitter_.setActive( false );
            if ( bGameOver_ == false ) {
                toGameOver( siteManager );
            }
        };

        siteAccPanels_.Add( panel );
    }

    void setAllSiteAccBtnActive( bool isActive ) {
        foreach( var p in siteAccPanels_ ) {
            p.SiteAccBtn.enabled = isActive;
            p.SiteAccBtn.interactable = true;   // インタラクティブは可にしておく
        }
    }

    void toGameOver( SiteManager siteManager ) {
        bGameOver_ = true;
        // カメラ位置を変更
        // ボタン操作は不可に
        // 全てのパネルを消去
        setAllSiteAccBtnActive( false );
        foreach ( var panel in siteAccPanels_ ) {
            panel.gameObject.SetActive( false );
        }

        Vector3 endPos = Vector3.zero;
        Quaternion endRot = Quaternion.identity;
        siteManager.getCameraPose( out endPos, out endRot );
        Vector3 startPos = Camera.main.transform.position;
        Quaternion startRot = Camera.main.transform.rotation;
        float startRad = startPos.magnitude;
        float endRad = endPos.magnitude;
        GlobalState.time( 2.0f, (sec, t) => {
            var p = SphereSurfUtil.lerp( startPos, endPos, t ) * Lerps.Float.easeInOut( startRad, endRad, t );
            var q = Quaternion.Lerp( startRot, endRot, t );
            Camera.main.transform.position = p;
            Camera.main.transform.rotation = q;
            return true;
        } ).finish( () => {
            gameOverObj_.SetActive( true );
            gameOverBtn_.gameObject.SetActive( true );
            gameOverBtn_.onClick.AddListener( () => {
                DeltaLerp.clearAllLerps();
                SceneManager.LoadScene( "main" );
                Destroy( gameOverBtn_.gameObject );
            } );
        } );
    }

    class GameStart : State< GameManager > {
        public GameStart(GameManager parent) : base( parent ) { }
        protected override State innerInit() {
            parent_.siteEmitter_.setActive( true );
            return new SiteSelectWait( parent_ );
        }
    }

    //  サイト指定待ち状態
    //  サイトを指定 -> サイトのポジションへ
    class SiteSelectWait : State< GameManager > {
        public SiteSelectWait(GameManager parent) : base( parent ) { }
    }

    // サイトポジションへ移動
    //  サイト先へ到着 -> サイト処理へ
    class MoveToSite : State<GameManager> {
        public MoveToSite(GameManager parent) : base( parent ) { }
    }

    // サイト処理中
    //  別サイト選択 -> サイトポジションへ移動
    class SiteProc : State< GameManager > {
        public SiteProc(GameManager parent) : base( parent ) { }
    }

    State state_;
    SiteEmitter siteEmitter_;   // サイト発生者
    List<SiteAccPanel> siteAccPanels_ = new List<SiteAccPanel>();
    SiteManager curSelectSite_ = null;
    bool bGameOver_ = false;
    float curMissionTime_ = 0.0f;
}
