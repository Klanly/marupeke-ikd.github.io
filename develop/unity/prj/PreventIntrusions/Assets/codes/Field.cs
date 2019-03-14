using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// フィールド

public class Field : MonoBehaviour {

	[SerializeField]
	FieldPlate platePrefab_;

	[SerializeField]
	Barricade barricadePrefab_;

	[SerializeField]
	Transform fieldRoot_;

	public class Param {
		public Vector2Int region_ = new Vector2Int( 10, 8 );
		public Vector2Int playerPos_ = new Vector2Int( 4, 4 );
	}

	public void setup(Param param) {
		param_ = param;

		// フィールドプレート敷き詰め
		for ( int y = 0; y < param_.region_.y; ++y ) {
			for ( int x = 0; x < param_.region_.x; ++x ) {
				var plate = Instantiate<FieldPlate>( platePrefab_ );
				plate.transform.parent = fieldRoot_;
				plate.transform.localPosition = new Vector3( x, 0, y );
				plate.setup( FieldPlate.FieldType.Conclete, Random.Range( 0, 16 ) );
			}
		}

		// バリケードテスト
		for ( int x = 0; x < param_.region_.x; ++x ) {
			int idx = Random.Range( 0, param_.region_.y + 1 );
			var barri = Instantiate<Barricade>( barricadePrefab_ );
			barri.transform.parent = fieldRoot_;
			barri.transform.localPosition = new Vector3( 0.5f + x, 0.0f, idx );
			idx = ( idx + Random.Range( 1, param_.region_.y ) ) % param_.region_.y;
			barri = Instantiate<Barricade>( barricadePrefab_ );
			barri.transform.parent = fieldRoot_;
			barri.transform.localPosition = new Vector3( 0.5f + x, 0.0f, idx );
		}
		for ( int y = 0; y < param_.region_.y; ++y ) {
			int idx = Random.Range( 0, param_.region_.x + 1 );
			var barri = Instantiate<Barricade>( barricadePrefab_ );
			barri.transform.parent = fieldRoot_;
			barri.transform.localPosition = new Vector3( idx, 0.0f, 0.5f + y );
			barri.transform.localRotation = Quaternion.Euler( 0.0f, 90.0f, 0.0f );
			idx = ( idx + Random.Range( 1, param_.region_.x ) ) % param_.region_.x;
			barri = Instantiate<Barricade>( barricadePrefab_ );
			barri.transform.parent = fieldRoot_;
			barri.transform.localPosition = new Vector3( idx, 0.0f, 0.5f + y );
			barri.transform.localRotation = Quaternion.Euler( 0.0f, 90.0f, 0.0f );
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	Param param_;
}
