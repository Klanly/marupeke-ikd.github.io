using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [SerializeField]
    GameManager gameManagerPrefab_;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if ( gameManager_ == null ) {
            gameManager_ = PrefabUtil.createInstance( gameManagerPrefab_, null );
            gameManager_.FinishCallback = () => {
                Destroy( gameManager_.gameObject );
                gameManager_ = null;
            };
        }
    }

    GameManager gameManager_;
}
