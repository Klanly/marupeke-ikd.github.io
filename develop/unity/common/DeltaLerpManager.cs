using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeltaLerpManager : MonoBehaviour {
	void Update() {
		DeltaLerpUpdater.getInstance().update();
	}
}

public class DeltaLerpUpdater {
	private DeltaLerpUpdater() {
		var obj = new GameObject( "DeltaLerpUpdater" );
		obj.AddComponent< DeltaLerpManager >();
	}

	static public DeltaLerpUpdater getInstance() {
		return uppdater_g;
	}

	public void add( DeltaLerp.Result lerpObj ) {
		addList_.Add( lerpObj );
	}

	public void update() {
		if ( addList_.Count > 0 ) {
			foreach ( var obj in addList_ ) {
				updateList_.AddLast( obj );
			}
			addList_.Clear();
		}
		for ( var it = updateList_.First; it != null; ) {
			var obj = it.Value;
			if ( obj.update() == false ) {
				// リンク削除
				var removeIt = it;
				it = it.Next;
				updateList_.Remove( removeIt );
			} else {
				it = it.Next;
			}
		}
	}

	static DeltaLerpUpdater uppdater_g = new DeltaLerpUpdater();
	List< DeltaLerp.Result > addList_ = new List<DeltaLerp.Result>();
	LinkedList< DeltaLerp.Result > updateList_ = new LinkedList<DeltaLerp.Result>();
}

public class DeltaLerp {

	public class Result {
		virtual public bool update() {
			return false;
		}
	}
	public class Float : Result {
		protected Float( System.Func< bool > innerUpdate ) {
			innerUpdate_ = innerUpdate;
		}

		// 経過時刻更新と補間計算のテンプレート
		static bool updateTime<T>( ref float preSec, T len, float sec, System.Func<float, float, float, T, bool> deltaCallback, System.Action finishCallback, System.Func< float, float, T > calcDelta ) {
				float dt = Time.deltaTime;
				float t = 0.0f;
				bool bFinish = false;
				float curSec = preSec;
				if ( curSec + dt >= sec ) {
					dt = sec - curSec;
					curSec = sec;
					t = 1.0f;
					bFinish = true;
				} else {
					curSec += dt;
					t = curSec / sec;
				}
				bool res = deltaCallback( curSec, t, dt, calcDelta( preSec, dt ) );
				preSec = curSec;
				if ( res == false || bFinish == true ) {
					if ( finishCallback != null )
					finishCallback();
					return false;
				}
				return true;	// 継続
		}

		// 線形補間
		//  len: 補間の長さ
		//  sec: 補間時間（秒）
		//  deltaCallback< sec, t, dt, delta >
		//   sec  : 経過秒
		//   t    : 経過補間係数（0～1）
		//   dt   : 前回からの差分時間
		//   delta: 差分値
		static public Result linear( float len, float sec, System.Func<float, float, float, float, bool> deltaCallback, System.Action finishCallback = null ) {
			if ( sec == 0.0f ) {
				deltaCallback( 0.0f, 1.0f, 0.0f, len );
				return null;
			}
			float preSec = 0.0f;
			var res = new Float( () => {
				return updateTime( ref preSec, len, sec, deltaCallback, finishCallback, (_, _dt) => {
					return len / sec * _dt;
				} );
			} );
			DeltaLerpUpdater.getInstance().add( res );
			return res;
		}

		// EaseIn
		//  len: 補間の長さ
		//  sec: 補間時間（秒）
		//  deltaCallback< sec, t, dt, delta >
		//   sec  : 経過秒
		//   t    : 経過補間係数（0～1）
		//   dt   : 前回からの差分時間
		//   delta: 差分値
		static public Result easeIn(float len, float sec, System.Func<float, float, float, float, bool> deltaCallback, System.Action finishCallback = null) {
			float preSec = 0.0f;
			var res = new Float( () => {
				return updateTime( ref preSec, len, sec, deltaCallback, finishCallback, (_preSec, _dt) => {
					return len / ( sec * sec ) * _dt * ( 2.0f * _preSec + _dt );
				} );
			} );
			DeltaLerpUpdater.getInstance().add( res );
			return res;
		}

