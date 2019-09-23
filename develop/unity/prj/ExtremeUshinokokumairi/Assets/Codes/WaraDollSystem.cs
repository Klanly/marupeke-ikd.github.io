using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaraDollSystem : MonoBehaviour
{
    [SerializeField]
    WaraDoll waraDollPrefab_;

    [SerializeField]
    Transform dollPos_;

    [SerializeField]
    KugiPoint[] kugiPoints_;


    public class Parameter {
        public int minKugiCount_ = 3;
        public int maxKugiCount_ = 5;
    }

    private void Awake() {
        waraDoll_ = PrefabUtil.createInstance( waraDollPrefab_, dollPos_, Vector3.zero, Quaternion.identity );
    }

    // セットアップ
    public void setup( Parameter param, HummerMotion hummer ) {
        parameter_ = param;
        hummer_ = hummer;
        for ( int i = 0; i < kugiPoints_.Length; ++i ) {
            int e = i;
            kugiPoints_[ i ].HitCallback = () => {
                hummer_.hit( kugiPoints_[ e ].transform.position );
            };
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ハンマーの位置をXY平面投影点へ
        Vector3 pos = Vector3.zero;
        if ( CameraUtil.calcClickPosition( Camera.main, Input.mousePosition, out pos ) == true ) {
            hummer_.setPosition( pos );
        }

    }

    WaraDoll waraDoll_;
    Parameter parameter_ = new Parameter();
    HummerMotion hummer_;
}
