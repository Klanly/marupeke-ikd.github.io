using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KugiPoint : MonoBehaviour
{
    [SerializeField]
    SpriteButton point_;

    // セットトップ
    //  kugiCount: 打ち込む釘の回数
    public void setup( int kugiCount ) {
        curKugiCount_ = 0;
        kugiCount_ = kugiCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        // 押し下げ時処理
        point_.OnDecide = ( btnName ) => {
            // 釘カウンターが残っていたらエフェクト出して減らす
            var p = GameManager.getInstance().ParticleEmitter.emit( "KugiHitPt" );
            p.transform.SetParent( transform );
            p.transform.localPosition = Vector3.zero;
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int kugiCount_ = 1;
    int curKugiCount_ = 0;
}
