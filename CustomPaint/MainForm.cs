using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomPaint
{
    public partial class MainForm : Form
    {
        private string currentFileName;
        public enum Tool { Кисточка, Ластик, Эллипс, Линия, Звезда};
        public static Color currentColor = Color.Black;
        public static int currentWidth = 10;
        public static Tool currentTool = 0;
        public static int NStar = 4;
        public static int widthCurrentWindow = 800;
        public static int heightCurrentWindow = 450;
        private Canvas canvas;
        public static string OldFile;

        public MainForm()
        {
            InitializeComponent();
            colorButton.BackColor = currentColor;
            toolWidthTextBox.Text = currentWidth.ToString();
        }

        private void красныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentColor = Color.Red;
            colorButton.BackColor = currentColor;
        }

        private void зеленыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentColor = Color.LightGreen;
            colorButton.BackColor = currentColor;
        }

        private void синийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentColor = Color.Blue;
            colorButton.BackColor = currentColor;
        }

        private void другойToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.FullOpen = true;
            if (colorDialog.ShowDialog() == DialogResult.Cancel)
                return;
            currentColor = colorDialog.Color;
            colorButton.BackColor = currentColor;
        }

        private void новыйРисунокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canvas = new Canvas();
            canvas.MdiParent = this;
            canvas.Show();
        }

        private void brushButton_Click(object sender, EventArgs e)
        {
            currentTool = 0;
            toolLabel.Text = currentTool.ToString();
        }

        private void eraserButton_Click(object sender, EventArgs e)
        {
            currentTool = (Tool) 1;
            toolLabel.Text = currentTool.ToString();
        }

        private void circleButton_Click(object sender, EventArgs e)
        {
            currentTool = (Tool) 2;
            toolLabel.Text = currentTool.ToString();
        }

        private void lineButton_Click(object sender, EventArgs e)
        {
            currentTool = (Tool) 3;
            toolLabel.Text = currentTool.ToString();
        }

        private void starButton_Click(object sender, EventArgs e)
        {
            currentTool = (Tool) 4;
            toolLabel.Text = currentTool.ToString();
            starVertex star = new starVertex();
            star.Show();    
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutPaint aboutPaint = new AboutPaint();
            aboutPaint.Show();
        }

        private void рисунокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            размерХолстаToolStripMenuItem.Enabled = !(ActiveMdiChild == null);
        }

        private void размерХолстаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CanvasSize cs = new CanvasSize(canvas.CanvasWidth, canvas.CanvasHeight);
            cs.ShowDialog();
            canvas.ResizeCanvas(cs.canvasWidth, cs.canvasHeight);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            canvas.ZoomIn();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            canvas.ZoomOut();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Save()
        {
            ((Canvas) ActiveMdiChild).Save();
        }

        private void SaveAs()
        {
            ((Canvas) ActiveMdiChild).SaveAs();
        }

        private void OpenImage()
        {
            openFileDialog1.Filter = "JPEG Image(*.jpeg)|*.jpeg|JPG image(*.jpg)|*.jpg|Windows Bitmap(*.bmp)|*.bmp";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            OldFile = openFileDialog1.FileName;
            canvas = new Canvas(OldFile);
            canvas.MdiParent = this;
            canvas.Show();
        }

        private void открытьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenImage();
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void toolWidthTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
            if (number == 13){
                int n = Int32.Parse(toolWidthTextBox.Text);
                if (n >= 3 && n <= 30)
                {
                    currentWidth = n;
                }
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}
