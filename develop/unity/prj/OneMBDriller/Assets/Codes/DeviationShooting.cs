using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 偏差射撃
public class DeviationShooting
{
	// 等速直線運動偏差射撃での打ち手の方角を計算
	static public bool calcDirction( Vector3 targetPos, Vector3 targetVelo, Vector3 shooterPos, float bulletVelo, out Vector3 collidePos, out Vector3 shootDir, out float reatchTime ) {
		var resDir = Vector3.zero;
		var resColPos = Vector3.zero;
		if ( targetPos == shooterPos ) {
			// 位置が重なっている場合はどの方向でも当たる
			resDir.x = 1.0f;
			shootDir = resDir;
			reatchTime = 0.0f;
			collidePos = resColPos;
			return true;
		}
		if ( bulletVelo <= 0.0f ) {
			// 撃ち手の弾が動いていなければ当てようがない
			shootDir = resDir;
			reatchTime = 0.0f;
			collidePos = resColPos;
			return false;
		}

		targetPos -= shooterPos;    // shooterPosを原点とした相対値に変更
		float A = targetVelo.sqrMagnitude - bulletVelo * bulletVelo;
		float B = Vector3.Dot( targetPos, targetVelo );
		float C = targetPos.sqrMagnitude;
		float D = B * B - A * C;
		if ( D < 0.0f ) {
			// どの方向へ撃っても当たらない
			shootDir = resDir;
			reatchTime = 0.0f;
			collidePos = resColPos;
			return false;
		}

		// 弾の速度が双方同じ場合の処理
		if ( A == 0.0f ) {
			if ( B == 0.0f ) {
				// 撃ち手から見て真横に移動しているため当てられない
				shootDir = resDir;
				reatchTime = 0.0f;
				collidePos = resColPos;
				return false;
			}
			reatchTime = -C / B * 0.5f;
			if ( reatchTime < 0.0f ) {
				// 届かない
				shootDir = resDir;
				reatchTime = 0.0f;
				collidePos = resColPos;
				return false;
			}
		} else {
			float tp = ( -B + Mathf.Sqrt( D ) ) / A;
			float tm = ( -B - Mathf.Sqrt( D ) ) / A;
			if ( tp < 0.0f && tm < 0.0f ) {
				// 届かない
				shootDir = resDir;
				reatchTime = 0.0f;
				collidePos = resColPos;
				return false;
			}
			reatchTime = Mathf.Max( tp, tm );
		}

		// 衝突位置・撃ち出し方向算出
		resColPos = targetPos + reatchTime * targetVelo + shooterPos;   // 元の座標系で
		resDir = ( resColPos - shooterPos ).normalized;
		shootDir = resDir;
		collidePos = resColPos;

		return true;
	}
}
