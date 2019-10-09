using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : GameManagerBase {

	void Start()
	{
		FaderManager.Fader.setColor( new Color( 0.0f, 0.0f, 0.0f, 1.0f ) );
		state_ = new FadeIn( this );
	}

	void Update()
	{
		stateUpdate();
	}

	class FadeIn : State<TitleManager> {
		public FadeIn(TitleManager parent) : base( parent ) { }
		protected override State innerInit()
		{
			FaderManager.Fader.to( new Color( 0.0f, 0.0f, 0.0f, 0.0f ), 1.5f, () => {
				setNextState( new Idle( parent_ ) );
			} );
			return this;
		}
	}

	class Idle : State<TitleManager> {
		public Idle(TitleManager parent) : base( parent ) { }
		protected override State innerUpdate()
		{
			if (Input.GetMouseButtonDown( 0 ) == true) {
				setNextState( new FadeOut( parent_ ) );
			}
			return this;
		}
	}

	class FadeOut : State<TitleManager> {
		public FadeOut(TitleManager parent) : base( parent ) { }
		protected override State innerInit()
		{
			FaderManager.Fader.to( new Color( 0.0f, 0.0f, 0.0f, 1.0f ), 1.5f, () => {
				parent_.finishCallback_();
			} );
			return this;
		}
	}
}
