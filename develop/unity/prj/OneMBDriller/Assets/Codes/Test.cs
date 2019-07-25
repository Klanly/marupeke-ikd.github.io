using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    Vector3 pos_;

    // Start is called before the first frame update
    void Start()
    {
        scm.ChangeChunkCallback = (acts, nonActs) => {
            string str = "   act: ";
            foreach ( var p in acts ) {
                str += p.ToString() + ", ";
            }
            Debug.Log( str );
            str = "nonAct: ";
            foreach ( var p in nonActs ) {
                str += p.ToString() + ", ";
            }
            Debug.Log( str );
        };
        scm.setup( 1.0f, 1, SquareChunkManager.PlaneType.XZ, Vector3.zero, pos_ );
    }

    // Update is called once per frame
    void Update()
    {
        scm.updateChunk( pos_ );
    }

    SquareChunkManager scm = new SquareChunkManager();
}
