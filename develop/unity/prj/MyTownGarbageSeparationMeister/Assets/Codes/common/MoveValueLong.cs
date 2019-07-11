using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 移動する値(Long型)
//
//  aimに向かうように値が動的に移動していきます
//  値移動中にsetAimメソッドに値を連続で設定すると移動量が合成されますが、最終的にはエイム値に辿り着きます。

public class MoveValueLong {
	
	public System.Action<long> Value {
		set { valueCallback_ = value; }
	}

	public MoveValueLong( long initVal, float sec ) {
		aimValue_ = initVal;
		curVal_ = initVal;
		setSec( sec );
	}

	// エイム値を更新
	public void setAim( long aim ) {
		if ( aimValue_ == aim )
			return;

		// 差分だけ動くDeltaLerpを追加
		DeltaLerp.Long.linear( aim - aimValue_, sec_, ( _sec, _t, _td, _delta ) => {
			curVal_ += _delta;
			if ( valueCallback_ != null )
				valueCallback_( curVal_ );
			return true;
		} );

		aimValue_ = aim;
	}

	// エイム値を取得
	public long getAim() {
		return aimValue_;
	}

	// 現在の値を取得
	public long getCurVal() {
		return curVal_;
	}

	// 遷移時間を変更
	public void setSec( float sec ) {
		if ( sec <= 0.0f )
			sec = Time.deltaTime;
		sec_ = sec;
	}

	long aimValue_ = 0;
	float sec_ = 0.0f;
	long curVal_ = 0;
	System.Action<long> valueCallback_;
}
