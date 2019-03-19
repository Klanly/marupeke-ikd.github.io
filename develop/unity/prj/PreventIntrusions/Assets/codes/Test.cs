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

	// Use this for initialization
	void Start () {
		checker_.setup( region_ );
		var walls = checker_.Walls;
		string[] dirs = new string[] { "L", "R", "U", "D" };
		int r = 30;
		for ( int i = 0; i < r; ++i ) {
			addWall( Random.Range( 0, region_.x ), Random.Range( 0, region_.y ), dirs[ Random.Range( 0, 4 ) ] );
		}
		updateWalls();
		check();
	}

	void addWall( int x, int y, string dir ) {
		var walls = checker_.Walls;
		walls.setWall( 0, 0, StockadeChecker.Wall.WallDir.Right, 1 );
		walls.setWall( 2, 0, StockadeChecker.Wall.WallDir.Right, 1 );
		walls.setWall( 2, 0, StockadeChecker.Wall.WallDir.Up, 1 );
		walls.setWall( 3, 0, StockadeChecker.Wall.WallDir.Up, 1 );
		walls.setWall( 5, 0, StockadeChecker.Wall.WallDir.Up, 1 );
		walls.setWall( 5, 1, StockadeChecker.Wall.WallDir.Left, 1 );
		walls.setWall( 0, 2, StockadeChecker.Wall.WallDir.Up, 1 );
		walls.setWall( 1, 2, StockadeChecker.Wall.WallDir.Right, 1 );
		walls.setWall( 2, 2, StockadeChecker.Wall.WallDir.Right, 1 );
		walls.setWall( 2, 2, StockadeChecker.Wall.WallDir.Up, 1 );
		walls.setWall( 3, 2, StockadeChecker.Wall.WallDir.Up, 1 );
		walls.setWall( 2, 3, StockadeChecker.Wall.WallDir.Up, 1 );
		walls.setWall( 4, 3, StockadeChecker.Wall.WallDir.Right, 1 );
		walls.setWall( 5, 3, StockadeChecker.Wall.WallDir.Up, 1 );
		walls.setWall( 1, 4, StockadeChecker.Wall.WallDir.Right, 1 );
		walls.setWall( 2, 4, StockadeChecker.Wall.WallDir.Up, 1 );
		walls.setWall( 3, 4, StockadeChecker.Wall.WallDir.Up, 1 );
		walls.setWall( 1, 5, StockadeChecker.Wall.WallDir.Up, 1 );
		walls.setWall( 2, 5, StockadeChecker.Wall.WallDir.Right, 1 );
		walls.setWall( 5, 5, StockadeChecker.Wall.WallDir.Up, 1 );
		walls.setWall( 1, 6, StockadeChecker.Wall.WallDir.Right, 1 );
		walls.setWall( 5, 6, StockadeChecker.Wall.WallDir.Left, 1 );

		/*
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
		*/
	}

	void check() {
		foreach( var f in floors_ ) {
			Destroy( f );
		}
		var region = checker_.check();
		// スペースごとに塗り分け
		Color[] colors = new Color[] {
			Color.red,
			Color.blue,
			Color.green,
			Color.yellow,
		};
		for ( int y = 0; y < region.GetLength(1); ++y ) {
			for ( int x = 0; x < region.GetLength(0); ++x ) {
				var f = Instantiate<TestFloor>( floorPrefab_ );
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
	Vector2Int region_ = new Vector2Int( 6, 8 );
}
