using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    Bridge[] bridges_;

    [SerializeField]
    UnityEngine.UI.Button[] oneShotButtons_;

    [SerializeField]
    HoldButton[] holdButtons_;

    [SerializeField]
    PassengerFactory passengerFactory_;

    [SerializeField]
    UnityEngine.UI.Image shipWarningUpImage_;

    [SerializeField]
    UnityEngine.UI.Image shipWarningDownImage_;

    [SerializeField]
    bool debugCreateHuman_ = false;

    [SerializeField]
    Human.Type debugHumanType_ = Human.Type.Human_Walk;

    [SerializeField]
    bool debugCreateShip_ = false;

    [SerializeField]
    int debugCreateLine_ = 0;

    // 衝突している橋を取得
    public Bridge getCollideBridge( Vector3 pos )
    {
        foreach ( var b in bridges_ ) {
           if ( b.isCollide( pos ) == true ) {
                return b;
            }
        }
        return null;
    }

    // 船接近警告をON
    public void warningShipApproaching( bool isUpper )
    {
        if (isUpper == true) {
            shipWarningUpBrinker_.reset();
            shipWarningUpBrinker_.setActive( true );
        } else {
            shipWarningDownBrinker_.reset();
            shipWarningDownBrinker_.setActive( true );
        }
    }

    // Use this for initialization
    void Start () {
        // ボタンと橋の挙動を関連付け
        oneShotButtons_[ 0 ].onClick.AddListener( () => {
            bridges_[ 0 ].switchOn();
        } );
        holdButtons_[ 0 ].OnPush = () => {
            bridges_[ 1 ].switchOn();
        };
        oneShotButtons_[ 1 ].onClick.AddListener( () => {
            bridges_[ 2 ].switchOn();
        } );
        holdButtons_[ 1 ].OnPush = () => {
            bridges_[ 3 ].switchOn();
        };

        // 船接近警告イメージ
        shipWarningUpBrinker_.setup( shipWarningUpImage_, 0.25f, 0.10f, 10, true, false );
        shipWarningDownBrinker_.setup( shipWarningDownImage_, 0.25f, 0.10f, 10, true, false );
    }

    // Update is called once per frame
    void Update () {
        if ( debugCreateHuman_ == true ) {
            debugCreateHuman_ = false;
            var human = passengerFactory_.create( debugHumanType_ );
            human.setup( this );
            float posZ = Random.Range( -2.0f, 2.0f );
            float posX = -60.0f;
            if ( debugCreateLine_ != 0 ) {
                var h = human as Human;
                h.setMoveDir( false );
                posX = 60.0f;
                posZ -= 30.0f;
            }
            var p = human.transform.position;
            p.x = posX;
            p.z = posZ;
            human.transform.position = p;
        }
        if ( debugCreateShip_ == true ) {
            debugCreateShip_ = false;
            var ship = passengerFactory_.create(Passenger.Type.Ship );
            ship.setup( this );
        }

        // 船接近警告ブリンカー
        shipWarningUpBrinker_.update();
        shipWarningDownBrinker_.update();
    }

    ImageBrinker shipWarningUpBrinker_ = new ImageBrinker();
    ImageBrinker shipWarningDownBrinker_ = new ImageBrinker();
}
