using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( MeshRenderer ) )]
[RequireComponent( typeof( MeshFilter ) )]
public class MazeMesh : MonoBehaviour {

    [SerializeField]
    Material sourceMaterial_;

    [SerializeField]
    Light light_;

    public void setParam(MazeCreator.Parameter param ) {
        var vertices = new List<Vector3>();
        var indices = new List<int>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();
        int idx = 0;
        foreach ( var cl in param.cellLevel_ ) {
            for ( int z = 0; z < cl.cells_.GetLength( 1 ); ++z ) {
                for ( int x = 0; x < cl.cells_.GetLength( 0 ); ++x ) {
                    var cell = cl.cells_[ z, x ];
                    foreach ( var w in cell.wallMeshes_ ) {
                        int num = w.appendVertices( ref vertices, ref normals, ref uvs );
                        for ( int i = 0; i < num; ++i ) {
                            indices.Add( idx );
                            idx++;
                        }
                    }
                    // ライト
                    var lt = Instantiate<Light>( light_ );
                    lt.transform.localPosition = cell.localPos_;
                }
            }
        }
        var mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();
        filter_.mesh = mesh;
        renderer_.material = material_;

        var rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
        collider_ = gameObject.AddComponent<MeshCollider>();

        param_ = param;
    }

    public MazeCreator.Parameter getParam() {
        return param_;
    }

    public Collider getCollider() {
        return collider_;
    }

    private void Awake() {
        renderer_ = GetComponent<MeshRenderer>();
        filter_ = GetComponent<MeshFilter>();
        material_ = new Material( sourceMaterial_ );
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    MeshRenderer renderer_;
    MeshFilter filter_;
    Material material_;
    MazeCreator.Parameter param_;
    MeshCollider collider_;
}
