using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// エンディング
public class Ending : MonoBehaviour {

    [SerializeField]
    float cameraRotAnglePerSec_ = 20.0f;

    [SerializeField]
    Light cameraLightPrefab_;

    [SerializeField]
    float cameraR_;

    [SerializeField]
    float cameraRad_;

    [SerializeField]
    GameObject magicCircle_;

    public System.Action FinishCallback { set { finishCallback_ = value; } }
    System.Action finishCallback_;

    public void setup( MazeCreator.Parameter param ) {
        param_ = param;
    }

	// Use this for initialization
	void Start () {
        state_ = new Wait( this );
	}
	
	// Update is called once per frame
	void Update () {
        if ( state_ != null )
            state_ = state_.update();
	}

    class Wait : State< Ending > {
        public Wait( Ending parent ) : base( parent ) {
        }

        protected override State innerUpdate() {
            if ( parent_.param_ != null ) {
                Camera.main.transform.SetParent( parent_.transform );
                return new CameraBack( parent_ );
            }
            return this;
        }
    }

    class LightDistribute : State< Ending > {
        public LightDistribute( Ending parent ) : base( parent ) {
        }
        protected override State innerInit() {
            return new CameraBack( parent_ );
        }
    }

    class CameraBack : State< Ending > {
        public CameraBack( Ending parent ) : base( parent ) {
        }

        protected override State innerInit() {
            // カメラを最上階から引きつつ回転移動
            var topCell = parent_.param_.getTopCell();
            Vector3 lookPos = topCell.localPos_;
            Vector3 floorPos = lookPos; 
            lookPos.y += parent_.param_.roomHeight_;
            floorPos.y += parent_.param_.roomHeight_ * 0.5f;
            float radius = parent_.param_.level_ * parent_.param_.roomWidthX_ * 0.5f;
            float r = 0.0f;
            GlobalState.time( 3.0f, (sec, t) => {
                r = Lerps.Float.easeOut01( t ) * radius;
                return true;
            } );
            float rad = 0.0f;
            GlobalState.start( () => {
                if ( parent_ == null )
                    return false;
                rad += Time.deltaTime * parent_.cameraRotAnglePerSec_ * Mathf.Deg2Rad;
                return true;
            } );
            float y = lookPos.y + 1.0f;
            GlobalState.time( 3.0f, (sec, t) => {
                y = lookPos.y + 1.0f + Lerps.Float.easeOut01( t ) * radius * 0.2f;
                return true;
            } );

            // カメラ位置更新
            Camera.main.transform.position = lookPos;
            Vector3 cameraPos = lookPos;
            var yOffset = new Vector3( 0.0f, -1.0f, 0.0f );
            GlobalState.start( () => {
                if ( parent_ == null )
                    return false;
                parent_.cameraR_ = r;
                parent_.cameraRad_ = rad;
                cameraPos.x = r * Mathf.Cos( rad ) + lookPos.x;
                cameraPos.z = r * Mathf.Sin( rad ) + lookPos.z;
                cameraPos.y = y;
                Camera.main.transform.position = cameraPos;
                Camera.main.transform.rotation = Quaternion.LookRotation( lookPos - cameraPos + yOffset );
                return true;
            } );

            // カメラにライトを装着
            var light = Instantiate<Light>( parent_.cameraLightPrefab_ );
            light.transform.SetParent( Camera.main.transform );
            light.transform.localPosition = Vector3.zero;
            light.transform.localRotation = Quaternion.identity;

            // 魔法円を設置
            parent_.magicCircle_.transform.position = floorPos + new Vector3( 0.0f, 0.05f, 0.0f );   // ちょっと上に


            // 時間が経った後に終了検知開始
            GlobalState.wait( 4.5f, () => {
                if ( Input.GetMouseButtonDown( 0 ) == true ) {
                    parent_.finishCallback_();
                    return false;
                }
                return true;
            } );

            return this;
        }
    }

    MazeCreator.Parameter param_;
    State state_;
}
