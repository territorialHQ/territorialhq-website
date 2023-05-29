using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TerritorialHQ.Helpers
{
    public static class ImageHelper
    {
        private const int _maxSize = 1920;
        private const int _quality = 80;
        private const float _crop_ratio = 1.5f;

        public static async Task<string> ProcessImage(IFormFile imageFile, string uploadDir, bool autoFileName = true, string? oldFile = null, bool crop = false)
        {
            using (var stream = imageFile.OpenReadStream())
            {
                if (!String.IsNullOrEmpty(oldFile))
                {
                    var oldFilePath = Path.Combine(uploadDir, oldFile);
                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);
                }

                var ext = Path.GetExtension(imageFile.FileName).ToLower();
                //await imageFile.CopyToAsync(ms);
                //ms.Flush();

                var i = Image.Load(stream);

                if (crop)
                {
                    i = AutoCrop(i);
                }
                i = AutoResize(i);

                var path = uploadDir;
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                var filename = imageFile.FileName;
                if (autoFileName)
                {
                    filename = Guid.NewGuid().ToString() + ext;
                }
                var filepath = Path.Combine(path, filename);

                if (!System.IO.File.Exists(filepath))
                    System.IO.File.Delete(filepath);

                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    switch (ext)
                    {
                        case ".png":
                            i.SaveAsPng(fs);
                            break;
                        case ".gif":
                            i.SaveAsGif(fs);
                            break;
                        default:
                            JpegEncoder encoder = new JpegEncoder()
                            {
                                Quality = _quality
                            };
                            i.SaveAsJpeg(fs, encoder);
                            break;
                    }
                }

                return filename;
            }
        }

        public static Image AutoResize(Image image)
        {
            if (image.Width > _maxSize || image.Height > _maxSize)
            {
                image.Mutate(
                        i => i.Resize(new ResizeOptions
                        {
                            Size = new Size(_maxSize, _maxSize),
                            Mode = ResizeMode.Max
                        })
                    );
            }

            return image;
        }

        public static Image AutoCrop(Image image)
        {
            float currRatio = (float)image.Width / (float)image.Height;

            if (currRatio == _crop_ratio)
                return image;

            if (currRatio > _crop_ratio)
            {
                var newWidth = image.Height * _crop_ratio;
                var cropRect = new Rectangle((int)Math.Round((image.Width / 2f) - (newWidth / 2f)), 0, (int)Math.Round(newWidth), image.Height);
                image.Mutate(i => i.Crop(cropRect));
            }
            else
            {
                var newHeight = image.Width / _crop_ratio;
                var cropRect = new Rectangle(0, (int)Math.Round((image.Height / 2f) - (newHeight / 2f)), image.Width, (int)Math.Round(newHeight));
                image.Mutate(i => i.Crop(image.Width, (int)Math.Round(newHeight)));
            }

            return image;
        }
    }
}
