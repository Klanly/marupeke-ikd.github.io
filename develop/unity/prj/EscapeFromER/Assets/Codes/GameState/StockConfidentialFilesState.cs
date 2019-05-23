using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 機密ファイルを探して保存
public class StockConfidentialFilesState : GameState {

	// Use this for initialization
	void Start () {
        // 対象Filerを選択可能に
        string[] itemNames = new string[] {
            "Filer9", "Filer15", "Filer24"
        };
        foreach ( var name in itemNames ) {
            var item = GameObjectUtil.find< Item >( name );
            if ( item != null ) {
                item.setEnableSelect( true );
            }
        }

        // インベントリーにFiler9,15,24が格納されたらOK
        var gm = GameManager.getInstance();
        var inv = gm.getInventory();
        GlobalState.start( () => {
            var list = inv.getItemList();
            int stockNum = 0;
            foreach ( var item in list ) {
                foreach ( var name in itemNames ) {
                    if ( item.Name == name ) {
                        stockNum++;
                    }
                }
                if ( stockNum == itemNames.Length ) {
                    complete();
                    return false;
                }
            }
            return true;
        } );
    }

    // Update is called once per frame
    void Update () {
		
	}
}
