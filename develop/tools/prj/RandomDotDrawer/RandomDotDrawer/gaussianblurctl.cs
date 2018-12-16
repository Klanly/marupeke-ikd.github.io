using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RandomDotDrawer
{
    public partial class gaussianblurctl : UserControl
    {
        public TextBox Radius { get { return radiusTxt; } }
        public TextBox SamplingNum { get { return samplingNum; } }

        public gaussianblurctl()
        {
            InitializeComponent();
        }
    }
}
