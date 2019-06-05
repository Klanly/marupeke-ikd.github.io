using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレハブ周りユーティリティ

public class PrefabUtil {
    // プレハブをロード
    static public T load< T >( string prefabName ) where T : MonoBehaviour {
#if UNITY_EDITOR
        // ロードに失敗した場合はエラーログを吐く
        var prefab = ResourceLoader.getInstance().loadSync<T>( prefabName );
        if ( prefab == null )
            Debug.LogError( "Failed to load prefab. [" + prefabName + "]" );
        return prefab;
#else
        return ResourceLoader.getInstance().loadSync< T >( prefabName );
#endif
    }

    // プレハブをロードしてインスタンシング（親付き）
    static public T createInstance< T >( string prefabName, Transform parent = null ) where T : MonoBehaviour {
        var prefab = ResourceLoader.getInstance().loadSync<T>( prefabName );
        var obj = GameObject.Instantiate<T>( prefab );
        obj.transform.SetParent( parent );
        return obj;
    }

    // プレハブをインスタンシングして親と関連付け
    static public T createInstance<T>( T prefab, Transform parent = null) where T : Object {
        var obj = GameObject.Instantiate<T>( prefab );
        var o = obj as MonoBehaviour;
        if ( o != null )
            o.transform.SetParent( parent );
        else {
            var o2 = obj as GameObject;
            if ( o2 != null )
                o2.transform.SetParent( parent );
        }
        return obj;
    }
}
