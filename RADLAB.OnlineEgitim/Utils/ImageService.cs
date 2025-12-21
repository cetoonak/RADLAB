using Ardalis.GuardClauses;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;

namespace RADLAB.OnlineEgitim.Utils
{
    public class ImageService
    {
        private Bitmap ResizeImage(Image image, int width)
        {
            int height = (int)(image.Height * (decimal)width / image.Width);

            var destRect = new Rectangle(0, 0, width, height);
            var resultImage = new Bitmap(width, height);
            resultImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(resultImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }

                return resultImage;
            }
        }

        private Image Base64ToImage(string img)
        {
            byte[] bytes = Convert.FromBase64String(img);

            Image result;

            using (var ms = new MemoryStream(bytes))
            {
                result = Image.FromStream(ms);
            }

            return result;
        }

        public string ResizeImage(string base64String, int width, ImageFormat format)
        {
            Guard.Against.Null(base64String, "Image");
            var img = Regex.Replace(base64String, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);
            Image resizeImage = ResizeImage(Base64ToImage(img), width);
            using MemoryStream ms1 = new MemoryStream();
            resizeImage.Save(ms1, format);

            return Convert.ToBase64String(ms1.ToArray());

            //return $"data:image/png;base64,{Convert.ToBase64String(ms1.ToArray())}";
        }
    }
}