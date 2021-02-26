using System;
using System.Drawing;
using System.Windows.Forms;

namespace CustomPaint.CustomConrtols
{
    public partial class test : UserControl
    {
        public test()
        {
            InitializeComponent();
        }

        protected override void OnTextChanged(EventArgs e)

        {
            double x;
            if (!double.TryParse(Text, out x))

                ForeColor = Color.Red;
            else
                ForeColor = Color.Black;
            base.OnTextChanged(e);
        }
    }
}