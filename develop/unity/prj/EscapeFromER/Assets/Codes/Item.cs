using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アイテム
public class Item : MonoBehaviour {

    [SerializeField]
    Collider collider_;

    [SerializeField]
    bool bInvisibleOnPickUp_ = true;   // ピックアップされたらオブジェクトを非表示にする？

    public string Name { get { return name_; } }

    // コライダーを取得
    public Collider getCollider() {
        return collider_;
    }

    // 領域を取得
    public Bounds getBound() {
        return bound_;
    }

    // 選択イベント発動
    public void onSelect() {
        Debug.Log( Name + ": Select" );
        bSelecting_ = true;
        if ( bInvisibleOnPickUp_ == true ) {
            collider_.gameObject.SetActive( false );
        }
    }

    // 今選択中？
    public bool isSelecting() {
        return bSelecting_;
    }

    // ピックアップするアイテム？
    public bool isPickUpItem() {
        return bEnablePickUp_;
    }

    // 選択の可否を設定
    public void setEnableSelect( bool isEnable ) {
        bEnableSelect_ = isEnable;
        if ( collider_ != null )
            collider_.enabled = bEnableSelect_;
    }

    private void Awake() {
        // ぶら下がっているメッシュの領域を計算
        var filters = GetComponentsInChildren<MeshFilter>();
        foreach ( var f in filters ) {
            bound_.Encapsulate( f.mesh.bounds );
        }

        // 初期状態での選択可否
        setEnableSelect( bEnableSelect_ );
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [SerializeField]
    string name_ = "";   // アイテム名

    [SerializeField]
    bool bEnablePickUp_ = true; // ピックアップ可能
    Bounds bound_ = new Bounds( Vector3.zero, Vector3.one );
    bool bSelecting_ = false;

    [SerializeField]
    bool bEnableSelect_ = true; // 選択可能
}
