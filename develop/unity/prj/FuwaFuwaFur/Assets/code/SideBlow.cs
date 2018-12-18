using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideBlow : Blower
{
    [SerializeField]
    Transform target_;

    [SerializeField]
    float blowActionRadius_ = 8.0f;

    [SerializeField]
    ParticleSystem blowParticle_;

    public void setTarget(Transform target)
    {
        target_ = target;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ( target_ == null )
            return;

        // ターゲットと同じYレベルに移動
        Vector3 pos = transform.localPosition;
        pos.y = target_.position.y;
        transform.localPosition = pos;

        // ターゲットが指定の距離に近付いたら、自分の吐き出し方向に向かってブロー発生
        Vector3 targePos = target_.position;
        Vector3 myPos = transform.localPosition;
        Vector3 toTargetV = targePos - myPos;
        Vector3 localDirect = transform.up;
        float ang = Mathf.Acos( Vector3.Dot( localDirect, toTargetV.normalized ) ) * Mathf.Rad2Deg;
        float dist = ( toTargetV ).magnitude;
        if ( dist < blowActionRadius_ ) {
            blowTasks_.Add( new BlowTask( toTargetV, blowPower_, blowDecRate_ ) );
            blowParticle_.Play();
        }
        updateTask();
    }
}
