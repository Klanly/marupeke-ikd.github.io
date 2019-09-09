using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 牌操作と落下管理
public class Controller : MonoBehaviour {

    [SerializeField]
    float fallSpeed_ = 1.0f;    // 1秒あたり落下速度

    [SerializeField]
    float quickFallSpeed_ = 5.0f;   // 急速落下速度

    void checkPlace( System.Action move, System.Action enableCallback ) {
        var gm = GameManager.getInstance();
        var field = gm.Field;
        var pos0 = paiObjects_[ 0 ].transform.localPosition;
        var pos1 = paiObjects_[ 1 ].transform.localPosition;
        move();
        if ( moveUtil.enablePlace( paiObjects_[ 0 ].transform.localPosition, field.Box ) && moveUtil.enablePlace( paiObjects_[ 0 ].transform.localPosition + paiObjects_[ 1 ].transform.localPosition, field.Box ) ) {
            enableCallback();
        } else {
            paiObjects_[ 0 ].transform.localPosition = pos0;
            paiObjects_[ 1 ].transform.localPosition = pos1;
        }
    }

    // 左回り
    void turnLeft() {
        var gm = GameManager.getInstance();
        var field = gm.Field;
        if ( rotIdx_ == 0 ) {
            checkPlace( () => {
                paiObjects_[ 1 ].transform.localPosition = new Vector3( 0.0f, field.UnitHeight, 0.0f );
            }, () => {
                rotIdx_++;
            } );
            return;
        }
        if ( rotIdx_ == 1 ) {
            checkPlace( () => {
                paiObjects_[ 1 ].transform.localPosition = new Vector3( -field.UnitWidth, 0.0f, 0.0f );
            }, () => {
                rotIdx_++;
            } );
            return;
        }
        if ( rotIdx_ == 2 ) {
            checkPlace( () => {
                paiObjects_[ 1 ].transform.localPosition = new Vector3( 0.0f, -field.UnitHeight, 0.0f );
            }, () => {
                rotIdx_++;
            } );
            return;
        }
        if ( rotIdx_ == 3 ) {
            checkPlace( () => {
                paiObjects_[ 1 ].transform.localPosition = new Vector3( field.UnitWidth, 0.0f, 0.0f );
            }, () => {
                rotIdx_ = 0;
            } );
            return;
        }
    }

    // 右回り
    void turnRight() {
        var gm = GameManager.getInstance();
        var field = gm.Field;
        if ( rotIdx_ == 0 ) {
            checkPlace( () => {
                paiObjects_[ 1 ].transform.localPosition = new Vector3( 0.0f, -field.UnitHeight, 0.0f );
            }, () => {
                rotIdx_ = 3;
            } );
            return;
        }
        if ( rotIdx_ == 3 ) {
            checkPlace( () => {
                paiObjects_[ 1 ].transform.localPosition = new Vector3( -field.UnitWidth, 0.0f, 0.0f );
            }, () => {
                rotIdx_--;
            } );
            return;
        }
        if ( rotIdx_ == 2 ) {
            checkPlace( () => {
                paiObjects_[ 1 ].transform.localPosition = new Vector3( 0.0f, field.UnitHeight, 0.0f );
            }, () => {
                rotIdx_--;
            } );
            return;
        }
        if ( rotIdx_ == 1 ) {
            checkPlace( () => {
                paiObjects_[ 1 ].transform.localPosition = new Vector3( field.UnitWidth, 0.0f, 0.0f );
            }, () => {
                rotIdx_--;
            } );
            return;
        }
    }

    // 左移動
    void transLeft() {
        var gm = GameManager.getInstance();
        var field = gm.Field;
        checkPlace( () => {
            var pos = paiObjects_[ 0 ].transform.localPosition;
            pos.x -= field.UnitWidth;
            paiObjects_[ 0 ].transform.localPosition = pos;
        }, () => {} );
    }

