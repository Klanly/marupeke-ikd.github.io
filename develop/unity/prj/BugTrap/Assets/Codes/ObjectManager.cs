using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// フィールド上のオブジェクト管理者
public class ObjectManager : MonoBehaviour
{
	[SerializeField]
	bool bDebug_AddTento_ = false;

	[SerializeField]
	bool bDebug_AddWood_ = false;

	[SerializeField]
	FieldObjectParam debugFieldObjectParam_ = null;

	[SerializeField]
	List<ObjectPair> prefabList_ = new List<ObjectPair>();


	[System.Serializable]
	class ObjectPair {
		public string name_;
		public FieldObject fieldObjectPrefab_;
	}

	[System.Serializable]
	public class FieldObjectParam {
		public string objName_;                                 // 生成するオブジェクト名
		public Vector3 initPos_ = Vector3.zero;                 // 初期位置
		public Vector3 forward_ = new Vector3( 1.0f, 0.0f, 0.0f );  // 最初の向き
		public bool waitCountDown_ = false;
	}

	// フィールドオブジェクトを追加
	public void addFieldObject( FieldObjectParam fieldParam ) {
		foreach ( var pair in prefabList_ ) {
			if ( pair.name_ == fieldParam.objName_ ) {
				Quaternion q = Quaternion.LookRotation( fieldParam.forward_ );
				var obj = PrefabUtil.createInstance( pair.fieldObjectPrefab_, transform, fieldParam.initPos_, q );
				obj.setObjectManager( this );
				obj.onStart( fieldParam.waitCountDown_ );

				barriers_.Add( obj );
			}
		}
	}

	// フィールド上の物との衝突チェック
	public List< FieldObject > checkCollide( FieldObject target ) {
		var list = new List<FieldObject>();
		foreach ( var b in barriers_ ) {
			if ( b != target && b.getShapeGroup().collide( target.getShapeGroup() ) == true ) {
				list.Add( b );
			}
		}
		return list;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if ( bDebug_AddTento_ == true ) {
			bDebug_AddTento_ = false;
			debugFieldObjectParam_.objName_ = "tento";
			addFieldObject( debugFieldObjectParam_ );
		}
		if (bDebug_AddWood_ == true) {
			bDebug_AddWood_ = false;
			debugFieldObjectParam_.objName_ = "wood";
			addFieldObject( debugFieldObjectParam_ );
		}
	}

	List<FieldObject> barriers_ = new List<FieldObject>();
}
