using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// フィールド

public class Field : MonoBehaviour {

	[SerializeField]
	protected FieldPlate platePrefab_;

	[SerializeField]
	protected Barricade barricadePrefab_;

	[SerializeField]
	protected Transform fieldRoot_;

	[SerializeField]
	Test testPrefab_;
	Test test_;

	public System.Action AllRegionStockadeCallback { set { allRegionStockadeCallback_ = value; } }

	public class Param {
		public Vector2Int region_ = new Vector2Int( 10, 8 );
		public Vector2Int playerPos_ = new Vector2Int( 4, 4 );
		public int maxBarricadeNum_ = 2;
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
		stcChecker_.setup( param.region_ );
		var walls = stcChecker_.Walls;

		// フィールドプレート敷き詰め
		plates_ = new FieldPlate[ param_.region_.x , param_.region_.y ];
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
		var indicesX = new List<int>();
		var indicesY = new List<int>();
		ListUtil.numbering( ref indicesX, param_.region_.x + 1 );
		ListUtil.numbering( ref indicesY, param_.region_.y + 1 );
		ListUtil.shuffle( ref indicesX );
		for ( int x = 0; x < param_.region_.x; ++x ) {
			ListUtil.shuffle( ref indicesY );
			int addBarricade = Random.Range( 0, param_.maxBarricadeNum_ - 1 );
			for ( int n = 0; n < 2 + addBarricade; ++n ) {
				int idx = indicesY[ n ];
				var barri = Instantiate<Barricade>( barricadePrefab_ );
				barri.transform.parent = fieldRoot_;
				barri.transform.localPosition = new Vector3( 0.5f + x, 0.0f, idx );
				hBarricades_[ x, idx ] = barri;
				walls.setWall( StockadeChecker.Wall.WallOrder.Horizontal, x, idx, 1 );
			}
		}
		for ( int y = 0; y < param_.region_.y; ++y ) {
			ListUtil.shuffle( ref indicesX );
			int addBarricade = Random.Range( 0, 2 );
			for ( int n = 0; n < 2 + addBarricade; ++n ) {
				int idx = indicesX[ n ];
				var barri = Instantiate<Barricade>( barricadePrefab_ );
				barri.transform.parent = fieldRoot_;
				barri.transform.localPosition = new Vector3( idx, 0.0f, 0.5f + y );
				barri.transform.localRotation = Quaternion.Euler( 0.0f, 90.0f, 0.0f );
				vBarricades_[ idx, y ] = barri;
				walls.setWall( StockadeChecker.Wall.WallOrder.Vertical, idx, y, 1 );
			}
		}

		updateBarricadeState();

		test_ = Instantiate<Test>( testPrefab_ );
		test_.transform.localPosition = new Vector3( 10.0f, 0.0f, 0.0f );
		test_.setup( param_.region_, stcChecker_ );
	}

