using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 呪いゲットカウンター
//  ローカル原点を右上として左方向へパーティクルを並べる

public class NoroiGetCounter : MonoBehaviour
{
    [SerializeField]
    float interval_ = 1.0f;

    [SerializeField]
    bool bDebug_ = false;

    public Vector3 getNextCountPosition() {
        return transform.position - new Vector3( -interval_ * count_, 0.0f, 0.0f );
    }

    public void add() {
        var p = GameManager.getInstance().ParticleEmitter.emit( "NoroiGetPt" );
        p.transform.SetParent( transform );
        p.transform.localPosition = new Vector3( -interval_ * count_, 0.0f, 0.0f );
        count_++;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if ( bDebug_ == true ) {
            bDebug_ = false;
            add();
        }
    }

    int count_ = 0;
}
