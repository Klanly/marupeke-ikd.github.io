using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 呪い完成パーティクル
//  
public class NoroiGetParticle : Particle
{
    public System.Action FinishCallback { set { finishCallback_ = value; } }
    public System.Action finishCallback_;

    public void setEndPosition( Vector3 ep ) {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
