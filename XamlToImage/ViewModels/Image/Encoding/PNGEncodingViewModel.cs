using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using XamlToPNG.Utilities;

namespace XamlToPNG.ViewModels.Image.Encoding
{
    public class PNGEncodingViewModel() : ImageEncodingViewModel("PNG", ".png")
    {
        public override byte[] Convert(RenderTargetBitmap bitmap)
        {
            PngBitmapEncoder encoder = new();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            using MemoryStream ms = new();
            encoder.Save(ms);
            return ms.ToArray();
        }
    }
}
