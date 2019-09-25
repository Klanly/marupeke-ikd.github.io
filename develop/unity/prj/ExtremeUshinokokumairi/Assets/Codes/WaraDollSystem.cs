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

    // 全て打ち込めたら呼び出される
    public System.Action AllHitCallback { set { allHitCallback_ = value; } }
    public System.Action allHitCallback_;

    public class Parameter {
        public int minKugiCount_ = 2;
        public int maxKugiCount_ = 5;
        public int minPosCount_ = 2;
        public int maxPosCount_ = 7;
    }

    private void Awake() {
        waraDoll_ = PrefabUtil.createInstance( waraDollPrefab_, dollPos_, Vector3.zero, Quaternion.identity );
        for ( int i = 0; i < kugiPoints_.Length; ++i ) {
            kugiPoints_[ i ].setActive( false );
        }
    }

    // セットアップ
    public void setup( Parameter param, HummerMotion hummer ) {
        parameter_ = param;
        hummer_ = hummer;
        int pointNum = Random.Range( param.minPosCount_, param.maxPosCount_ + 1 );
        int[] indices = new int[] { 0, 1, 2, 3, 4, 5, 6 };
        ListUtil.shuffle( ref indices );
        for ( int i = 0; i < pointNum; ++i ) {
            int e = indices[ i ];
            kugiPoints_[ e ].setup( Random.Range( param.minKugiCount_, param.maxKugiCount_ + 1 ) );
            kugiPoints_[ e ].HitCallback = ( pointName, remain, isFirst ) => {
                if ( bActive_ )
                    kugiHit( e, pointName, remain, isFirst );

            };
        }
        for ( int i = pointNum; i < kugiPoints_.Length; ++i ) {
            int e = indices[ i ];
            kugiPoints_[ e ].setup( 0 );
            kugiPoints_[ e ].gameObject.SetActive( false );
        }
    }

    // 釘を打った
    void kugiHit( int kugiIdx, string pointName, int remain, bool isFirst ) {
        hummer_.hit( kugiPoints_[ kugiIdx ].transform.position );

        // 成功失敗を通知
        hummer_.notifySuccessHit( remain >= 0 );

        if ( allHitCallback_ == null )
            return;

        for ( int i = 0; i < kugiPoints_.Length; ++i ) {
            if ( kugiPoints_[ i ].getCount() > 0 )
                return;
        }
        if ( allHitCallback_ != null ) {
            allHitCallback_();
            allHitCallback_ = null;
        }
    }

    // アクティブにする
    public void setActive( bool isActive ) {
        bActive_ = isActive;
        for ( int i = 0; i < kugiPoints_.Length; ++i ) {
            kugiPoints_[ i ].setActive( isActive );
        }
    }

    // エクストリーム
    public void extreme( bool isExtreme ) {
        if ( bExtreme_ == isExtreme )
            return;
        bExtreme_ = isExtreme;
        for ( int i = 0; i < kugiPoints_.Length; ++i ) {
            kugiPoints_[ i ].extreme( isExtreme );
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( bActive_ == true ) {
            // ハンマーの位置をXY平面投影点へ
            Vector3 pos = Vector3.zero;
            if ( CameraUtil.calcClickPosition( Camera.main, Input.mousePosition, out pos ) == true ) {
                hummer_.setPosition( pos );
            }
        }
    }

    WaraDoll waraDoll_;
    Parameter parameter_ = new Parameter();
    HummerMotion hummer_;
    bool bActive_ = false;
    bool bExtreme_ = false;
}
