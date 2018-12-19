using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

// PDS法によるドット打ち

namespace RandomDotDrawer
{
    public class DotAlgPDS : DotAlg
    {
        public DotAlgPDS( Graphics g, Bitmap bmp, float distance = 3.0f, int seed = 0 )
        {
            g_ = g;
            bmp_ = bmp;
            w_ = bmp_.Size.Width;
            h_ = bmp_.Size.Height;
            setDistance( distance );
            colBmp_ = new Bitmap( w_, h_ );
            colG_ = Graphics.FromImage( colBmp_ );
            seed_ = seed;
        }

        public void setDistance( float distance )
        {
            if ( distance < 0.0f )
                distance = 0.0f;
            distance_ = distance;
        }

        public void setPixelRadius( float radius )
        {
            pixelRaidus_ = radius;

        }

        public void setBrushColors( Color[] colors )
        {
            brushes_ = new Brush[ colors.Length ];
            for ( int i = 0; i < colors.Length; ++i ) {
                brushes_[ i ] = new SolidBrush( colors[ i ] );
            }
            colors_ = colors;
        }

        public override bool run( ref bool stopSim )
        {
            if ( g_ == null || bmp_ == null )
                return false;

            // イメージサイズ内の全座標インデックスをランダムシャッフル
            int[] indices = new int[ (int)w_ * (int)h_ ];
            int n = indices.Length;
            for ( int i = 0; i < n; ++i ) {
                indices[ i ] = i;
            }
            var r = new System.Random( seed_ );
            while( n > 1 ) {
                n--;
                int k = r.Next( n + 1 );
                int tmp = indices[ k ];
                indices[ k ] = indices[ n ];
                indices[ n ] = tmp;
            }

            n = indices.Length;
            colG_.FillRectangle( Brushes.White, 0, 0, w_, h_ );
            g_.FillRectangle( Brushes.White, 0, 0, w_, h_ );

            // ブラシカラー定義
            if ( brushes_ == null ) {
                brushes_ = new Brush[ 1 ] {
                    new SolidBrush( Color.Black )
                };
                colors_ = new Color[ 1 ] {
                    Color.Black
                };
            }

            Brush colBrush = new SolidBrush( Color.Black );

            // ランダム座標順に点打ち
            int x = 0;
            int y = 0;
            for ( int i = 0; i < n; ++i ) {
                indexToCoord( indices[ i ], ref x, ref y );

                // (x,y)が黒以外ならOK
                Color colColor = colBmp_.GetPixel( x, y );
                if ( colColor.R != 0 ) {
                    // 打つ点の半径が0.5以下はもう1ピクセルとしか打てない
                    if ( pixelRaidus_ <= 0.5f )
                        bmp_.SetPixel( x, y, getColor() );
                    else
                        wrappingDraw( g_, w_, h_, x, y, pixelRaidus_, getBrush() );

                    // コリジョン描画
                    wrappingDraw( colG_, w_, h_, x, y, distance_, colBrush );
                }
            }
            return true;
        }

        // インデックスを座標に変換
        void indexToCoord( int idx, ref int x, ref int y )
        {
            x = idx % w_;
            y = idx / w_;
        }

        // ラップ描画
        bool wrappingDraw( Graphics g, int w, int h, int x, int y, float hd, Brush brush )
        {
            // (x, y)に打つ
            g.FillEllipse( brush, x - hd, y - hd, hd * 2.0f, hd * 2.0f );

            // 上下左右に見切れていたらラップして描画
            float xOffset = 0.0f;
            float yOffset = 0.0f;
            if ( x < hd ) {
                // X-
                xOffset = +w;
            } else if ( x > w - hd ) {
                // X+
                xOffset = -w;
            }
            if ( y < hd ) {
                // Y-
                yOffset = +h;
            } else if ( y > h - hd ) {
                // Y+
                yOffset = -h;
            }

            if ( xOffset != 0.0f && yOffset != 0.0f ) {
                // 3点追加描画
                g.FillEllipse( brush, x - hd, y + yOffset - hd, hd * 2.0f, hd * 2.0f );
                g.FillEllipse( brush, x + xOffset - hd, y - hd, hd * 2.0f, hd * 2.0f );
                g.FillEllipse( brush, x + xOffset - hd, y + yOffset - hd, hd * 2.0f, hd * 2.0f );
            }
            else if ( xOffset != 0.0f || yOffset != 0.0f )
                g.FillEllipse( brush, x + xOffset - hd, y + yOffset - hd, hd * 2.0f, hd * 2.0f );

            return xOffset != 0.0f || yOffset != 0.0f;
        }

        Brush getBrush()
        {
            return brushes_[ brushRand_.Next( 0, brushes_.Length ) ];
        }

        Color getColor()
        {
            return colors_[ brushRand_.Next( 0, brushes_.Length ) ];
        }

        Graphics g_;
        Bitmap bmp_;
        Bitmap colBmp_;     // 衝突判定用BMP
        Graphics colG_;     // 衝突判定用キャンバス
        int w_ = 0;
        int h_ = 0;
        float distance_ = 0.0f;     // 点間距離
        float pixelRaidus_ = 0.5f;  // 打つ点の半径
        int seed_ = 0;              // 乱数シード
        Brush[] brushes_;           // ブラシセット
        Color[] colors_;            // ブラシカラーセット
        System.Random brushRand_ = new System.Random( 0 );
    }
}
