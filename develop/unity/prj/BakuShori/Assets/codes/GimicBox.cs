using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ギミックボックス
//
//  蓋部分に解除用のTrapを、内部にギミックとAnswerを持つ

public class GimicBox : Entity {

    // 蓋開け成功コールバック
    public System.Action SuccessCallback { set { successCallback_ = value; } }

    // 蓋開け失敗コールバック
    public System.Action FailureCallback { set { failureCallback_ = value; } }

    override public int Index {
        set {
            index_ = value;
            trap_.getAnswer().Index = index_;
        }
        get {
            return index_;
        }
    }

    void Awake()
    {
        ObjectType = EObjectType.GimicBox;
    }

    virtual public void setup( LayoutSpec spec, int randomNumber, Trap trap )
    {
        setChildrenListSize( spec.gimcBoxEntityStockNum_ );
        setTrap( trap );
    }

    // ギミックを登録
    public bool setGimic( Gimic gimic )
    {
        if ( gimic == null || childrenEntities_[ 0 ] != null )
            return false;   // 既に登録されている
        gimic_ = gimic;
        gimic_.Index = index_;
        setEntity( 0, gimic );  // 0番に登録

        // 蓋が閉じている時はギミックをOFFに
        gimic_.gameObject.SetActive( false );
        return true;
    }

    // 蓋トラップを登録
    void setTrap( Trap trap )
    {
        trap_ = trap;
        trap_.getAnswer().Index = Index;
        trap_.SuccessCallback = () => {
            successCallback_();
            gimic_.gameObject.SetActive( true );
        };
        trap_.FailureCallback = () => {
            failureCallback_();
        };
    }

    // 蓋トラップを取得
    public Trap getTrap()
    {
        return trap_;
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

    // 子に所属しているアンサーを取得
    public List<Answer> getAnswres()
    {
        var list = new List<Answer>();
        foreach ( var e in childrenEntities_ ) {
            var ans = e as Answer;
            if ( ans != null )
                list.Add( ans );
        }
        return list;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    Trap trap_;
    Gimic gimic_;
    System.Action successCallback_;
    System.Action failureCallback_;
}
