using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CommonCollector {
    public partial class Form1 : Form {

        string inputFolderPath_ = "";
        public Form1() {
            InitializeComponent();
        }

        private void inputCommonFolderBtn_Click(object sender, EventArgs e) {
//            var dlg = new FolderBrowserDialog();
//            if ( dlg.ShowDialog() == DialogResult.OK ) {
//                inputCommonFolderTxt.Text = dlg.SelectedPath;
//            }
            var ofd = new OpenFileDialog();
            ofd.ReadOnlyChecked = true;
            ofd.ShowHelp = false;
            ofd.CheckFileExists = false;
            ofd.FileName = "Select src common folder";
            ofd.FileOk += ( _sender, _e ) => {
                ofd.FileName = System.IO.Path.GetDirectoryName(ofd.FileName);
            };
            if ( ofd.ShowDialog() == DialogResult.OK )
            {
                inputCommonFolderTxt.Text = ofd.FileName;
                inputFolderPath_ = ofd.FileName;
            }
        }

        private void outputFolderBtn_Click(object sender, EventArgs e) {
//            var dlg = new FolderBrowserDialog();
//            if ( dlg.ShowDialog() == DialogResult.OK ) {
//               outputFolderTxt.Text = dlg.SelectedPath;
//            }
            var ofd = new OpenFileDialog();
            ofd.ReadOnlyChecked = true;
            ofd.ShowHelp = false;
            ofd.CheckFileExists = false;
            ofd.FileName = "Select dest common folder";
            ofd.FileOk += (_sender, _e) => {
                ofd.FileName = System.IO.Path.GetDirectoryName(ofd.FileName);
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                outputFolderTxt.Text = ofd.FileName;
            }
        }

        private void runBtn_Click(object sender, EventArgs e) {
            // 入力元フォルダ内を出力先フォルダにコピー
            // ただし.metaは除く
            if ( inputCommonFolderTxt.Text == "" )
                return;
            if ( outputFolderTxt.Text == "" )
                return;

            // 上書きの場合はDest側のフォルダ下のファイルおよびフォルダを全部消す
            if (bCheckOverwrite.Checked == true)
            {
                DirectoryInfo target = new DirectoryInfo( outputFolderTxt.Text);
                foreach (FileInfo file in target.GetFiles()) {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in target.GetDirectories()) {
                    dir.Delete(true);
                }
            }

            string[] files = System.IO.Directory.GetFiles( inputCommonFolderTxt.Text, "*.cs", SearchOption.AllDirectories );

            foreach ( var path in files ) {
                // 出力先フォルダが無い場合は作成
                var outputPath = outputFolderTxt.Text + "\\" + path.Replace( inputCommonFolderTxt.Text, "" );
                var outputFolder = System.IO.Path.GetDirectoryName( outputPath );
                if ( Directory.Exists( outputFolder ) == false ) {
                    Directory.CreateDirectory( outputFolder );
                }
                System.IO.File.Copy( path, outputPath, true );
            }

            MessageBox.Show("更新しました。");
        }
    }
}
