using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 移動する値(Color型)
//
//  aimに向かうように値が動的に移動していきます
//  値移動中にsetAimメソッドに値を連続で設定すると移動量が合成されますが、最終的にはエイム値に辿り着きます。

public class MoveValueColor {

	public System.Action<Color> Value {
		set { valueCallback_ = value; }
	}

	public MoveValueColor(Color initVal, float sec) {
		aimValue_ = initVal;
		curVal_ = initVal;
		setSec( sec );
	}

	// エイム値を更新
	public void setAim(Color aim, float sec = -1.0f) {
		if ( aimValue_ == aim )
			return;
		if ( sec >= 0.0f )
			sec_ = sec;

		// 差分だけ動くDeltaLerpを追加
		DeltaLerp.Clr.linear( aim - aimValue_, sec_, (_sec, _t, _td, _delta) => {
			curVal_ += _delta;
			if ( valueCallback_ != null )
				valueCallback_( curVal_ );
			return true;
		} );

		aimValue_ = aim;
	}

	// エイム値を取得
	public Color getAim() {
		return aimValue_;
	}

	// 現在の値を取得
	public Color getCurVal() {
		return curVal_;
	}

	// 遷移時間を変更
	public void setSec(float sec) {
		if ( sec < 0.0f )
			sec = 0.0f;
		sec_ = sec;
	}

	Color aimValue_ = Color.white;
	float sec_ = 0.0f;
	Color curVal_ = Color.white;
	System.Action<Color> valueCallback_;
}
