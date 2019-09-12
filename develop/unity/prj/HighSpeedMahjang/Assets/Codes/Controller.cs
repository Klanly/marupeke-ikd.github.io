using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 牌操作と落下管理
public class Controller : MonoBehaviour {

    [SerializeField]
    float fallSpeed_ = 1.0f;    // 1秒あたり落下速度

    [SerializeField]
    float quickFallSpeed_ = 5.0f;   // 急速落下速度

    public void toStart() {
        bStart_ = true;
    }

    void checkPlace(System.Action move, System.Action enableCallback) {
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
        }, () => { } );
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
    void fall(bool isQuick) {
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
            parent_.rotIdx_ = 0;
        }
        protected override State innerInit() {
            // 新規の牌を2つ作成しFieldのクライアント位置へ
            // [0]を中心とする。[0,1]の並びして初期位置へ
            var gm = GameManager.getInstance();
            var field = gm.Field;
            var nextPaiManager = gm.NextPaiManager;
            var pais = nextPaiManager.pop();
            parent_.paiObjects_[ 0 ] = pais[ 0 ];
            parent_.paiObjects_[ 1 ] = pais[ 1 ];
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
            var pos0 = parent_.paiObjects_[ 0 ].transform.localPosition;
            var pos1 = parent_.paiObjects_[ 0 ].transform.localPosition + parent_.paiObjects_[ 1 ].transform.localPosition;
            if (
                parent_.moveUtil.enableFall( pos0, fallDist_ * Time.deltaTime, field.Box ) == true &&
                parent_.moveUtil.enableFall( pos1, fallDist_ * Time.deltaTime, field.Box ) == true
            ) {
                pos0.y -= fallDist_ * Time.deltaTime;
                parent_.paiObjects_[ 0 ].transform.localPosition = pos0;
            } else {
                // 底を合わせる
                parent_.paiObjects_[ 0 ].transform.localPosition = parent_.moveUtil.calcCellCenter( parent_.paiObjects_[ 0 ].transform.localPosition );
                var p = parent_.moveUtil.calcCellCenter( parent_.paiObjects_[ 0 ].transform.localPosition + parent_.paiObjects_[ 1 ].transform.localPosition );
                parent_.paiObjects_[ 1 ].transform.localPosition = p - parent_.paiObjects_[ 0 ].transform.position;
                return new Fix( parent_ );
            }

            if ( Input.GetKeyDown( KeyCode.Z ) == true ) {
                // 左回り
                parent_.turnLeft();
            } else if ( Input.GetKeyDown( KeyCode.X ) == true ) {
                // 右回り
                parent_.turnRight();
            }

            if ( Input.GetKeyDown( KeyCode.LeftArrow ) == true ) {
                // 左移動
                parent_.transLeft();
            } else if ( Input.GetKeyDown( KeyCode.RightArrow ) == true ) {
                // 右移動
                parent_.transRight();
            }

            return this;
        }
        float fallDist_ = 1.0f;
    }

    class Fix : State<Controller> {
        public Fix(Controller parent) : base( parent ) {
        }
        protected override State innerUpdate() {
            // 指定時間待つ
            t_ -= Time.deltaTime;
            if ( t_ <= 0.0f ) {
                return new FallAfterFix( parent_ );
            }
            return this;
        }
        float t_ = 0.1f;
    }

    class FallAfterFix : State<Controller> {
        public FallAfterFix(Controller parent) : base( parent ) { }
        protected override State innerInit() {
            // 牌の関係を絶つ
            parent_.paiObjects_[ 1 ].transform.SetParent( null );

            // 落下対象牌を判定
            var field = GameManager.getInstance().Field;
            var pos0 = parent_.paiObjects_[ 0 ].transform.position;
            var pos1 = parent_.paiObjects_[ 1 ].transform.position;
            if ( parent_.moveUtil.enableFall( pos0, 0.1f, field.Box ) == false ) {
                // 0は接地
                // 1が0の上にある場合、及び接地なら固定確定
                if ( parent_.rotIdx_ == 1 || parent_.moveUtil.enableFall( pos1, 0.1f, field.Box ) == false ) {
                    return new AddToField( parent_ );
                }
                // 1落下決定
                target_ = parent_.paiObjects_[ 1 ];
            } else if ( parent_.moveUtil.enableFall( pos1, 0.1f, field.Box ) == false ) {
                // 1は接地
                // 0が1の上にある場合、及び接地なら固定確定
                if ( parent_.rotIdx_ == 3 || parent_.moveUtil.enableFall( pos0, 0.1f, field.Box ) == false ) {
                    return new AddToField( parent_ );
                }
                // 0落下決定
                target_ = parent_.paiObjects_[ 0 ];
            } else {
                return new AddToField( parent_ );
            }
            return null;
        }

        protected override State innerUpdate() {
            var field = GameManager.getInstance().Field;
            var pos = target_.transform.position;
            if ( parent_.moveUtil.enableFall( pos, fallSpeed_ * Time.deltaTime, field.Box ) == true ) {
                pos.y -= fallSpeed_ * Time.deltaTime;
                target_.transform.position = pos;
            } else {
                // 底を合わせる
                target_.transform.position = parent_.moveUtil.calcCellCenter( target_.transform.position );
                return new AddToField( parent_ );
            }
            return this;
        }
        float fallSpeed_ = 20.0f;
        PaiObject target_;
    }

    class AddToField : State<Controller> {
        public AddToField(Controller parent) : base( parent ) {
        }
        protected override State innerUpdate() {
            // 指定時間待つ
            t_ -= Time.deltaTime;
            if ( bWait_ == false && t_ <= 0.0f ) {
                bWait_ = true;
                // フィールドに追加
                var field = GameManager.getInstance().Field;
                field.addPai( parent_.paiObjects_[ 0 ], parent_.moveUtil.convPosToIdx( parent_.paiObjects_[ 0 ].transform.position, true ) );
                field.addPai( parent_.paiObjects_[ 1 ], parent_.moveUtil.convPosToIdx( parent_.paiObjects_[ 1 ].transform.position, true ) );

                field.updateBox( (res) => {
                    if ( res == true )
                        setNextState( new SetNewPai( parent_ ) );
                    else
                        setNextState( new Stop( parent_ ) );
                } );
            }
            return this;
        }
        float t_ = 0.2f;
        bool bWait_ = false;
    }

    class Stop : State<Controller> {
        public Stop(Controller parent) : base( parent ) { }
    }
}
