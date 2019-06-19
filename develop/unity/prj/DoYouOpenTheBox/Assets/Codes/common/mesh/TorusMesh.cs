using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( MeshRenderer ) )]
[RequireComponent( typeof( MeshFilter ) )]
public class TorusMesh : MonoBehaviour {

	[SerializeField, Range(0.0f, 100.0f)]
	float innerRadius_ = 1.0f;

	[SerializeField, Range(0.01f, 100.0f)]
	float tubeRadius_ = 0.5f;

	[SerializeField, Range(3, 360)]
	int circleSepNum_ = 16;

	[SerializeField, Range(3, 360)]
	int cylinderSepNum_ = 16;

	public float InnerRadius { set { innerRadius_ = ( value <= 0.0f ? 0.0f : value ); } get { return innerRadius_; } }
	public float TubeRadius { set { tubeRadius_ = ( value <= 0.0f ? 0.0f : value ); } get { return tubeRadius_; } }
	public int CircleSepNum {  set { circleSepNum_ = ( value < 3 ? 3 : value ); } get { return circleSepNum_; } }
	public int CylinderSepNum { set { cylinderSepNum_ = ( value < 3 ? 3 : value ); } get { return cylinderSepNum_; } }

	// 内部パラメータでトーラス作成
	public void create() {
		create( innerRadius_, tubeRadius_, circleSepNum_, cylinderSepNum_ );
	}

	// トーラス作成
	//  innerRadius   : トーラスの中空円の半径
	//  tubeRadius    : チューブの半径
	//  circleSepNum  : トーラス円の分割数
	//  cylinderSepNum: トーラス円柱の稜線分割数
	public void create(float innerRadius, float tubeRadius, int circleSepNum, int cylinderSepNum ) {
		innerRadius_ = innerRadius;
		tubeRadius_ = tubeRadius;
		circleSepNum_ = circleSepNum;
		cylinderSepNum_ = cylinderSepNum;
		float Ri = innerRadius;
		float Rt = tubeRadius;
		float Rc = Ri + Rt;
		int Xsep = circleSepNum;
		int Ysep = cylinderSepNum;
		int Vn = Xsep * Ysep;   // 頂点数
		float Tx = 360.0f / Ysep * Mathf.Deg2Rad;   // 円柱分割角度
		float Ty = 360.0f / Xsep * Mathf.Deg2Rad;   // トーラス円分割角度

		Vector3 v = Vector3.zero;	// 計算用

		// 頂点座標・法線
		Vector3[] vertices = new Vector3[ Vn ];     // 頂点バッファ
		Vector3[] norms = new Vector3[ Vn ];		// 法線バッファ
		int vcnt = 0;
		for ( int y = 0; y < Ysep; ++y ) {
			for ( int x = 0; x < Xsep; ++x ) {
				v.x = 0.0f;
				v.y = Rt * Mathf.Sin( Tx * y );
				v.z = -Rt * Mathf.Cos( Tx * y );
				float z = v.z + Rc;
				vertices[ vcnt ] = new Vector3( -z * Mathf.Sin( Ty * x ), v.y, z * Mathf.Cos( Ty * x ) );
				norms[ vcnt ] = new Vector3( -v.z * Mathf.Sin( Ty * x ), v.y, v.z * Mathf.Cos( Ty * x ) );
				vcnt++;
			}
		}

		// インデックス
		int[] indices = new int[ Vn * 6 ];	// インデックスバッファ
		for ( int e = 0; e < Vn; ++e ) {
			int y = e / Xsep;
			int p0 = e;
			int p1 = ( e + Xsep ) % Vn;
			int p2 = ( p0 + 1 ) % Xsep + y * Xsep;
			int p3 = ( p2 + Xsep ) % Vn;
			indices[ e * 6 + 0 ] = p0;
			indices[ e * 6 + 1 ] = p2;
			indices[ e * 6 + 2 ] = p1;
			indices[ e * 6 + 3 ] = p1;
			indices[ e * 6 + 4 ] = p2;
			indices[ e * 6 + 5 ] = p3;
		}

		var mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.normals = norms;
		mesh.triangles = indices;
		var filter = GetComponent< MeshFilter >();
		filter.mesh = mesh;

		created_ = true;
	}

	// Use this for initialization
	void Start () {
		if ( created_ == false )
			create( innerRadius_, tubeRadius_, circleSepNum_, cylinderSepNum_ );
	}

	bool created_ = false;
}
