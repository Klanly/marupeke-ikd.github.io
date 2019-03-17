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

		hBarricades_ = new Barricade[ param_.region_.x, param_.region_.y + 1 ];
		vBarricades_ = new Barricade[ param_.region_.x + 1, param_.region_.y ];
		objectPoses_ = new int[ param_.region_.x, param_.region_.y ];
		for ( int x = 0; x < param_.region_.x; ++x ) {
			for ( int y = 0; y < param_.region_.y; ++y ) {
				objectPoses_[ x, y ] = 0;
			}
		}

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
			hBarricades_[ x, idx ] = barri;

			idx = ( idx + Random.Range( 1, param_.region_.y ) ) % param_.region_.y;
			barri = Instantiate<Barricade>( barricadePrefab_ );
			barri.transform.parent = fieldRoot_;
			barri.transform.localPosition = new Vector3( 0.5f + x, 0.0f, idx );
			hBarricades_[ x, idx ] = barri;
		}
		for ( int y = 0; y < param_.region_.y; ++y ) {
			int idx = Random.Range( 0, param_.region_.x + 1 );
			var barri = Instantiate<Barricade>( barricadePrefab_ );
			barri.transform.parent = fieldRoot_;
			barri.transform.localPosition = new Vector3( idx, 0.0f, 0.5f + y );
			barri.transform.localRotation = Quaternion.Euler( 0.0f, 90.0f, 0.0f );
			vBarricades_[ idx, y ] = barri;

			idx = ( idx + Random.Range( 1, param_.region_.x ) ) % param_.region_.x;
			barri = Instantiate<Barricade>( barricadePrefab_ );
			barri.transform.parent = fieldRoot_;
			barri.transform.localPosition = new Vector3( idx, 0.0f, 0.5f + y );
			barri.transform.localRotation = Quaternion.Euler( 0.0f, 90.0f, 0.0f );
			vBarricades_[ idx, y ] = barri;
		}
	}

	// 敵を登録
	public bool addEnemy( Enemy enemy, Vector2Int initPos ) {
		if ( isSpace( initPos ) == false )
			return false;
		enemy.transform.parent = fieldRoot_;
		enemies_.AddLast( enemy );
		objectPoses_[ initPos.x, initPos.y ] = typeEnemy_g;
		return true;
	}

	public Barricade getBarricadeOnCell( Vector2Int pos, KeyCode key, ref Vector2Int elem ) {
		if ( pos.x < 0 || pos.y < 0 || pos.x >= param_.region_.x || pos.y >= param_.region_.y )
			return null;
		switch ( key ) {
		case KeyCode.RightArrow:
				elem.x = pos.x + 1;
				elem.y = pos.y;
				return vBarricades_[ pos.x + 1, pos.y ];
		case KeyCode.LeftArrow:
				elem.x = pos.x;
				elem.y = pos.y;
				return vBarricades_[ pos.x, pos.y ];	
		case KeyCode.UpArrow:
				elem.x = pos.x;
				elem.y = pos.y + 1;
				return hBarricades_[ pos.x, pos.y + 1 ];
		case KeyCode.DownArrow:
				elem.x = pos.x;
				elem.y = pos.y;
				return hBarricades_[ pos.x, pos.y ];	
		}
		return null;
	}

	// 指定位置は有効？
	public bool isValidateCoord( Vector2Int pos ) {
		return !( pos.x < 0 || pos.y < 0 || pos.x >= param_.region_.x || pos.y >= param_.region_.y );
	}

	// 指定位置にスペースがある？
	public bool isSpace( Vector2Int pos ) {

		if ( isValidateCoord( pos ) == false )
			return false;

		if ( objectPoses_[ pos.x, pos.y ] != 0 )
			return false;

		return true;
	}

	// バリケードを動かす
	public bool moveBarricade(Vector2Int pos, KeyCode key, KeyCode dir, float sec ) {
		Vector2Int elem = Vector2Int.zero;
		var barricade = getBarricadeOnCell( pos, key, ref elem );
		if ( barricade == null )
			return false;
		var offset = KeyHelper.offset( dir );
		System.Action offsetter = null;
		switch ( dir ) {
		case KeyCode.LeftArrow:
				offsetter = () => {
					vBarricades_[ elem.x, elem.y ] = null;
					vBarricades_[ elem.x - 1, elem.y ] = barricade;
				};
				break;
		case KeyCode.RightArrow:
				offsetter = () => {
					vBarricades_[ elem.x, elem.y ] = null;
					vBarricades_[ elem.x + 1, elem.y ] = barricade;
				};
				break;
		case KeyCode.DownArrow:
				offsetter = () => {
					hBarricades_[ elem.x, elem.y ] = null;
					hBarricades_[ elem.x, elem.y - 1 ] = barricade;
				};
				break;
		case KeyCode.UpArrow:
				offsetter = () => {
					hBarricades_[ elem.x, elem.y ] = null;
					hBarricades_[ elem.x, elem.y + 1 ] = barricade;
				};
				break;
		}
		var len = new Vector3( offset.x, 0.0f, offset.y );
		var start = barricade.transform.localPosition;
		var end = start + len;
		GlobalState.time( sec, (_sec, t) => {
			barricade.transform.localPosition = Lerps.Vec3.linear( start, end, t );
			offsetter();
			return true;
		} );
		return true;
	}

	// プレート横数を取得
	public int getWidth() {
		return param_.region_.x;
	}

	// プレート縦数を取得
	public int getHeight() {
		return param_.region_.y;
	}

	// 敵を囲ったかチェック
	public void checkEnemyStockade() {
		// 全エネミーに対して周囲バリケードの検索を命令
		for ( var node = enemies_.First; node != null; ) {
			var e = node.Value;
			if ( e.checkStockade() == true ) {
				// 囲まれているので、エネミーを削除
				e.toDestroy();
				node = node.Next;
				enemies_.Remove( node );
			} else {
				node = node.Next;
			}
		}
	}

	// 敵の整数座標を更新
	public void updateEnemyPos( Enemy enemy, Vector2Int prePos, Vector2Int pos ) {
		objectPoses_[ prePos.x, prePos.y ] = 0;
		objectPoses_[ pos.x, pos.y ] = typeEnemy_g;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	class Edge {
		public Barricade barricade_ = null;	// L,R,D,U
	}

	Barricade[,] hBarricades_;	// 水平バリケード
	Barricade[,] vBarricades_;	// 垂直バリケード
	Param param_;
	LinkedList<Enemy> enemies_ = new LinkedList<Enemy>();
	int[,] objectPoses_;

	static int typeEnemy_g = 2;
	static int typePlayer_g = 1;
}
