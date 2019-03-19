using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	[SerializeField]
	GameObject hWallPrefab_;

	[SerializeField]
	GameObject vWallPrefab_;

	[SerializeField]
	TestFloor floorPrefab_;

	[SerializeField]
	TestFloor compFloorPrefab_;

	public void setup(Vector2Int region, StockadeChecker checker ) {
		bInitialized_ = true;
		checker_ = checker;
		region_ = region;
	}

	// Use this for initialization
	void Start () {
		if ( bInitialized_ == false ) {
			checker_.setup( region_ );
			var walls = checker_.Walls;
			string[] dirs = new string[] { "L", "R", "U", "D" };
			int r = 250;
			for ( int i = 0; i < r; ++i ) {
				addWall( Random.Range( 0, region_.x ), Random.Range( 0, region_.y ), dirs[ Random.Range( 0, 4 ) ] );
			}
		}
		updateWalls();
		check();
		updateWalls();
	}

	void addWall( int x, int y, string dir ) {
		var walls = checker_.Walls;
		switch (dir) {
		case "L":
			walls.setWall( x, y, StockadeChecker.Wall.WallDir.Left, 1 );
			break;
		case "R":
			walls.setWall( x, y, StockadeChecker.Wall.WallDir.Right, 1 );
			break;
		case "D":
			walls.setWall( x, y, StockadeChecker.Wall.WallDir.Down, 1 );
			break;
		case "U":
			walls.setWall( x, y, StockadeChecker.Wall.WallDir.Up, 1 );
			break;
		}
	}

	void check() {
		foreach( var f in floors_ ) {
			Destroy( f );
		}
		var completeStockadeList = new List<bool>();
		var region = checker_.check( ref completeStockadeList );
		var walls = checker_.Walls;

#if false
		// 全て完璧にするために壁を境に向かい合うフロアの
		// IDが一緒の場合、その壁を取り除く
		var dirs = new StockadeChecker.Wall.WallDir[] {
			StockadeChecker.Wall.WallDir.Up,
			StockadeChecker.Wall.WallDir.Right,
			StockadeChecker.Wall.WallDir.Down,
			StockadeChecker.Wall.WallDir.Left,
		};
		Vector2Int[] moveDirs = new Vector2Int[] {
			new Vector2Int( 0, 1 ),		// UP
			new Vector2Int( 1, 0 ),		// Right
			new Vector2Int( 0, -1 ),	// Down
			new Vector2Int( -1, 0 )		// Left
		};
		for (int y = 0; y < region.GetLength( 1 ); ++y) {
			for (int x = 0; x < region.GetLength( 0 ); ++x) {
				int floorId = region[ x, y ];
				for ( int i = 0; i < 4; ++i ) {
					var id = walls.getWall( x, y, dirs[ i ] );
					int sx = x + moveDirs[ i ].x;
					int sy = y + moveDirs[ i ].y;
					if ( sx >= 0 && sx < region.GetLength( 0 ) && sy >= 0 && sy < region.GetLength( 1 ) ) {
						int tgtId = region[ sx, sy ];
						if (floorId == tgtId) {
							walls.setWall( x, y, dirs[ i ], 0 );
						}
					}
				}
			}
		}
		region = checker_.check( ref completeStockadeList );
#endif

		// スペースごとに塗り分け
		Color[] colors = new Color[] {
			Color.red,
			Color.blue,
			Color.green,
			Color.yellow,
		};
		for ( int y = 0; y < region.GetLength(1); ++y ) {
			for ( int x = 0; x < region.GetLength(0); ++x ) {
				TestFloor f = null;
				if ( completeStockadeList[ region[ x, y ] ] == true )
					f = Instantiate<TestFloor>( compFloorPrefab_ );
				else
					f = Instantiate<TestFloor>( floorPrefab_ );
				f.transform.localPosition = new Vector3( x, y, 0.0f );
				f.setColor( colors[ region[ x, y ] % colors.Length ] );
				floors_.Add( f.gameObject );
			}
		}
	}

	// Update is called once per frame
	void Update () {
	}

	void updateWalls() {
		foreach( var w in walls_ ) {
			Destroy( w );
		}

		var walls = checker_.Walls;

		for ( int x = 0; x< region_.x + 1; ++x ) {
			for ( int y = 0; y < region_.y; ++y ) {
				int id = walls.getWall( StockadeChecker.Wall.WallOrder.Vertical, x, y );
				if ( id > 0 ) {
					var wall = Instantiate<GameObject>( vWallPrefab_ );
					walls_.Add( wall );
					wall.transform.localPosition = new Vector3( x, y, 0 );
				}
			}
		}
		for (int x = 0; x < region_.x; ++x) {
			for (int y = 0; y < region_.y + 1; ++y) {
				int id = walls.getWall( StockadeChecker.Wall.WallOrder.Horizontal, x, y );
				if (id > 0) {
					var wall = Instantiate<GameObject>( hWallPrefab_ );
					walls_.Add( wall );
					wall.transform.localPosition = new Vector3( x, y, 0 );
				}
			}
		}
	}

	void OnDrawGizmos() {
		if (checker_ == null || checker_.Walls == null )
			return;

		// 格子
		Gizmos.color = Color.white;
		Vector2Int region = region_;
		for ( int x = 0; x < region.x + 1; ++x ) {
			Gizmos.DrawLine( new Vector3( x, 0, 0 ), new Vector3( x, region.y, 0 ) );
		}
		for (int y = 0; y < region.y + 1; ++y) {
			Gizmos.DrawLine( new Vector3( 0, y, 0 ), new Vector3( region.x, y, 0 ) );
		}

		// 壁
		Gizmos.color = Color.red;
		var walls = checker_.Walls;
		for ( int y = 0; y < region.y; ++y ) {
			for ( int x = 0; x < region.x; ++x ) {
				int id = 0;
				id = walls.getWall( x, y, StockadeChecker.Wall.WallDir.Left );
				if ( id > 0 )
					Gizmos.DrawLine( new Vector3( x, y, 0 ), new Vector3( x, y + 1, 0 ) );
				id = walls.getWall( x, y, StockadeChecker.Wall.WallDir.Right );
				if (id > 0)
					Gizmos.DrawLine( new Vector3( x + 1, y, 0 ), new Vector3( x + 1, y + 1, 0 ) );
				id = walls.getWall( x, y, StockadeChecker.Wall.WallDir.Down );
				if (id > 0)
					Gizmos.DrawLine( new Vector3( x, y, 0 ), new Vector3( x + 1, y, 0 ) );
				id = walls.getWall( x, y, StockadeChecker.Wall.WallDir.Up );
				if (id > 0)
					Gizmos.DrawLine( new Vector3( x, y + 1, 0 ), new Vector3( x + 1, y + 1, 0 ) );
			}
		}
	}

	StockadeChecker checker_ = new StockadeChecker();
	List<GameObject> walls_ = new List<GameObject>();
	List<GameObject> floors_ = new List<GameObject>();
	Vector2Int region_ = new Vector2Int( 16, 16 );
	bool bInitialized_ = false;
}
