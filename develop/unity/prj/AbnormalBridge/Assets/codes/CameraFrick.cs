using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraFrick : MonoBehaviour {

    Camera camera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if ( Input.GetMouseButtonDown( 0 ) == true ) {
            // 押し下げ位置がUIの上以外だったら採用
            if ( isPointerOverUIObject( Input.mousePosition ) == false  ) {
                
            }
        }		
	}

    // EventSystem.current.IsPointerOverGameObject(touch.fingerId)
    // がうまく動かないので、その代わり
    public static bool isPointerOverUIObject(Vector2 screenPosition)
    {
        // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
        // the ray cast appears to require only eventData.position.
        PointerEventData eventDataCurrentPosition = new PointerEventData( EventSystem.current );
        eventDataCurrentPosition.position = screenPosition;

        EventSystem.current.RaycastAll( eventDataCurrentPosition, raycastResults );
        var over = raycastResults.Count > 0;
        raycastResults.Clear();
        return over;
    }

    private static List<RaycastResult> raycastResults = new List<RaycastResult>();  // リストを使いまわす
}
