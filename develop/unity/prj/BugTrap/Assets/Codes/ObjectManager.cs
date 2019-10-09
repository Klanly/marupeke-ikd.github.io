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
	bool bDebug_Wood_V_ = false;

	[SerializeField]
	bool bDebug_Wood_H_ = false;

	[SerializeField]
	bool bDebug_Wood_RUp_ = false;

	[SerializeField]
	bool bDebug_Wood_LUp_ = false;

	[SerializeField]
	bool bDebug_AddGoal_ = false;

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
		public Vector3 forward_ = new Vector3( 0.0f, 0.0f, 1.0f );  // 最初の向き
		public bool waitCountDown_ = false;
	}

	// フィールドオブジェクトを追加
	public void addFieldObject( FieldObjectParam fieldParam ) {
		foreach ( var pair in prefabList_ ) {
			if ( pair.name_ == fieldParam.objName_ ) {
				Quaternion q = Quaternion.LookRotation( fieldParam.forward_ );
				var obj = PrefabUtil.createInstance( pair.fieldObjectPrefab_, transform, fieldParam.initPos_ );
				obj.transform.rotation = q * obj.transform.rotation;
				obj.setObjectManager( this );
				obj.onStart( fieldParam.waitCountDown_ );

				var bug = obj as Bug;
				if ( bug != null ) {
					bugs_.Add( bug );
				} else {
					barriers_.Add( obj );
				}
			}
		}
	}

	// フィールド上の物との衝突チェック
	public List< FieldObject > checkCollide( FieldObject target ) {
		var list = new List<FieldObject>();
		var targetShape = target.getShapeGroup();
		foreach ( var b in barriers_ ) {
			if ( b != target && b.getShapeGroup().collide( targetShape ) == true ) {
				list.Add( b );
			}
		}
		return list;
	}

	// フィールド上のBugとの衝突チェック
	public List<FieldObject> checkCollideToBug(FieldObject target)
	{
		var list = new List<FieldObject>();
		foreach (var b in bugs_) {
			if (b != target && b.getShapeGroup().collide( target.getShapeGroup() ) == true) {
				list.Add( b );
			}
		}
		return list;
	}

	// 捉えた虫を削除
	public void catchBug( Bug bug ) {
		foreach( var b in bugs_) {
			if ( b == bug ) {
				bugs_.Remove( b );

				// 残り虫数を更新

				// 削除
				Destroy( b.gameObject );
				return;
			}
		}
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
		if (bDebug_Wood_V_ == true) {
			bDebug_Wood_V_ = false;
			debugFieldObjectParam_.objName_ = "wood_v";
			addFieldObject( debugFieldObjectParam_ );
		}
		if (bDebug_Wood_H_ == true) {
			bDebug_Wood_H_ = false;
			debugFieldObjectParam_.objName_ = "wood_h";
			addFieldObject( debugFieldObjectParam_ );
		}
		if (bDebug_Wood_RUp_ == true) {
			bDebug_Wood_RUp_ = false;
			debugFieldObjectParam_.objName_ = "wood_ru";
			addFieldObject( debugFieldObjectParam_ );
		}
		if (bDebug_Wood_LUp_ == true) {
			bDebug_Wood_LUp_ = false;
			debugFieldObjectParam_.objName_ = "wood_lu";
			addFieldObject( debugFieldObjectParam_ );
		}
		if (bDebug_AddGoal_ == true) {
			bDebug_AddGoal_ = false;
			debugFieldObjectParam_.objName_ = "goal";
			addFieldObject( debugFieldObjectParam_ );
		}
	}

	List<FieldObject> barriers_ = new List<FieldObject>();
	List<Bug> bugs_ = new List<Bug>();
}
