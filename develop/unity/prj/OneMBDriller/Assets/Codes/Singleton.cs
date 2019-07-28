using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// シングルトン
public class Singleton< T > where T : new()
{
    static public void set( T globalObject ) {
        instance_ = globalObject;
    }

    static public T getInstance() {
        if ( instance_ == null ) {
            instance_ = new T();
        }
        return instance_;
    }

    static T instance_;
}
