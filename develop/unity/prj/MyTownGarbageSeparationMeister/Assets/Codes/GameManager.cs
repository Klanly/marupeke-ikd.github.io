using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    TurnTableManager turnTableManage_;

    [SerializeField]
    GabageInfoWindow infoWindow_;

    [SerializeField]
    bool debugNext_ = false;

    void setNext() {
        infoWindow_.shrink();
        turnTableManage_.setNext( (param) => {
            infoWindow_.setParam( param );
            infoWindow_.start();
        } );
    }

    // Start is called before the first frame update
    void Start()
    {
        var param = new TurnTable.Param();
        var card0 = new Card.Param();
        card0.name = "ペットボトル";
        card0.material = "PET";
        card0.weight = 10.0f;
        card0.weightUnit = "g";
        card0.dimensionX = 7.0f;
        card0.dimensionY = 25.0f;
        card0.dimensionZ = 7.0f;
        card0.dimensionUnit = "cm";
        param.cards.Add( card0 );

        var card1 = new Card.Param();
        card1.name = "生ごみ";
        card1.material = "野菜の切れ端";
        card1.weight = 60.0f;
        card1.weightUnit = "g";
        card1.dimensionX = 10.0f;
        card1.dimensionY = 8.0f;
        card1.dimensionZ = 7.0f;
        card1.dimensionUnit = "cm";
        param.cards.Add( card1 );

        turnTableManage_.setup( param );
    }

    // Update is called once per frame
    void Update()
    {
        if ( debugNext_ ) {
            debugNext_ = false;
            setNext();
        }
    }
}
