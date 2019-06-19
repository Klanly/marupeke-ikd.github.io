using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    EatenToMimic eatenToMimicPrefab_;

    public System.Action ClearCallback { get; set; }
    public System.Action< bool > FinishCallback { get; set; }

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

    // ステージクリア
    void clear() {
        bClear_ = true;
        ClearCallback();
    }

    // ミミックに食われた！
    void eaten() {
        bEaten_ = true;
        GlobalState.wait( 1.00f, () => {
            var obj = PrefabUtil.createInstance<EatenToMimic>( eatenToMimicPrefab_, transform );
            // メインカメラOFFに
            Camera.main.gameObject.SetActive( false );
            // 演出終了待ち
            obj.FinishCallback = () => {
                FinishCallback( false );
            };
            return false;
        } );
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( bClear_ == true || bEaten_ == true )
            return;
        if ( Input.GetMouseButtonDown( 0 ) == true ) {
            var ray = Camera.main.ScreenPointToRay( new Vector3( Screen.width / 2.0f, Screen.height / 2.0f ) );
            RaycastHit hit;
            if ( Physics.Raycast( ray, out hit, 100.0f ) == true ) {
                {
                    var obj = hit.collider.gameObject.GetComponent<Treasurebox>();
                    if ( obj != null ) {
                        obj.onClick();
                        if ( obj.isMimic() == true ) {
                            eaten();
                        }
                        return;
                    }
                }

                {
                    var obj = hit.collider.gameObject.GetComponent<Door>();
                    if ( obj != null ) {
                        if ( key_ != null ) {
                            // ステージクリア
                            clear();
                        }
                    }
                }

                {
                    var obj = hit.collider.gameObject.GetComponent<Key>();
                    if ( obj != null ) {
                        key_ = true;
                        Destroy( obj.gameObject );
                    }
                }

            }
        }       
    }

    List<Mimic> mimics_ = new List<Mimic>();
    List<Treasurebox> treasureboxes_ = new List<Treasurebox>();
    bool bClear_ = false;
    bool key_ = false;
    bool bEaten_ = false;
}
