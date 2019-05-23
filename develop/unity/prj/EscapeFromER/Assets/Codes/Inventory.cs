using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// インベントリー

public class Inventory : MonoBehaviour {

    [SerializeField]
    Camera invCamera_;

    [SerializeField]
    Canvas canvas_;

    [SerializeField]
    List< InventoryIcon > iconList_;

    [SerializeField]
    InventoryItemList invItemList_;

    // アイテムリストを取得
    public List< Item > getItemList() {
        return items_;
    }

    // アイテムを追加
    public void add( Item item ) {
        items_.Add( item );

        // アイテムに関連するアイコン等をオープン
        foreach ( var icon in iconList_ ) {
            if ( icon.Name == item.Name ) {
                icon.gameObject.SetActive( true );
                icon.getButton().onClick.AddListener( () => {
                    if ( bEnableIconSelect_ == true )
                        setIconAction( icon.Name );
                } );
            }
        }
    }

    // インベントリー画面表示
    public void show( System.Action closeCallback ) {
        invCamera_.gameObject.SetActive( true );
        closeCallback_ = closeCallback;
        canvas_.gameObject.SetActive( true );
        bShow_ = true;
        bCompClose_ = false;
    }

    // インベントリー表示中？
    public bool isShow() {
        return bShow_;
    }

    // インベントリー完全に閉じた？
    public bool compClose() {
        return bCompClose_;
    }

    // アイコンを消す
    void deleteIcon( string name ) {
        InventoryIcon target = null;
        foreach ( var icon in iconList_ ) {
            if ( icon.Name == name ) {
                target = icon;
                break;
            }
        }
        if ( target != null ) {
            iconList_.Remove( target );
            Destroy( target.gameObject );
        }
    }

    // アイコンを表示
    void showIcon( string name ) {
        // アイテムに関連するアイコン等をオープン
        foreach ( var icon in iconList_ ) {
            if ( icon.Name == name ) {
                icon.gameObject.SetActive( true );
                icon.getButton().onClick.AddListener( () => {
                    setIconAction( icon.Name );
                } );
            }
        }
    }

    private void Awake() {
        canvas_.gameObject.SetActive( false );
        foreach ( var icon in iconList_ ) {
            icon.gameObject.SetActive( false );
        }
    }

    // Use this for initialization
    void Start () {
        itemOpeStates_.Add( new FilerOpeState() );
        itemOpeStates_.Add( new RotPazzleOpeState() );
	}
	
	// Update is called once per frame
	void Update () {
        if ( bShow_ == false )
            return;

        // [Q]でインベントリークローズ
        if ( Input.GetKeyDown( KeyCode.Q ) == true ) {
            invCamera_.gameObject.SetActive( false );
            canvas_.gameObject.SetActive( false );
            bShow_ = false;
            if ( closeCallback_ != null )
                closeCallback_();
            GlobalState.wait( 0.1f, () => {
                bCompClose_ = true;
                return false;
            } );
        }
    }

    // アイコンアクション
    void setIconAction( string name ) {
        bool fook = false;
        var removeList = new List<ItemOpeState>();
        foreach ( var ope in itemOpeStates_ ) {
            if ( ope.isTargetName( name ) == true ) {
                fook = true;
                if ( ope.check( this, invItemList_, name ) == true ) {
                    // 条件を満たしたので消す
                    removeList.Add( ope );
                }
            }
        }
        if ( removeList.Count > 0 ) {
            foreach ( var ope in removeList ) {
                itemOpeStates_.Remove( ope );
            }
            removeList.Clear();
        }

        if ( fook == false ) {
            if ( invItemList_.showItem( name ) == true ) {
            }
        }
    }

    // アイコン選択の可否を設定
    void setEnableIcon( bool isEnable ) {
        bEnableIconSelect_ = isEnable;
    }

    // 回転パズル起動
    void wakeUpRotPazzle() {
        // 回転パズル中はパネル内のアイテムを選択不可にする
        setEnableIcon( false );
        var pazzle = PrefabUtil.createInstance<RotPazzle>( "Prefabs/RotPazzle", invCamera_.transform );
        pazzle.setCamera( invCamera_ );
        pazzle.transform.localPosition = new Vector3( 0.0f, 0.0f, 11.0f );
        pazzle.ClearCallback = () => {
            // 機密ファイル追加
            Debug.Log( "Add Conf. file" );
            GlobalState.wait( 1.0f, () => {
                Destroy( pazzle.gameObject );
                showIcon( "ConfFile" );
                invItemList_.showItem( "ConfFile" );
                var item = invItemList_.getItem( "ConfFile" );
                if ( item != null )
                    items_.Add( item );

                setEnableIcon( true );

                return false;
            } );
        };
    }

