using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    Speaker speakerPrefab_;

    [SerializeField]
    Transform root_;

    [SerializeField]
    Player player0_;

    // 合致したスピーカーをゲット
    public void getSpeakers( Player player, Speaker[] speakers ) {
        // スピーカーを取り除く
        string name = speakers[ 0 ].getSEName();
        if ( speakerSet_.ContainsKey( name ) == false )
            return;
        speakerSet_[ name ][ 0 ].removeAction( () => {
            speakerSet_[ name ][ 0 ].gameObject.SetActive( false );
        } );
        speakerSet_[ name ][ 1 ].removeAction( () => {
            speakerSet_[ name ][ 1 ].gameObject.SetActive( false );
        } );
    }

    void Start () {
        // SEデータの一括読み込み
        var soundDataNum = Sound_data.getInstance().getRowNum();
        for ( int i = 0; i < soundDataNum; ++i ) {
            var param = Sound_data.getInstance().getParamFromIndex( i );
            SoundAccessor.getInstance().loadSE( "Sounds/" + param.filename_, param.name_ );
        }

        // プレイヤー設定
        player0_.setup( this );

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
            // スピーカーをSEの数×2個散りばめる
            var soundDataNum = Sound_data.getInstance().getRowNum();
            var cd = new CardDistributer();
            var poses = cd.create( soundDataNum * 2, 0.17f, 0.22f, 0.05f );

            int e = 0;
            for ( int i = 0; i < soundDataNum; ++i ) {
                var param = Sound_data.getInstance().getParamFromIndex( i );
                Speaker[] speakers = new Speaker[ 2 ];
                for ( int j = 0; j < 2; ++j ) {
                    var speaker = Instantiate<Speaker>( parent_.speakerPrefab_ );
                    speaker.transform.parent = parent_.root_;
                    speaker.transform.localPosition = poses[ e ];
                    speaker.transform.localRotation = Quaternion.Euler( 0.0f, Random.Range( 0.0f, 360.0f ), 0.0f );
                    speaker.setSE( param.name_ );
                    speakers[ j ] = speaker;
                    e++;
                }
                parent_.speakerSet_[ param.name_ ] = speakers;
            }
            return this;
        }
    }

    State state_;
    Dictionary<string, Speaker[]> speakerSet_ = new Dictionary<string, Speaker[]>();
}
