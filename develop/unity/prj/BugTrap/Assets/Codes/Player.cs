using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤー
public class Player : MonoBehaviour
{
	[System.Serializable]
	class Item {
		public string name_;
		public UnityEngine.UI.Button itemButtons_;
		public GameObject fieldItemPrefab_;
		public GameObject fieldObject_;
	}

	[SerializeField]
	Transform fieldObjRoot_;

	[SerializeField]
	List<Item> items_ = new List<Item>();

	public void setup( GameManager gameManager ) {
		gameManager_ = gameManager;
		foreach ( var i in items_ ) {
			var item = i;
			item.itemButtons_.onClick.AddListener( () => { selectItem( item ); } );
			item.fieldObject_ = PrefabUtil.createInstance( item.fieldItemPrefab_, fieldObjRoot_, Vector3.zero );
			item.fieldObject_.SetActive( false );
		}
	}

	void selectItem( Item item ) {
		if (curSelectItem_ != null && curSelectItem_ != item ) {
			// 現在のアイテムを非表示に
			curSelectItem_.fieldObject_.SetActive( false );
		}
		// アイテムを切り替え
		curSelectItem_ = item;
		curSelectItem_.fieldObject_.SetActive( true );
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		// アイテムが選択されている時はカーソル位置と平面の交点位置にアイテムを移動
		var ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		if ( curSelectItem_ != null ) {
			Vector3 colPos = Vector3.zero;
			CollideUtil.colPosRayPlane( out colPos, ray.origin, ray.direction, Vector3.zero, Vector3.up );
			fieldObjRoot_.localPosition = colPos;

			// GroundRegion内ならアイテム配置
			if (Input.GetMouseButtonDown( 0 ) == true) {
				RaycastHit hit;
				if (Physics.Raycast( ray, out hit ) == true) {
					if (hit.collider.tag == "GroundRegion") {
						// 配置
						CollideUtil.colPosRayPlane( out colPos, ray.origin, ray.direction, Vector3.zero, Vector3.up );
						var param = new ObjectManager.FieldObjectParam();
						param.initPos_ = colPos;
						param.objName_ = curSelectItem_.name_;
						gameManager_.ObjectManager.addFieldObject( param );

						// 洗濯アイテムはリセット
						curSelectItem_.fieldObject_.SetActive( false );
						curSelectItem_ = null;
					}
				}
			}
		}
	}

	Item curSelectItem_ = null;
	GameManager gameManager_;
}
