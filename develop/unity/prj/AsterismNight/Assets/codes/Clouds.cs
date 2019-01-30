using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour {

    [SerializeField]
    GameObject[] cloudPrefabs_;

    [SerializeField]
    float scale_ = 1.0f;

    public void create( float radius )
    {
        // 指定の範囲をある程度覆う雲を発生
        var obj = Instantiate<GameObject>( cloudPrefabs_[ Random.Range( 0, cloudPrefabs_.Length ) ] );
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one * radius * scale_;

        renderer_ = obj.GetComponent<SpriteRenderer>();
        initColor_ = renderer_.color;
    }

    public void setAlpha( float alpha )
    {
        if ( alpha < 0.0f ) {
            renderer_.color = initColor_;
            return;
        }

        Color c = renderer_.color;
        c.a = alpha;
        renderer_.color = c;
    }

    public void remove( float sec, bool isDestroy )
    {
        if ( renderer_ == null )
            return;

        Color c = renderer_.color;
        float initAlpha = c.a;
        GlobalState.time( sec, (_sec, _t) => {
            c.a = Lerps.Float.easeInOut( initAlpha, 0.0f, _t );
            renderer_.color = c;
            return true;
        } ).finish( () => {
            if ( isDestroy == true )
                Destroy( gameObject );
        } );
    }

    SpriteRenderer renderer_;
    Color initColor_;
}
