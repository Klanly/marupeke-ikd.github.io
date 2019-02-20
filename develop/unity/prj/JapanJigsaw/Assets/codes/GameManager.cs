using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    GameObject pieceRoot_;

    [SerializeField]
    bool bStop_ = false;

    [SerializeField]
    Mode mode_ = Mode.Easy;

    [SerializeField]
    TextMesh prefNameText_;

    [SerializeField]
    Timer timer_;

    [SerializeField]
    UnityEngine.UI.Text remainText_;

    [SerializeField]
    UnityEngine.UI.Text nextPrefText_;

    [SerializeField]
    UnityEngine.UI.Button retireBtn_;

    [SerializeField]
    GameObject titleUIs_;

    [SerializeField]
    GameObject ingameUIs_;

    [SerializeField]
    UnityEngine.UI.Button easyBtn_;

    [SerializeField]
    UnityEngine.UI.Button normalBtn_;

    [SerializeField]
    UnityEngine.UI.Button hardBtn_;

    [SerializeField]
    UnityEngine.UI.Image compImage_;

    [SerializeField]
    UnityEngine.UI.Button toCompBtn_;

    class PrefData
    {
        public PrefData( string name, int isCoast )
        {
            name_ = name;
            isCoast_ = ( isCoast > 0 ? true : false );
        }
        public string name_;
        public bool isCoast_;
    }
    private void Awake()
    {
        prefNames_ = new Dictionary< string, PrefData> {
            {"Aichi Ken", new PrefData( "愛知県", 1 ) },
            {"Akita Ken", new PrefData( "秋田県", 1 ) },
            {"Aomori Ken", new PrefData( "青森県", 1 ) },
            {"Chiba Ken", new PrefData( "千葉県", 1 ) },
            {"Ehime Ken", new PrefData( "愛媛県", 1 ) },
            {"Fukui Ken", new PrefData( "福井県", 1 ) },
            {"Fukuoka Ken", new PrefData( "福岡県", 1 ) },
            {"Fukushima Ken", new PrefData( "福島県", 1 ) },
            {"Gifu Ken", new PrefData( "岐阜県", 0 ) },
            {"Gunma Ken", new PrefData( "群馬県", 0 ) },
            {"Hiroshima Ken", new PrefData( "広島県", 1 ) },
            {"Hokkai Do", new PrefData( "北海道", 1 ) },
            {"Hyogo Ken", new PrefData( "兵庫県", 1 ) },
            {"Ibaraki Ken", new PrefData( "茨城県", 1 ) },
            {"Ishikawa Ken", new PrefData( "石川県", 1 ) },
            {"Iwate Ken", new PrefData( "岩手県", 1 ) },
            {"Kagawa Ken", new PrefData( "香川県", 1 ) },
            {"Kagoshima Ken", new PrefData( "鹿児島県", 1 ) },
            {"Kanagawa Ken", new PrefData( "神奈川県", 1 ) },
            {"Kochi Ken", new PrefData( "高知県", 1 ) },
            {"Kumamoto Ken", new PrefData( "熊本県", 1 ) },
            {"Kyoto Fu", new PrefData( "京都府", 1 ) },
            {"Mie Ken", new PrefData( "三重県", 1 ) },
            {"Miyagi Ken", new PrefData( "宮城県", 1 ) },
            {"Miyazaki Ken", new PrefData( "宮崎県", 1 ) },
            {"Nagano Ken", new PrefData( "長野県", 0 ) },
            {"Nagasaki Ken", new PrefData( "長崎県", 1 ) },
            {"Nara Ken", new PrefData( "奈良県", 0 ) },
            {"Niigata Ken", new PrefData( "新潟県", 1 ) },
            {"Oita Ken", new PrefData( "大分県", 1 ) },
            {"Okayama Ken", new PrefData( "岡山県", 1 ) },
            {"Okinawa Ken", new PrefData( "沖縄県", 1 ) },
            {"Osaka Fu", new PrefData( "大阪府", 1 ) },
            {"Saga Ken", new PrefData( "佐賀県", 1 ) },
            {"Saitama Ken", new PrefData( "埼玉県", 0 ) },
            {"Shiga Ken", new PrefData( "滋賀県", 0 ) },
            {"Shimane Ken", new PrefData( "島根県", 1 ) },
            {"Shizuoka Ken", new PrefData( "静岡県", 1 ) },
            {"Tochigi Ken", new PrefData( "栃木県", 0 ) },
            {"Tokushima Ken", new PrefData( "徳島県", 1 ) },
            {"Tokyo To", new PrefData( "東京都", 1 ) },
            {"Tottori Ken", new PrefData( "鳥取県", 1 ) },
            {"Toyama Ken", new PrefData( "富山県", 1 ) },
            {"Wakayama Ken", new PrefData( "和歌山県", 1 ) },
            {"Yamagata Ken", new PrefData( "山形県", 1 ) },
            {"Yamaguchi Ken", new PrefData( "山口県", 1 ) },
            {"Yamanashi Ken", new PrefData( "山梨県", 0 ) },

        };

        prefNameText_.gameObject.SetActive( false );

        pieces_ = pieceRoot_.gameObject.GetComponentsInChildren<Piece>();
        foreach ( var p in pieces_ ) {
            p.setName( prefNames_[ p.name ].name_ );
        }

        remainPieceNum_ += pieces_.Length;
        nextPrefText_.gameObject.SetActive( false );

        updateRemain();

        // リタイア時振る舞い
        retireBtn_.onClick.AddListener( () => {
            retire();
        } );

        toCompBtn_.onClick.AddListener( () => {
            toComp();
        } );

        retireBtn_.gameObject.SetActive( false );

        ingameUIs_.gameObject.SetActive( false );

        compImage_.gameObject.SetActive( false );
    }

    void Start() {
        state_ = new Title( this );
    }

    void Update() {
        if ( bStop_ == true )
            return;

        if ( state_ != null )
            state_ = state_.update();
        if ( cameraState_ != null )
            cameraState_ = cameraState_.update();
    }

    void updateRemain()
    {
        remainText_.text = string.Format( "{0}/{1}", remainPieceNum_, pieces_.Length );
        if ( mode_ == Mode.Hard ) {
            if ( curSelectPref_ >= selectPrefs_.Count )
                nextPrefText_.text = "Finish !";
            else
                nextPrefText_.text = "Next : " + selectPrefs_[ curSelectPref_ ];
        }
    }

    // リタイア
    void retire()
    {
        retireBtn_.gameObject.SetActive( false );
        foreach( var p in pieces_ ) {
            p.stay( () => {
                p.reset();
            } );
        }
        GlobalState.time( 1.0f, (sec, t) => {
            return true;
        } ).finish(()=> {
            state_ = new Title( this );
        } );
    }

    // 完成へ
    void toComp()
    {
        foreach ( var p in pieces_ ) {
            p.stay();
        }
        state_ = new Complete( this );
    }

    class BaseState : State
    {
        public BaseState(GameManager parent)
        {
            parent_ = parent;
        }
        protected GameManager parent_;
    }

    class Title : BaseState
    {
        public Title( GameManager parent ) : base( parent ) { }
        protected override State innerInit()
        {
            // 難易度ボタン
             parent_.easyBtn_.onClick.AddListener( () => {
                 parent_.mode_ = Mode.Easy;
                 resetAllBtn();
                 setNextState( new Setup( parent_ ) );
            } );
            parent_.normalBtn_.onClick.AddListener( () => {
                parent_.mode_ = Mode.Normal;
                resetAllBtn();
                setNextState( new Setup( parent_ ) );
            } );
            parent_.hardBtn_.onClick.AddListener( () => {
                parent_.mode_ = Mode.Hard;
                resetAllBtn();
                setNextState( new Setup( parent_ ) );
            } );

            parent_.timer_.stop();
            parent_.timer_.clear();
            parent_.ingameUIs_.gameObject.SetActive( false );
            parent_.titleUIs_.gameObject.SetActive( true );
            parent_.compImage_.gameObject.SetActive( false );
            return null;
        }
        private void resetAllBtn()
        {
            parent_.easyBtn_.onClick.RemoveAllListeners();
            parent_.normalBtn_.onClick.RemoveAllListeners();
            parent_.hardBtn_.onClick.RemoveAllListeners();
            parent_.titleUIs_.gameObject.SetActive( false );
        }

        protected override State innerUpdate()
        {
            return this;
        }
    }

    class Setup : BaseState
    {
        public Setup( GameManager parent ) : base( parent )
        {

        }

        protected override State innerInit()
        {
            parent_.remainPieceNum_ = parent_.pieces_.Length;
            parent_.nextPrefText_.gameObject.SetActive( false );
            parent_.retireBtn_.gameObject.SetActive( true );
            parent_.ingameUIs_.gameObject.SetActive( true );
            var text_ = parent_.retireBtn_.GetComponentInChildren<UnityEngine.UI.Text>();
            text_.text = "リタイア";

            foreach ( var p in parent_.pieces_ ) {
                p.reset();
            }

            parent_.cameraState_ = new CameraInitWait( parent_ );

            // ハードモード時の選択ピース列を作成
            if ( parent_.mode_ == Mode.Hard ) {
                parent_.nextPrefText_.gameObject.SetActive( true );
                parent_.nextPrefText_.text = "";

                var coastPrefs = new List<string>();
                var innerPrefs = new List<string>();
                foreach ( var p in parent_.prefNames_ ) {
                    if ( p.Value.isCoast_ == true ) {
                        coastPrefs.Add( p.Value.name_ );
                    } else {
                        innerPrefs.Add( p.Value.name_ );
                    }
                }
                parent_.selectPrefs_.Clear();
                ListUtil.shuffle( ref coastPrefs, Random.Range( 0, 1000 ) );
                ListUtil.shuffle( ref innerPrefs, Random.Range( 0, 1000 ) );
                foreach ( var s in coastPrefs )
                    parent_.selectPrefs_.Add( s );
                foreach ( var s in innerPrefs )
                    parent_.selectPrefs_.Add( s );

                parent_.curSelectPref_ = 0;
            }
            parent_.updateRemain();

            return new Shuffle( parent_ );
        }
    }

    class CameraInitWait : BaseState
    {
        public CameraInitWait(GameManager parent) : base( parent ) { }
        protected override State innerInit()
        {
            parent_.gameStartCallback_ = () => { bStart_ = true; };
            return null;
        }
        protected override State innerUpdate()
        {
            if ( bStart_ == true )
                return new AllowCameraOperation( parent_ );
            return this;
        }
        bool bStart_ = false;
    }

    class AllowCameraOperation : BaseState
    {
        public AllowCameraOperation(GameManager parent) : base( parent ) { }
        protected override State innerUpdate()
        {
            var roll = Input.mouseScrollDelta;

            // [W][S]でカメラのズームイン/アウト
            float speedUnit = 0.8f;
            if ( Input.GetKey( KeyCode.W ) == true || roll.y > 0.0f ) {
                // ズームイン
                Ray mouseRay = Camera.main.ScreenPointToRay( Input.mousePosition );
                Vector3 colPos;
                CollideUtil.colPosRayPlane( out colPos, mouseRay.origin, mouseRay.direction, Vector3.zero, Vector3.up );
                var forward = ( colPos - Camera.main.transform.position ).normalized;
                var pos = Camera.main.transform.position;
                if ( ( forward + pos ).y >= 0.1f + speedUnit ) {
                    Camera.main.transform.position = pos + forward * speedUnit;
                }
            } else if ( Input.GetKey( KeyCode.S ) == true || roll.y < 0.0f ) {
                // ズームアウト
                Ray mouseRay = Camera.main.ScreenPointToRay( Input.mousePosition );
                Vector3 colPos;
                CollideUtil.colPosRayPlane( out colPos, mouseRay.origin, mouseRay.direction, Vector3.zero, Vector3.up );
                var forward = ( colPos - Camera.main.transform.position ).normalized;
                var pos = Camera.main.transform.position;
                if ( ( forward + pos ).y <= 40.0f - speedUnit ) {
                    Camera.main.transform.position = pos - forward * speedUnit;
                }
            }

            // ピックアップ中でなく左ドラッグの場合は
            // カメラを移動
            if ( parent_.bPickingUpPiece_ == false ) {
                if ( Input.GetMouseButtonDown( 0 ) == true ) {
                    clickPos_ = Input.mousePosition;
                }
                if ( Input.GetMouseButton( 0 ) == true ) {
                    var cp = Input.mousePosition;
                    if ( cp != clickPos_ ) {
                        Ray preMouseRay = Camera.main.ScreenPointToRay( clickPos_ );
                        Ray curMouseRay = Camera.main.ScreenPointToRay( cp );
                        Vector3 prePos, curPos;
                        CollideUtil.colPosRayPlane( out prePos, preMouseRay.origin, preMouseRay.direction, Vector3.zero, Vector3.up );
                        CollideUtil.colPosRayPlane( out curPos, curMouseRay.origin, curMouseRay.direction, Vector3.zero, Vector3.up );
                        var cameraMoveDir = prePos - curPos;
                        var cameraPos = Camera.main.transform.position;
                        Camera.main.transform.position = cameraPos + cameraMoveDir;

                        clickPos_ = cp;
                    }
                }
            }
            return this;
        }

        Vector3 clickPos_;
    }

    // ピースをシャッフル
    class Shuffle : BaseState
    {
        public Shuffle(GameManager parent) : base( parent ) { }

        protected override State innerInit()
        {
            // ピースを適当な位置と回転でシャッフル
            foreach ( var p in parent_.pieces_ ) {
                count_++;
                float rot = ( ( int )( 180.0f - Random.value * 360.0f ) / 20.0f ) * 20.0f;
                var curQ = p.transform.localRotation;
                var initPos = p.transform.localPosition;
                var offsetPos = Randoms.Vec3.valueCenterXZ() * 14.0f;
                float xAngle = 180.0f * ( parent_.mode_ == Mode.Normal ? 0.0f : Random.Range( 0, 2 ) );
                GlobalState.time( Random.Range( 1.0f, 1.3f ), (sec, t) => {
                    p.transform.localPosition = Lerps.Vec3.easeOutStrong( initPos, offsetPos, t );
                    if ( parent_.mode_ != Mode.Easy ) {
                        p.transform.localRotation = Quaternion.Euler( xAngle * t, rot * t, 0.0f ) * curQ;
                    }
                    return true;
                } ).finish( () => {
                    count_--;
                } );
            }
            return null;
        }

        protected override State innerUpdate()
        {
            if ( count_ == 0 )
                return new TimerStart( parent_ );
            return this;
        }

        int count_ = 0;
    }

    class TimerStart : BaseState
    {
        public TimerStart(GameManager parent) : base( parent ) { }
        protected override State innerInit()
        {
            if ( parent_.gameStartCallback_ != null )
                parent_.gameStartCallback_();
            parent_.timer_.start();
            return new Idle( parent_ );
        }
    }

    class Idle : BaseState
    {
        public Idle(GameManager parent) : base( parent ) {
            parent_.prefNameText_.gameObject.SetActive( false );
            parent_.bPickingUpPiece_ = false;
        }
        protected override State innerUpdate()
        {
            if ( Input.GetMouseButtonDown( 0 ) == true ) {
                var pickUpPiece = checkPickUp();
                if ( pickUpPiece != null ) {
                    return new PickUpPiece( parent_, pickUpPiece );
                }
            }
            else if ( Input.GetKeyDown( KeyCode.Q ) == true ) {
                var pickUpPiece = checkPickUp();
                if ( pickUpPiece != null ) {
                    return new TurnBackPiece( parent_, pickUpPiece );
                }
            }
            return this;
        }

        Piece checkPickUp()
        {
            // 左クリックでレイを飛ばしピース上だったら摘まむ
            Ray mouseRay = Camera.main.ScreenPointToRay( Input.mousePosition );
            RaycastHit hit;
            // Rayが衝突したらピースをピックアップ
            if ( Physics.Raycast( mouseRay, out hit ) == true ) {
                var onCollide = hit.collider.transform.GetComponent<OnCollideCallback>();
                if ( onCollide != null ) {
                    // 親がpiece
                    return onCollide.transform.parent.gameObject.GetComponent<Piece>();
                }
            } else {
                // 裏面のピースも検索
                Ray invRay = new Ray( mouseRay.origin + 100.0f * mouseRay.direction, -mouseRay.direction );
                // Rayが衝突したらピースをピックアップ
                if ( Physics.Raycast( invRay, out hit ) == true ) {
                    var onCollide = hit.collider.transform.GetComponent<OnCollideCallback>();
                    if ( onCollide != null ) {
                        // 親がpiece
                        return onCollide.transform.parent.gameObject.GetComponent<Piece>();
                    }
                }
            }
            return null;
        }
    }

    // ピースを裏返す
    class TurnBackPiece : BaseState
    {
        public TurnBackPiece(GameManager parent, Piece pickUpPiece) : base( parent )
        {
            pickUpPiece_ = pickUpPiece;
        }
        protected override State innerInit()
        {
            var initQ = pickUpPiece_.transform.localRotation;
            var endQ = Quaternion.Euler( 0.0f, 0.0f, 180.0f ) * initQ;
            float interval = 0.5f;
            float height = 3.0f;
            var initPos = pickUpPiece_.transform.localPosition;
            GlobalState.time( interval, (sec, t) => {
                float y = 4.0f * height * sec / interval * ( 1.0f - sec / interval );
                var offset = new Vector3( 0.0f, y, 0.0f );
                pickUpPiece_.transform.localPosition = initPos + offset;
                pickUpPiece_.transform.localRotation = Lerps.Quaternion.easeOut( initQ, endQ, t );
                return true;
            } ).finish( () => {
                pickUpPiece_.transform.localRotation = endQ;
                setNextState( new Idle( parent_ ) );
            } );
            return this;
        }

        Piece pickUpPiece_;
    }

    class PickUpPiece : BaseState
    {
        public PickUpPiece( GameManager parent, Piece pickUpPiece ) : base( parent ) {
            // ピックアップ中を通知
            parent_.bPickingUpPiece_ = true;
            pickUpPiece_ = pickUpPiece;

            // ピックアップ時のピースの位置からオフセットを算出
            Ray mouseRay = Camera.main.ScreenPointToRay( Input.mousePosition );
            Vector3 colPos;
            CollideUtil.colPosRayPlane( out colPos, mouseRay.origin, mouseRay.direction, Vector3.zero, Vector3.up );
            offset_ = pickUpPiece_.transform.position - colPos;
        }

        protected override State innerInit()
        {
            // 名前プレート表示
            if ( parent_.mode_ != Mode.Hard ) {
                parent_.prefNameText_.gameObject.SetActive( true );
                parent_.prefNameText_.text = pickUpPiece_.getName();
            }
            return null;
        }
        protected override State innerUpdate()
        {
            // 左クリックを離した時にピースが定位置にあればハメる
            // そうでなければピースを離す
            // ハードモードの場合は現在の都道府県と一致している時だけ有効に
            if ( Input.GetMouseButtonUp( 0 ) == true ) {
                bool validatePiece = true;
                if ( parent_.mode_ == Mode.Hard ) {
                    if ( parent_.selectPrefs_[ parent_.curSelectPref_ ] != pickUpPiece_.getName() )
                        validatePiece = false;
                }
                if ( validatePiece == true && pickUpPiece_.isStayPosition() == true ) {
                    pickUpPiece_.stay();

                    // 残りピースが無くなったら完成！
                    parent_.remainPieceNum_--;
                    if ( parent_.mode_ == Mode.Hard )
                        parent_.curSelectPref_++;
                    parent_.updateRemain();
                    if ( parent_.remainPieceNum_ == 0 ) {
                        return new Complete( parent_ );
                    }
                }
                return new Idle( parent_ );
            }
            else {
                // ドラッグ中
                // マウスの位置の先にある平面にピースを移動
                Ray mouseRay = Camera.main.ScreenPointToRay( Input.mousePosition );
                Vector3 colPos;
                CollideUtil.colPosRayPlane( out colPos, mouseRay.origin, mouseRay.direction, Vector3.zero, Vector3.up );
                pickUpPiece_.transform.localPosition = colPos + offset_;
                parent_.prefNameText_.transform.position = colPos + offset_ + Vector3.up * 0.02f;

                // [A][D]キー押し下げで左右回転
                if ( Input.GetKey( KeyCode.A ) == true ) {
                    rotT_ += Time.deltaTime;
                    rotT_ = Mathf.Clamp01( rotT_ );
                    var q = Quaternion.Euler( 0.0f, -Lerps.Float.linear( rotUnitS_, rotUnitE_, rotT_ ), 0.0f );
                    var curQ = pickUpPiece_.transform.localRotation;
                    pickUpPiece_.transform.localRotation = q * curQ;
                }
                else if ( Input.GetKey( KeyCode.D ) == true ) {
                    rotT_ += Time.deltaTime;
                    rotT_ = Mathf.Clamp01( rotT_ );
                    var q = Quaternion.Euler( 0.0f, Lerps.Float.linear( rotUnitS_, rotUnitE_, rotT_ ), 0.0f );
                    var curQ = pickUpPiece_.transform.localRotation;
                    pickUpPiece_.transform.localRotation = q * curQ;
                } else {
                    rotT_ = 0.0f;
                }
            }
            return this;
        }

        Piece pickUpPiece_;
        Vector3 offset_ = Vector3.zero;
        float rotUnitS_ = 0.5f;
        float rotUnitE_ = 5.0f;
        float rotT_ = 0.0f;
    }

    class Complete : BaseState
    {
        public Complete(GameManager parent) : base( parent ) {
        }

        protected override State innerInit()
        {
            parent_.timer_.stop();
            parent_.compImage_.gameObject.SetActive( true );
            parent_.compImage_.color = Color.clear;
            var text_ = parent_.retireBtn_.GetComponentInChildren<UnityEngine.UI.Text>();
            text_.text = "タイトルへ";

            GlobalState.time( 1.2f, (sec, t) => {
                parent_.compImage_.color = Color.Lerp( Color.clear, Color.white, t );
                return true;
            } );
            return null;
        }

        protected override State innerUpdate()
        {
            return this;
        }
    }

    enum Mode
    {
        Easy,
        Normal,
        Hard,
    }

    State state_;
    State cameraState_;
    Piece[] pieces_;
    System.Action gameStartCallback_;
    bool bPickingUpPiece_ = false;
    Dictionary<string, PrefData> prefNames_;
    int remainPieceNum_ = 0;
    List<string> selectPrefs_ = new List<string>();
    int curSelectPref_ = 0;
}
