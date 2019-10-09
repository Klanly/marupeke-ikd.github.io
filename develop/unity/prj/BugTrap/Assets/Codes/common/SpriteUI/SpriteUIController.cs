using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// スプライトUIコントローラ
public class SpriteUIController : MonoBehaviour
{
    [SerializeField]
    float maxDistance_ = 25.0f;

    // 現在のポジションで強制的にマウスクリックを1回行う
    public void forceMouseButtonClick() {
        bForceMouseButton0Down_ = true;
        bForceMouseButton0Up_ = false;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if ( Input.GetMouseButtonDown( 0 ) == true || bForceMouseButton0Down_ == true ) {
            if ( bForceMouseButton0Down_ == true ) {
                bForceMouseButton0Down_ = false;
                bForceMouseButton0Up_ = true;
            }
            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            RaycastHit hit;
            if ( Physics.Raycast( ray, out hit , maxDistance_ ) == true ) {
                var UI = hit.collider.gameObject.GetComponent< SpriteUI >();
                if ( UI != null ) {
                    UI.onDown();
                    if ( curSelectUI_ != null && curSelectUI_ != UI )
                        curSelectUI_.onCancel();
                    curSelectUI_ = UI;
                }
            }
        } else if ( Input.GetMouseButtonUp( 0 ) == true || bForceMouseButton0Up_ == true ) {
            if ( bForceMouseButton0Up_ == true ) {
                bForceMouseButton0Up_ = false;
            }
            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            RaycastHit hit;
            if ( Physics.Raycast( ray, out hit, maxDistance_ ) == true ) {
                var UI = hit.collider.gameObject.GetComponent< SpriteUI >();
                if ( UI != null ) {
                    if ( curSelectUI_ != null && UI != curSelectUI_ ) {
                        curSelectUI_.onCancel();
                        curSelectUI_ = null;
                    }
                    UI.onUp();
                } else if ( curSelectUI_ ) {
                    curSelectUI_.onCancel();
                    curSelectUI_ = null;
                }
            } else {
                if ( curSelectUI_ ) {
                    curSelectUI_.onCancel();
                    curSelectUI_ = null;
                }
            }
        }
    }

    SpriteUI curSelectUI_ = null;
    bool bForceMouseButton0Down_ = false;
    bool bForceMouseButton0Up_ = false;
}
