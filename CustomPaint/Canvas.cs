using System.Drawing;
using System.Windows.Forms;
using System;
using System.Drawing.Imaging;
using System.IO;


namespace CustomPaint
{
    public partial class Canvas : Form
    {
        private bool drawing;
        private int oldX, oldY;
        private Bitmap bmp;
        private int zoom = 1;
        private bool isSaved = false;
        private bool isFromFile = false;
        private bool isChanged = false;
        private string filename;

        public int CanvasWidth
        {
            get
            {
                return pictureBox1.Width;
            }
        }
        public int CanvasHeight
        {
            get
            {
                return pictureBox1.Height;
            }
        }

        public Canvas()
        {
            InitializeComponent();
            bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            pictureBox1.Image = bmp;
            isSaved = false;
            isChanged = false;
            isFromFile = false;
        }

        public Canvas(String FileName)
        {
            InitializeComponent();
            Text = FileName;
            bmp = new Bitmap(FileName);
            Graphics g = Graphics.FromImage(bmp);
            pictureBox1.Width = bmp.Width;
            pictureBox1.Height = bmp.Height;
            pictureBox1.Image = bmp;
            isSaved = false;
            isChanged = false;
            isFromFile = true;
            filename = FileName;
        }

        public void ResizeCanvas(int width, int height)
        {
            pictureBox1.Width = width;
            pictureBox1.Height = height;
            Bitmap tbmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(tbmp);
            g.Clear(Color.White);
            g.DrawImage(bmp, new Point(0, 0));
            bmp = tbmp;
            pictureBox1.Image = bmp;
        }


        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            switch (MainForm.currentTool)
            {
                case MainForm.Tool.Кисточка:
                    if (e.Button == MouseButtons.Left)
                    {
                        Graphics graphics = Graphics.FromImage(bmp);
                        graphics.DrawLine(new Pen(MainForm.currentColor, MainForm.currentWidth), oldX, oldY, e.X, e.Y);
                        oldX = e.X;
                        oldY = e.Y;
                        pictureBox1.Invalidate();
                    }
                    break;
                case MainForm.Tool.Ластик:
                    if (e.Button == MouseButtons.Left)
                    {
                        Graphics graphics = Graphics.FromImage(bmp);
                        graphics.DrawLine(new Pen(pictureBox1.BackColor, MainForm.currentWidth), oldX, oldY, e.X, e.Y);
                        oldX = e.X;
                        oldY = e.Y;
                        pictureBox1.Invalidate();
                    }
                    break;
                case MainForm.Tool.Эллипс:
                    if (drawing)
                    {
                        Graphics graphics = pictureBox1.CreateGraphics();
                        graphics.DrawEllipse(new Pen(MainForm.currentColor, MainForm.currentWidth), oldX, oldY, e.X - oldX, e.Y - oldY);
                        pictureBox1.Invalidate();
                    }
                    break;
                case MainForm.Tool.Линия:
                    if (drawing)
                    {
                        Graphics graphics = pictureBox1.CreateGraphics();
                        graphics.DrawLine(new Pen(MainForm.currentColor, MainForm.currentWidth), oldX, oldY, e.X, e.Y);
                        pictureBox1.Invalidate();
                    }
                    break;
                case MainForm.Tool.Звезда:
                    if (drawing)
                    {
                        Graphics graphics = pictureBox1.CreateGraphics();
                        graphics.DrawPolygon(new Pen(MainForm.currentColor, MainForm.currentWidth), GetStarPoints(e));
                        pictureBox1.Invalidate();
                    }
                    break;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)   
        {
            if (MainForm.Tool.Эллипс == MainForm.currentTool)
            {
                Graphics graphics = Graphics.FromImage(bmp);
                graphics.DrawEllipse(new Pen(MainForm.currentColor, MainForm.currentWidth), oldX, oldY, e.X - oldX, e.Y - oldY);
                pictureBox1.Invalidate();
            }
            if (MainForm.Tool.Линия == MainForm.currentTool)
            {
                Graphics graphics = Graphics.FromImage(bmp);
                graphics.DrawLine(new Pen(MainForm.currentColor, MainForm.currentWidth), oldX, oldY, e.X, e.Y);
                pictureBox1.Invalidate();
            }
            if (MainForm.Tool.Звезда == MainForm.currentTool)
            {
                Graphics graphics = Graphics.FromImage(bmp);
                graphics.DrawPolygon(new Pen(MainForm.currentColor, MainForm.currentWidth), GetStarPoints(e));
                pictureBox1.Invalidate();
            }
            drawing = false;
            isChanged = true;
        }

        private void pictureBox1_MouseDown_1(object sender, MouseEventArgs e)
        {
            oldX = e.X;
            oldY = e.Y;
            drawing = true;
        }

