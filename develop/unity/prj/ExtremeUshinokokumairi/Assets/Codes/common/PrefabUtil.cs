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
    static public T createInstance<T>( T prefab, Transform parent = null, Vector3? refPosition = null, Quaternion? refRotation = null ) where T : Object {
        var obj = GameObject.Instantiate<T>( prefab );
        var o = obj as MonoBehaviour;
        if ( o != null ) {
            o.transform.SetParent( parent );
            if ( refPosition != null ) {
                o.transform.localPosition = refPosition ?? Vector3.zero;
            }
            if ( refRotation != null ) {
                o.transform.localRotation = refRotation ?? Quaternion.identity;
            }
            return obj;
        }

        var o2 = obj as GameObject;
        if ( o2 != null ) {
            o2.transform.SetParent( parent );
            if ( refPosition != null ) {
                o2.transform.localPosition = refPosition ?? Vector3.zero;
            }
            if ( refRotation != null ) {
                o.transform.localRotation = refRotation ?? Quaternion.identity;
            }
            return obj;
        }

        var o3 = obj as Component;
        if ( o3 != null ) {
            o3.transform.SetParent( parent );
        }
        if ( refPosition != null ) {
            o3.transform.localPosition = refPosition ?? Vector3.zero;
        }
        if ( refRotation != null ) {
            o.transform.localRotation = refRotation ?? Quaternion.identity;
        }
        return obj;
    }

    static Vector3 defaultRefPos_g = new Vector3( 0.0f, 0.0f, 0.0f );
}
