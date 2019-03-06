using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 6角形エッジムーブライン
public class HexEdgeMoveLine : MonoBehaviour {

	[SerializeField]
	int hexNumX_ = 10;

	[SerializeField]
	int changeFrameCount_ = 60;

	[SerializeField]
	float lenPerFrame_ = 0.1f;

	[SerializeField]
	int drawFrameNum_ = 1200;

	[SerializeField]
	int emitNum_ = 10;

	[SerializeField]
	GLLineTrail linePrefab_;

	[SerializeField]
	GLLines glLines_;

	[SerializeField]
	GameObject root_;

	// Use this for initialization
	void Start () {
		float L = changeFrameCount_ * lenPerFrame_;
		for ( int i = 0; i < emitNum_; ++i ) {
			GlobalState.wait( Random.value * 3.0f, () => {
				var line = Instantiate<GLLineTrail>( linePrefab_ );
				line.transform.parent = root_.transform;
				line.transform.localPosition = Vector3.zero;
				int row = Random.Range( 0, 50 );
				float ofs = ( row % 2 == 0 ? 0.0f : L * Mathf.Sqrt( 3.0f ) * 0.5f );
				var moveLine = new Move( new Vector3( ofs + L * Mathf.Sqrt( 3.0f ) * Random.Range( 0, hexNumX_ + 1 ), L * 3.0f / 2.0f * row, 0.0f ), line, lenPerFrame_, changeFrameCount_, drawFrameNum_ );
				moveLines_.Add( moveLine );
				glLines_.addLine( line );
				return false;
			} );
		}		
	}
	
	// Update is called once per frame
	void Update () {
		foreach( var line in moveLines_ ) {
			line.update();
		}		
	}

	List<Move> moveLines_ = new List<Move>();

	class Move {
		public Move( Vector3 initPos, GLLineTrail line, float lenPerFrame, int changeFrameNum, int drawFrameNum ) {
			initPos_ = initPos;
			pos_ = initPos;
			line_ = line;
			lenPerFrame_ = lenPerFrame;
			changeFrameNum_ = changeFrameNum;
			drawFrameNum_ = drawFrameNum;
			curDrawFrameCount_ = drawFrameNum;

			line_.transform.localPosition = pos_;
			line_.reset( line_.transform.position );
			state_ = upMove;
		}

		void upMove() {
			var pos = line_.transform.localPosition;
			line_.transform.localPosition = pos + dir_ * lenPerFrame_;
			curFrameCount_++;
			if ( curFrameCount_ == changeFrameNum_ ) {
				curFrameCount_ = 0;
				dir_ = ( Random.Range( 0, 2 ) == 0 ? rightDir_g : leftDir_g );
				state_ = lrMove;
			}
		}

		void lrMove() {
			var pos = line_.transform.localPosition;
			line_.transform.localPosition = pos + dir_ * lenPerFrame_;
			curFrameCount_++;
			if ( curFrameCount_ == changeFrameNum_ ) {
				curFrameCount_ = 0;
				dir_ = Vector3.up;
				state_ = upMove;
			}
		}

		public void update() {
			state_();
			curDrawFrameCount_--;
			if ( curDrawFrameCount_ <= 0 ) {
				curDrawFrameCount_ = drawFrameNum_;
				line_.reset( initPos_ );
				line_.transform.localPosition = initPos_;
				dir_ = Vector3.up;
				state_ = upMove;
			}
		}

		float lenPerFrame_ = 1.0f;
		System.Action state_;
		GLLineTrail line_;
		Vector3 pos_ = Vector3.zero;
		Vector3 dir_ = Vector3.up;
		static Vector3 rightDir_g = new Vector3( Mathf.Sqrt( 3.0f ) * 0.5f, 0.5f, 0.0f );
		static Vector3 leftDir_g = new Vector3( -Mathf.Sqrt( 3.0f ) * 0.5f, 0.5f, 0.0f );
		int changeFrameNum_ = 60;
		int curFrameCount_ = 0;
		int drawFrameNum_ = 0;
		int curDrawFrameCount_ = 0;
		Vector3 initPos_ = Vector3.zero;
	}
}
