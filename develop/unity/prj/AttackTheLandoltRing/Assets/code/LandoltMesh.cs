using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandoltMesh : MonoBehaviour {

    // ランドルト環メッシュ生成
    // degree: 環の隙間となる角度
    // n     : 分割数
    public void setup(float innerRadius, float degree, float tickness, int n )
    {
        // メッシュ作成
        float th = degree * Mathf.Deg2Rad * 0.5f;
        float r = innerRadius;
        float t = tickness;
        float ro = r + t;
        float w = r * Mathf.Sin( th );
        float a = Mathf.Deg2Rad * ( 360.0f - degree ) / n;
        float b = Mathf.Asin( w / ( r + t ) );
        float z = tickness;

        spaceSize_ = w;

        // 頂点座標
        // 頂点数 = [ Up + Bottom ] + Edge
        //        = [ ( n + 1 ) * 2 * 2 ] + 4
        int vtxNum = ( n + 1 ) * 4 + 4;
        Vector3[] vertices = new Vector3[ vtxNum ];
        // Up
        for ( int i = 0; i <= n; ++i ) {
            int e = 2 * i;
            vertices[ e     ] = new Vector3( r  * Mathf.Cos( i * a + th ), r  * Mathf.Sin( i * a + th ), -z );
            vertices[ e + 1 ] = new Vector3( ro * Mathf.Cos( i * a + th ), ro * Mathf.Sin( i * a + th ), -z );
        }
        // Bottom
        for ( int i = 0; i <= n; ++i ) {
            int e = 2 * i + 2 * ( n + 1 );
            vertices[ e ] = new Vector3( r * Mathf.Cos( i * a + th ), r * Mathf.Sin( i * a + th ), z );
            vertices[ e + 1 ] = new Vector3( ro * Mathf.Cos( i * a + th ), ro * Mathf.Sin( i * a + th ), z );
        }
        // Edge
        int idx = 4 * ( n + 1 );
        int ed0 = idx;
        int ed1 = idx + 1;
        int ed2 = idx + 2;
        int ed3 = idx + 3;
        float x = ( r + t ) * Mathf.Cos( b );
        vertices[ ed0 ] = new Vector3( x,  w, -z );
        vertices[ ed1 ] = new Vector3( x,  w,  z );
        vertices[ ed2 ] = new Vector3( x, -w, -z );
        vertices[ ed3 ] = new Vector3( x, -w,  z );

        // インデックス
        // 個数 = Up + Down + OutSize + InSide + Edge
        //      = 6n + 6n + 6n + 6n + 18 * 2
        int idxNum = 6 * n * 4 + 18 * 2;
        int[] indices = new int[ idxNum ];

        // Up
        int[] baseIdx0 = new int[ 3 ] { 0, 2, 1 };
        int[] baseIdx1 = new int[ 3 ] { 2, 3, 1 };
        for ( int i = 0; i < n; ++i ) {
            int e = 6 * i;
            indices[ e     ] = baseIdx0[ 0 ] + 2 * i;
            indices[ e + 1 ] = baseIdx0[ 1 ] + 2 * i;
            indices[ e + 2 ] = baseIdx0[ 2 ] + 2 * i;
            indices[ e + 3 ] = baseIdx1[ 0 ] + 2 * i;
            indices[ e + 4 ] = baseIdx1[ 1 ] + 2 * i;
            indices[ e + 5 ] = baseIdx1[ 2 ] + 2 * i;
        }

        // Down
        int[] baseIdxD0 = new int[ 3 ] { 0, 1, 2 };
        int[] baseIdxD1 = new int[ 3 ] { 2, 1, 3 };
        int M = ( n + 1 ) * 2;
        for ( int i = 0; i < n; ++i ) {
            int e = 6 * i + 6 * n;
            indices[ e     ] = baseIdxD0[ 0 ] + 2 * i + M;
            indices[ e + 1 ] = baseIdxD0[ 1 ] + 2 * i + M;
            indices[ e + 2 ] = baseIdxD0[ 2 ] + 2 * i + M;
            indices[ e + 3 ] = baseIdxD1[ 0 ] + 2 * i + M;
            indices[ e + 4 ] = baseIdxD1[ 1 ] + 2 * i + M;
            indices[ e + 5 ] = baseIdxD1[ 2 ] + 2 * i + M;
        }

        // OutSide
        int[] baseIdxOS0 = new int[ 3 ] { M + 1, 1, M + 3 };
        int[] baseIdxOS1 = new int[ 3 ] { 1, 3, M + 3 };
        for ( int i = 0; i < n; ++i ) {
            int e = 6 * i + 6 * n * 2;
            indices[ e ] = baseIdxOS0[ 0 ] + 2 * i;
            indices[ e + 1 ] = baseIdxOS0[ 1 ] + 2 * i;
            indices[ e + 2 ] = baseIdxOS0[ 2 ] + 2 * i;
            indices[ e + 3 ] = baseIdxOS1[ 0 ] + 2 * i;
            indices[ e + 4 ] = baseIdxOS1[ 1 ] + 2 * i;
            indices[ e + 5 ] = baseIdxOS1[ 2 ] + 2 * i;
        }

        // InSide
        int[] baseIdxIS0 = new int[ 3 ] { M + 0, M + 2, 0 };
        int[] baseIdxIS1 = new int[ 3 ] { 0, M + 2, 2 };
        for ( int i = 0; i < n; ++i ) {
            int e = 6 * i + 6 * n * 3;
            indices[ e ] = baseIdxIS0[ 0 ] + 2 * i;
            indices[ e + 1 ] = baseIdxIS0[ 1 ] + 2 * i;
            indices[ e + 2 ] = baseIdxIS0[ 2 ] + 2 * i;
            indices[ e + 3 ] = baseIdxIS1[ 0 ] + 2 * i;
            indices[ e + 4 ] = baseIdxIS1[ 1 ] + 2 * i;
            indices[ e + 5 ] = baseIdxIS1[ 2 ] + 2 * i;
        }

        // Edge
        idx = 6 * n * 4;
        int[] edgeIndices = new int[ 36 ] {
            M, 0, ed1,
            0, ed0, ed1,
            ed1, ed0, M + 1,
            ed0, 1, M + 1,
            ed0, 0, 1,
            M + 1, M, ed1,

            M + 2 * n, ed2, 2 * n,
            ed3, ed2, M + 2 * n,
            ed3, 2 * n + 1, ed2,
            M + 2 * n + 1, 2 * n + 1, ed3,
            2 * n + 1, 2 * n, ed2,
            ed3, M + 2 * n, M + 2 * n + 1
        };

        for ( int i = 0; i < 36; ++i ) {
            int e = 6 * n * 4;
            indices[ e + i ] = edgeIndices[ i ];
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.RecalculateNormals();

        var filter = GetComponent<MeshFilter>();
        filter.mesh = mesh;
    }

    public float getSpaceRadius()
    {
        return spaceSize_;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    float spaceSize_ = 0.0f;
}
