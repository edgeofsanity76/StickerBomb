using System.Drawing;
using System.Drawing.Drawing2D;

// ReSharper disable PossibleLossOfFraction

#pragma warning disable CA1416

namespace EoS.StickerBomb;

public class Generator : IDisposable
{
    private Bitmap? _canvass;
    private Graphics? _graphics;
    private bool _canvassSet;

    public record GridCell
    {
        public Point Position;
        public int Width;
        public int Height;
    }

    private readonly List<Bitmap> _stickers = new();
    private readonly List<GridCell> _gridCells = new();

    private int _originalX;
    private int _originalY;

    public List<GridCell> GridCells => _gridCells;

    private void ApplyBorder(Bitmap bmp, int borderSize, Color borderColor)
    {
        if (borderSize <= 1) return;
            
        var pixels = FindAllPixelsThatHaveAnAdjacentTransparentPixel(bmp);

        using var g = Graphics.FromImage(bmp);

        g.SmoothingMode = SmoothingMode.AntiAlias;
        foreach (var pixel in pixels)
        {
            g.FillEllipse(new SolidBrush(borderColor), pixel.X - borderSize, pixel.Y - borderSize, borderSize * 2, borderSize * 2);
        }
    }

    public void Initialize(int width, int height)
    {
        if (width <= 0 || height <= 0) throw new ArgumentException("Width and height must be greater than 0.");

        //Set canvass
        _originalX = width;
        _originalY = height;

        //Create canvass with 10% overlap so that stickers can go over the edge
        var overlapX = (int)(width * 1.1);
        var overlapY = (int)(height * 1.1);

        _canvass = new Bitmap(overlapX, overlapY);
        _graphics = Graphics.FromImage(_canvass);

        CreateGrid(overlapX, overlapY, 5);

        //Draw transparent background
        _graphics.Clear(Color.Transparent);

        //Set high quality rendering
        _graphics.SmoothingMode = SmoothingMode.HighQuality;
        foreach (var cell in _gridCells) 
        {
            //Draw rectangle with light gray border
            _graphics.DrawRectangle(new Pen(Color.LightGray, 1), cell.Position.X, cell.Position.Y, cell.Width, cell.Height);
        }
            
        _canvassSet = true;
    }

    public void ApplyStickers(Action<Bitmap>? onUpdate)
    {
        foreach (var cell in GridCells) ApplyStickers(cell, onUpdate);
    }

    public void ApplyStickers(GridCell cell, Action<Bitmap>? onUpdate)
    {
        ApplyStickers(cell, onUpdate, 20);
    }

    public void ApplyStickers(GridCell cell, Action<Bitmap>? onUpdate, int maxImagesPerCell)
    {
        if (cell.Width <= 0 || cell.Height <= 0) throw new ArgumentException("Cell width and height must be greater than 0.");
        if (_stickers.Count == 0) throw new InvalidOperationException("No stickers loaded. Call LoadStickers() first.");
        if (maxImagesPerCell <= 0) throw new ArgumentException("Max images per cell must be greater than 0.");
            
        //Apply stickers to canvass
        if (!_canvassSet) throw new InvalidOperationException("Canvass not set. Call Initialize() first.");

        var seed = DateTime.Now.Millisecond;
        var rnd = new Random(seed);
            
        var cellArea = new Bitmap(cell.Width, cell.Height);
            
        using (var g = Graphics.FromImage(cellArea))
        {
            g.DrawImage(_canvass!, new Rectangle(0, 0, cellArea.Width, cellArea.Height), new Rectangle(cell.Position.X, cell.Position.Y, cellArea.Width, cellArea.Height), GraphicsUnit.Pixel);
        }

        var transparentPixels = FindAllTransparentPixels(cellArea);

        //Shuffle list of pixels
        transparentPixels = transparentPixels.OrderBy(_ => rnd.Next()).ToList();

        foreach (var pixel in transparentPixels.Take(maxImagesPerCell))
        {
            var sticker = _stickers[rnd.Next(0, _stickers.Count)];
                
            //Resize
            var newHeight = rnd.Next(100, rnd.Next(100, (int)(_originalX * 0.1) + 100));
            var aspectRatio = (float)sticker.Width / sticker.Height;
            var newWidth = (int)(newHeight * aspectRatio);
            var resizedSticker = Resize(sticker, newWidth, newHeight);

            //Rotate
            var angle = rnd.Next(-60, 60);
            var rotatedSticker = Rotate(resizedSticker, angle);

            //Position
            var x = cell.Position.X + pixel.X - resizedSticker.Width;
            var y = cell.Position.Y + pixel.Y - resizedSticker.Height;

            //Apply white border
            ApplyBorder(rotatedSticker, 2, Color.White);

            //Draw on canvass
            _graphics!.DrawImage(rotatedSticker, x, y);


            if (onUpdate == null) continue;

            //If onUpdate is set, crop canvass to original and send update
            var updateCanvass = new Bitmap(_originalX, _originalY);
            using (var g = Graphics.FromImage(updateCanvass))
            {
                g.DrawImage(_canvass!, new Rectangle(0, 0, updateCanvass.Width, updateCanvass.Height), new Rectangle((_canvass!.Width - updateCanvass.Width) / 2, (_canvass.Height - updateCanvass.Height) / 2, updateCanvass.Width, updateCanvass.Height), GraphicsUnit.Pixel);
            }

            onUpdate.Invoke((Bitmap)updateCanvass.Clone());
        }
    }

