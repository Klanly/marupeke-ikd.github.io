using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 波のワールド

namespace WaveGenerator
{
	public class World
	{
		public World( int pxWidth, int pxHeight, float worldWidth, Vector2 center ) {
			grid_ = new float[ pxWidth, pxHeight ];
			for ( int y = 0; y < pxHeight; ++y ) {
				for ( int x = 0; x < pxWidth; ++x ) {
					grid_[ x, y ] = 0.0f;
				}
			}
			pxWidth_ = pxWidth;
			pxHeight_ = pxHeight;
			worldLen_.x_ = worldWidth;
			worldLen_.y_ = ( float )pxHeight / pxWidth * worldWidth;
			center_ = center;
			unit_ = worldWidth / pxWidth;
			lbPos_ = center_.sub( ref lbPos_, worldLen_.mul( ref lbPos_, 0.5f ) );
		}

		// グリッドを取得
		public float[,] Grid { get { return grid_; } }

		// グリッドの横幅ピクセル数を取得
		public int GridPixelWidth { get { return pxWidth_; } }

		// グリッドの縦幅ピクセル数を取得
		public int GridPixelHeight { get { return pxHeight_; } }

		// 範囲のワールド縦横幅を取得
		public Vector2 WorldLen { get { return worldLen_; } }

		// 範囲中心点のワールド座標を取得
		public Vector2 Center { get { return center_; } }

		// グリッド座標をワールド座標に変換
		//  x, y  : グリッド座標
		//  outPos: (x,y)に対応したワールド座標（グリッドの中心点に対応した位置）を返す
		public void getWorldPos( int x, int y, ref Vector2 outPos ) {
			outPos.x_ = lbPos_.x_ + ( x + 0.5f ) * unit_;
			outPos.y_ = lbPos_.y_ + ( y + 0.5f ) * unit_;
		}

