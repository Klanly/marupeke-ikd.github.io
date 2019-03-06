using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTextEffect : MonoBehaviour {

	[SerializeField]
	TextMesh text_;

	[SerializeField]
	TextMesh levelText_;

	[SerializeField]
	GameOver gameOver_;

	[SerializeField]
	GameObject levelRoot_;

	[SerializeField]
	Congratulations congra_;

	public void reset( int level ) {
		level_ = level;
		int maxLevel = TowerParameterTable.getInstance().getParamNum();
		bFinalLevel_ = ( level_ == maxLevel );
		if ( bFinalLevel_ == true )
			text_.text = string.Format( "Final Tower" );
		else
			text_.text = string.Format( "Tower {0:00}", level );

		state_ = new Intro( this );
	}

	public GameOver getGameOver() {
		return gameOver_;
	}

	public Congratulations getCongratulation() {
		return congra_;
	}

	public void hideLevelFrame() {
		levelRoot_.SetActive( false );
	}

	private void Awake() {
		levelTextColor_ = levelText_.color;
		levelText_.color = Color.clear;
		towerNumberColor_ = text_.color;
		text_.color = Color.clear;
	}
	// Use this for initialization
	void Start () {
		obj_ = text_.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if ( state_ != null )
			state_ = state_.update();
	}

	class Intro : State< StageTextEffect > {
		public Intro(StageTextEffect parent) : base( parent ) {
		}
		protected override State innerInit() {
			parent_.obj_.SetActive( true );
			var initPos = new Vector3( 14.0f, 0.0f, 0.0f );
			var finishPos = new Vector3( -14.0f, 0.0f, 0.0f );
			GlobalState.time( 0.15f, (sec, t) => {
				parent_.text_.color = Color.Lerp( Color.clear, parent_.towerNumberColor_, t );
				parent_.obj_.transform.localPosition = Lerps.Vec3.linear( initPos, Vector3.zero, t );
				return true;
			} ).nextTime( 0.6f, (sec, t) => {
				return true;
			} ).nextTime( 0.15f, (sec, t) => {
				parent_.text_.color = Color.Lerp( parent_.towerNumberColor_, Color.clear, t );
				parent_.obj_.transform.localPosition = Lerps.Vec3.easeIn( Vector3.zero, finishPos, t );
				return true;
			} ).finish( () => {
				parent_.obj_.SetActive( false );
			} );

			// Level
			var scale = new Vector3( 1.5f, 1.5f, 1.5f );
			Color initLevelColorC = ( parent_.bFirstLevel_ == true ? Color.clear : parent_.levelTextColor_ );
			GlobalState.time( 0.5f, (sec, t) => {
				parent_.levelText_.color = Color.Lerp( initLevelColorC, Color.clear, t );
				return true;
			} ).oneFrame( () => {
				parent_.levelText_.text = string.Format( "{0}", parent_.level_ );
				parent_.levelText_.color = parent_.levelTextColor_;
			}).nextTime( 0.25f, (sec, t) => {
				parent_.levelText_.transform.localScale = Lerps.Vec3.easeOutStrong( scale, Vector3.one, t );
				return true;
			} );
			return this;
		}
	}

	GameObject obj_;
	State state_;
	int level_ = 0;
	Color towerNumberColor_;
	Color levelTextColor_;
	bool bFirstLevel_ = true;
	bool bFinalLevel_ = false;
}
