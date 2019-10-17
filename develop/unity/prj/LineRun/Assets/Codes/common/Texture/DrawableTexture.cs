using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 直接描画可能テクスチャ
//  実際に描画するだけでなくコリジョン用テクスチャなどにも使えます

public class DrawableTexture
{
	// 描画テクスチャを作成
	public void setup( int width, int height, Color clearColor, FilterMode finterMode = FilterMode.Point )
	{
		width_ = width;
		height_ = height;
		tex_ = new Texture2D( width, height, TextureFormat.RGBA32, false );
		tex_.filterMode = finterMode;
		clearColors_ = new Color32[ width * height ];
		clearColor_ = clearColor;
		for (int i = 0; i < width * height; ++i) {
			clearColors_[ i ] = clearColor_;
		}
		tex_.SetPixels32( clearColors_ );
		tex_.Apply();

		sprite_ = Sprite.Create( tex_, new Rect( 0.0f, 0.0f, width, height ), Vector2.zero );
	}

	// テクスチャをクリア
	public void clear()
	{
		tex_.SetPixels32( clearColors_ );
		tex_.Apply();
	}

	// スプライトを取得
	public Sprite getSprite()
	{
		return sprite_;
	}

	// テクスチャを取得
	public Texture2D getTexture()
	{
		return tex_;
	}

	// 点を描画
	public void setPixel( int x, int y, Color color, bool useLoop )
	{
		if ( useLoop == false && ( x < 0 || x >= width_ || y < 0 || y >= height_ ) ) {
			return;
		}

		x = x < 0 ? ( width_ + x % width_ ) % width_ : x % width_;
		y = y < 0 ? ( height_ + y % height_ ) % height_ : y % height_;

		tex_.SetPixel( x, y, color );
	}

	// 点群を描画
	public void setPixels( Vector2Int[] points, Color color, bool useLoop )
	{
		foreach ( var p in points ) {
			setPixel( p.x, p.y, color, useLoop );
		}
	}

	// 線分を描画
	public void drawLine( int sx, int sy, int ex, int ey, Color color, bool useLoop )
	{
		int dx = Mathf.Abs( ex - sx );
		int dy = Mathf.Abs( ey - sy );
		int stepX = sx < ex ? 1 : -1;
		int stepY = sy < ey ? 1 : -1;
		int err = dx - dy;
		int x = sx;
		int y = sy;
		while( true ) {
			setPixel( x, y, color, useLoop );
			if (x == ex && y == ey)
				break;
			int e = 2 * err;
			if ( e > -dy ) {
				err -= dy;
				x += stepX;
			}
			if ( e < dx ) {
				err += dx;
				y += stepY;
			}
		}
	}

	// 色を取得
	public Color getColor( int x, int y, bool useLoop )
	{
		if (useLoop == false && ( x < 0 || x >= width_ || y < 0 || y >= height_ )) {
			return clearColor_;
		}
		x = x < 0 ? ( width_ + x % width_ ) % width_ : x % width_;
		y = y < 0 ? ( height_ + y % height_ ) % height_ : y % height_;

		return tex_.GetPixel( x, y );
	}

	// 描画反映
	//  これを呼び出して初めてテクスチャへの変更が反映される
	public void apply()
	{
		tex_.Apply();
	}

	Texture2D tex_;
	Sprite sprite_;
	Color clearColor_ = Color.black;
	Color32[] clearColors_;
	int width_ = 0;
	int height_ = 0;
}