		// カラーバイト配列を作成
		//  useNomalize : 値を0～255に正規化する？
		//  useCentering: 高さ0を128に合わせる？normalizeがtrueの場合は高さ0=128ををキープしつつ正規化します。
		//  scale       : useNormalizeがfalseの時にだけ有効で、値をスケーリングする（マイナス及び255を超えた値はクランプ）
		public byte[] createColorByteArray( bool useNormalize = true, bool useCentering = true, float scale = 1.0f ) {
			if ( useNormalize == true && useCentering == false ) {
				// ノーマライズパラメータ算出
				float minVal = grid_[ 0, 0 ];
				float maxVal = grid_[ 0, 0 ];
				for (int y = 0; y < GridPixelHeight; ++y) {
					for (int x = 0; x < GridPixelWidth; ++x) {
						if (grid_[ x, y ] < minVal)
							minVal = grid_[ x, y ];
						else if (grid_[ x, y ] > maxVal)
							maxVal = grid_[ x, y ];
					}
				}
				float len = maxVal - minVal;

				// 色情報を格納するbyte配列を作成
				int stride = GridPixelWidth * 4;
				var data = new Byte[ stride * GridPixelHeight ];
				for (int y = 0; y < GridPixelHeight; ++y) {
					for (int x = 0; x < GridPixelWidth; ++x) {
						// 値取得
						//  Bitmapの書き込みは上下反転
						byte level = ( byte )( ( grid_[ x, GridPixelHeight - y - 1 ] - minVal ) / len * 255.0f );

						// 書き込み
						data[ y * stride + x * 4 ] = level;     // B
						data[ y * stride + x * 4 + 1 ] = level; // G
						data[ y * stride + x * 4 + 2 ] = level; // R
						data[ y * stride + x * 4 + 3 ] = 255;       // A
					}
				}
				return data;
			}

			if ( useNormalize == true && useCentering == true ) {
				// 0.0f = 128をキープしつつ正規化
				// 絶対値の最大値を算出
				float maxVal = Math.Abs( grid_[ 0, 0 ] );
				for (int y = 0; y < GridPixelHeight; ++y) {
					for (int x = 0; x < GridPixelWidth; ++x) {
						if (Math.Abs( grid_[ x, y ] ) > maxVal)
							maxVal = Math.Abs( grid_[ x, y ] );
					}
				}

				// 色情報を格納するbyte配列を作成
				int stride = GridPixelWidth * 4;
				var data = new Byte[ stride * GridPixelHeight ];
				if (maxVal == 0.0f) {
					for (int y = 0; y < GridPixelHeight; ++y) {
						for (int x = 0; x < GridPixelWidth; ++x) {
							// 書き込み
							data[ y * stride + x * 4 ] = 128;     // B
							data[ y * stride + x * 4 + 1 ] = 128; // G
							data[ y * stride + x * 4 + 2 ] = 128; // R
							data[ y * stride + x * 4 + 3 ] = 255; // A
						}
					}
				} else {
					for (int y = 0; y < GridPixelHeight; ++y) {
						for (int x = 0; x < GridPixelWidth; ++x) {
							// 値取得
							//  Bitmapの書き込みは上下反転
							int intLevel = ( int )( ( scale * grid_[ x, GridPixelHeight - y - 1 ] ) / maxVal * 127.0f + 128 );
							byte level = ( byte )( intLevel >= 256 ? 255 : ( intLevel < 0 ? 0 : intLevel ) );

							// 書き込み
							data[ y * stride + x * 4 ] = level;     // B
							data[ y * stride + x * 4 + 1 ] = level; // G
							data[ y * stride + x * 4 + 2 ] = level; // R
							data[ y * stride + x * 4 + 3 ] = 255;       // A
						}
					}
				}
				return data;
			}

			if (useCentering == true) {
				// 色情報を格納するbyte配列を作成
				int stride = GridPixelWidth * 4;
				var data = new Byte[ stride * GridPixelHeight ];
				for (int y = 0; y < GridPixelHeight; ++y) {
					for (int x = 0; x < GridPixelWidth; ++x) {
						// 値取得
						//  Bitmapの書き込みは上下反転
						int intLevel = ( int )( ( scale * grid_[ x, GridPixelHeight - y - 1 ] ) + 128 );
						byte level = ( byte )( intLevel >= 256 ? 255 : ( intLevel < 0 ? 0 : intLevel ) );

						// 書き込み
						data[ y * stride + x * 4 ] = level;     // B
						data[ y * stride + x * 4 + 1 ] = level; // G
						data[ y * stride + x * 4 + 2 ] = level; // R
						data[ y * stride + x * 4 + 3 ] = 255;       // A
					}
				}
				return data;
			} else {
				// そのまま出力
				// 色情報を格納するbyte配列を作成
				int stride = GridPixelWidth * 4;
				var data = new Byte[ stride * GridPixelHeight ];
				for (int y = 0; y < GridPixelHeight; ++y) {
					for (int x = 0; x < GridPixelWidth; ++x) {
						// 値取得
						//  Bitmapの書き込みは上下反転
						int intLevel = ( int )( ( scale * grid_[ x, GridPixelHeight - y - 1 ] ) );
						byte level = ( byte )( intLevel >= 256 ? 255 : ( intLevel < 0 ? 0 : intLevel ) );

						// 書き込み
						data[ y * stride + x * 4 ] = level;     // B
						data[ y * stride + x * 4 + 1 ] = level; // G
						data[ y * stride + x * 4 + 2 ] = level; // R
						data[ y * stride + x * 4 + 3 ] = 255;       // A
					}
				}
				return data;
			}

			return null; // ???
		}

		float[,] grid_;   // グリッド
		int pxWidth_;   // グリッドの横ピクセル数
		int pxHeight_;  // グリッドの縦ピクセル数
		Vector2 worldLen_ = new Vector2();	// グリッドの縦横ワールド幅
		Vector2 center_ = new Vector2();     // グリッドの中心点のワールド座標
		float unit_;    // 1グリッドのワールド空間でのサイズ幅
		Vector2 lbPos_ = new Vector2();	// グリッドの左下ワールド位置
	}
}
