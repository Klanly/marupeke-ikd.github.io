using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public void setup( GameManager manager ) {
        manager_ = manager;
    }

	void Start () {
        state_ = new Gaming( this );
	}
	
	void Update () {
        if ( state_ != null )
            state_ = state_.update();

    }

    Speaker isSelectSpeaker() {
        // レイ飛ばす
        var ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        RaycastHit hit = new RaycastHit();
        if ( Physics.Raycast( ray, out hit ) == true ) {
            return hit.transform.GetComponent<Speaker>();
        }
        return null;
    }

    class Gaming : State< Player > {
        public Gaming(Player parent) : base( parent ) { }
        protected override State innerInit() {
            return null;
        }
        protected override State innerUpdate() {
            // カメラズーム
            var scrDelta = Input.mouseScrollDelta;
            float zoomScale = 0.2f;
            if ( scrDelta.y != 0.0f ) {
                var cp = Camera.main.transform.position;
                var nextY = cp.y + ( Camera.main.transform.forward.y ) * scrDelta.y * zoomScale;
                if ( nextY >= 1.6f && nextY <= 7.0f ) {
                    cp += Camera.main.transform.forward * scrDelta.y * zoomScale;
                    Camera.main.transform.position = cp;
                }
            }

            if ( Input.GetMouseButtonDown( 0 ) == true ) {
                // フィールドドラッグ開始
                fieldDrugging_ = true;
                clickPos_ = Input.mousePosition;
                cameraPicker_.startPicking( Camera.main, Input.mousePosition, Vector3.up, Vector3.zero );
            }

            if ( Input.GetMouseButtonUp( 0 ) == true ) {
                // フィールドドラッグ終了
                fieldDrugging_ = false;
                if ( ( clickPos_ - Input.mousePosition ).magnitude <= 0.1f ) {
                    // スピーカ選択？
                    var speaker = parent_.isSelectSpeaker();
                    if ( speaker != null ) {
                        speaker.playSE();
                    }
                    // 1つ目ならスタック
                    if ( bSelectFirst_ == true ) {
                        firstSelectSpeaker_ = speaker;
                        bSelectFirst_ = false;
                    } else {
                        // 2つ目なので合致したら得点を
                        if ( firstSelectSpeaker_.getSEName() == speaker.getSEName() ) {
                            // 合致！
                            parent_.manager_.getSpeakers( parent_, new Speaker[] { firstSelectSpeaker_, speaker } );
                        }
                        bSelectFirst_ = true;
                    }
                }
            }

            if ( fieldDrugging_ == true && Input.GetMouseButton( 0 ) == true ) {
                cameraPicker_.updateCameraPos( Input.mousePosition );
            }
            return this;
        }
        CameraPicker cameraPicker_ = new CameraPicker();
        bool fieldDrugging_ = false;
        Vector3 clickPos_ = Vector3.zero;
        bool bSelectFirst_ = true;
        Speaker firstSelectSpeaker_ = null;
    }

    GameManager manager_;
    State state_;
}
