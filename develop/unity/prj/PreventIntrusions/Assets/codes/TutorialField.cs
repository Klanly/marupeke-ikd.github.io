using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialField : Field {

	public void setup( Param param, int[,] hBarricade, int[,] vBarricade ) {
		param_ = param;

		hBarricades_ = new Barricade[ param_.region_.x, param_.region_.y + 1 ];
		vBarricades_ = new Barricade[ param_.region_.x + 1, param_.region_.y ];
		objectPoses_ = new int[ param_.region_.x, param_.region_.y ];
		for ( int x = 0; x < param_.region_.x; ++x ) {
			for ( int y = 0; y < param_.region_.y; ++y ) {
				objectPoses_[ x, y ] = 0;
			}
		}
		stcChecker_.setup( param.region_ );
		var walls = stcChecker_.Walls;

		// フィールドプレート敷き詰め
		plates_ = new FieldPlate[ param_.region_.x, param_.region_.y ];
		for ( int y = 0; y < param_.region_.y; ++y ) {
			for ( int x = 0; x < param_.region_.x; ++x ) {
				var plate = Instantiate<FieldPlate>( platePrefab_ );
				plate.transform.parent = fieldRoot_;
				plate.transform.localPosition = new Vector3( x, 0, y );
				plate.setup( FieldPlate.FieldType.Conclete, Random.Range( 0, 16 ) );
				plates_[ x, y ] = plate;
			}
		}

		// バリケードテスト
		for ( int x = 0; x < param_.region_.x; ++x ) {
			for ( int y = 0; y < param_.region_.y + 1; ++y ) {
				hBarricades_[ x, y ] = null;
				if ( hBarricade[x,y] != 0 ) {
					var barri = Instantiate<Barricade>( barricadePrefab_ );
					barri.transform.parent = fieldRoot_;
					barri.transform.localPosition = new Vector3( 0.5f + x, 0.0f, y );
					walls.setWall( StockadeChecker.Wall.WallOrder.Horizontal, x, y, 1 );
					hBarricades_[ x, y ] = barri;
				}
			}
		}
		for ( int y = 0; y < param_.region_.y; ++y ) {
			for ( int x = 0; x < param_.region_.x + 1; ++x ) {
				vBarricades_[ x, y ] = null;
				if ( vBarricade[ x, y ] != 0 ) {
					var barri = Instantiate<Barricade>( barricadePrefab_ );
					barri.transform.parent = fieldRoot_;
					barri.transform.localPosition = new Vector3( x, 0.0f, 0.5f + y );
					barri.transform.localRotation = Quaternion.Euler( 0.0f, 90.0f, 0.0f );
					vBarricades_[ x, y ] = barri;
					walls.setWall( StockadeChecker.Wall.WallOrder.Vertical, x, y, 1 );
				}
			}
		}

		updateBarricadeState();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
