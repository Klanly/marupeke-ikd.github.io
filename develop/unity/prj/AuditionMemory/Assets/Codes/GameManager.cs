using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    Speaker speakerPrefab_;

    [SerializeField]
    Transform root_;

    void Start () {
        SoundAccessor.getInstance().loadSE( "Sounds/catvoice", "cat" );
        state_ = new Setup( this );
	}
	
	// Update is called once per frame
	void Update () {
        if ( state_ != null )
            state_ = state_.update();
	}

    class Setup : State< GameManager > {
        public Setup( GameManager parent ) : base( parent ) {

        }
        protected override State innerInit() {
            // スピーカーを52個散りばめる
            for ( int i = 0; i < 52; ++i ) {
                var speaker = Instantiate<Speaker>( parent_.speakerPrefab_ );
                speaker.transform.parent = parent_.root_;
                speaker.transform.localPosition = new Vector3( Random.Range( -4.0f, 4.0f ), 0.0f, Random.Range( -4.0f, 4.0f ) );
                speaker.transform.localRotation = Quaternion.Euler( 0.0f, Random.Range( 0.0f, 360.0f ), 0.0f );
            }
            return this;
        }
    }

    State state_;
}
