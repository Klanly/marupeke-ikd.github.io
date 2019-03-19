using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 囲いチェッカー
//
//  指定のマス目フィールドに設置された壁により完全に囲まれている箇所を
//  チェックします。
//  setup後にWallプロパティが返すWallsオブジェクトを通して壁を設置・削除
//  出来ます。

public class StockadeChecker {
	// 壁管理クラス
	public class Wall
	{
		public enum WallDir
		{
			Left,
			Right,
			Down,
			Up
		}

		public enum WallOrder
		{
			Horizontal,
			Vertical
		}

		// 壁を設定
		//  sx, xy  : フロアの座標
		//  dir     : 壁の方向
		//  wallId  : 壁のId。0で壁無し、それ以外は壁と認識
		public bool setWall( int sx, int sy, WallDir dir, int wallId ) {
			if (sx < 0 || sx >= region_.x || sy < 0 || sy >= region_.y)
				return false;
			switch (dir) {
			case WallDir.Left:
				return setWall(	WallOrder.Vertical, sx, sy, wallId );
			case WallDir.Right:
				return setWall( WallOrder.Vertical, sx + 1, sy, wallId );
			case WallDir.Down:
				return setWall( WallOrder.Horizontal, sx, sy, wallId );
			case WallDir.Up:
				return setWall( WallOrder.Horizontal, sx, sy + 1, wallId );
			}
			return true;
		}

		// 壁を設定
		//  order : 水平壁か垂直壁か
		//  wx, wy: 壁の配置座標（水平、垂直それぞれの）
		//  wallId: 壁のId。0で壁無し、それ以外は壁と認識
		public bool setWall( WallOrder order, int wx, int wy, int wallId ) {
			if (wx < 0 || wy < 0)
				return false;
			if ( order == WallOrder.Horizontal ) {
				if ( wx >= region_.x || wy >= region_.y + 1 )
					return false;
				hWalls_[ wx, wy ] = wallId;
			} else {
				if ( wx >= region_.x + 1 || wy >= region_.y )
					return false;
				vWalls_[ wx, wy ] = wallId;
			}
			return true;
		}

		// 壁を取得
		//  sx, sy: フロアの座標
		//  dir   : 壁の方向
		public int getWall( int sx, int sy, WallDir dir ) {
			if (sx < 0 || sx >= region_.x || sy < 0 || sy >= region_.y)
				return 0;
			switch( dir ) {
			case WallDir.Left:
				if ( sx == 0 && vWalls_[ sx, sy ] == 0 )
					return fieldEdge_g;
				return vWalls_[ sx, sy ];
			case WallDir.Right:
				if ( sx == region_.x - 1 && vWalls_[ sx + 1, sy ] == 0 )
					return fieldEdge_g;
				return vWalls_[ sx + 1, sy ];
			case WallDir.Down:
				if ( sy == 0 && hWalls_[ sx, sy ] == 0 )
					return fieldEdge_g;
				return hWalls_[ sx, sy ];
			case WallDir.Up:
				if ( sy == region_.y - 1 && hWalls_[ sx, sy + 1 ] == 0 )
					return fieldEdge_g;
				return hWalls_[ sx, sy + 1 ];
			}
			return 0;
		}

		// 壁を取得
		//  order : 水平壁か垂直壁か
		//  wx, wy: 壁の配置座標（水平、垂直それぞれの）
		public int getWall( WallOrder order, int wx, int wy ) {
			if (wx < 0 || wy < 0)
				return 0;
			if (order == WallOrder.Horizontal) {
				// 水平壁
				if ( wx < region_.x ) {
					if ( ( wy == 0 || wy == region_.y ) && hWalls_[ wx, wy ] == 0 ) {
						return fieldEdge_g;     // フィールドの端っこ
					}
					return hWalls_[ wx, wy ];
				}
			} else {
				// 垂直壁
				if ( wy < region_.y ) {
					if ( ( wx == 0 || wx == region_.x ) && vWalls_[ wx, wy ] == 0 ) {
						return fieldEdge_g;		// フィールドの端っこ
					}
					return vWalls_[ wx, wy ];
				}
			}
			return 0;
		}

		// 指定のフロアがスペース？（四方に壁が無い領域）
		public bool isSpace( int sx, int sy ) {
			return !( 
				getWall( sx, sy, WallDir.Left ) != 0 ||
				getWall( sx, sy, WallDir.Right ) != 0 ||
				getWall( sx, sy, WallDir.Down ) != 0 ||
				getWall( sx, sy, WallDir.Up ) != 0
				);
		}

		// 指定のフロアが四方を壁に囲まれてる？
		public bool isJustOneFloor( int sx, int sy ) {
			return !(
				getWall( sx, sy, WallDir.Left ) == 0 ||
				getWall( sx, sy, WallDir.Right ) == 0 ||
				getWall( sx, sy, WallDir.Down ) == 0 ||
				getWall( sx, sy, WallDir.Up ) == 0
				);
		}

