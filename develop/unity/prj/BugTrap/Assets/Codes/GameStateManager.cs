using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : GameManagerBase {

	[SerializeField]
	TitleManager titlePrefab_ = null;

	[SerializeField]
	GameManager gamePrefab_ = null;

	void Start()
	{
		state_ = new Title( this );
	}

	void Update()
	{
		stateUpdate();
	}

	class Title : State<GameStateManager> {
		public Title(GameStateManager parent) : base( parent ) { }
		protected override State innerInit()
		{
			manager_ = PrefabUtil.createInstance( parent_.titlePrefab_, parent_.transform, Vector3.zero );
			manager_.FinishCallbacak = () => {
				Destroy( manager_.gameObject );
				setNextState( new Game( parent_ ) );
			};
			return this;
		}
		TitleManager manager_;
	}

	class Game : State<GameStateManager> {
		public Game(GameStateManager parent) : base( parent ) { }
		protected override State innerInit()
		{
			manager_ = PrefabUtil.createInstance( parent_.gamePrefab_, parent_.transform, Vector3.zero );
			manager_.FinishCallbacak = () => {
				Destroy( manager_.gameObject );
				setNextState( new Title( parent_ ) );
			};
			return this;
		}
		GameManager manager_;
	}
}
