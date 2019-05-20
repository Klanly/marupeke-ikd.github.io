﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GameObject関連ユーティリティ
public class GameObjectUtil {

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
}
