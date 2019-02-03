using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ギミックボックス
//
//  蓋部分に解除用のTrapを、内部にギミックとAnswerを持つ

public class GimicBox : Entity {

    void Awake()
    {
        ObjectType = EObjectType.GimicBox;
    }

    virtual public void setup( LayoutSpec spec, int randomNumber )
    {
        setChildrenListSize( spec.gimcBoxEntityStockNum_ );
    }

    // ギミックを登録
    public bool setGimic( Gimic gimic )
    {
        if ( gimic != null || childrenEntities_[ 0 ] != null )
            return false;   // 既に登録されている
        gimic_ = gimic;
        setEntity( 0, gimic );  // 0番に登録
        return true;
    }

    // 蓋トラップを登録
    public void setTrap( Trap trap )
    {
        trap_ = trap;
    }

    // 蓋の答えを取得
    public Answer getTrapAnswer()
    {
        if ( trap_ == null ) {
            Debug.LogAssertion( "GimicBox: error: no trap exist." );
            return null;
        }
        return trap_.getAnswer();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    Trap trap_;
    Gimic gimic_;
}
