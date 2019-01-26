using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    int astId_ = 1;

    [SerializeField]
    Star starUnitPrefab_;

    [SerializeField]
    AstLine linePrefab_;

    void createAsterism( int astId )
    {
        var d = AsterismDataUtil.getData( astId_ );

        // 恒星
        float radius = 50.0f;

        for ( int i = 0; i < d.stars_.Count; ++i ) {
            var star = d.stars_[ i ];
            var obj = Instantiate<Star>( starUnitPrefab_ );
            var pos = SphereSurfUtil.convPolerToPos( star.pos_.x, star.pos_.y );
            obj.transform.position = pos * radius;
            obj.setHipId( star.hipId_ );
            obj.setPolerCoord( star.pos_.x, star.pos_.y );
        }

        // ライン
        for ( int i = 0; i < d.lines_.Count; ++i ) {
            var line = d.lines_[ i ];
            var obj = Instantiate<AstLine>( linePrefab_ );
            var spos = SphereSurfUtil.convPolerToPos( line.start_.x, line.start_.y );
            var epos = SphereSurfUtil.convPolerToPos( line.end_.x, line.end_.y );
            obj.setLine( spos * radius, epos * radius );
        }
    }

    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if ( preId_ != astId_ ) {
            createAsterism( astId_ );
            preId_ = astId_;
        }
    }

    int preId_ = -1;
}