    // 右移動
    void transRight() {
        var gm = GameManager.getInstance();
        var field = gm.Field;
        checkPlace( () => {
            var pos = paiObjects_[ 0 ].transform.localPosition;
            pos.x += field.UnitWidth;
            paiObjects_[ 0 ].transform.localPosition = pos;
        }, () => { } );
    }

    // 急速落下
    void fall( bool isQuick ) {
        curFallSpeed_ = ( isQuick ? quickFallSpeed_ : fallSpeed_ );
    }

    private void Awake() {
        state_ = new Wait( this );
    }

    void Start() {
        var gm = GameManager.getInstance();
        moveUtil.PaiH = gm.Field.UnitHeight;
        moveUtil.UnitX = gm.Field.UnitWidth;
        moveUtil.UnitY = gm.Field.UnitHeight;
    }

    void Update() {
        if ( state_ != null ) {
            state_ = state_.update();
        }
    }

    State state_;
    PaiObject[] paiObjects_ = new PaiObject[ 2 ];
    int rotIdx_ = 0;
    float curFallSpeed_ = 0.0f;
    PaiMoveUtil moveUtil = new PaiMoveUtil();


    [SerializeField]
    bool bStart_ = false;

    class Wait : State<Controller> {
        public Wait(Controller parent) : base( parent ) {
        }
        protected override State innerInit() {
            GlobalState.start( () => {
                if ( parent_.bStart_ == true ) {
                    setNextState( new SetNewPai( parent_ ) );
                    return false;
                }
                return true;
            } );
            return this;
        }
    }

    class SetNewPai : State<Controller> {
        public SetNewPai(Controller parent) : base( parent ) {
        }
        protected override State innerInit() {
            // 新規の牌を2つ作成しFieldのクライアント位置へ
            // [0]を中心とする。[0,1]の並びして初期位置へ
            var gm = GameManager.getInstance();
            var field = gm.Field;
            var gen = gm.PaiGenerator;
            parent_.paiObjects_[ 0 ] = gen.createRandom();
            parent_.paiObjects_[ 1 ] = gen.createRandom();
            parent_.paiObjects_[ 0 ].transform.SetParent( field.ClientRoot );
            parent_.paiObjects_[ 1 ].transform.SetParent( parent_.paiObjects_[ 0 ].transform );
            parent_.paiObjects_[ 1 ].transform.localPosition = new Vector3( field.UnitWidth, 0.0f, 0.0f );

            var pos = field.getPos( field.XNum / 2 - 1, field.YNum );
            parent_.paiObjects_[ 0 ].transform.localPosition = pos;

            return new Fall( parent_ );
        }
    }

    class Fall : State<Controller> {
        public Fall(Controller parent) : base( parent ) {
            parent_.fall( false );
        }
        protected override State innerUpdate() {
            var gm = GameManager.getInstance();
            var field = gm.Field;

            if ( Input.GetKey( KeyCode.DownArrow ) == true ) {
                // 急速落下
                parent_.fall( true );
            } else {
                parent_.fall( false );
            }
            fallDist_ = parent_.curFallSpeed_;

            // 落下可能？
            var pos = parent_.paiObjects_[ 0 ].transform.localPosition;
            if ( parent_.moveUtil.enableFall( pos, fallDist_ * Time.deltaTime, field.Box ) == true ) {
                pos.y -= fallDist_ * Time.deltaTime;
                parent_.paiObjects_[ 0 ].transform.localPosition = pos;
            }

            if ( Input.GetKeyDown(KeyCode.Z) == true ) {
                // 左回り
                parent_.turnLeft();
            } else if ( Input.GetKeyDown(KeyCode.X) == true ) {
                // 右回り
                parent_.turnRight();
            }
            
            if ( Input.GetKeyDown(KeyCode.LeftArrow) == true ) {
                // 左移動
                parent_.transLeft();
            } else if ( Input.GetKeyDown( KeyCode.RightArrow ) == true ){
                // 右移動
                parent_.transRight();
            }

            return this;
        }
        float fallDist_ = 1.0f;
    }
}
