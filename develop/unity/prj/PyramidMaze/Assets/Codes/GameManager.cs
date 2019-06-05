using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    MazeCreateTest maze_;

    [SerializeField]
    Player player_;

    [SerializeField]
    GameObject magicCirclePrefab_;    // 出口魔法円

    [SerializeField]
    Ending ending_;

    [SerializeField]
    UnityEngine.UI.Image fader_;

    [SerializeField]
    GameObject[] keyImages_;

    [SerializeField]
    GameObject[] nullKeyImages_;



    public System.Action FinishCallback { set { finishCallback_ = value; } }

    public void setup(int level ) {
        maze_.setup( level );
        ending_.gameObject.SetActive( false );

        player_.GoEndingCallback = () => {
            ending_.gameObject.SetActive( true );
            ending_.setup( maze_.getParam() );
            ending_.transform.localPosition = Vector3.zero;
        };

        foreach ( var k in keyImages_ ) {
            k.gameObject.SetActive( false );
        }
        foreach ( var nk in nullKeyImages_ ) {
            nk.gameObject.SetActive( false );
        }
        for ( int i = 0; i < maze_.getKeyNum(); ++i ) {
            nullKeyImages_[ i ].SetActive( true );
        }

        player_.ItemGetCallback = (item) => {

            // 鍵イメージ表示
            keyImages_[ curGetKeyNum_ ].SetActive( true );
            nullKeyImages_[ curGetKeyNum_ ].SetActive( false );

            curGetKeyNum_++;

            if ( curGetKeyNum_ >= maze_.getKeyNum() ) {
                // 出口魔法陣を表示
                var param = maze_.getParam();
                var topCell = param.getTopCell();
                var magicCircle = PrefabUtil.createInstance<GameObject>( magicCirclePrefab_, transform );
                magicCircle.transform.localPosition = topCell.localPos_ + new Vector3( 0.0f, param.roomHeight_ * 0.45f, 0.0f );   // 天井へ
                magicCircle.gameObject.SetActive( true );
            }
        };

        bInitialize_ = true;
    }

    // Use this for initialization

    private void Awake() {
        ending_.gameObject.SetActive( false );
        Color c = fader_.color;
        c.a = 1.0f;
        fader_.color = c;
        state_ = new FadeIn( this );
    }
    void Start () {
        if ( bInitialize_ == false )
            setup( 2 );
	}
	
	// Update is called once per frame
	void Update () {
        if ( state_ != null )
            state_ = state_.update();
    }

    class FadeIn : State< GameManager > {
        public FadeIn( GameManager parent ) : base( parent ) { }
        protected override State innerInit() {
            Color c = parent_.fader_.color;
            c.a = 1.0f;
            Color ec = c;
            ec.a = 0.0f;
            GlobalState.time( 2.0f, (sec, t) => {
                parent_.fader_.color = Color.Lerp( c, ec, t );
                return true;
            } ).finish(() => {
                setNextState( new Idle( parent_ ) );
            } );
            return this;
        }
    }

    class Idle : State< GameManager > {
        public Idle(GameManager parent) : base( parent ) { }
        protected override State innerInit() {
            parent_.ending_.FinishCallback = () => {
                setNextState( new FadeOut( parent_ ) );
            };

            return this;
        }
    }

    class FadeOut : State< GameManager > {
        public FadeOut(GameManager parent) : base( parent ) { }
        protected override State innerInit() {
            Color c = parent_.fader_.color;
            Color ec = c;
            ec.a = 1.0f;
            GlobalState.time( 2.5f, (sec, t) => {
                parent_.fader_.color = Color.Lerp( c, ec, t );
                return true;
            } )
            .wait( 1.0f )
            .finish( () => {
                parent_.finishCallback_();
                setNextState( null );
            } );
            return this;
        }
    }

    System.Action finishCallback_;
    bool bInitialize_ = false;
    State state_;
    int curGetKeyNum_ = 0;
}
