using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// スプライトUIコントローラ
public class SpriteUIController : MonoBehaviour
{
    [SerializeField]
    float maxDistance_ = 25.0f;

    void Start()
    {
        
    }

    void Update()
    {
        // クリック押し下げ判定
        if ( Input.GetMouseButtonDown( 0 ) == true ) {
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
        } else if ( Input.GetMouseButtonUp( 0 ) == true ) {
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
        } else {
            // カーソルフォーカス判定
/*
            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            RaycastHit hit;
            if ( Physics.Raycast( ray, out hit, maxDistance_ ) == true ) {
                var UI = hit.collider.gameObject.GetComponent<SpriteUI>();
                if ( UI != null ) {
                    if ( curFocusUI_ == null ) {
                        curFocusUI_ = UI;
                        UI.onFocus();
                    } else if ( curFocusUI_ != UI ) {
                        curSelectUI_.releaseFocus();
                        curFocusUI_ = UI;
                        UI.onFocus();
                    }
                }
            } else {
                // カーソル案フォーカス
                if ( curFocusUI_ != null ) {
                    curFocusUI_.onUnfocus();
                    curFocusUI_ = null;
                }
            }
*/
        }
    }

    SpriteUI curSelectUI_ = null;
}
