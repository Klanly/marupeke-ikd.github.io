using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemList : MonoBehaviour {

    [SerializeField]
    List<Item> items_;

    public bool showItem( string name ) {
        foreach ( var item in items_ ) {
            if ( item.Name == name ) {
                if ( curShowItem_ != null )
                    curShowItem_.gameObject.SetActive( false );
                item.gameObject.SetActive( true );
                curShowItem_ = item;
                return true;
            }
        }
        return false;
    }

    public void hide() {
        if ( curShowItem_ != null )
            curShowItem_.gameObject.SetActive( false );
    }

    public Item getCurShowItem() {
        return curShowItem_;
    }

    public Item getItem( string name ) {
        foreach ( var item in items_ ) {
            if ( item.Name == name )
                return item;
        }
        return null;
    }

    private void Awake() {
        foreach ( var item in items_ ) {
            item.gameObject.SetActive( false );
        }
    }
    Item curShowItem_ = null;
}