		// EaseOut
		//  len: 補間の長さ
		//  sec: 補間時間（秒）
		//  deltaCallback< sec, t, dt, delta >
		//   sec  : 経過秒
		//   t    : 経過補間係数（0～1）
		//   dt   : 前回からの差分時間
		//   delta: 差分値
		static public Result easeOut(float len, float sec, System.Func<float, float, float, float, bool> deltaCallback, System.Action finishCallback = null) {
			float preSec = 0.0f;
			var res = new Float( () => {
				return updateTime( ref preSec, len, sec, deltaCallback, finishCallback, (_preSec, _dt) => {
					return len / sec * _dt * ( 2.0f - ( 2.0f * _preSec + _dt ) / sec );
				} );
			} );
			DeltaLerpUpdater.getInstance().add( res );
			return res;
		}

		// EaseInOut
		//  len: 補間の長さ
		//  sec: 補間時間（秒）
		//  deltaCallback< sec, t, dt, delta >
		//   sec  : 経過秒
		//   t    : 経過補間係数（0～1）
		//   dt   : 前回からの差分時間
		//   delta: 差分値
		static public Result easeInOut(float len, float sec, System.Func<float, float, float, float, bool> deltaCallback, System.Action finishCallback = null) {
			float preSec = 0.0f;
			float a = len / ( sec * sec );
			var res = new Float( () => {
				return updateTime( ref preSec, len, sec, deltaCallback, finishCallback, (_preSec, _dt) => {
					float s = _preSec + _dt;
					return a * ( s * s * ( 3.0f - 2.0f * s / sec ) - _preSec * _preSec * ( 3.0f - 2.0f * _preSec / sec ) );
				} );
			} );
			DeltaLerpUpdater.getInstance().add( res );
			return res;
		}

		// EaseInExp（指数関数でのEaseIn）
		//  len: 補間の長さ
		//  sec: 補間時間（秒）
		//  deltaCallback< sec, t, dt, delta >
		//   sec  : 経過秒
		//   t    : 経過補間係数（0～1）
		//   dt   : 前回からの差分時間
		//   delta: 差分値
		static public Result easeInExp(float len, float sec, System.Func<float, float, float, float, bool> deltaCallback, System.Action finishCallback = null) {
			float preSec = 0.0f;
			Result res = null;
			if ( len >= 0.0f ) {
				res = new Float( () => {
					return updateTime( ref preSec, len, sec, deltaCallback, finishCallback, (_preSec, _dt) => {
						return Mathf.Pow( 1.0f + len, _preSec / sec ) * ( Mathf.Pow( 1.0f + len, _dt / sec ) - 1.0f );
					} );
				} );
			} else {
				res = new Float( () => {
					return updateTime( ref preSec, len, sec, deltaCallback, finishCallback, (_preSec, _dt) => {
						return Mathf.Pow( 1.0f - len, _preSec / sec ) * ( 1.0f - Mathf.Pow( 1.0f - len, _dt / sec ) );
					} );
				} );
			}
			DeltaLerpUpdater.getInstance().add( res );
			return res;
		}

		// EaseOutExp（指数関数でのEaseIn）
		//  len: 補間の長さ
		//  sec: 補間時間（秒）
		//  deltaCallback< sec, t, dt, delta >
		//   sec  : 経過秒
		//   t    : 経過補間係数（0～1）
		//   dt   : 前回からの差分時間
		//   delta: 差分値
		static public Result easeOutExp(float len, float sec, System.Func<float, float, float, float, bool> deltaCallback, System.Action finishCallback = null) {
			float preSec = 0.0f;
			Result res = null;
			if ( len >= 0.0f ) {
				res = new Float( () => {
					float a = ( Mathf.Exp( len ) - 1 );
					return updateTime( ref preSec, len, sec, deltaCallback, finishCallback, (_preSec, _dt) => {
						return Mathf.Log( ( sec + ( _preSec + _dt ) * a ) / ( sec + _preSec * a ) );
					} );
				} );
			} else {
				res = new Float( () => {
					float a = ( Mathf.Exp( -len ) - 1 );
					return updateTime( ref preSec, len, sec, deltaCallback, finishCallback, (_preSec, _dt) => {
						return Mathf.Log( ( sec + _preSec * a ) / ( sec + ( _preSec + _dt ) * a ) );
					} );
				} );
			}
			DeltaLerpUpdater.getInstance().add( res );
			return res;
		}

