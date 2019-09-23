using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 神木
public class Tree : MonoBehaviour
{
    [SerializeField]
    Transform[] dollPoses_;

    public void setDoolSys( WaraDollSystem waraDoll ) {
        if ( curIdx_ >= 0 ) {
            waraDools_[ ( curIdx_ + waraDools_.Length - 1) % waraDools_.Length ] = null;
            curIdx_ = ( ( curIdx_ + 1 ) % waraDools_.Length );
        } else {
            curIdx_ = 0;
        }
        waraDools_[ curIdx_ ] = waraDoll;

        // 位置を登録
        waraDoll.transform.SetParent( dollPoses_[ curIdx_ ].transform );
        waraDoll.transform.localPosition = Vector3.zero;
        waraDoll.transform.localRotation = Quaternion.identity;
    }

    public void turnNext( System.Action finishCallback ) {
        // 120度回転
        var sq = transform.localRotation;
        var eq = Quaternion.Euler( 0.0f, -120.0f, 0.0f ) * sq;
        GlobalState.time( 1.0f, (sec, t) => {
            transform.localRotation = Lerps.Quaternion.easeInOut( sq, eq, t );
            return true;
        } ).finish( () => {
            finishCallback();
        } );
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int curIdx_ = -1;
    WaraDollSystem[] waraDools_ = new WaraDollSystem[ 3 ];
}
