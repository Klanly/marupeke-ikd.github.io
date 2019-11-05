using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ウィンドウルーム
//  円柱状の部屋を想定

public class WindowRoom : MonoBehaviour
{
	[SerializeField]
	Window windowPrefab_ = null;

	[SerializeField]
	int windowColNum_ = 10;     // 窓の列数

	[SerializeField]
	float windowColMergin_ = 1.0f;	// 窓の横の隙間

	[SerializeField]
	int windowRowNum_ = 3;		// 窓の行数

	[SerializeField]
	float windowBottom_ = 2.0f; // 最下段の窓の高さ（窓の中央の高さ）

	[SerializeField]
	bool bDebugNoShuffle_ = false;

	// Start is called before the first frame update
	void Start() {
		var w = PrefabUtil.createInstance( windowPrefab_ );
		float fw = w.getFrameWidth() + windowColMergin_;
		float fh = w.getFrameHeight();
		float th = 180.0f / windowColNum_ * Mathf.Deg2Rad;
		float r = fw * 0.5f / Mathf.Tan( th );
		for ( int y = 0; y < windowRowNum_; ++y ) {
			for ( int i = 0; i < windowColNum_; ++i ) {
				var obj = PrefabUtil.createInstance( windowPrefab_, transform );
				obj.transform.localPosition = new Vector3( r * Mathf.Cos( i * 2 * th ), fh * y + windowBottom_, r * Mathf.Sin( i * 2 * th ) );
				var dir = obj.transform.localPosition;
				dir.y = 0;
				obj.transform.forward = dir.normalized;

				windows_.Add( obj );
			}
		}
		Destroy( w.gameObject );

		// 窓をシャッフル
		if (bDebugNoShuffle_ == false) {
			List<int> indices = new List<int>();
			ListUtil.numbering( ref indices, windows_.Count );
			ListUtil.shuffle( ref indices );
			int k = 0;
			foreach (var j in indices) {
				windows_[ k ].setOtherWindow( windows_[ j ] );
				k++;
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	List<Window> windows_ = new List<Window>();
}