		protected int[,] vWalls_;	// 垂直壁
		protected int[,] hWalls_;   // 水平壁
		protected Vector2Int region_;
		static int fieldEdge_g = 255;	// フィールドのエッヂ
	}

	class WallSetter : Wall
	{
		public void setup( Vector2Int region ) {
			region_ = region;
			hWalls_ = new int[ region.x, region.y + 1 ];	// 水平壁はY軸方向に1つ多い
			vWalls_ = new int[ region.x + 1, region.y ];	// 垂直壁はX軸方向に1つ多い
		}
	}

	public Wall Walls { get { return walls_; } }

	// 領域設定
	public void setup( Vector2Int region ) {
        region_ = region;
		field_ = new int[ region_.x, region_.y ];
		for ( int x = 0; x < region_.x; ++x ) {
			for ( int y = 0; y < region_.y; ++y)  {
				field_[ x, y ] = 0;
			}
		}
		walls_.setup( region );
		checkField_ = new int[ region_.x, region_.y ];
	}

	// 囲いチェック
	public int[,] check() {
		for ( int x = 0; x < region_.x; ++x ) {
			for ( int y = 0; y < region_.y; ++y ) {
				checkField_[ x, y ] = 0;
			}
		}

		// (0,0)からチェック開始
		int moveDirIdx = 0;     // 0: Up, 1: Right, 2: Down, 3: Left
		Vector2Int[] moveDirs = new Vector2Int[] {
			new Vector2Int( 0, 1 ),		// UP
			new Vector2Int( 1, 0 ),		// Right
			new Vector2Int( 0, -1 ),	// Down
			new Vector2Int( -1, 0 )		// Left
		};
		Wall.WallDir[] leftHandDirs = new Wall.WallDir[] {	// 移動方向に対する左手向き
			Wall.WallDir.Left,
			Wall.WallDir.Up,
			Wall.WallDir.Right,
			Wall.WallDir.Down
		};
		int curFloorIdx = 0;
		for ( int x = 0; x < region_.x; ++x ) {
			for ( int y = 0; y < region_.y; ++y ) {
				// フロアにチェックマークがある所は検索済みなのでスキップ
				if ( checkField_[ x, y ] != 0 ) {
					continue;
				}
				// フロアにチェックマークが無くて四方に壁が無い所はスペースなので
				// 塗りつぶしプロセスへ（スペースが新規はありえない）
				if ( walls_.isSpace( x, y ) == true ) {
					// 自分の四方のどこかにマークがあるので検索
					int mark = 0;
					for ( int i = 0; i < 4; ++i ) {
						mark = checkField_[ x + moveDirs[ i ].x, y + moveDirs[ i ].y ];
						if ( mark != 0 )
							break;
					}
					// 自分自身と四方を再マーク
					checkField_[ x, y ] = mark;
					for ( int i = 0; i < 4; ++i ) {
						checkField_[ x + moveDirs[ i ].x, y + moveDirs[ i ].y ] = mark;
					}
					continue;
				}

				// 新規の囲いチェックプロセス //
				//  左手法で囲いを調べる
				curFloorIdx++;

				// チェックマークが無くて四方がすべて壁の特殊ケースは
				// ここでマークを入れる
				if ( walls_.isJustOneFloor( x, y ) == true ) {
					checkField_[ x, y ] = curFloorIdx;
					continue;
				}

				int cx = x;
				int cy = y;
				int sx = x;	// スタート座標
				int sy = y; // スタート座標
				int startDir = 0;	// スタート時の向き
				bool bStarted = false;
				bool bEnd = false;
				int tmpCount = region_.x * region_.y * 4;
				while ( tmpCount != 0 ) {
					tmpCount--;
					// フロアにチェックマーク
					if ( checkField_[ cx, cy ] == 0 ) {
						checkField_[ cx, cy ] = curFloorIdx;
					}
					for ( int i = 0; i < 4; ++i ) {
						int id = walls_.getWall( cx, cy, leftHandDirs[ moveDirIdx ] );
						if ( id == 0 ) {
							// 左手の方向（正面+3）へ移動確定
							moveDirIdx = ( moveDirIdx + 3 ) % 4;
							if ( bStarted == false ) {
								bStarted = true;
								sx = x;
								sy = y;
								startDir = moveDirIdx;
							} else {
								// スタート地点に戻ってきた？
								if ( cx == sx && cy == sy && moveDirIdx == startDir ) {
									// ゴール！
									bEnd = true;
									break;
								}
							}
							cx += moveDirs[ moveDirIdx ].x;
							cy += moveDirs[ moveDirIdx ].y;
							break;
						}
						moveDirIdx = ( ++moveDirIdx ) % 4;
					}
					if ( bEnd == true )
						break;
				}
			}
		}

		return checkField_;
	}

    Vector2Int region_;
	int[,] field_;
	WallSetter walls_ = new WallSetter();
	int[,] checkField_;
}
