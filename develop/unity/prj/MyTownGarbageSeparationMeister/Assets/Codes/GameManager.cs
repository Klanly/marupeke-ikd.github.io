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
        var table = Data_tokyo_shibuya_data.getInstance();
        int n = table.getRowNum();
        for ( int i = 0; i < n; ++i ) {
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
