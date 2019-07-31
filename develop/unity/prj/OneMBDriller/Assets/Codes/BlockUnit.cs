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

    [SerializeField]
    GameObject block_;

    public void setBlock( Block block ) {
        allBlockOff();
        if ( block == null )
            return;

        switch ( block.type_ ) {
            case Block.Type.Juel0:
                diamond_.SetActive( true );
                break;
            case Block.Type.Juel1:
                sapphire_.SetActive( true );
                break;
            case Block.Type.Trap0:
                block_.SetActive( true );
                break;
            case Block.Type.Trap1:
                block_.SetActive( true );
                break;
        }
    }

    public void allBlockOff() {
        diamond_.SetActive( false );
        sapphire_.SetActive( false );
        block_.SetActive( false );
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
