using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTableManager : MonoBehaviour
{
    [SerializeField]
    TurnTable turnTable_;


    public void setup(TurnTable.Param param ) {
        turnTable_.setup( param );
    }

    public bool setNext( System.Action<Card.Param> finishCallback ) {
        return turnTable_.turnNext( finishCallback );
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
