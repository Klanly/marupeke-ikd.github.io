using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

// ガウスブラーフィルタ

namespace RandomDotDrawer
{
    class Filter_GaussianBlur : FilterBase
    {
        public Filter_GaussianBlur(Graphics g, Bitmap srcBmp, float radius, int samplingNum ) : base(g, srcBmp)
        {
            radius_ = radius;
            samplingNum_ = samplingNum;
        }

        override public void run()
        {
            if ( radius_ <= 0.0f || samplingNum_ <= 0 )
                return;

            // X軸方向ブラー用のコンパチブルBMP作成
            var xBlurBmp = new Bitmap( srcBmp_.Width, srcBmp_.Height );
            var xBlurG = Graphics.FromImage( xBlurBmp );

            // X軸方向へブラー
            float unit = radius_ / samplingNum_;
            for ( int y = 0; y < srcBmp_.Height; ++y ) {
                for ( int x = 0; x < srcBmp_.Width; ++x ) {
                    float r = 0.0f;
                    float g = 0.0f;
                    float b = 0.0f;
                    float wt = 0.0f;
                    for ( int s = -samplingNum_; s <= samplingNum_; ++s ) {
                        float wx = 3.0f * (float)s / samplingNum_;
                        float w = (float)Math.Exp( -( wx * wx ) * 0.5f );
                        wt += w;
                        Color c = sampling( srcBmp_, x, s * unit, y, 0.0f );
                        r += c.R * w;
                        g += c.G * w;
                        b += c.B * w;
                    }
                    xBlurBmp.SetPixel( x, y, Color.FromArgb( 255, ( int )( r / wt ), ( int )( g / wt ), ( int )( b / wt ) ) );
                }
            }

            // Y軸方向へブラー
            for ( int y = 0; y < srcBmp_.Height; ++y ) {
                for ( int x = 0; x < srcBmp_.Width; ++x ) {
                    float r = 0.0f;
                    float g = 0.0f;
                    float b = 0.0f;
                    float wt = 0.0f;
                    for ( int s = -samplingNum_; s <= samplingNum_; ++s ) {
                        float wx = 3.0f * ( float )s / samplingNum_;
                        float w = ( float )Math.Exp( -( wx * wx ) * 0.5f );
                        wt += w;
                        Color c = sampling( xBlurBmp, x, 0.0f, y, s * unit );
                        r += c.R * w;
                        g += c.G * w;
                        b += c.B * w;
                    }
                    srcBmp_.SetPixel( x, y, Color.FromArgb( 255, ( int )( r / wt ), ( int )( g / wt ), ( int )( b / wt ) ) );
                }
            }
        }

        float radius_ = 1.0f;
        int samplingNum_ = 3;
    }
}
