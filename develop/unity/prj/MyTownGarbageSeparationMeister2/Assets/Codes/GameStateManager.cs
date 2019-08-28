using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [SerializeField]
    TitleManager titlePrefab_;

    [SerializeField]
    GameManager gamePrefab_;


    private void Awake() {
        state_ = new Title( this );
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( state_ != null )
            state_ = state_.update();
    }

    class Title : State<GameStateManager> {
        public Title(GameStateManager parent) : base( parent ) {
        }
        protected override State innerInit() {
            manager_ = PrefabUtil.createInstance<TitleManager>( parent_.titlePrefab_, parent_.transform );
            manager_.FinishCallback = () => {
                setNextState( new Game( parent_ ) );
                Destroy( manager_.gameObject );
            };
            return this;
        }
        TitleManager manager_;
    }

    class Game : State<GameStateManager> {
        public Game( GameStateManager parent ) : base( parent ) {
        }
        protected override State innerInit() {
            manager_ = PrefabUtil.createInstance<GameManager>( parent_.gamePrefab_, parent_.transform );
            manager_.FinishCallback = () => {
                setNextState( new Title( parent_ ) );
                Destroy( manager_.gameObject );
            };
            return this;
        }
        GameManager manager_;
    }

    State state_;
}