    class ItemOpeState {
        public virtual bool check( Inventory inv, InventoryItemList list, string name ) {
            return true;
        }
        public virtual bool isTargetName( string name ) {
            return false;
        }
    }

    // ファイラー操作
    class FilerOpeState : ItemOpeState {
        public override bool isTargetName(string name) {
            if ( name == "Filer9" || name == "Filer15" || name == "Filer24" ) {
                return true;
            }
            return false;
        }
        public override bool check( Inventory inv, InventoryItemList list, string name ) {
            // Filer9, 15, 24の何れかのアイコンをクリックした場合、
            // ファイラーに対応するRotPazzleに差し替え。Filerは消す。
            if ( name != "Filer9" && name != "Filer15" && name != "Filer24" ) {
                return false;
            }
            list.showItem( name );
            inv.deleteIcon( name );

            if ( name == "Filer9" ) {
                // RotPazzle1追加
                inv.showIcon( "RotPazzle1" );
                list.showItem( "RotPazzle1" );
                bPazzle1_ = true;
            } else if ( name == "Filer15") {
                // RotPazzle2追加
                inv.showIcon( "RotPazzle2" );
                list.showItem( "RotPazzle2" );
                bPazzle2_ = true;
            } else if ( name == "Filer24" ) {
                // RotPazzle3追加
                inv.showIcon( "RotPazzle3" );
                list.showItem( "RotPazzle3" );
                bPazzle3_ = true;
            }
            if ( bPazzle1_ == true && bPazzle2_ == true && bPazzle3_ == true )
                return true;

            return false;
        }
        bool bPazzle1_ = false;
        bool bPazzle2_ = false;
        bool bPazzle3_ = false;
    }

    // RotPazzle操作
    class RotPazzleOpeState : ItemOpeState {
        public override bool check( Inventory inv, InventoryItemList list, string name ) {
            // RotPazzleの合成
            // 全部揃ったらRotPazzleを発動
            var cur = list.getCurShowItem();
            if ( cur.Name == name )
                return false;
            Dictionary<string, int> flags = new Dictionary<string, int> {
                { "RotPazzle1", 1 },
                { "RotPazzle2", 2 },
                { "RotPazzle3", 4 },
                { "RotPazzle12", 3 },
                { "RotPazzle13", 5 },
                { "RotPazzle23", 6 },
            };
            flag_ |= flags[ name ];
            if ( flag_ == 7 ) {
                // 全部揃った
                foreach ( var n in flags ) {
                    inv.deleteIcon( n.Key );
                }
                inv.wakeUpRotPazzle();
                list.hide();
                return true;
            }
            // 合成が発生していない場合はそのまま
            if ( flag_ == 1 || flag_ == 2 || flag_ == 4 ) {
                list.showItem( name );
                return false;
            }

            // 合成が発生
            // 今選択したアイコンは消して
            // 合成アイコンに差し替え
            string[] dels = new string[] {
                "RotPazzle1", "RotPazzle2", "RotPazzle3"
            };
            for ( int i = 0; i < 3; ++i ) {
                if ( ( ( flag_ >> i ) & 0x1 ) == 1 ) {
                    inv.deleteIcon( dels[ i ] );
                }
            }

            Dictionary<int, string> flagToName = new Dictionary<int, string> {
                { 3, "RotPazzle12" },
                { 5, "RotPazzle13" },
                { 6, "RotPazzle23" },
            };

            string compName = flagToName[ flag_ ];
            inv.showIcon( compName );
            list.showItem( compName );

            return false;
        }

        public override bool isTargetName(string name) {
            if (
                name == "RotPazzle1" ||
                name == "RotPazzle2" ||
                name == "RotPazzle3" ||
                name == "RotPazzle12" ||
                name == "RotPazzle13" ||
                name == "RotPazzle23"
               ) {
                return true;
            }
            return false;
        }
        int flag_ = 0;
    }

    List< Item > items_ = new List< Item >();
    System.Action closeCallback_;
    bool bShow_ = false;
    bool bCompClose_ = true;
    bool bEnableIconSelect_ = true;
    List<ItemOpeState> itemOpeStates_ = new List<ItemOpeState>();
}
