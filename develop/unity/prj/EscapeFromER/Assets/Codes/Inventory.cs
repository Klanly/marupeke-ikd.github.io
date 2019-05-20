using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// インベントリー

public class Inventory : MonoBehaviour {

    // アイテムリストを取得
    public List< Item > getItemList() {
        return items_;
    }

    // アイテムを追加
    public void add( Item item ) {
        items_.Add( item );
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    List< Item > items_ = new List< Item >();
}
