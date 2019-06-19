using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMoveMotiion : MonoBehaviour
{
    [SerializeField]
    float moveSpeed_ = 1.0f / 60.0f;    // 1フレームでの移動スピード

    [SerializeField]
    Vector3 moveRangeMin_ = -Vector3.one;

    [SerializeField]
    Vector3 moveRangeMax_ = Vector3.one;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // WASDで移動（現在の姿勢ベース）
        // AD: 左右へ平行移動
        // WS: 前後へ平行移動
        Vector3 xMove = Vector3.zero;
        Vector3 zMove = Vector3.zero;
        if ( Input.GetKey( KeyCode.A ) == true ) {
            xMove -= Camera.main.transform.right * moveSpeed_;
        }
        if ( Input.GetKey( KeyCode.D ) == true ) {
            xMove += Camera.main.transform.right * moveSpeed_;
        }
        if ( Input.GetKey( KeyCode.S ) == true ) {
            var f = Camera.main.transform.forward;
            f.y = 0.0f;
            zMove -= f.normalized * moveSpeed_;
        }
        if ( Input.GetKey( KeyCode.W ) == true ) {
            var f = Camera.main.transform.forward;
            f.y = 0.0f;
            zMove += f.normalized * moveSpeed_;
        }

        var p = transform.localPosition;
        p += xMove + zMove;
        if ( p.x < moveRangeMin_.x ) {
            p.x = moveRangeMin_.x;
        } else if ( p.x > moveRangeMax_.x ) {
            p.x = moveRangeMax_.x;
        }
        if ( p.z < moveRangeMin_.z ) {
            p.z = moveRangeMin_.z;
        } else if ( p.z > moveRangeMax_.z ) {
            p.z = moveRangeMax_.z;
        }
        if ( p.y < moveRangeMin_.y ) {
            p.y = moveRangeMin_.y;
        } else if ( p.y > moveRangeMax_.y ) {
            p.y = moveRangeMax_.y;
        }
        transform.localPosition = p;
    }
}
