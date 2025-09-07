namespace StickerBomb
{
    public partial class frmStickerBombGenerator : Form
    {
        private readonly List<string> _stickerFiles = new();

        public frmStickerBombGenerator()
        {
            InitializeComponent();
            btnGenerate.Enabled = _stickerFiles.Count > 0;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            btnGenerate.Enabled = false;
            btnAddImageFiles.Enabled = false;

            using var generator = new EoS.StickerBomb.Generator();

            var x = (int)numX.Value;
            var y = (int)numY.Value;

            //Set canvass size
            generator.Initialize(x, y);

            //Load stickers from Stickers folder

            generator.LoadStickers(_stickerFiles.ToArray());

            generator.ApplyStickers(bitmap =>
            {
                pictureBox1.Image = bitmap;
                pictureBox1.Refresh();
                Application.DoEvents();

            });

            var canvass = generator.GetCanvass();
            pictureBox1.Image = canvass;


            //Create filename for output file
            //Ensure output folder exists
            var outputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output");
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);
            
            var outputFile = Path.Combine(outputFolder, $"StickerBomb_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            generator.SaveCanvass(outputFile);
            MessageBox.Show($"Sticker bomb saved to {outputFile}");

            btnGenerate.Enabled = _stickerFiles.Count > 0;
            btnAddImageFiles.Enabled = true;
        }

        private void btnAddImageFiles_Click(object sender, EventArgs e)
        {
            //Open file dialog to select image files
            using var ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif";

            if (ofd.ShowDialog() != DialogResult.OK) return;

            _stickerFiles.Clear();

            foreach (var file in ofd.FileNames)
            {
                if (_stickerFiles.Contains(file)) continue;
                _stickerFiles.Add(file);
            }

            btnGenerate.Enabled = _stickerFiles.Count > 0;
        }















        ////load single image
        //var bmp = new Bitmap(@"3P_Galatea_1_O_D.png");

        ////Resize to 50% using graphics
        //var newWidth = bmp.Width / 2;
        //var newHeight = bmp.Height / 2;
        //var resizedBmp = new Bitmap(newWidth, newHeight);
        //using (var g = Graphics.FromImage(resizedBmp))
        //{
        //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //    g.DrawImage(bmp, 0, 0, newWidth, newHeight);
        //}

        //bmp = resizedBmp;


        //var pixels = FindAllPixelsThatHaveAnAdjacentTransparentPixel(bmp);

        ////Draw red dots on those pixels
        //using (var g = Graphics.FromImage(bmp))
        //{
        //    //Set smoothing mode to high quality
        //    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        //    foreach (var pixel in pixels)
        //    {
        //        g.FillEllipse(Brushes.Black, pixel.X - 2, pixel.Y - 2, 4, 4);
        //    }
        //}

        //var whitePixels = FindAllPixelsThatHaveAnAdjacentTransparentPixel(bmp);

        ////Draw white dots on those pixels
        //using (var g = Graphics.FromImage(bmp))
        //{
        //    //Set smoothing mode to high quality
        //    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        //    foreach (var pixel in whitePixels)
        //    {
        //        g.FillEllipse(Brushes.White, pixel.X - 1, pixel.Y - 1, 2, 2);
        //    }
        //}


        //pictureBox1.Image = bmp;



    }

    //private Point[] FindAllPixelsThatHaveAnAdjacentTransparentPixel(Bitmap bmp)
    //{
    //    //Find all pixels that have an adjacent transparent pixel
    //    var pixels = new List<Point>();
    //    for (int y = 1; y < bmp.Height - 1; y++)
    //    {
    //        for (int x = 1; x < bmp.Width - 1; x++)
    //        {
    //            var pixel = bmp.GetPixel(x, y);
    //            if (pixel.A == 0)
    //                continue;
    //            var adjacentPixels = new Point[]
    //            {
    //                new Point(x - 1, y),
    //                new Point(x + 1, y),
    //                new Point(x, y - 1),
    //                new Point(x, y + 1),
    //                new Point(x - 1, y - 1),
    //                new Point(x + 1, y - 1),
    //                new Point(x - 1, y + 1),
    //                new Point(x + 1, y + 1),
    //            };
    //            foreach (var adjacentPixel in adjacentPixels)
    //            {
    //                var adjacentPixelColor = bmp.GetPixel(adjacentPixel.X, adjacentPixel.Y);
    //                if (adjacentPixelColor.A == 0)
    //                {
    //                    pixels.Add(new Point(x, y));
    //                    break;
    //                }
    //            }
    //        }
    //    }

    //    return pixels.ToArray();
    //}


}



