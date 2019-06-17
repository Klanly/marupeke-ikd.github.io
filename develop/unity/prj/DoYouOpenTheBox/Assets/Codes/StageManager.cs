using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private void Awake() {
        // ステージ下に所属している宝箱、ミミックをかき集める
        GameObjectUtil.getChildrenComponents( gameObject, ref mimics_, true );
        GameObjectUtil.getChildrenComponents( gameObject, ref treasureboxes_, true );

        foreach ( var mimic in mimics_ ) {
            mimic.ClickCallback = confirmOpen;
        }
        foreach ( var box in treasureboxes_ ) {
            box.ClickCallback = confirmOpen;
        }
    }

    // オープンを確認
    void confirmOpen( Treasurebox box ) {
        box.open( 2.0f );
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetMouseButtonDown( 0 ) == true ) {
            var ray = Camera.main.ScreenPointToRay( new Vector3( Screen.width / 2.0f, Screen.height / 2.0f ) );
            RaycastHit hit;
            if ( Physics.Raycast( ray, out hit, 100.0f ) == true ) {
                var box = hit.collider.gameObject.GetComponent< Treasurebox >();
                if ( box != null ) {
                    box.onClick();
                }
            }
        }       
    }

    List<Mimic> mimics_ = new List<Mimic>();
    List<Treasurebox> treasureboxes_ = new List<Treasurebox>();
}
