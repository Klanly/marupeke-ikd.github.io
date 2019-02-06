using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ギミックネジトラップ
//
//  ギミック：
//   右回し及び左回し
//   回転数(1～3)

public class ScrewTrap : Trap {

    [SerializeField]
    Rotate rotate_ = Rotate.Left;

    [SerializeField]
    int rotNum_ = 1;

    [SerializeField]
    ScrewTrapAnswer answerPrefab_;

    [SerializeField]
    OnAction onAction_;

    [SerializeField]
    GameObject screwModel_;

    public enum Rotate : int
    {
        Left = 0,
        Right = 1
    }
    // セットアップ指示
    public override void setup( int randomNumber, bool forGimicBox )
    {
        rotate_ = (Rotate)( randomNumber % 2 );
        rotNum_ = ( randomNumber % 3 ) + 1;
        createAnswer( randomNumber, forGimicBox );
    }

    // 答え作成
    void createAnswer( int randomNumber, bool forGimicBox)
    {
        var obj =Instantiate<ScrewTrapAnswer>( answerPrefab_ );
        obj.setAnswer( randomNumber, rotate_, rotNum_ );
        answer_ = obj;

        if ( forGimicBox == true )
            answer_.setChildrenListSize( 1 );
    }

    // Use this for initialization
    void Start () {
        // ネジがクリックされたら回転イベントへ
        onAction_.ActionCallback = ( obj, eventStr ) => {
            state_ = new DragToStart( this );
        };
	}
	
	// Update is called once per frame
	void Update () {
        if ( state_ != null )
            state_ = state_.update();

    }

    // カーソルのレイとネジの平面との衝突点を算出
    bool getMouseRayColPos( out Vector3 colPos )
    {
        Ray mouseRay = Camera.main.ScreenPointToRay( Input.mousePosition );
        Vector3 origin = transform.position;
        return CollideUtil.colPosRayPlane( out colPos, mouseRay.origin, mouseRay.direction, origin, transform.forward );
    }

    class StateBase : State
    {
        public StateBase( ScrewTrap parent )
        {
            parent_ = parent;
        }
        protected ScrewTrap parent_;
    }

    // 回転開始点までドラッグ
    class DragToStart : StateBase
    {
        public DragToStart( ScrewTrap parent ) : base( parent )
        {

        }

        protected override State innerUpdate()
        {
            // ドラッグを止めたら失敗
            if ( Input.GetMouseButton( 0 ) == false ) {
                return new Failure( parent_ );
            }

            // ネジの原点からネジのXY平面への衝突点までの距離が一定以上離れたら
            // 開始位置確定
            Vector3 colPos;
            if ( parent_.getMouseRayColPos( out colPos ) == false ) {
                // カーソルが外れると失敗
                return new Failure( parent_ );
;            }

            var origin = parent_.transform.position;
            float dist = ( colPos - origin ).magnitude;
            Debug.Log( "dist: " + dist );
            if ( dist >= startDist_ ) {
                // ネジ原点から衝突位置までのベクトルを基線と確定
                var baseDir = colPos - origin;
                return new Turn( parent_, baseDir.normalized );
            }
            return this;
        }

        float startDist_ = 0.1f;
    }

    // ねじ回し中
    class Turn : StateBase
    {
        public Turn(ScrewTrap parent, Vector3 baseDir ) : base( parent ) {
            origin_ = parent_.transform.position;
            curBaseDir_ = baseDir;
            successRotMin_ = ( parent.rotate_ == Rotate.Left ? 1.0f : -1.0f ) * 360.0f * parent.rotNum_;
            successRotMax_ = successRotMin_ + ( parent.rotate_ == Rotate.Left ? 1.0f : -1.0f ) * 360.0f;
            if ( successRotMin_ > successRotMax_ ) {
                float tmp = successRotMax_;
                successRotMax_ = successRotMin_;
                successRotMin_ = tmp;
            }
        }

        protected override State innerUpdate()
        {
            Vector3 colPos;
            if ( parent_.getMouseRayColPos( out colPos ) == false ) {
                // カーソルが外れると失敗
                return new Failure( parent_ );
            }
            var dir = ( colPos - origin_ ).normalized;
            float rad = Mathf.Acos( Vector3.Dot( curBaseDir_, dir ) ) * ( Vector3.Dot( Vector3.Cross( curBaseDir_, dir ), parent_.transform.forward ) < 0.0f ? 1.0f : -1.0f );
            float deg = rad * Mathf.Rad2Deg;

            // ドラッグを止めた時に範囲内だったら成功
            // 範囲外だったら失敗
            if ( Input.GetMouseButton( 0 ) == false ) {
                if ( successRotMin_ <= curDeg_ + deg && curDeg_ + deg <= successRotMax_ ) {
                    return new Success( parent_ );
                }
                return new Failure( parent_ );
            }

            Quaternion q = Quaternion.AngleAxis( deg, parent_.transform.up ) * curQ_;
            if ( Mathf.Abs( deg ) >= 10.0f ) {
                // 基線を更新
                curDeg_ += deg;
                curBaseDir_ = dir;
                curQ_ = q;
                Debug.Log( "curDeg: " + successRotMin_ + " < " + curDeg_ + " < " + successRotMax_ );
            }
            parent_.screwModel_.transform.localRotation = q;
            return this;
        }

        Vector3 baseDir_;   // 0度基線
        Vector3 curBaseDir_;
        Vector3 origin_;
        float curDeg_ = 0.0f;
        Quaternion curQ_ = Quaternion.identity;
        float successRotMin_ = 0.0f;
        float successRotMax_ = 0.0f;
    }

    // ねじ回し成功
    class Success : StateBase
    {
        public Success(ScrewTrap parent) : base( parent ) { }
        protected override State innerInit()
        {
            parent_.successCallback_();
            return null;
        }
    }

    // ねじ回し失敗
    class Failure : StateBase
    {
        public Failure(ScrewTrap parent) : base( parent ) { }
        protected override State innerInit()
        {
            parent_.failureCallback_();
            return null;
        }
    }

    State state_;
}