		// EaseOutOver（lenをはみ出すEaseOut）
		//  len: 補間の長さ
		//  sec: 補間時間（秒）
		//  m  : はみ出し強度(0-1)
		//  deltaCallback< sec, t, dt, delta >
		//   sec  : 経過秒
		//   t    : 経過補間係数（0～1）
		//   dt   : 前回からの差分時間
		//   delta: 差分値
		static public Result easeOutOver(float len, float sec, float m, System.Func<float, float, float, float, bool> deltaCallback, System.Action finishCallback = null) {
			float preSec = 0.0f;
			float m2 = m * 0.499f + 0.501f;
			float a = len / ( sec * ( 1.0f - 2.0f * m2 ) );
			float it = 1.0f / sec;
			var res = new Float( () => {
				return updateTime( ref preSec, len, sec, deltaCallback, finishCallback, (_preSec, _dt) => {
					return a * ( it * ( _preSec + _dt ) * ( _preSec + _dt ) - it * _preSec * _preSec - 2.0f * m2 * _dt );
				} );
			} );
			DeltaLerpUpdater.getInstance().add( res );
			return res;
		}

		// EaseInOver（lenをはみ出すEaseIn）
		//  len: 補間の長さ
		//  sec: 補間時間（秒）
		//  m  : はみ出し強度(0-1)
		//  deltaCallback< sec, t, dt, delta >
		//   sec  : 経過秒
		//   t    : 経過補間係数（0～1）
		//   dt   : 前回からの差分時間
		//   delta: 差分値
		static public Result easeInOver(float len, float sec, float m, System.Func<float, float, float, float, bool> deltaCallback, System.Action finishCallback = null) {
			float preSec = 0.0f;
			float m2 = m * 0.333f + 0.667f;
			float a = len / ( sec * sec * ( 2.0f - 3.0f * m2 ) );
			var res = new Float( () => {
				return updateTime( ref preSec, len, sec, deltaCallback, finishCallback, (_preSec, _dt) => {
					float s = _preSec + _dt;
					return a * ( 2.0f / sec * ( s * s * s - _preSec * _preSec * _preSec ) - 3.0f * m2 * ( s * s - _preSec * _preSec ) );
				} );
			} );
			DeltaLerpUpdater.getInstance().add( res );
			return res;
		}

		// EaseInOutOver（lenをはみ出すEaseInOut）
		//  len: 補間の長さ
		//  sec: 補間時間（秒）
		//  m  : はみ出し強度(0-1)
		//  deltaCallback< sec, t, dt, delta >
		//   sec  : 経過秒
		//   t    : 経過補間係数（0～1）
		//   dt   : 前回からの差分時間
		//   delta: 差分値
		static public Result easeInOutOver(float len, float sec, float m, System.Func<float, float, float, float, bool> deltaCallback, System.Action finishCallback = null) {
			float preSec = 0.0f;
			float m2 = m * 0.2f + 0.8f;
			float p = len / ( sec * ( 6.0f * m2 * m2 - 6.0f * m2 + 1.0f ) );
			float a = -2.0f / ( sec * sec );
			float b = 3.0f / sec;
			float c = 6.0f * m2 * ( m2 - 1 );
			var res = new Float( () => {
				return updateTime( ref preSec, len, sec, deltaCallback, finishCallback, (_preSec, _dt) => {
					float s = _preSec;
					float sd = _preSec + _dt;
					return p * ( a * ( sd * sd * sd - s * s * s ) + b * ( sd * sd - s * s) + c * ( sd - s ) );
				} );
			} );
			DeltaLerpUpdater.getInstance().add( res );
			return res;
		}

		override public bool update() {
			return innerUpdate_();
		}

