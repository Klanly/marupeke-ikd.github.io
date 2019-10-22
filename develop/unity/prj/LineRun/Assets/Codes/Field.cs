using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// フィールドの操作
//  フィールドルーピングを実現する

public class Field : MonoBehaviour
{
	[SerializeField]
	float fieldWidth_ = 256.0f;

	[SerializeField]
	float fieldHeight_ = 256.0f;

	[SerializeField]
	Camera mainCamera_ = null;

	// ループ境界先のオブジェクトを描画するカメラ
	//  0:左上  1:上  2:右上
	//  3:左          4:右
	//  5:左下  6:下 7 :右下
	[SerializeField]
	Camera[] subCameras_ = new Camera[ 8 ];

	public float Left { get { return -fieldWidth_ * 0.5f; } }
	public float Right { get { return fieldWidth_ * 0.5f; } }
	public float Bottom { get { return -fieldHeight_ * 0.5f; } }
	public float Top { get { return fieldHeight_ * 0.5f; } }
	public float Width { get { return fieldWidth_; } }
	public float Height { get { return fieldHeight_; } }

	// 柵を登録
	public void addRailling( Railling railling )
	{
		raillings_.Enqueue( railling );
		if ( raillings_.Count > curMaxRaillingNum_ ) {
			var destRail = raillings_.Dequeue();
			if (destRail != null)
				Destroy( destRail.gameObject );
		}
	}

	private void Awake()
	{
		foreach ( var c in subCameras_ ) {
			c.gameObject.SetActive( false );
		}
	}

	void Start()
    {
		float deg = mainCamera_.fieldOfView / 2.0f;
		float asp = (float)Screen.width / Screen.height;
		float vy = Mathf.Tan( deg * Mathf.Deg2Rad );
		float vx = vy * asp;

		rays_[ 0 ] = ( new Vector4( -vx, -vy, 1.0f ) ).normalized;
		rays_[ 1 ] = ( new Vector4(  vx, -vy, 1.0f ) ).normalized;
		rays_[ 2 ] = ( new Vector4( -vx,  vy, 1.0f ) ).normalized;
		rays_[ 3 ] = ( new Vector4(  vx,  vy, 1.0f ) ).normalized;

		colPoses_[ 0 ] = Vector3.zero;
		colPoses_[ 1 ] = Vector3.zero;
		colPoses_[ 2 ] = Vector3.zero;
		colPoses_[ 3 ] = Vector3.zero;
	}

	private void LateUpdate()
	{
		foreach (var c in subCameras_) {
			c.gameObject.SetActive( false );
			c.transform.rotation = mainCamera_.transform.rotation;
		}

		// メインカメラのXY平面への撮影範囲を監視
		// 境界( ±fieldWidth_、±fieldHeight_ )を跨いでいたら
		// 先のオブジェクトをSubCameraで撮影する
		var invView = mainCamera_.cameraToWorldMatrix;
		invView.m20 *= -1.0f;
		invView.m21 *= -1.0f;
		invView.m22 *= -1.0f;
		invView.m23 *= -1.0f;
		Swaps.swap( ref invView.m12, ref invView.m21 );
		var wr0 = invView * rays_[ 0 ];
		var wr1 = invView * rays_[ 1 ];
		var wr2 = invView * rays_[ 2 ];
		var wr3 = invView * rays_[ 3 ];
		var cameraPos = mainCamera_.transform.position;

		CollideUtil.colPosRayPlane( out colPoses_[ 0 ], cameraPos, wr0, Vector3.zero, Vector3.back );
		CollideUtil.colPosRayPlane( out colPoses_[ 1 ], cameraPos, wr1, Vector3.zero, Vector3.back );
		CollideUtil.colPosRayPlane( out colPoses_[ 2 ], cameraPos, wr2, Vector3.zero, Vector3.back );
		CollideUtil.colPosRayPlane( out colPoses_[ 3 ], cameraPos, wr3, Vector3.zero, Vector3.back );
		wr0 = colPoses_[ 0 ];
		wr1 = colPoses_[ 1 ];
		wr2 = colPoses_[ 2 ];
		wr3 = colPoses_[ 3 ];

		float hw = fieldWidth_ / 2.0f;
		float hh = fieldHeight_ / 2.0f;

		var wrs = new Vector2[ 4 ] {
			wr0, wr1, wr2, wr3
		};
		var actives = new bool[ 8 ] {
			false, false, false, false, false, false, false, false
		};
		var ofsets = new Vector3[ 8 ] {
			new Vector3(  1.0f, -1.0f, 0.0f ),
			new Vector3(  0.0f, -1.0f, 0.0f ),
			new Vector3( -1.0f, -1.0f, 0.0f ),
			new Vector3(  1.0f,  0.0f, 0.0f ),
			new Vector3( -1.0f,  0.0f, 0.0f ),
			new Vector3(  1.0f,  1.0f, 0.0f ),
			new Vector3(  0.0f,  1.0f, 0.0f ),
			new Vector3( -1.0f,  1.0f, 0.0f ),
		};
		checkViewCollision( wrs, new Vector2( -hw, -hh ), ref actives, 3, 5, 6 );
		checkViewCollision( wrs, new Vector2(  hw, -hh ), ref actives, 4, 6, 7 );
		checkViewCollision( wrs, new Vector2( -hw,  hh ), ref actives, 0, 1, 3 );
		checkViewCollision( wrs, new Vector2(  hw,  hh ), ref actives, 1, 2, 4 );
		for ( int i = 0; i < 8; ++i ) {
			if ( actives[ i ] == true ) {
				subCameras_[ i ].transform.position = cameraPos + new Vector3( fieldWidth_ * ofsets[ i ].x, fieldHeight_ * ofsets[ i ].y, 0.0f );
				subCameras_[ i ].gameObject.SetActive( true );
			}
		}
	}

	void checkViewCollision( Vector2[] wrs, Vector2 p, ref bool[] actives, int i0, int i1, int i2 ) {
		actives[ i0 ] = true;
		actives[ i1 ] = true;
		actives[ i2 ] = true;
	}

	void Update()
    {
		// 柵の長さを更新
		t_ += Time.deltaTime;
		float N = 10;
		float RN1 = 25;
		float RN2 = 55;
		float b = Mathf.Log( ( RN1 - N ) / ( RN2 - N ) ) / Mathf.Log( 60.0f / 120.0f );
		float a = ( RN1 - N ) / ( Mathf.Pow( 60.0f, b ) );
		curMaxRaillingNum_ = ( int )( a * Mathf.Pow( t_, b ) + N );
	}

	Vector4[] rays_ = new Vector4[ 4 ];
	Vector3[] colPoses_ = new Vector3[ 4 ];
	Queue<Railling> raillings_ = new Queue<Railling>();
	[SerializeField]
	int curMaxRaillingNum_ = 10;
	float t_ = 0.0f;
}
