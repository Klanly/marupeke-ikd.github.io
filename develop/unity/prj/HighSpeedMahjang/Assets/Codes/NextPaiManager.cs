using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 次の予約牌管理人
public class NextPaiManager : MonoBehaviour {

    [SerializeField]
    Transform[] poses_;

    [SerializeField]
    int debugPaiType = -1;

    public  PaiObject[] pop() {
        var ary = new PaiObject[ 2 ];
        ary[ 0 ] = createPai();
        ary[ 0 ].transform.SetParent( poses_[ poses_.Length - 1 ] );
        ary[ 0 ].transform.localPosition = Vector3.zero;

        ary[ 1 ] = createPai();
        ary[ 1 ].transform.SetParent( poses_[ poses_.Length - 1 ] );
        ary[ 1 ].transform.localPosition = new Vector3( 2.0f, 0.0f, 0.0f );

        paiList_.Enqueue( ary );

        var outAry = paiList_.Dequeue();

        int i = 0;
        foreach ( var a in paiList_ ) {
            a[ 0 ].transform.SetParent( poses_[ i ] );
            a[ 0 ].transform.localPosition = Vector3.zero;
            a[ 1 ].transform.SetParent( poses_[ i ] );
            a[ 1 ].transform.localPosition = new Vector3( 2.0f, 0.0f, 0.0f );
            i++;
        }

        return outAry;
    }

    private void Awake() {
    }

    PaiObject createPai() {
        if ( debugPaiType != -1 ) {
            return GameManager.getInstance().PaiGenerator.create( debugPaiType );
        }
        return GameManager.getInstance().PaiGenerator.createRandom();
    }

    // Start is called before the first frame update
    void Start()
    {
        for ( int i = 0; i < poses_.Length; ++i ) {
            var ary = new PaiObject[ 2 ];
            ary[ 0 ] = createPai();
            ary[ 0 ].transform.SetParent( poses_[ i ] );
            ary[ 0 ].transform.localPosition = Vector3.zero;

            ary[ 1 ] = createPai();
            ary[ 1 ].transform.SetParent( poses_[ i ] );
            ary[ 1 ].transform.localPosition = new Vector3( 2.0f, 0.0f, 0.0f );

            paiList_.Enqueue( ary );
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Queue<PaiObject[]> paiList_ = new Queue<PaiObject[]>();
}
