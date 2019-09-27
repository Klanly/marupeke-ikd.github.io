using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GameManagerBase {
	void Start()
	{
		FaderManager.Fader.setColor( new Color( 0.0f, 0.0f, 0.0f, 1.0f ) );
		state_ = new FadeIn( this );
	}

	void Update()
	{
		stateUpdate();
	}

	class FadeIn : State<GameManager> {
		public FadeIn(GameManager parent) : base( parent ) { }
		protected override State innerInit()
		{
			FaderManager.Fader.to( new Color( 0.0f, 0.0f, 0.0f, 0.0f ), 1.5f, () => {
				setNextState( new Idle( parent_ ) );
			} );
			return this;
		}
	}

	class Idle : State<GameManager> {
		public Idle(GameManager parent) : base( parent ) { }
		protected override State innerUpdate()
		{
			if (Input.GetMouseButtonDown( 0 ) == true) {
				setNextState( new FadeOut( parent_ ) );
			}
			return this;
		}
	}

	class FadeOut : State<GameManager> {
		public FadeOut(GameManager parent) : base( parent ) { }
		protected override State innerInit()
		{
			FaderManager.Fader.to( new Color( 0.0f, 0.0f, 0.0f, 1.0f ), 1.5f, () => {
				parent_.finishCallback_();
			} );
			return this;
		}
	}
}