        private PointF[] GetStarPoints(MouseEventArgs e)
        {

            double BRadius, SRadius;
            BRadius = Math.Sqrt((oldX - e.X) * (oldX - e.X) + (oldY - e.Y) * (oldY - e.Y));
            SRadius = BRadius / 2.0;
            double x0 = oldX, y0 = oldY;
            int n = MainForm.NStar;
            double alpha = 0;
            double a = alpha, da = Math.PI / n, l;
            PointF[] points = new PointF[2*n + 1];


            for (int k = 0; k < 2 * n + 1; k++)
            {
                l = k % 2 == 0 ? BRadius : SRadius;
                points[k] = new PointF((float)(x0 + l * Math.Cos(a)), (float)(y0 + l * Math.Sin(a)));
                a += da;
            }

            return points;
        }

        public void ZoomIn()
        {
            try
            {

                var scale = 1.2;
                var old = bmp.Clone();

                Width = (int) (bmp.Width * scale);
                Height = (int) (bmp.Height * scale);


                bmp = new Bitmap((int)(bmp.Width * scale),(int)(bmp.Height * scale));
                Graphics g = Graphics.FromImage(bmp);
                g.DrawImage(old as Image, new Rectangle(0, 0, bmp.Width, bmp.Height));
                pictureBox1.Image = bmp;
                pictureBox1.BackgroundImage = bmp;
            }
            catch (Exception)
            {
            }
        }
        public void ZoomOut()
        {
            try
            {
                var old = bmp.Clone();
                var scale = 1.2;
                Width = (int)(bmp.Width / scale);
                Height = (int)(bmp.Height / scale);

                bmp = new Bitmap((int)(bmp.Width / scale), (int)(bmp.Height / scale));
                Graphics g = Graphics.FromImage(bmp);
                g.DrawImage(old as Image, new Rectangle(0, 0, bmp.Width, bmp.Height));
                pictureBox1.Image = bmp;
                pictureBox1.BackgroundImage = bmp;

            }
            catch (Exception)
            {
            }
        }


        public void Save()
        {
            if (isFromFile)
            {
                Text = Text.Replace("\\", "//");
                bmp.Save(Text);
                isSaved = true;
                isChanged = false;
            }
            else
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.AddExtension = true;
                dlg.Filter = "Windows Bitmap (*.bmp)|*.bmp| Файлы JPEG (*.jpg)|*.jpg";
                ImageFormat[] ff = { ImageFormat.Bmp, ImageFormat.Jpeg };
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    bmp.Save(dlg.FileName);
                    filename = dlg.FileName;
                    FileInfo fi = new FileInfo(dlg.FileName);
                    string s = dlg.FileName.Substring(0, dlg.FileName.Length - fi.Name.Length);
                    string S = fi.Name;
                    Text = S;
                }

                isSaved = true;
                isChanged = false;
                isFromFile = true;
            }
        }

        public void SaveInFile()
        {
            bmp.Save(filename);
            isSaved = true;
            isChanged = false;
        }

        private void Canvas_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isChanged)
            {
                try
                {
                    if (isFromFile)
                    {
                        var result = MessageBox.Show("Сохранить изображение в форме " + Text + " перед закрытием?",
                            Text, MessageBoxButtons.YesNoCancel);
                        switch (result)
                        {
                            case DialogResult.Yes:
                                SaveInFile();

                                break;
                            case DialogResult.No:
                                break;
                            case DialogResult.Cancel:
                                e.Cancel = true;
                                break;
                        }
                    }
                    else if (!isSaved)
                    {
                        DialogResult result = MessageBox.Show($"Сохранить изображение в форме {Text} перед закрытием?",
                            Text, MessageBoxButtons.YesNoCancel);
                        switch (result)
                        {
                            case DialogResult.Yes:
                                ////bmp.Save(Path.Replace(".bmp", "1.bmp"));
                                //bmp.Save(Path);
                                Save();
                                break;
                            case DialogResult.No: break;
                            case DialogResult.Cancel:
                                e.Cancel = true;
                                break;
                        }
                    }
                }
                catch (Exception k)
                {
                    MessageBox.Show(k.Message);
                }
            }
        }

        public void SaveAs()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.AddExtension = true;
            dlg.Filter = "Windows Bitmap (*.bmp)|*.bmp| Файлы JPEG (*.jpg)|*.jpg";
            ImageFormat[] ff = { ImageFormat.Bmp, ImageFormat.Jpeg };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                bmp.Save(dlg.FileName);
                filename = dlg.FileName;
                FileInfo fi = new FileInfo(dlg.FileName);
                string s = dlg.FileName.Substring(0, dlg.FileName.Length - fi.Name.Length);
                string S = fi.Name;
                Text = S;
            }

            isFromFile = true;
            isChanged = false;
            isSaved = true;
        }

    }
}
