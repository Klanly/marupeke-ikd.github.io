using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

    [SerializeField]
    TitleManager titlePrefab_;

    [SerializeField]
    GameManager gamePrefab_;

    private void Awake() {
        state_ = new Title( this );
    }
	
	void Update () {
        if ( state_ != null )
            state_ = state_.update();
	}

    class Title : State< GameStateManager > {
        public Title(GameStateManager parent) : base( parent ) { }
        protected override State innerInit() {
            manager_ = PrefabUtil.createInstance( parent_.titlePrefab_, parent_.transform );
            manager_.FinishCallback = (level) => {
                Destroy( manager_.gameObject );
                setNextState( new Game( parent_, level ) );
            };
            return this;
        }
        TitleManager manager_;
    }

    class Game : State< GameStateManager > {
        public Game(GameStateManager parent, int level ) : base( parent ) { }
        protected override State innerInit() {
            manager_ = PrefabUtil.createInstance( parent_.gamePrefab_, parent_.transform );
            manager_.FinishCallback = () => {
                Destroy( manager_.gameObject );
                setNextState( new Title( parent_ ) );
            };
            return this;
        }
        GameManager manager_;
    }
    State state_;
}
