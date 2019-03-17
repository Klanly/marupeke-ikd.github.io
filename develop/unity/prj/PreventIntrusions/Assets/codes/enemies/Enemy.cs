using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵

public class Enemy : MonoBehaviour {

	public class Param {
	}

	public Vector2Int Pos { get { return pos_; } }

	// セットアップ
	public void setup( Field field, Param param, Vector2Int initPos ) {
		field_ = field;
		param_ = param;
		pos_ = initPos;

		// 主座標更新
		transform.localPosition = new Vector3( pos_.x + 0.5f, 0.0f, pos_.y + 0.5f );
	}

	// 位置をセット
	public void setPos( Vector2Int pos ) {
		var prePos = pos_;
		pos_ = pos;

		// フィールドに報告
		field_.updateEnemyPos( this, prePos, pos_ );
	}

	// 囲まれているかチェック
	public bool checkStockade() {
		return false;
	}

	// 囲まれてやられた
	public void toDestroy() {
		Destroy( gameObject );
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	protected Field field_;
	protected Param param_;
	Vector2Int pos_;	// 現在位置
}
