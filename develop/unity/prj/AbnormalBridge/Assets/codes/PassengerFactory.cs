using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 渡る物を作る人

public class PassengerFactory : MonoBehaviour {

    [SerializeField]
    Human humanPrefab_;

    [SerializeField]
    Ship shipPrefab_;

    [SerializeField]
    bool debugCreate_ = false;

    [SerializeField]
    Passenger.Type debugCreatePassenger_ = Passenger.Type.Human_Walk;

    // 生成
    public Passenger create( Passenger.Type type )
    {
        switch ( type ) {
            case Passenger.Type.Human_Walk: {
                    Human human = Instantiate<Human>( humanPrefab_ );
                    human.setState( Human.ActionState.State_Walk );
                    return human;
                }
            case Passenger.Type.Human_Run: {
                    Human human = Instantiate<Human>( humanPrefab_ );
                    human.setState( Human.ActionState.State_Run );
                    return human;
                }
            case Passenger.Type.Ship: {
                    Ship ship = Instantiate<Ship>( shipPrefab_ );
                    return ship;
                }
        }
        return null;
    }

    private void Update()
    {
        if ( debugCreate_ == true ) {
            debugCreate_ = false;
            create( debugCreatePassenger_ );
        }
    }
}
