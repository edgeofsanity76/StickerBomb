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

            var maxStickersPerCell = (int)numMaxStickersPerCell.Value;

            //Set canvass size
            generator.Initialize(x, y);

            //Load stickers from Stickers folder

            generator.LoadStickers(_stickerFiles.ToArray());

            generator.ApplyStickers(bitmap =>
            {
                pictureBox1.Image = bitmap;
                pictureBox1.Refresh();
                Application.DoEvents();

            }, bitmap =>
            {
                var outputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output");
                if (!Directory.Exists(outputFolder))
                    Directory.CreateDirectory(outputFolder);

                var outputFile = Path.Combine(outputFolder, $"StickerBomb_{DateTime.Now:yyyyMMdd_HHmmss}.png");

                bitmap.Save(outputFile, System.Drawing.Imaging.ImageFormat.Png);
                MessageBox.Show($"Sticker bomb saved to {outputFile}");

                btnGenerate.Enabled = _stickerFiles.Count > 0;
                btnAddImageFiles.Enabled = true;
            }, maxStickersPerCell);
        }

        private void ExampleUsage()
        {
            using var generator = new EoS.StickerBomb.Generator();
            generator.Initialize(2000, 2000);

            //Get sticker fil from Stickers folder
            var stickerFiles =
                Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Stickers"), "*.png");

            generator.LoadStickers(stickerFiles);

            generator.ApplyStickers(progressBitmap =>
            {
                //Display progress or do something else
            }, bitmap => { bitmap.Save("sticker_bomb.png", System.Drawing.Imaging.ImageFormat.Png); });
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

        private void frmStickerBombGenerator_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
