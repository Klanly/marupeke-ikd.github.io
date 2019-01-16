using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

    [SerializeField]
    UnityEngine.UI.Image logo_;

    [SerializeField]
    UnityEngine.UI.Text copyRightText_;

    [SerializeField]
    UnityStandardAssets.ImageEffects.BlurOptimized blur_;

    [SerializeField]
    Camera camera_;

    [SerializeField]
    GameObject mainCameraPosObj_;


    public System.Action OnFinish { set { onFinishCallback_ = value; } }
    System.Action onFinishCallback_;

    public void initialize()
    {
        blurIteration_ = blur_.blurIterations;
        blurSpread_ = blur_.blurSize;
        mainCameraPos_ = camera_.transform.position;
        mainCameraRot_ = camera_.transform.rotation;

        state_ = new State_Idle( this );
    }

    // タイトルに戻るステートを再生
    public void toTitle()
    {
        logo_.gameObject.SetActive( true );
        copyRightText_.gameObject.SetActive( true );
        state_ = new State_ToTitle( this );
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ( state_ != null )
           state_ = state_.update();
	}

    class StateBase : State
    {
        public StateBase( TitleManager parent )
        {
            parent_ = parent;
        }
        protected TitleManager parent_;
    }

    // タイトルに戻る
    class State_ToTitle : StateBase
    {
        public State_ToTitle(TitleManager parent) : base( parent ) { }
        // 内部初期化
        override protected void innerInit()
        {
            // メインカメラをタイトル時位置に戻す
            // またブラーの数値を戻す
            parent_.blur_.enabled = true;
            cameraPos_ = parent_.camera_.transform.position;
            cameraRot_ = parent_.camera_.transform.rotation;
        }

        // 内部状態
        override protected State innerUpdate()
        {
            t_ += Time.deltaTime;
            float t2 = Mathf.SmoothStep( 0.0f, 1.0f, t_ / time_ );
            parent_.blur_.blurSize = Mathf.Lerp( 0.0f, parent_.blurSpread_, t2 );
            parent_.camera_.transform.position = Vector3.Slerp( cameraPos_, parent_.mainCameraPosObj_.transform.position, t2 );
            parent_.camera_.transform.rotation = Quaternion.Slerp( cameraRot_, parent_.mainCameraPosObj_.transform.rotation, t2 );
            if ( t_ >= time_ )
                return new State_Idle( parent_ );
            return this;
        }

        Vector3 cameraPos_ = Vector3.zero;
        Quaternion cameraRot_ = Quaternion.identity;
        float t_ = 0.0f;
        float time_ = 3.0f;
    }

    // タイトルアイドル
    class State_Idle : StateBase
    {
        public State_Idle( TitleManager parent ) : base( parent ) {}

        // 内部状態
        // 内部初期化
        override protected void innerInit()
        {
            // メインカメラをタイトル時位置に戻す
            // またブラーの数値を戻す
            parent_.blur_.enabled = true;
            parent_.camera_.transform.position = parent_.mainCameraPosObj_.transform.position;
            parent_.camera_.transform.rotation = parent_.mainCameraPosObj_.transform.rotation;
        }

        override protected State innerUpdate()
        {
            if ( Input.GetMouseButtonDown( 0 ) == true ) {
                return new State_ToGame( parent_ );
            }
            return this;
        }
    }

    // タイトルからゲームへ
    class State_ToGame : StateBase
    {
        public State_ToGame( TitleManager parent ) : base( parent ) { }
        // 内部初期化
        override protected void innerInit()
        {
            // メインカメラをゲームのアングルへ移動
            // またブラーの数値を0にして最後にOFFにする
            parent_.logo_.gameObject.SetActive( false );
            parent_.copyRightText_.gameObject.SetActive( false );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            t_ += Time.deltaTime;
            float t2 = Mathf.SmoothStep( 0.0f, 1.0f, t_ / time_ );
            parent_.blur_.blurSize = Mathf.Lerp( parent_.blurSpread_, 0.0f, t2 );
            parent_.camera_.transform.position = Vector3.Slerp( parent_.mainCameraPosObj_.transform.position, parent_.mainCameraPos_, t2 );
            parent_.camera_.transform.rotation = Quaternion.Slerp( parent_.mainCameraPosObj_.transform.rotation, parent_.mainCameraRot_, t2 );

            if ( t_ >= time_ ) {
                parent_.blur_.enabled = false;
                if ( parent_.onFinishCallback_ != null )
                    parent_.onFinishCallback_();
                return null;
            }
            return this;
        }

        Vector3 cameraPos_ = Vector3.zero;
        Quaternion cameraRot_ = Quaternion.identity;
        float t_ = 0.0f;
        float time_ = 3.0f;
    }

    int blurIteration_ = 3;
    float blurSpread_ = 0.7f;
    Vector3 mainCameraPos_ = Vector3.zero;
    Quaternion mainCameraRot_ = Quaternion.identity;
    State state_ = null;
}
