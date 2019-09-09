using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 牌を生成
public class PaiGenerator : MonoBehaviour
{
    [SerializeField]
    PaiObject paiPrefab_;

    [SerializeField]
    List< Texture > paiTextures_;

    // ランダムな牌を生成
    public PaiObject createRandom() {
        var pai = PrefabUtil.createInstance( paiPrefab_ );
        int paiIdx = Random.Range( 0, paiTextures_.Count );
        Texture tex = paiTextures_[ paiIdx ];
        pai.setup( paiIdx, tex );
        return pai;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
