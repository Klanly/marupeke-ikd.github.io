using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    float selectDist_ = 2.0f;

    [SerializeField]
    Inventory inventory_;

    [SerializeField]
    FPSCameraMotion fpsMotion_;

    Focus focusPrefab_;
    GameState curGameState_;

    static public GameManager getInstance() {
        return instance_g;
    }

    private void Awake() {
        instance_g = this;
        focusPrefab_ = PrefabUtil.load<Focus>( "Prefabs/Focus" );
        curGameState_ = PrefabUtil.createInstance<GameState>( "Prefabs/ConfidentialFileCreateState", transform );
    }

    // インベントリー取得
    public Inventory getInventory() {
        return inventory_;

    }

    // Use this for initialization
    void Start () {
        state_ = new FieldState( this );
	}
	
	// Update is called once per frame
	void Update () {
        if ( state_ != null )
            state_ = state_.update();
	}

    // フィールド上
    class FieldState : State< GameManager > {
        public FieldState( GameManager parent ) : base( parent ) {
            parent.fpsMotion_.setActive( true );
            standUpHeight_ = parent.fpsMotion_.transform.localPosition.y;
        }
        // コリジョンチェック
        bool checkCollision( Item item ) {
            var ray = Camera.main.ScreenPointToRay( new Vector3( Screen.width / 2, Screen.height / 2, 0.0f ) );
            RaycastHit hit;
            return item.getCollider().Raycast( ray, out hit, parent_.selectDist_ );
        }

        protected override State innerUpdate() {

            // 左クリックで現在選択中のアイテムに対して選択イベント発動
            if ( Input.GetMouseButtonDown( 0 ) == true && selectingItem_ != null && selectingItem_.isSelecting() == false ) {
                selectingItem_.onSelect();
                if ( selectingItem_.isPickUpItem() == true ) {
                    parent_.inventory_.add( selectingItem_ );
                }
            }

            // [Z}でしゃがみ/立ち切り替え
            if ( Input.GetKeyDown( KeyCode.Z ) == true ) {
                var curPos = parent_.fpsMotion_.transform.localPosition;
                if ( bCrouching_ == true ) {
                    // 立つ
                    bCrouching_ = !bCrouching_;
                    var standUpPos = curPos;
                    standUpPos.y = standUpHeight_;
                    GlobalState.time( 0.5f, (sec, t) => {
                        parent_.fpsMotion_.transform.localPosition = Lerps.Vec3.easeOut( curPos, standUpPos, t );
                        return true;
                    } );
                } else {
                    // しゃがむ
                    bCrouching_ = !bCrouching_;
                    var crouchingPos = curPos;
                    crouchingPos.y = standUpHeight_ * 0.5f;
                    GlobalState.time( 0.5f, (sec, t) => {
                        parent_.fpsMotion_.transform.localPosition = Lerps.Vec3.easeOut( curPos, crouchingPos, t );
                        return true;
                    } );
                }
            }

            // 現在選択中のアイテムとのコリジョンをチェック
            // コリジョンが発生していたら他のアイテムへのコリジョンチェックはスキップ
            if ( selectingItem_ != null ) {
                if ( checkCollision( selectingItem_ ) == false ) {
                    // フォーカスが外れた
                    curFocus_.destroy();
                    curFocus_ = null;
                    selectingItem_ = null;
                }
            }

            // レイの先にある最短距離アイテムをピックアップ
            if ( selectingItem_ == null ) {
                var ray = Camera.main.ScreenPointToRay( new Vector3( Screen.width / 2, Screen.height / 2, 0.0f ) );
                RaycastHit hit;
                if ( Physics.Raycast( ray, out hit, parent_.selectDist_ ) == true ) {
                    selectingItem_ = GameObjectUtil.getParentComponent<Item>( hit.collider.gameObject );
                    if ( selectingItem_ != null ) {
                        // フォーカスをアイテムにアタッチ
                        curFocus_ = Instantiate<Focus>( parent_.focusPrefab_ );
                        curFocus_.setSize( selectingItem_.getBound() );
                        curFocus_.transform.SetParent( selectingItem_.transform );
                        curFocus_.active();
                    }
                }
            }

            return this;
        }
        Item selectingItem_ = null;
        Focus curFocus_ = null;
        bool bCrouching_ = false;
        float standUpHeight_ = 0.0f;
    }

    static GameManager instance_g = null;
    State state_;
}
