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
        public Form1() {
            InitializeComponent();
        }

        private void inputCommonFolderBtn_Click(object sender, EventArgs e) {
            var dlg = new FolderBrowserDialog();
            if ( dlg.ShowDialog() == DialogResult.OK ) {
                inputCommonFolderTxt.Text = dlg.SelectedPath;
            }
        }

        private void outputFolderBtn_Click(object sender, EventArgs e) {
            var dlg = new FolderBrowserDialog();
            if ( dlg.ShowDialog() == DialogResult.OK ) {
                outputFolderTxt.Text = dlg.SelectedPath;
            }
        }

        private void runBtn_Click(object sender, EventArgs e) {
            // 入力元フォルダ内を出力先フォルダにコピー
            // ただし.metaは除く
            if ( inputCommonFolderTxt.Text == "" )
                return;
            if ( outputFolderTxt.Text == "" )
                return;

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
        }
    }
}
