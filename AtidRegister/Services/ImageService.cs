using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace AtidRegister.Services
{
    /// <summary>
    /// a <see langword="static"/> Service class for image manipulation.
    /// </summary>
    public class ImageService
    {
        /// <summary>
        /// Converts image of types jpeg or png to base64 string, including prefix
        /// </summary>
        /// <param name="file">the image IFormFile instance</param>
        /// <returns>base64 string, e.g. image/jpeg;base64,DATADATADATA</returns>
        public static async Task<string> GetJpegBase64String(IFormFile file)
        {
            // create Image instance
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var img = Image.FromStream(memoryStream);
            var finalImage = img;
            int newWidth = img.Width, newHeight = img.Height;
            double HeightWidthRatio = (double)img.Height / img.Width;
            if (img.Width > 250)
            { // change and calculate new height & width:
                newWidth = 250;
                newHeight = (int)(newWidth * HeightWidthRatio);
            }
            if (newWidth != img.Width)
                finalImage = ResizeImage(img, newWidth, newHeight);
            memoryStream = new MemoryStream();
            // copy to ms
            finalImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            // get final base64 string + prefix
            string base64MemoryString = Convert.ToBase64String(memoryStream.ToArray());
            return "image/jpeg;base64," + base64MemoryString;
        }
        /// <summary>
        /// Resizes an Image, Preserving it's quality.
        /// Code by @mpen on StackOverflow.com
        /// <see cref="https://stackoverflow.com/questions/1922040/how-to-resize-an-image-c-sharp"/>
        /// </summary>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
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
            }

            return destImage;
        }
    }
}
