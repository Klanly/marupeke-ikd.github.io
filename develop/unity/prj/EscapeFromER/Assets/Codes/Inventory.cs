using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// インベントリー

public class Inventory : MonoBehaviour {

    // アイテムリストを取得
    public List< Item > getItemList() {
        return items_;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    List< Item > items_ = new List< Item >();
}
