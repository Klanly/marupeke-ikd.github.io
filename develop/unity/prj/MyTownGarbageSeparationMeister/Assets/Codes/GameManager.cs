using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField]
    TurnTableManager turnTableManage_;

    [SerializeField]
    GabageInfoWindow infoWindow_;

    [SerializeField]
    SpriteButton[] buttons_;

    [SerializeField]
    bool debugNext_ = false;

    void turnNext() {
        uiActive( false );
        infoWindow_.shrink();
        if ( turnTableManage_.setNext( (param) => {
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
    }

    void Start() {
        var param = new TurnTable.Param();
        var card0 = new Card.Param();
        card0.name = "ペットボトル";
        card0.material = "PET";
        card0.weight = 10.0f;
        card0.weightUnit = "g";
        card0.dimensionX = 7.0f;
        card0.dimensionY = 25.0f;
        card0.dimensionZ = 7.0f;
        card0.dimensionUnit = "cm";
        card0.answer = "recycle";
        param.cards.Add( card0 );

        var card1 = new Card.Param();
        card1.name = "生ごみ";
        card1.material = "野菜の切れ端";
        card1.weight = 60.0f;
        card1.weightUnit = "g";
        card1.dimensionX = 10.0f;
        card1.dimensionY = 8.0f;
        card1.dimensionZ = 7.0f;
        card1.dimensionUnit = "cm";
        card1.answer = "burn";
        param.cards.Add( card1 );

        turnTableManage_.setup( param );

        uiActive( false );

        // ボタン
        foreach ( var b in buttons_ ) {
            b.OnDecide = (name) => {
                onSelect( name );
            };
        }
    }

    // ボタンを押して選択した
    void onSelect(string name) {
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
        finishCallback();
    }

    // 不正解
    void onUncorrect(System.Action finishCallback) {
        Debug.Log( "不正解…" );
        finishCallback();
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
            setNextState( new Gaming( parent_ ) );
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

    class GameFinish : State< GameManager> {
        public GameFinish( GameManager parent ): base( parent ) {
        }
        protected override State innerInit() {
            return this;
        }
    }
    State state_;
}
