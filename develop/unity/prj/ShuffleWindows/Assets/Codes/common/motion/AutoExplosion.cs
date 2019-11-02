using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 爆破点を元に自分自身を吹っ飛ばす
public class AutoExplosion : MonoBehaviour
{
	public enum DestroyFlag {
		GameObject,
		Component,
		Unit,
		Continue
	}

	class Unit {
		public void setup(Vector3 g, Vector3 corePos, float corePower, float sonicVector, float decRate, System.Func<float, DestroyFlag> destroyCondition) {
			g_ = g / 60.0f;     // 1フレーム重力加速度に変更
			decRate_ = decRate;
			corePos_ = corePos;
			corePower_ = corePower;
			sonicVector_ = sonicVector;
			destroyCondition_ = destroyCondition;
			t_ = 0.0f;
			state_ = sonicCheck;
		}

		public bool update( AutoExplosion parent )
		{
			if (state_ == null)
				return true;
			if (parent == null)
				return true;
			var preState = state_;
			bool res = state_( parent );
			if ( preState != state_ ) {
				return update( parent );
			}
			return res;
		}

		// 衝撃波が伝わるまで
		bool sonicCheck( AutoExplosion parent )
		{
			t_ += Time.deltaTime;
			var p = parent.transform.position;
			float d = ( corePos_ - p ).magnitude;
			if ( sonicVector_ * t_ >= d ) {
				// 発動
				state_ = move;
				v_ = ( p - corePos_ ).normalized * ( corePower_ * Mathf.Pow( decRate_, d ) );
				t_ = 0.0f;
			}
			return false;
		}

		// 発動
		bool move( AutoExplosion parent )
		{
			v_ += g_;
			parent.transform.position += v_ / 60.0f;
			t_ += Time.deltaTime;
			if ( destroyCondition_ != null ) {
				var flag = destroyCondition_( t_ );
				if ( flag == DestroyFlag.GameObject ) {
					GameObject.Destroy( parent.gameObject );
					state_ = null;
					return true;
				} else if ( flag == DestroyFlag.Component ) {
					GameObject.Destroy( parent );
					state_ = null;
					return true;
				} else if (flag == DestroyFlag.Unit ) {
					state_ = null;
					return true;
				}
			}
			return false;
		}

		Vector3 g_ = Vector3.zero;
		Vector3 corePos_ = Vector3.zero;
		float corePower_ = 0.0f;
		float sonicVector_ = 0.0f;
		float decRate_ = 0.90f;
		float t_ = 0.0f;
		System.Func<float, DestroyFlag> destroyCondition_ = null;
		System.Func< AutoExplosion, bool > state_;
		Vector3 v_ = Vector3.zero;	// 現在の速度（m/sec）
	}

	// 設定
	//  呼び出すと自動的にONになる
	//  g          : 重力加速度（kgm/sec^2）
	//  corePos    : 爆発中心点
	//  corePower  : 中心点の爆発力（kgm/sec^2）
	//  sonicVector: 衝撃波が伝わる速さ（m/sec）。中心点から自分の位置へ衝撃波が伝わった時に初めて発動する。0だと発動しない。
	//  decRate    : 爆発力の減衰率（1m当たりcorePowerの減少度）
	//  destroyCondition: 削除条件デリゲータ。引数は現在の経過時間。
	//                     DestroyFlag.GameObjectを返すとlimitSecに関わらずオブジェクトを削除。
	//                     DestroyFlag.Componentを返すとこのコンポーネントを削除。
	//                     DestroyFlag.Continueを返すと継続。
	public void setup( Vector3 g, Vector3 corePos, float corePower, float sonicVector, float decRate, System.Func< float, DestroyFlag > destroyCondition ) {
		var unit = new Unit();
		unit.setup( g, corePos, corePower, sonicVector, decRate, destroyCondition );
		units_.AddLast( unit );
		enabled = true;
	}

	private void Awake()
	{
		enabled = false;	// アタッチ時は発動しない
	}

	private void Update()
	{
		var it = units_.First;
		while ( it != null ) {
			if ( it.Value.update( this ) == true ) {
				var dit = it;
				it = it.Next;
				units_.Remove( dit );
				continue;
			}
			it = it.Next;
		}
	}

	LinkedList<Unit> units_ = new LinkedList<Unit>();
}
