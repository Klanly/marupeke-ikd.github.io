using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RandomDotDrawer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            filtersLB.SelectedIndex = 0;
        }

        private void runBtn_Click(object sender, EventArgs e)
        {
            releaseOldResources();

            // イメージ初期化
            int texSize = getInt( textureSizeTxt.Text, 256 );
            if ( texSize <= 0 )
                texSize = 256;
            texSize = nearPow2( texSize );
            bmp_ = createEmptyCanvas( texSize, texSize, out g_ );

            float dist = getFloat( distanceTxt.Text, 5.0f );
            float pixelRadius = getFloat( pixelRadiusTxt.Text, 0.5f );
            int seed = getInt( randomSeedTxt.Text, 0 );
            int colorRangeMin = getInt( colorRangeMinTxt.Text, 0 );
            int colorRangeMax = getInt( colorRangeMaxTxt.Text, 0 );
            if ( colorRangeMin > colorRangeMax ) {
                int tmp = colorRangeMin;
                colorRangeMin = colorRangeMax;
                colorRangeMax = tmp;
            }
            Color[] colors = new Color[ colorRangeMax - colorRangeMin + 1 ];
            for ( int i = 0; i < colors.Length; ++i ) {
                int g = colorRangeMin + i;
                colors[ i ] = Color.FromArgb( 255, g, g, g );
            }

            g_.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g_.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            DotAlgPDS alg = new DotAlgPDS( g_, bmp_, dist, seed );
            alg.setPixelRadius( pixelRadius );
            alg.setBrushColors( colors );
            alg.run( ref stopSim_ );

            // イメージ表示
            dotImage.Image = bmp_;
            dotImage.Refresh();
        }

        private void filterApplyBtn_Click(object sender, EventArgs e)
        {
            if ( bmp_ == null )
                return;

            // フィルター適用
            var selectedName = filtersLB.Text;
            FilterBase filter = null;
            switch ( selectedName ) {
                case "Gaussian Blur":
                    float radius = getFloat( gaussianBlurCtl.Radius.Text, 0.0f );
                    int samplingNum = getInt( gaussianBlurCtl.SamplingNum.Text, 10 );
                    filter = new Filter_GaussianBlur( g_, bmp_, radius, samplingNum );
                    break;
            }

            if ( filter != null )
                filter.run();

            dotImage.Refresh();
        }

        // 有効な値を取得
        float getFloat( string text, float initVal )
        {
            float f = 0.0f;
            if ( float.TryParse( text, out f ) == false )
                return initVal;
            return f;
        }

        int getInt(string text, int initVal)
        {
            int i = 0;
            if ( int.TryParse( text, out i ) == false )
                return initVal;
            return i;
        }

        // 指定サイズの空のビットマップを作成
        Bitmap createEmptyCanvas( int w, int h, out Graphics canvas )
        {
            Bitmap bmp = new Bitmap( w, h );
            Graphics g = Graphics.FromImage( bmp );
            canvas = g;
            return bmp;
        }

        // 2のべき乗数を取得
        int nearPow2(int n)
        {
            // nが0以下の時は0とする。
            if ( n <= 0 )
                return 0;

            // (n & (n - 1)) == 0 の時は、nが2の冪乗であるため、そのままnを返す。
            if ( ( n & ( n - 1 ) ) == 0 )
                return n;

            // bitシフトを用いて、2の冪乗を求める。
            int ret = 1;
            while ( n > 0 ) { ret <<= 1; n >>= 1; }
            return ret;
        }

        // リソース解放
        void releaseOldResources()
        {
            if ( g_ != null)
                g_.Dispose();
            if ( bmp_ != null )
                bmp_.Dispose();
            if ( pen_ != null )
                pen_.Dispose();
        }

        // 保存
        private void saveBtn_Click(object sender, EventArgs e)
        {
            if ( bmp_ == null )
                return;

            // クリップボードにコピー
            Clipboard.SetImage( bmp_ );
        }

        private bool stopSim_ = false;  // シミュレーションストップ指示
        Graphics g_ = null;             // 描画対象キャンバス
        Bitmap bmp_ = null;             // 描画先Bitmap
        Pen pen_ = null;                // ペン
    }
}
