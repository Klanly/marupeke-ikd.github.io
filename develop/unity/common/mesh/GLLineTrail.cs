using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLLineTrail : GLLine {

	[SerializeField]
	int pointNum_ = 10;

	[SerializeField]
	Color startColor_ = Color.white;

	[SerializeField]
	Color endColor_ = Color.white;

	[SerializeField]
	float alpha_ = 1.0f;

	// ライン本体
	//  GLLineTrailのマテリアルで描画
	// トップ位置を更新
	public Vector3 TopPositon { set { top_ = value; } get { return top_; } }

	// セットアップ
	public void setup(int pointNum, Color startColor, Color endColor ) {
		if ( pointNum < 3 )
			pointNum = 3;
		pointNum_ = pointNum;

		points_ = new float[ pointNum_ * 3 ];
		colors_ = new Color[ pointNum_ ];
		for ( int i = 0; i < pointNum; ++i ) {
			colors_[ i ] = Color.Lerp( startColor, endColor, ( float )( i ) / ( pointNum - 1 ) );
			points_[ i * 3 ] = 0.0f;
			points_[ i * 3 + 1 ] = 0.0f;
			points_[ i * 3 + 2 ] = 0.0f;
		}
		bInitialized_ = true;
	}

	// リセット
	public void reset( Vector3 pos ) {
		if ( bInitialized_ == false ) {
			setup( pointNum_, startColor_, endColor_ );
			transform.localPosition = pos;
			reset( transform.position );
		}
		for ( int i = 0; i < pointNum_; ++i ) {
			points_[ i * 3 ] = pos.x;
			points_[ i * 3 + 1 ] = pos.y;
			points_[ i * 3 + 2 ] = pos.z;
		}
	}

	// 頂点カラーを変更
	public void setColors(int index, Color color, bool useDefaultAlpha = false ) {
		if ( index >= colors_.Length )
			return;
		if ( useDefaultAlpha == true ) {
			colors_[ index ] = new Color( color.r, color.g, color.b, colors_[ index ].a );
		} else {
			colors_[ index ] = color;
		}
	}

	// 全体のα値を設定
	//  個々のα値と掛け算される
	public void setAllAlpha(float alpha) {
		alpha_ = alpha;
	}

	// アクティブ変更
	//  更新は行われる
	public void setActive(bool isActive) {
		bActive_ = isActive;
	}

	// 描画（GLLineTrailから呼ばれる）
	override public void draw() {
		if ( bActive_ == false || gameObject.activeInHierarchy == false )
			return;
		int idx = curPos_;
		for ( int i = 0; i < pointNum_ - 1; ++i ) {
			idx = ( pointNum_ + idx ) % pointNum_;
			int e = ( pointNum_ + idx - 1 ) % pointNum_;
			Color c = colors_[ i ];
			c.a *= alpha_;
			GL.Color( c );
			GL.Vertex3( points_[ idx * 3 ], points_[ idx * 3 + 1 ], points_[ idx * 3 + 2 ] );
			GL.Vertex3( points_[ e * 3 ], points_[ e * 3 + 1 ], points_[ e * 3 + 2 ] );
			idx--;
		}
	}

	void Start () {
		if ( bInitialized_ == false ) {
			setup( pointNum_, startColor_, endColor_ );
			reset( transform.position );
		}
		curPos_ = -1;
	}
	
	// Update is called once per frame
	void Update () {
		var pos = transform.position;
		if ( curPos_ == - 1 ) {
			for ( int i = 0; i < pointNum_; ++i ) {
				points_[ i * 3 ] = pos.x;
				points_[ i * 3 + 1 ] = pos.y;
				points_[ i * 3 + 2 ] = pos.z;
			}
		}
		// 位置更新
		curPos_ = ( curPos_ + 1 ) % pointNum_;
		points_[ curPos_ * 3 ] = pos.x;
		points_[ curPos_ * 3 + 1 ] = pos.y;
		points_[ curPos_ * 3 + 2 ] = pos.z;
	}

	float[] points_;
	Color[] colors_;
	bool bInitialized_ = false;
	bool bActive_ = true;
	Vector3 top_ = Vector3.zero;
	int curPos_ = 0;
}
