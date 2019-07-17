using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// マルチラインTextMesh
//  複数行分のTextMeshを管理
public class MultiMeshText : MonoBehaviour
{
	[SerializeField]
	TextMesh textMeshPrefab_;	// テキストメッシュのプレハブ

	[SerializeField]
	List<string> strList_;  // 文字列リスト

    [SerializeField]
    float height_ = 0.0f;

	[SerializeField]
	bool debugUpdateAll_ = false;

    public void clear() {
        strList_.Clear();
    }

    public Vector2 getRegion() {
        return region_;
    }

    public void setHeight( float height ) {
        height_ = height;
    }

    public void setStr( int idx, string str ) {
        if ( idx >= strList_.Count ) {
            for ( int i = strList_.Count; i < idx + 1; ++i )
                strList_.Add( "" );
        }
        strList_[ idx ] = str;
    }

	public void updateAll() {
        int n = textMeshes_.Count;
        if ( strList_.Count > n ) {
            for ( int i = 0; i < strList_.Count - n; ++i ) {
                var obj = PrefabUtil.createInstance<TextMesh>( textMeshPrefab_, transform );
                textMeshes_.Add( obj );
			}
		} else if ( strList_.Count < textMeshes_.Count ) {
			for ( int i = strList_.Count; i < n; ++i ) {
                var removeObj = textMeshes_[ textMeshes_.Count - 1 ];
                Destroy( removeObj.gameObject );
                textMeshes_.RemoveAt( textMeshes_.Count - 1 );
			}
		}
        // TextMeshと文字列の数を合わせたので文字を更新
        if ( strList_.Count == 0 ) {
            region_ = Vector2.zero;
        } else {
            var min = new Vector2( float.MaxValue, float.MaxValue );
            var max = new Vector2( float.MinValue, float.MinValue );
            for ( int i = 0; i < strList_.Count; ++i ) {
                textMeshes_[ i ].text = strList_[ i ];
                textMeshes_[ i ].transform.localPosition = new Vector3( 0.0f, -i * height_, 0.0f );
                var renderer = textMeshes_[ i ].GetComponent<Renderer>();
                var b = renderer.bounds;
                if ( min.x > b.min.x )
                    min.x = b.min.x;
                if ( min.y > b.min.y )
                    min.y = b.min.y;
                if ( max.x < b.max.x )
                    max.x = b.max.x;
                if ( max.y < b.max.y )
                    max.y = b.max.y;
            }
            region_ = max - min;
        }
    } 

    void Start()
    {
		updateAll();
	}

	void Update()
    {
        if (debugUpdateAll_ == true ) {
			debugUpdateAll_ = false;
			updateAll();
		}
    }

	List<TextMesh> textMeshes_ = new List<TextMesh>();

    [SerializeField]
    Vector2 region_ = Vector2.zero;
}
