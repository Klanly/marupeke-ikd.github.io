using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GameObject関連ユーティリティ
public class GameObjectUtil {

    // 指定コンポーネントを持っている親を取得
    static public T getParentComponent< T >( GameObject gameObject, bool containeMe = true ) where T : MonoBehaviour {
        if ( gameObject == null )
            return null;
        if ( containeMe == true ) {
            var obj = gameObject.GetComponent< T >();
            if ( obj != null )
                return obj;
        }
        if ( gameObject.transform.parent == null )
            return null;
        return getParentComponent< T >( gameObject.transform.parent.gameObject );
    }

    // ヒエラルキー内のオブジェクトを取得
    static public T find< T >( string name, bool ignoreInvisible = false ) where T : MonoBehaviour {
        if ( ignoreInvisible == true ) {
            var obj = GameObject.Find( name );
            if ( obj == null )
                return null;
            return obj.GetComponent<T>();
        }

        // 見えていない物も検索
        var objects = UnityEngine.Resources.FindObjectsOfTypeAll( typeof( GameObject ) );
        foreach ( var o in objects ) {
            if ( o.name == name ) {
                var obj = o as GameObject;
                return obj.GetComponent<T>();
            }
        }

        return null;
    }

    // ヒエラルキーから削除
    static public void remove( string[] names ) {
        var objects = UnityEngine.Resources.FindObjectsOfTypeAll( typeof( GameObject ) );
        foreach ( var o in objects ) {
            foreach ( var n in names ) {
                if ( o.name == n ) {
                    Object.Destroy( o );
                }
            }
        }
    }

    // 指定の親の下にある特定コンポーネントを持った子オブジェクトを取得
    static public void getChildrenComponents<T>(GameObject parent, ref List<T> outList, bool isRecursive = true ) where T : Component {
        if ( parent == null )
            return;
        for ( int i = 0; i < parent.transform.childCount; ++i ) {
            var children = parent.transform.GetChild( i );
            var obj = children.GetComponent< T >();
            if ( obj != null )
                outList.Add( obj );
            if ( isRecursive == true )
                getChildrenComponents<T>( children.gameObject, ref outList, isRecursive );
        }
    }
}
