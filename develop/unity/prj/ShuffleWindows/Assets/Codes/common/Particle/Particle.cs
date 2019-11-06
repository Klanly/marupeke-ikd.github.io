using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    [SerializeField]
    bool autoDestroy_ = true;

    [SerializeField]
    float lifeTime_ = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        if ( autoDestroy_ == true ) {
            t_ += Time.deltaTime;
            if ( t_ >= lifeTime_ ) {
                Destroy( gameObject );
            }
        }
    }

    float t_ = 0.0f;
}
