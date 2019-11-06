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

	[SerializeField]
	int initCorrectNum_ = 8;

	[SerializeField]
	GameObject room_ = null;

	[SerializeField]
	CubeMapRenderer cubeMapRenderer_;

	[SerializeField]
	UnityEngine.UI.Image congImage_ = null;


	public bool isClear()
	{
		return bClear_;
	}

	public bool checkClear()
	{
		foreach ( var w in windows_) {
			if ( w.isCorrect() == false )
				return false;
		}
		bClear_ = true;
		return true;
	}

	public void toClear()
	{
		if ( alwaysClearScene_ == true )
			return;
		alwaysClearScene_ = true;

		// クリアシーン
		Destroy( room_ );

		foreach (var w in windows_) {
			var mv = w.gameObject.AddComponent<Mover>();
			mv.Dir = w.transform.localPosition;
			mv.Speed = 1.0f;
			Destroy( w.gameObject, 10.0f );
		}

		cubeMapRenderer_.transform.position = Vector3.zero;
		cubeMapRenderer_.toClear();

		congImage_.gameObject.SetActive( true );
		var sc = new Color( 1.0f, 1.0f, 1.0f, 0.0f );
		var ec = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
		GlobalState.time( 2.0f, (sec, t) => {
			congImage_.color = Color.Lerp( sc, ec, t );
			return true;
		});
	}

	// Start is called before the first frame update
	void Start() {
		var sampleWindow = PrefabUtil.createInstance( windowPrefab_ );
		float fw = sampleWindow.getFrameWidth() + windowColMergin_;
		float fh = sampleWindow.getFrameHeight();
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
		Destroy( sampleWindow.gameObject );

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

			// 指定個の窓は基準用に正解にする
			ListUtil.shuffle( ref indices );
			for ( int i = 0; i < initCorrectNum_; ++i ) {
				var correctWindow = windows_[ indices[ i ] ];
				foreach ( var w in windows_ ) {
					if ( w.getOtherWindow() == correctWindow ) {
						var tmp = w.getOtherWindow();
						var tmp2 = correctWindow.getOtherWindow();
						correctWindow.setOtherWindow( tmp );
						w.setOtherWindow( tmp2 );
						break;
					}
				}
			}
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	List<Window> windows_ = new List<Window>();
	bool bClear_ = false;
	bool alwaysClearScene_ = false;
}