		System.Func< bool > innerUpdate_;
	}


	public class Long : Result {
//		protected Long(System.Func<bool> innerUpdate) {
//			innerUpdate_ = innerUpdate;
//		}

		// 経過時刻更新と補間計算のテンプレート
		static bool updateTime(ref float preSec, float sec, System.Func<float, float, float, long, bool> deltaCallback, System.Action finishCallback, System.Func<float, float, long> calcDelta) {
			float dt = Time.deltaTime;
			float t = 0.0f;
			bool bFinish = false;
			float curSec = preSec;
			if ( curSec + dt >= sec ) {
				dt = sec - curSec;
				curSec = sec;
				t = 1.0f;
				bFinish = true;
			} else {
				curSec += dt;
				t = curSec / sec;
			}
			bool res = deltaCallback( curSec, t, dt, calcDelta( preSec, dt ) );
			preSec = curSec;
			if ( res == false || bFinish == true ) {
				if ( finishCallback != null )
					finishCallback();
				return false;
			}
			return true;    // 継続
		}

		// 線形補間
		//  len: 補間の長さ
		//  sec: 補間時間（秒）
		//  deltaCallback< sec, t, dt, delta >
		//   sec  : 経過秒
		//   t    : 経過補間係数（0～1）
		//   dt   : 前回からの差分時間
		//   delta: 差分値
		static public Result linear(long len, float sec, System.Func<float, float, float, long, bool> deltaCallback, System.Action finishCallback = null) {
			float area = 0.0f;
			float dt = 0.0f;
			long curLen = 0;
			return Float.linear( 1.0f, sec, (_sec, _t, _dt, _delta) => {
				area += _delta;
				dt += _dt;
				if ( _sec == sec ) {
					// ラスト更新
					deltaCallback( _sec, 1.0f, dt, len - curLen );
					return false;
				}
				long def = ( long )( len * area );
				if ( def != 0 ) {
					curLen += def;
					deltaCallback( _sec, _t, dt, def );
					area = 0.0f;
					dt = 0.0f;
				}
				return true;
			},
			finishCallback
			);
		}

//		System.Func<bool> innerUpdate_;
	}


	public class Clr : Result {
		protected Clr(System.Func<bool> innerUpdate) {
			innerUpdate_ = innerUpdate;
		}

		// 経過時刻更新と補間計算のテンプレート
		static bool updateTime(ref float preSec, float sec, System.Func<float, float, float, Color, bool> deltaCallback, System.Action finishCallback, System.Func<float, float, Color> calcDelta) {
			float dt = Time.deltaTime;
			float t = 0.0f;
			bool bFinish = false;
			float curSec = preSec;
			if ( curSec + dt >= sec ) {
				dt = sec - curSec;
				curSec = sec;
				t = 1.0f;
				bFinish = true;
			} else {
				curSec += dt;
				t = curSec / sec;
			}
			bool res = deltaCallback( curSec, t, dt, calcDelta( preSec, dt ) );
			preSec = curSec;
			if ( res == false || bFinish == true ) {
				if ( finishCallback != null )
					finishCallback();
				return false;
			}
			return true;    // 継続
		}

		// 線形補間
		//  len: 補間の長さ
		//  sec: 補間時間（秒）
		//  deltaCallback< sec, t, dt, delta >
		//   sec  : 経過秒
		//   t    : 経過補間係数（0～1）
		//   dt   : 前回からの差分時間
		//   delta: 差分値
		static public Result linear(Color len, float sec, System.Func<float, float, float, Color, bool> deltaCallback, System.Action finishCallback = null) {
			if ( sec <= 0.0f ) {
				deltaCallback( 0.0f, 1.0f, 0.0f, len );
				return null;
			}
			float preSec = 0.0f;
			var res = new Clr( () => {
				return updateTime( ref preSec, sec, deltaCallback, finishCallback, (_, _dt) => {
					return len / sec * _dt;
				} );
			} );
			DeltaLerpUpdater.getInstance().add( res );
			return res;
		}

		override public bool update() {
			return innerUpdate_();
		}

		System.Func<bool> innerUpdate_;
	}
}
