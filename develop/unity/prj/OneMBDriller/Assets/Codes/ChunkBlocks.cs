using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// チャンク内ブロック敷き詰め
public class ChunkBlocks : MonoBehaviour
{
    [SerializeField]
    BlockUnit blockPrefb_;

    [SerializeField]
    int size_ = 8;

    [SerializeField]
    float blockSize_ = 1.0f;

    private void Awake() {
        for ( int y = 0; y < size_; ++y ) {
            for ( int x = 0; x < size_; ++x ) {
                var obj = PrefabUtil.createInstance( blockPrefb_, transform );
                obj.transform.localPosition = new Vector3( x * blockSize_, 0.0f, y * blockSize_ );
                Block b = new Block();
                b.type_ = Block.Type.Juel0;
                obj.setBlock( b );
            }
        }
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
