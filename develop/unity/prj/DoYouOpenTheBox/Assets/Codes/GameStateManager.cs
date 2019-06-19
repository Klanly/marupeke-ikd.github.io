using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {
    [SerializeField]
    TitleManager titlePrefab_;

    [SerializeField]
    GameManager gamePrefab_;

    [SerializeField]
    int initStage_ = 0;

    private void Awake() {
        state_ = new Title( this );
    }

    void Update() {
        if ( state_ != null )
            state_ = state_.update();
    }

    class Title : State<GameStateManager> {
        public Title(GameStateManager parent) : base( parent ) {
        }
        protected override State innerInit() {
            title_ = PrefabUtil.createInstance( parent_.titlePrefab_, parent_.transform );
            title_.FinishCallback = () => {
                Destroy( title_.gameObject );
                setNextState( new Game( parent_ ) );
            };
            return this;
        }
        TitleManager title_;
    }

    class Game : State<GameStateManager> {
        public Game(GameStateManager parent ) : base( parent ) { }
        protected override State innerInit() {
            game_ = PrefabUtil.createInstance( parent_.gamePrefab_, parent_.transform );
            game_.initStage( parent_.initStage_ );
            game_.FinishCallback = () => {
                Destroy( game_.gameObject );
                setNextState( new Title( parent_ ) );
            };
            return this;
        }
        GameManager game_;
    }

    State state_;
}
