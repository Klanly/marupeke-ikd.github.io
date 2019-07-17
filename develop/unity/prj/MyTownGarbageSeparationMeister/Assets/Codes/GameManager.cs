using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField]
    TurnTableManager turnTableManage_;

    [SerializeField]
    GabageInfoWindow infoWindow_;

    [SerializeField]
    ErrorWindowFrame errWindow_;

    [SerializeField]
    SpriteButton[] buttons_;

    [SerializeField]
    GameObject good_;

    [SerializeField]
    GameObject no_;

    [SerializeField]
    TextMesh conter_;

    [SerializeField]
    ResultManager result_;

    [SerializeField]
    bool debugNext_ = false;

    [SerializeField]
    int debugInitCardIdx_ = 0;

    public System.Action FinishCallback { set { finishCallback_ = value; } }
    System.Action finishCallback_ = null;


    void turnNext() {
        uiActive( false );
        infoWindow_.shrink();
        if ( turnTableManage_.setNext( (param) => {

            // カウンタ
            conter_.text = string.Format( "正解：{0} 不正解：{1}  {2}/{3}", correctNum_, missNum_, correctNum_ + missNum_ + 1, turnTableManage_.getCardNum() );

            infoWindow_.setParam( param );
            infoWindow_.start( () => {
                // UI許可
                uiActive( true );
            } );
        } ) == false ) {
            // 終了
            state_ = new GameFinish( this );
        }
    }

    void uiActive(bool isActive) {
        foreach ( var b in buttons_ ) {
            b.setEnable( isActive );
        }
    }

    private void Awake() {
        state_ = new Intro( this );
        good_.SetActive( false );
        no_.SetActive( false );
        errWindow_.gameObject.SetActive( false );
        result_.gameObject.SetActive( false );
    }

    void Start() {
        var param = new TurnTable.Param();
        var table = Data_tokyo_shibuya_data.getInstance();
        int n = table.getRowNum();
        for ( int i = debugInitCardIdx_; i < n; ++i ) {
            var p = table.getParamFromIndex( i );
            var card = new Card.Param();
            card.name = p.name_;
            card.material = p.material_;
            card.weight = p.weight_;
            card.weightUnit = p.weightUnit_;
            card.dimensionX = p.dimensionX_;
            card.dimensionY = p.dimensionY_;
            card.dimensionZ = p.dimensionZ_;
            card.dimensionUnit = p.dimensionUnit_;
            card.answer = p.answer_;
            card.image = p.image_;
            card.comment = p.comment_;
            param.cards.Add( card );
        }

        turnTableManage_.setup( param );

        uiActive( false );

        // ボタン
        foreach ( var b in buttons_ ) {
            b.OnDecide = (name) => {
                onSelect( name );
            };
        }

        // カウンタ
        conter_.text = string.Format( "正解：{0} 不正解：{1}  {2}/{3}", correctNum_, missNum_, 0, turnTableManage_.getCardNum() );
    }

    // ボタンを押して選択した
    void onSelect(string name) {
        uiActive( false );
        var param = turnTableManage_.getCurCardParam();
        if ( param.answer == name ) {
            // 正解
            onCorrect( () => {
                turnNext();
            } );
        } else {
            // 不正解
            onUncorrect( () => {
                turnNext();
            } );
        }
    }

    // 正解
    void onCorrect(System.Action finishCallback) {
        Debug.Log( "正解！" );

        // カウンタ
        correctNum_++;
        conter_.text = string.Format( "正解：{0} 不正解：{1}  {2}/{3}", correctNum_, missNum_, correctNum_ + missNum_, turnTableManage_.getCardNum() );

        good_.SetActive( true );
        GlobalState.wait( 1.5f, () => {
            good_.SetActive( false );
            return false;
        } );
        finishCallback();
    }

    // 不正解
    void onUncorrect(System.Action finishCallback) {
        Debug.Log( "不正解…" );

        // カウンタ
        missNum_++;
        conter_.text = string.Format( "正解：{0} 不正解：{1}  {2}/{3}", correctNum_, missNum_, correctNum_ + missNum_, turnTableManage_.getCardNum() );

        no_.SetActive( true );
        var param = turnTableManage_.getCurCardParam();
        GlobalState.wait( 1.5f, () => {
            no_.SetActive( false );
            return false;
        }, () => {
            errWindow_.gameObject.SetActive( true );
            errWindow_.setStr( param.comment );
        } )
        .wait( 3.0f )
        .finish( () => {
            errWindow_.gameObject.SetActive( false );
            finishCallback();
        } );
    }

    // Update is called once per frame
    void Update() {
        if ( debugNext_ ) {
            debugNext_ = false;
            turnNext();
        }

        if ( state_ != null )
            state_ = state_.update();
    }

    class Intro : State<GameManager> {
        public Intro(GameManager parent) : base( parent ) {
        }

        protected override State innerInit() {
            FaderManager.Fader.to( 0.0f, 1.0f, () => {
                setNextState( new Gaming( parent_ ) );
            } );
            return this;
        }
    }

    class Gaming : State<GameManager> {
        public Gaming(GameManager parent) : base( parent ) {
            parent.turnNext();
        }
        protected override State innerInit() {
            return this;
        }
    }

    class GameFinish : State<GameManager> {
        public GameFinish(GameManager parent) : base( parent ) {
        }
        protected override State innerInit() {
            GlobalState.wait( 1.5f, () => {
                parent_.result_.setup( parent_.correctNum_, parent_.missNum_ );
                parent_.result_.gameObject.SetActive( true );
                parent_.result_.FinishCallback = () => {
                    setNextState( new FadeOut( parent_ ) );
                };
                return false;
            } );
            return this;
        }
    }

    class FadeOut : State<GameManager> {
        public FadeOut(GameManager parent) : base( parent ) {
        }
        protected override State innerInit() {
            FaderManager.Fader.to( 1.0f, 1.0f, () => {
                parent_.finishCallback_();
            } );
            return null;
        }
    }

    State state_;
    int correctNum_ = 0;
    int missNum_ = 0;
}
