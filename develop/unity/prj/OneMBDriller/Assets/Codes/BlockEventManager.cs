using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ブロックイベント
//  破壊したブロックに対応したイベントを発生させる
public class BlockEventManager : MonoBehaviour
{
    [SerializeField]
    EnemyBullet01 enemyBullet01Pref_;

    [SerializeField]
    EnemyBullet02 enemyBullet02Pref_;

    // イベント発生
    public void emitEvent( Block block ) {
        var idx = block.getIdx();
        var emitPos = new Vector3( idx.x + 0.5f, 0.0f, idx.y + 0.5f );
        // var b = PrefabUtil.createInstance( enemyBullet01Pref_, null );
        var b = PrefabUtil.createInstance( enemyBullet02Pref_, null );
        b.transform.position = emitPos;
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
