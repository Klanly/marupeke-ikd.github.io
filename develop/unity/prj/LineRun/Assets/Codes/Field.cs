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

	[SerializeField]
	Camera[] subCameras_ = new Camera[ 5 ]; // ループ境界先のオブジェクトを描画するカメラ 0: 左境界、1:上下境界、2:斜め左境界、3:右境界、4:斜め右境界

	public float Left { get { return -fieldWidth_ * 0.5f; } }
	public float Right { get { return fieldWidth_ * 0.5f; } }
	public float Bottom { get { return -fieldHeight_ * 0.5f; } }
	public float Top { get { return fieldHeight_ * 0.5f; } }
	public float Width { get { return fieldWidth_; } }
	public float Height { get { return fieldHeight_; } }

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
		bool bLR = false;
		if (wr0.x < -hw || wr2.x < -hw) {
			// 左側はみ出し
			subCameras_[ 0 ].transform.position = cameraPos + new Vector3( fieldWidth_, 0.0f, 0.0f );
			subCameras_[ 0 ].gameObject.SetActive( true );
			if (wr2.y > hh || wr3.y > hh) {
				// 左上はみ出し
				subCameras_[ 1 ].transform.position = cameraPos + new Vector3( 0.0f, -fieldHeight_, 0.0f );
				subCameras_[ 1 ].gameObject.SetActive( true );
				subCameras_[ 2 ].transform.position = cameraPos + new Vector3( fieldWidth_, -fieldHeight_, 0.0f );
				subCameras_[ 2 ].gameObject.SetActive( true );
			} else if (wr0.y < -hh || wr1.y < -hh) {
				// 左下はみ出し
				subCameras_[ 1 ].transform.position = cameraPos + new Vector3( 0.0f, fieldHeight_, 0.0f );
				subCameras_[ 1 ].gameObject.SetActive( true );
				subCameras_[ 2 ].transform.position = cameraPos + new Vector3( fieldWidth_, fieldHeight_, 0.0f );
				subCameras_[ 2 ].gameObject.SetActive( true );
			}
			bLR = true;
		} if (wr1.x > hw || wr3.x > hw) {
			// 右側はみ出し
			subCameras_[ 3 ].transform.position = cameraPos + new Vector3( -fieldWidth_, 0.0f, 0.0f );
			subCameras_[ 3 ].gameObject.SetActive( true );
			if (wr2.y > hh || wr3.y > hh) {
				// 右上はみ出し
				subCameras_[ 1 ].transform.position = cameraPos + new Vector3( 0.0f, -fieldHeight_, 0.0f );
				subCameras_[ 1 ].gameObject.SetActive( true );
				subCameras_[ 4 ].transform.position = cameraPos + new Vector3( -fieldWidth_, -fieldHeight_, 0.0f );
				subCameras_[ 4 ].gameObject.SetActive( true );
			} else if (wr0.y < -hh || wr1.y < -hh) {
				// 右下はみ出し
				subCameras_[ 1 ].transform.position = cameraPos + new Vector3( 0.0f, fieldHeight_, 0.0f );
				subCameras_[ 1 ].gameObject.SetActive( true );
				subCameras_[ 4 ].transform.position = cameraPos + new Vector3( -fieldWidth_, fieldHeight_, 0.0f );
				subCameras_[ 4 ].gameObject.SetActive( true );
			}
			bLR = true;
		}
		
		if (bLR == false ) {
			if (wr2.y > hh || wr3.y > hh) {
				// 上はみ出し
				subCameras_[ 1 ].transform.position = cameraPos + new Vector3( 0.0f, -fieldHeight_, 0.0f );
				subCameras_[ 1 ].gameObject.SetActive( true );
			} else if (wr0.y < -hh || wr1.y < -hh) {
				// 下はみ出し
				subCameras_[ 1 ].transform.position = cameraPos + new Vector3( 0.0f, fieldHeight_, 0.0f );
				subCameras_[ 1 ].gameObject.SetActive( true );
			}
		}
	}

	void Update()
    {
	}

	Vector4[] rays_ = new Vector4[ 4 ];
	Vector3[] colPoses_ = new Vector3[ 4 ];
}
