using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RandomDotDrawer
{
    public class FilterBase
    {
        public FilterBase( Graphics g, Bitmap srcBmp )
        {
            g_ = g;
            srcBmp_ = srcBmp;
        }
        virtual public void run()
        {

        }

        // 補間サンプリング（ループ対応）
        protected Color sampling(Bitmap b, int x, float ofsX, int y, float ofsY)
        {
            float sx = x + b.Width + ofsX;
            float sy = y + b.Height + ofsY;
            int p = ( int )( sx );
            int q = ( int )( sy );
            float rx = sx - p;
            float ry = sy - q;
            p %= b.Width;
            q %= b.Height;
            int p1 = ( p + 1 ) % b.Width;
            int q1 = ( q + 1 ) % b.Height;
            Color c00 = b.GetPixel( p, q );
            Color c10 = b.GetPixel( p1, q );
            Color c01 = b.GetPixel( p, q1 );
            Color c11 = b.GetPixel( p1, q1 );
            Color c0 = learp( c00, c10, rx );
            Color c1 = learp( c01, c11, rx );
            return learp( c0, c1, ry );
        }

        // 2色の線形補間
        protected Color learp(Color c0, Color c1, float t)
        {
            return Color.FromArgb(
                255,
                ( int )( c0.R * ( 1.0f - t ) + c1.R * t ),
                ( int )( c0.G * ( 1.0f - t ) + c1.G * t ),
                ( int )( c0.B * ( 1.0f - t ) + c1.B * t )
            );
        }

        protected Graphics g_;
        protected Bitmap srcBmp_;
    }
}
