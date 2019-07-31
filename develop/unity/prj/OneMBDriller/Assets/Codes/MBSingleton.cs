using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// シングルトン
public abstract class MBSingleton< T > : MonoBehaviour where T : MonoBehaviour
{
    static public T getInstance() {
        if ( instance_ == null ) {
			// ヒエラルキに存在しているオブジェクトをセット
			var t = typeof( T );
			instance_ = (T)FindObjectOfType( t );
			if ( instance_ == null ) {
				Debug.LogError( "Not exist singleton object <" + t.ToString() + "> in hierarchy." );
				return null;
			}
        }
        return instance_;
    }

	private void Awake() {
		// 単一性を確保
		if ( instance_ == null ) {
			instance_ = this as T;
		} else if ( instance_ != this ) {
			Destroy( this );
		}
	}

	static T instance_;
}
