using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : GameManagerBase {
    [SerializeField]
    TitleManager titlePrefab_;

    [SerializeField]
    GameManager gamePrefab_;

    // Start is called before the first frame update
    void Start() {
        state_ = new Title( this );
    }

    // Update is called once per frame
    void Update() {
        stateUpdate();
    }

    class Title : State<GameStateManager> {
        public Title(GameStateManager parent) : base( parent ) { }
        protected override State innerInit() {
            Debug.Log( "Title" );
            title_ = PrefabUtil.createInstance( parent_.titlePrefab_, parent_.transform, Vector3.zero );
            title_.FinishCallbacak = () => {
                setNextState( new Game( parent_ ) );
                Destroy( title_.gameObject );
            };
            return this;
        }
        TitleManager title_;
    }

    class Game : State<GameStateManager> {
        public Game(GameStateManager parent) : base( parent ) { }
        protected override State innerInit() {
            Debug.Log( "Game" );
            game_ = PrefabUtil.createInstance( parent_.gamePrefab_, parent_.transform, Vector3.zero );
            game_.FinishCallbacak = () => {
                setNextState( new Title( parent_ ) );
                Destroy( game_.gameObject );
            };
            return this;
        }
        GameManager game_;
    }
}
