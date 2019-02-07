using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// トラップファクトリー
public class TrapFactory : MonoBehaviour {

    [SerializeField]
    Trap[] trapPrefabs_;

    public enum TrapType : int
    {
        Screw = 0,  // ギミックネジ
        TypeNum = 1
    }

    // シード設定
    public void setSeed( int seed )
    {
        random_ = new System.Random( seed );
    }

    // 生成
    public Trap create(LayoutSpec spec)
    {
        if ( trapPrefabs_.Length < (int)TrapType.TypeNum )
            return null;

        if ( spec.gimicNum_ == 0 )
            return null;

        return Instantiate<Trap>( trapPrefabs_[ random_.Next() % (int)TrapType.TypeNum ] );
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    System.Random random_ = new System.Random( 0 );
}
