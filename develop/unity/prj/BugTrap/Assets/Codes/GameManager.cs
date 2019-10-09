using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : GameManagerBase {
	[SerializeField]
	int stageId_ = 0;

	[SerializeField]
	ObjectManager objectManager_;

	[SerializeField]
	StageManager stageManager_;

	[SerializeField]
	GameObject ground_;

	[SerializeField]
	GameObject groundRegion_;

	[SerializeField]
	UnityEngine.UI.Image clearImage_;

	[SerializeField]
	UnityEngine.UI.Button retryBtn_;

	[SerializeField]
	Player player_;

	public ObjectManager ObjectManager { get { return objectManager_; } }

	private void Awake()
	{
		clearImage_.gameObject.SetActive( false );
	}
	void Start()
	{
		FaderManager.Fader.setColor( new Color( 0.0f, 0.0f, 0.0f, 1.0f ) );
		state_ = new CreateStage( this );
		player_.setup( this );
		retryBtn_.onClick.AddListener( () => {
			if (bClear_ == true)
				return;
			state_ = new FadeOut( this );
		} );
	}

	void Update()
	{
		stateUpdate();
	}

	void setGoals( List<Goal> goals, int bugNum ) {
		emitBugNum_ = bugNum;
		foreach ( var g in goals ) {
			g.BugCatchCallback = () => {
				if (bClear_ == true)
					return;
				catchBugNum_++;
				if (catchBugNum_ == emitBugNum_) {
					// クリア
					toClear();
				}
			};
		}
	}

	void toClear()
	{
		bClear_ = true;
	}

	int emitBugNum_ = 0;
	int catchBugNum_ = 0;
	bool bClear_ = false;

	class CreateStage : State<GameManager> {
		public CreateStage(GameManager parent) : base( parent ) { }
		protected override State innerInit()
		{
			var data = parent_.stageManager_.createStage( parent_.stageId_, parent_.objectManager_ );
			parent_.ground_.transform.localPosition = data.center_;
			parent_.groundRegion_.transform.localScale = new Vector3( data.region_.x, 1.0f, data.region_.y );
			parent_.groundRegion_.transform.localPosition = data.center_;
			parent_.setGoals( data.goals_, data.emitBugNum_ );
			return new FadeIn( parent_ );
		}
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
			if ( parent_.bClear_ == true ) {
				return new Clear( parent_ );
			}
			return this;
		}
	}

	class Clear : State<GameManager > {
		public Clear( GameManager parent ) : base( parent ) { }
		protected override State innerInit() {
			Color sc = parent_.clearImage_.color;
			sc.a = 0.0f;
			Color ec = parent_.clearImage_.color;
			parent_.clearImage_.color = sc;
			parent_.clearImage_.gameObject.SetActive( true );
			GlobalState.time( 1.5f, (sec, t) => {
				parent_.clearImage_.color = Color.Lerp( sc, ec, t );
				return true;
			} ).finish(() => {
				setNextState( new FadeOut( parent_ ) );
			} );
			return this;
		}
	}

	class FadeOut : State<GameManager> {
		public FadeOut(GameManager parent) : base( parent ) { }
		protected override State innerInit()
		{
			FaderManager.Fader.to( new Color( 0.0f, 0.0f, 0.0f, 1.0f ), 1.5f, () => {
				// parent_.finishCallback_();
				SceneManager.LoadScene( "game" );
			} );
			return this;
		}
	}
}
