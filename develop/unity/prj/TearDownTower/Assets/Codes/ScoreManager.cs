using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// スコア管理人
//
//  スコアルール：
//   Σ(破壊ブロック列）列内の破壊したブロック数 × 100点 × チェーン数

public class ScoreManager : MonoBehaviour {

	[SerializeField]
	UnityEngine.UI.Text scoreText_;

	public void setActive( bool isActive ) {
		scoreText_.gameObject.SetActive( isActive );
	}

	public void breakBlocks( int colNum, int rowNum, int chainCount ) {
		var curScore = score_.getAim();
		score_.setAim( curScore + colNum * rowNum * blockScore_ * chainCount );
	}

	private void Awake() {
		score_ = new MoveValueLong( 0, 1.0f );
		scoreText_.text = "0";
		score_.Value = (_score) => {
			scoreText_.text = string.Format( "{0:#,0}", _score );
		};
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnGUI() {
		GUI.Label( new Rect( 0, 50, 300, 60 ), "score: " + score_.getCurVal() );
	}

	MoveValueLong score_;
	long blockScore_ = 100;
}
