using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float speed_ = 2.0f;

    [SerializeField]
    float maxHeight_ = 300.0f;

    [SerializeField]
    float minHeight_ = 1.5f;

    private void Awake() {
        motion_ = GetComponent<FPSCameraMotion>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKey( KeyCode.Q ) ) {
            curY_ += speed_;
            curY_ = ( curY_ > maxHeight_ ? maxHeight_ : curY_ );
            var p = transform.position;
            p.y = curY_;
            transform.position = p;
        }
        else if ( Input.GetKey( KeyCode.Z ) ) {
            curY_ -= speed_;
            curY_ = ( curY_ < minHeight_ ? minHeight_ : curY_ );
            var p = transform.position;
            p.y = curY_;
            transform.position = p;
        }
    }

    float curY_ = 1.5f;
    FPSCameraMotion motion_;
}