	// フィールド領域を取得
	public Vector2Int getRegion() {
		return param_.region_;
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
	public bool moveBarricade(Vector2Int pos, KeyCode key, KeyCode dir, float sec, System.Action finishCallback ) {
		Vector2Int elem = Vector2Int.zero;
		var barricade = getBarricadeOnCell( pos, key, ref elem );
		if ( barricade == null ) {
			finishCallback();
			return false;
		}
		var offset = KeyHelper.offset( dir );
		System.Action offsetter = null;
		var walls = stcChecker_.Walls;
		switch ( dir ) {
		case KeyCode.LeftArrow:
				offsetter = () => {
					vBarricades_[ elem.x, elem.y ] = null;
					vBarricades_[ elem.x - 1, elem.y ] = barricade;
					walls.setWall( StockadeChecker.Wall.WallOrder.Vertical, elem.x, elem.y, 0 );
					walls.setWall( StockadeChecker.Wall.WallOrder.Vertical, elem.x - 1, elem.y, 1 );
				};
				break;
		case KeyCode.RightArrow:
				offsetter = () => {
					vBarricades_[ elem.x, elem.y ] = null;
					vBarricades_[ elem.x + 1, elem.y ] = barricade;
					walls.setWall( StockadeChecker.Wall.WallOrder.Vertical, elem.x, elem.y, 0 );
					walls.setWall( StockadeChecker.Wall.WallOrder.Vertical, elem.x + 1, elem.y, 1 );
				};
				break;
		case KeyCode.DownArrow:
				offsetter = () => {
					hBarricades_[ elem.x, elem.y ] = null;
					hBarricades_[ elem.x, elem.y - 1 ] = barricade;
					walls.setWall( StockadeChecker.Wall.WallOrder.Horizontal, elem.x, elem.y, 0 );
					walls.setWall( StockadeChecker.Wall.WallOrder.Horizontal, elem.x, elem.y - 1, 1 );
				};
				break;
		case KeyCode.UpArrow:
				offsetter = () => {
					hBarricades_[ elem.x, elem.y ] = null;
					hBarricades_[ elem.x, elem.y + 1 ] = barricade;
					walls.setWall( StockadeChecker.Wall.WallOrder.Horizontal, elem.x, elem.y, 0 );
					walls.setWall( StockadeChecker.Wall.WallOrder.Horizontal, elem.x, elem.y + 1, 1 );
				};
				break;
		}
		var len = new Vector3( offset.x, 0.0f, offset.y );
		var start = barricade.transform.localPosition;
		var end = start + len;
		GlobalState.time( sec, (_sec, t) => {
			barricade.transform.localPosition = Lerps.Vec3.linear( start, end, t );
			return true;
		} ).finish(()=> {
			offsetter();
			finishCallback();
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

	// 指定座標が囲い位置内？
	public bool isStockadePos( Vector2Int pos ) {
		if ( isValidateCoord( pos ) == false )
			return false;
		int id = floorIds_[ pos.x, pos.y ];
		return ( id != 0 && completeFloorIdList_[ id ] == true );
	}

	public bool isAllRegionStockaded() {
		for ( int x = 0; x < param_.region_.x; ++x ) {
			for ( int y = 0; y < param_.region_.y; ++y ) {
				if ( isStockadePos( new Vector2Int( x, y ) ) == false )
					return false;
			}
		}
		return true;
	}

	// 敵を囲ったかチェック
	public void checkEnemyStockade() {
		updateBarricadeState();

		// 全エネミーに対して周囲バリケードの検索を命令
		for ( var node = enemies_.First; node != null; ) {
			var e = node.Value;
			if ( e.checkStockade( floorIds_, completeFloorIdList_ ) == true ) {
				// 囲まれているので、エネミーを削除
				objectPoses_[ e.Pos.x, e.Pos.y ] = 0;
				e.toDestroy();
				var deleteNode = node;
				node = node.Next;
				enemies_.Remove( deleteNode );
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

	// バリケードの囲い状態を更新
	protected void updateBarricadeState() {
		floorIds_ = stcChecker_.check( ref completeFloorIdList_ );
		for ( int x = 0; x < param_.region_.x; ++x ) {
			for ( int y = 0; y < param_.region_.y; ++y ) {
				int id = floorIds_[ x, y ];
				if ( completeFloorIdList_[ id ] == true )
					plates_[ x, y ].setColor( Color.red );
				else
					plates_[ x, y ].setColor( Color.white );
			}
		}

		// もし全領域を囲えていたら報告
		if ( isAllRegionStockaded()  == true ) {
			if ( allRegionStockadeCallback_ != null )
				allRegionStockadeCallback_();
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
#if false
		// バリケード整合性チェック	
		if ( param_ == null || hBarricades_ == null || vBarricades_ == null ) {
			return;
		}
		for ( int x = 0; x < param_.region_.x; ++x ) {
			int counter = 0;
			for ( int y = 0; y < param_.region_.y + 1; ++y ) {
				if ( hBarricades_[ x, y ] != null )
					counter++;
			}
			if ( counter != 2 ) {
				Debug.LogWarning( "Barricade Error! hBarricade_[" + x + ",*]" );
				break;
			}
		}
		for ( int y = 0; y < param_.region_.y; ++y ) {
			int counter = 0;
			for ( int x = 0; x < param_.region_.x + 1; ++x ) {
				if ( vBarricades_[ x, y ] != null )
					counter++;
			}
			if ( counter != 2 ) {
				Debug.LogWarning( "Barricade Error! vBarricade_[*," + y + "]" );
				break;
			}
		}
#endif
	}

	private void OnDrawGizmos() {
		if ( param_ == null || floorIds_ == null || stcChecker_ == null )
			return;

		var wall = stcChecker_.Walls;
		if ( wall == null )
			return;

		for ( int x = 0; x < param_.region_.x; ++x ) {
			for ( int y = 0; y < param_.region_.y + 1; ++y ) {
				var id = wall.getWall( StockadeChecker.Wall.WallOrder.Horizontal, x, y );
				if ( id != 0 ) {
					Gizmos.DrawLine( new Vector3( x, 2.0f, y ), new Vector3( x + 1, 2.0f, y ) );
				}
			}
		}
		for ( int x = 0; x < param_.region_.x + 1; ++x ) {
			for ( int y = 0; y < param_.region_.y; ++y ) {
				var id = wall.getWall( StockadeChecker.Wall.WallOrder.Vertical, x, y );
				if ( id != 0 ) {
					Gizmos.DrawLine( new Vector3( x, 2.0f, y ), new Vector3( x, 2.0f, y + 1 ) );
				}
			}
		}
	}

	class Edge {
		public Barricade barricade_ = null;	// L,R,D,U
	}

	protected Barricade[,] hBarricades_;    // 水平バリケード
	protected Barricade[,] vBarricades_;	// 垂直バリケード
	protected Param param_;
	LinkedList<Enemy> enemies_ = new LinkedList<Enemy>();
	protected int[,] objectPoses_;
	protected StockadeChecker stcChecker_ = new StockadeChecker();
	protected FieldPlate[,] plates_;
	int[,] floorIds_;
	List<bool> completeFloorIdList_ = new List<bool>();
	System.Action allRegionStockadeCallback_;

	static int typeEnemy_g = 2;
	static int typePlayer_g = 1;
}
