using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ブロック1個動作管理
public class BlockUnit : MonoBehaviour
{
    [SerializeField]
    GameObject diamond_;

    [SerializeField]
    GameObject sapphire_;

    public void setBlock( Block block ) {
        allBlockOff();
        switch ( block.type_ ) {
            case Block.Type.Juel0:
                diamond_.SetActive( true );
                break;
            case Block.Type.Juel1:
                sapphire_.SetActive( true );
                break;
        }
    }

    void allBlockOff() {
        diamond_.SetActive( false );
        sapphire_.SetActive( false );
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
