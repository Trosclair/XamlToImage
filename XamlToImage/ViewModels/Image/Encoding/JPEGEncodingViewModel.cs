using System.IO;
using System.Windows.Media.Imaging;

namespace XamlToPNG.ViewModels.Image.Encoding
{
    public class JPEGEncodingViewModel() : ImageEncodingViewModel("JPEG", ".jpeg")
    {
        public override byte[] Convert(RenderTargetBitmap bitmap)
        {
            JpegBitmapEncoder encoder = new()
            {
                QualityLevel = 100
            };
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            using MemoryStream ms = new();
            encoder.Save(ms);
            return ms.ToArray();
        }
    }
}
