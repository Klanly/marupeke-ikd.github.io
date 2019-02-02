using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 爆弾ボックスに配置する物を表す
// リスト構造で配置順を表現

public class Entity : MonoBehaviour {

    public class Stock
    {
        public Entity Parent { set { parent_ = value; } get { return parent_; } }
        public int Index {  set { index_ = value; } get { return index_; } }

        public bool setEntity( Entity entity )
        {
            return parent_.setEntity( index_, entity );
        }

        Entity parent_;
        int index_;
    }

    public enum EObjectType
    {
        Empty,           // 空
        BombBox,         // 爆弾箱
        GimicBox,        // ギミックボックス
        Gimic,           // ギミック
        ScrewAnswer,     // ギミックネジの解答
        GimicBoxAnswer,  // ギミックボックスの解答
        GimicAnswer,     // ギミックの解答
    }

    public EObjectType ObjectType { set { objectType_ = value; } get { return objectType_; } }
    public int Index { set { index_ = value; } get { return index_; } }

    // 子Entityリストのサイズを設定
    virtual public bool setChildrenListSize( int size )
    {
        if ( childrenEntities_.Count == size )
            return true; // 同サイズ

        var list = new List<Entity>();

        if ( childrenEntities_.Count > size ) {
            for ( int i = 0; i < size; ++i ) {
                list.Add( childrenEntities_[ i ] );
            }
        } else {
            for ( int i = childrenEntities_.Count; i < size; ++i ) {
                childrenEntities_.Add( null );
            }
        }
        return true;
    }

    // Entityを登録
    virtual public bool setEntity( int index, Entity entity )
    {
        if ( index >= childrenEntities_.Count )
            return false;
        childrenEntities_[ index ] = entity;
        return true;
    }

    // 空いている位置にEntityを登録
    public bool setEntity( Entity entity )
    {
        for ( int i = 0; i < childrenEntities_.Count; ++i ) {
            if ( childrenEntities_[ i ] == null && setEntity( i, entity ) == true ) {
                return true;
            }
        }
        return false;
    }

    // Entityを取得
    public Entity getEntity( int index )
    {
        if ( index >= childrenEntities_.Count )
            return null;
        return childrenEntities_[ index ];
    }

    // 有効なEntityの数を取得
    public int getValidateEntityNum()
    {
        int num = 0;
        foreach ( var e in childrenEntities_ ) {
            num += ( e != null ? 1 : 0 );
        }
        return num;
    }

    // 空になっているストックを取得
    //  isRecursive: 子Emptyのも集める
    public List< Stock > getEmptyStocks( bool isRecursive )
    {
        var list = new List<Stock>();
        getInnerEmptyStocks( ref list, isRecursive );
        return list;
    }

    // 自分以下を含まない空になっているストックを取得
    public List<Stock> getParentAndTheOtherEmptyStocks()
    {
        var list = new List<Stock>();
        if ( parent_ == null ) {
            // トップなので該当なし
            return list;
        }

        var topParent = getTop();
        topParent.getInnerEmptyStocks( ref list, true, this );
        return list;
    }

    // 空になっているストックを取得
    //  isRecursive: 子Emptyのも集める
    //  ignore: 無視するEntity
    void getInnerEmptyStocks(ref List<Stock> list, bool isRecursive, Entity ignore = null )
    {
        if ( this == ignore )
            return;

        for ( int i = 0; i < childrenEntities_.Count; ++i ) {
            if ( childrenEntities_[ i ] == null ) {
                var stock = new Stock();
                stock.Parent = this;
                stock.Index = i;
                list.Add( stock );

                if ( isRecursive == true )
                    childrenEntities_[ i ].getInnerEmptyStocks( ref list, isRecursive, ignore );
            }
        }
    }

    // トップEntityを取得
    Entity getTop()
    {
        if ( parent_ == null )
            return this;
        return parent_.getTop();
    }

    EObjectType objectType_;
    protected List<Entity> childrenEntities_ = new List<Entity>();
    int index_ = 0;
    Entity parent_;
}
