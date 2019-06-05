using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    FPSCameraMotion fpsCamera_;

    [SerializeField]
    PlayerUpDown upDown_;

    [SerializeField]
    MazeMesh mazeMesh_;

    [SerializeField]
    Torch torch_;

    [SerializeField]
    Torch torch2_;

    [SerializeField]
    float radius_ = 0.1f;

    [SerializeField]
    UnityEngine.UI.Text floorText_;

    [SerializeField]
    bool debugKey_ = false;

    [SerializeField]
    Transform torchRoot_;

    public System.Action GoEndingCallback { set { goEndingCallback_ = value; } }
    System.Action goEndingCallback_;

    public System.Action< Item > ItemGetCallback { set { itemGetCallback_ = value; } }
    System.Action<Item> itemGetCallback_;

    // 落下
    void fall( System.Action finishCallback ) {
        if ( bExit_ == true )
            return;
        // 足元に床が無い場合に落下成立
        var mazeCollider = mazeMesh_.getCollider();
        var ray = new Ray( transform.position, Vector3.down );
        RaycastHit hit;
        if ( mazeCollider.Raycast( ray, out hit, 0.7f ) == false ) {
            // 落下
            Debug.Log( "Faaaal!!" );
            float totalTime = LerpAction.jumpDown( 9.8f, 0.15f, 0.05f, -1.0f, 0.1f, 0.05f, 0.0f, true );
            var curPos = transform.localPosition;
            var def = Vector3.zero;
            GlobalState.time( totalTime, (sec, t) => {
                def.y = LerpAction.jumpDown( 9.8f, 0.15f, 0.05f, -1.0f, 0.1f, 0.05f, sec );
                transform.localPosition = curPos + def;
                return true;
            } ).finish( () => {
                finishCallback();
            } );
        } else {
            finishCallback();
            noUpDown();
        }
    }

    void jump(System.Action finishCallback) {
        if ( bExit_ == true )
            return;

        // 上に天井が無い
        // 鍵を手に入れた後最上階にいた場合に上昇成立
        var mazeCollider = mazeMesh_.getCollider();
        var ray = new Ray( transform.position, Vector3.up );
        bool isExitCondition = ( hasAllKey() == true && isExitCell() == true );
        RaycastHit hit;
        if ( mazeCollider.Raycast( ray, out hit, 0.7f ) == false || isExitCondition == true ) {
            // 上昇
            Debug.Log( "Riseeeee!!" );
            float totalTime = LerpAction.jump( 9.8f, 0.15f, 0.05f, 1.0f, 0.1f, 0.05f, 0.0f, true );
            var curPos = transform.localPosition;
            var def = Vector3.zero;
            GlobalState.time( totalTime, (sec, t) => {
                def.y = LerpAction.jump( 9.8f, 0.15f, 0.05f, 1.0f, 0.1f, 0.05f, sec );
                transform.localPosition = curPos + def;
                return true;
            } ).finish(() => {
                finishCallback();
            } );
            // ゴール？
            if ( isExitCondition == true ) {
                bExit_ = true;
                // エンディング起動
                GlobalState.wait( 1.0f, () => {
                    goEndingCallback_();
                    return false;
                } );
            }
        } else {
            finishCallback();
            noUpDown();
        }
    }

    void attachTorch( Torch torchPrefab, Vector3 point, Vector3 normal ) {
        var torch = PrefabUtil.createInstance<Torch>( torchPrefab, torchRoot_ );
        var p = point;
        torch.transform.localPosition = p;
        // 壁から少し傾けて設定
        if ( Vector3.Dot( normal, Vector3.up ) < 0.1f ) {
            Vector3 dir = normal + new Vector3( 0.0f, -0.3f, 0.0f );
            var q = Quaternion.LookRotation( dir );
            torch.transform.localRotation = q;
        }
        torch.gameObject.SetActive( true );
    }

    void noUpDown() {
        if ( bExit_ == true )
            return;
        // レイの先にある何かをチェック
        var ray = Camera.main.ScreenPointToRay( new Vector3( Screen.width * 0.5f, Screen.height * 0.5f, 0.0f ) );
        RaycastHit hit;
        if ( Physics.Raycast( ray, out hit, 0.8f ) == true ) {
            var mazeMesh = hit.collider.GetComponent<MazeMesh>();
            if ( mazeMesh != null ) {
                // 壁に松明を追加。ただし天井は付けない。
                if ( hit.normal.y > -0.2f ) {
                    attachTorch( torch_, hit.point, hit.normal );
                }
            } else {
                // アイテム？
                if ( hit.collider.GetComponent<Item>() != null ) {
                    var item = hit.collider.GetComponent<Item>();
                    correctItem( item );
                }
                var torch = hit.collider.GetComponent<Torch>();
                if ( torch != null && torch.TypeName == "orange" ) {
                    // 松明を消す
                    Destroy( torch.gameObject );
                }
            }
        }
    }

    void setBlueTorch() {
        if ( bExit_ == true )
            return;
        // レイの先にある何かをチェック
        var ray = Camera.main.ScreenPointToRay( new Vector3( Screen.width * 0.5f, Screen.height * 0.5f, 0.0f ) );
        RaycastHit hit;
        if ( Physics.Raycast( ray, out hit, 0.7f ) == true ) {
            var mazeMesh = hit.collider.GetComponent<MazeMesh>();
            if ( mazeMesh != null ) {
                // 壁に松明を追加。ただし天井は付けない。
                if ( hit.normal.y > -0.2f ) {
                    attachTorch( torch2_, hit.point, hit.normal );
                }
            } else {
                var torch = hit.collider.GetComponent<Torch>();
                if ( torch != null && torch.TypeName == "blue" ) {
                    // 松明を消す
                    Destroy( torch.gameObject );
                }
            }
        }
    }

    // アイテムを収集
    void correctItem( Item item ) {
        if ( item.ItemName == "key" ) {
            // 鍵ゲット
            bKey_ = true;
            Destroy( item.gameObject );
        }
        itemGetCallback_( item );
    }

    // 鍵持ってる？
    bool hasAllKey() {
        return bKey_;
    }

    // 最上階にいる？
    bool isExitCell() {
        var cell = mazeMesh_.getCellFromPosition( transform.position );
        return cell.level_ + 1 == mazeMesh_.getParam().level_;
    }

    private void Awake() {
        GlobalState.start( () => {
            if ( mazeMesh_.getParam() != null && mazeMesh_.getParam().isReady() == true ) {
                return false;
            }
            return true;
        } );
        upDown_.enabled = false;

        state_ = new WakeUp( this );
    }

    // Use this for initialization
    void Start () {
        upDown_.FallCallback = fall;
        upDown_.JumpCallback = jump;
        upDown_.NoUpDownCallback = noUpDown;
    }

    // Update is called once per frame
    void Update () {
        if ( state_ != null )
            state_ = state_.update();

#if UNITY_EDITOR
        if ( debugKey_ == true ) {
            bKey_ = true;
        }
#endif
    }

    class WakeUp : State< Player > {
        public WakeUp( Player parent ) : base( parent ) { }
        protected override State innerInit() {
            parent_.fpsCamera_.setEnable( false );
            parent_.fpsCamera_.showReticle( false );
            parent_.upDown_.enabled = false;
            parent_.floorText_.text = "";

            var p = parent_.transform.localPosition;
            var q = parent_.transform.localRotation;
            var fp = p;
            fp.y = 0.5f;    // 立った時の高さ
            var x = Vector3.Cross( Vector3.up, parent_.transform.forward ).normalized;
            var z = Vector3.Cross( x, Vector3.up );
            var fq = Quaternion.LookRotation( z );
            GlobalState.time( 2.0f, (sec, t) => {
                parent_.transform.localRotation = Lerps.Quaternion.easeInOut( q, fq, t );
                parent_.transform.localPosition = Lerps.Vec3.easeInOut( p, fp, t );
                return true;
            } ).finish(() => {
                setNextState( new Maze( parent_ ) );
            } );
            return this;
        }
    }

    class Maze : State< Player > {
        public Maze( Player parent ) : base( parent ) { }
        protected override State innerInit() {
            parent_.fpsCamera_.setEnable( true );
            Color sc = parent_.floorText_.color;
            sc.a = 0.0f;
            Color ec = parent_.floorText_.color;
            GlobalState.time( 1.5f, (sec, t) => {
                parent_.floorText_.color = Color.Lerp( sc, ec, t );
                return true;
            } );

            parent_.fpsCamera_.enabled = true;
            parent_.fpsCamera_.showReticle( true );
            parent_.fpsCamera_.resetPose( parent_.transform.forward );
            parent_.upDown_.enabled = true;

            return this;
        }

        protected override State innerUpdate() {

            // 迷路とのコリジョンをチェック
            var cell = parent_.mazeMesh_.getCellFromPosition( parent_.transform.position );
            if ( cell == null ) {
                return this;
            }
            float distance = 0.0f;
            Vector3 normal;
            bool isColl = cell.getClosestWall( parent_.transform.position, out distance, out normal );

            // 衝突していたら押し戻す
            if ( isColl == true && distance < parent_.radius_ ) {
                var p = parent_.transform.position;
                p += normal * ( parent_.radius_ - distance );
                parent_.transform.position = p;
            }

            // フロア表記
            var curCell = parent_.mazeMesh_.getCellFromPosition( parent_.transform.position );
            parent_.floorText_.text = string.Format( "Floor {0}({1},{2})", curCell.level_ + 1, curCell.z_ + 1, curCell.x_ + 1 );

            // 右クリック（青トーチ）
            if ( Input.GetMouseButtonDown( 1 ) == true ) {
                parent_.setBlueTorch();
            }

            return this;
        }
    }

    State state_;
    bool bKey_ = false;
    bool bExit_ = false;
}