    public Bitmap GetCanvass()
    {
        if (!_canvassSet) throw new InvalidOperationException("Canvass not set. Call Initialize() first.");
        return _canvass!;
    }

    public void SaveCanvass(string filePath)
    {
        if (!_canvassSet) throw new InvalidOperationException("Canvass not set. Call Initialize() first.");

        //Crop canvass to original size
        var croppedCanvass = new Bitmap(_originalX, _originalY);
        using (var g = Graphics.FromImage(croppedCanvass))
        {
            g.DrawImage(_canvass!, 0, 0, new Rectangle(0, 0, _originalX, _originalY), GraphicsUnit.Pixel);
        }
        croppedCanvass.Save(filePath);
    }

    public void LoadStickers(params string[] filePaths)
    {
        _stickers.Clear();

        foreach (var filePath in filePaths)
        {
            if (!File.Exists(filePath)) continue;
            var bmp = new Bitmap(filePath);
            _stickers.Add(bmp);
        }
    }

    private void CreateGrid(int width, int height, int cellsPerAxis)
    {
        if (cellsPerAxis <= 0) throw new ArgumentException("Cells per axis must be greater than 0.");
        if (width <= 0 || height <= 0) throw new ArgumentException("Width and height must be greater than 0.");

        _gridCells.Clear();

        var cellWidth = width / cellsPerAxis;
        var cellHeight = height / cellsPerAxis;
        for (var y = 0; y < height; y += cellHeight)
        {
            for (var x = 0; x < width; x += cellWidth)
            {
                var position = new Point(x, y);

                _gridCells.Add(new GridCell
                {
                    Position = position,
                    Width = cellWidth,
                    Height = cellHeight
                });
            }
        }
    }


    public Bitmap Resize(Bitmap sourceBitmap, int newWidth, int newHeight)
    {
        if (newWidth <= 0 || newHeight <= 0) throw new ArgumentException("New width and height must be greater than 0.");

        var resizedBmp = new Bitmap(newWidth, newHeight);
        using var g = Graphics.FromImage(resizedBmp);
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.DrawImage(sourceBitmap, 0, 0, newWidth, newHeight);
        return resizedBmp;
    }

    public Bitmap Rotate(Bitmap sourceBitmap, float angle)
    {
        var rotate = new Matrix();
            
        rotate.Translate(sourceBitmap.Width / -2, sourceBitmap.Height / -2, MatrixOrder.Append);
        rotate.RotateAt(angle, new Point(0, 0), MatrixOrder.Append);

        using var gp = new GraphicsPath();
            
        gp.AddPolygon(new Point(0, 0), new Point(sourceBitmap.Width, 0), new Point(0, sourceBitmap.Height));
        gp.Transform(rotate);
            
        var pts = gp.PathPoints;
        var boundingBox = BoundingBox(sourceBitmap, rotate);
        var bmpDest = new Bitmap(boundingBox.Width, boundingBox.Height);

        using var dest = Graphics.FromImage(bmpDest);
            
        var destTransform = new Matrix();
            
        destTransform.Translate(bmpDest.Width / 2, bmpDest.Height / 2, MatrixOrder.Append);
        dest.Transform = destTransform;
        dest.DrawImage(sourceBitmap, pts);

        return bmpDest;
    }

    private static Rectangle BoundingBox(Image img, Matrix matrix)
    {
        var gu = new GraphicsUnit();
        var rImg = Rectangle.Round(img.GetBounds(ref gu));
        var topLeft = new Point(rImg.Left, rImg.Top);
        var topRight = new Point(rImg.Right, rImg.Top);
        var bottomRight = new Point(rImg.Right, rImg.Bottom);
        var bottomLeft = new Point(rImg.Left, rImg.Bottom);
        var points = new[] { topLeft, topRight, bottomRight, bottomLeft };
        var gp = new GraphicsPath(points, [(byte)PathPointType.Start, (byte)PathPointType.Line, (byte)PathPointType.Line, (byte)PathPointType.Line]);

        gp.Transform(matrix);
            
        return Rectangle.Round(gp.GetBounds());
    }

    public List<Point> FindAllTransparentPixels(Bitmap bmp)
    {
        var pixels = new List<Point>();
        for (var y = 0; y < bmp.Height; y += 20)
        {
            for (var x = 0; x < bmp.Width; x += 20)
            {
                var pixel = bmp.GetPixel(x, y);
                if (pixel.A == 0)
                {
                    pixels.Add(new Point(x, y));
                }
            }
        }

        return pixels;
    }

    private static List<Point> FindAllPixelsThatHaveAnAdjacentTransparentPixel(Bitmap bmp)
    {
        var pixels = new List<Point>();
        for (var y = 1; y < bmp.Height - 1; y++)
        {
            for (var x = 1; x < bmp.Width - 1; x++)
            {
                var pixel = bmp.GetPixel(x, y);
                if (pixel.A == 0)
                    continue;

                var adjacentPixels = new List<Color>
                {
                    bmp.GetPixel(x - 1, y),
                    bmp.GetPixel(x + 1, y),
                    bmp.GetPixel(x, y - 1),
                    bmp.GetPixel(x, y + 1)
                };
                if (adjacentPixels.Any(p => p.A == 0))
                {
                    pixels.Add(new Point(x, y));
                }
            }
        }

        return pixels;
    }

    public void Dispose()
    {
        _canvass?.Dispose();
        _graphics?.Dispose();
    }
}