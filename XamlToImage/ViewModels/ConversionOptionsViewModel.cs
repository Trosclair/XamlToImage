using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using XamlToPNG.Utilities;
using XamlToPNG.ViewModels.Image.Encoding;

namespace XamlToPNG.ViewModels
{
    public class ConversionOptionsViewModel : ObservableObject
    {
        private ImageEncodingViewModel? selectedImageEncoding;
        private ResolutionViewModel resolution;
        public ImageEncodingViewModel? SelectedImageEncoding { get => selectedImageEncoding; set { selectedImageEncoding = value; OnPropertyChanged(nameof(SelectedImageEncoding)); } }
        public ResolutionViewModel Resolution { get => resolution; set { resolution = value; OnPropertyChanged(nameof(Resolution)); } }

        public RelayCommand RemoveConversionOptionCommand { get; }

        public ConversionOptionsViewModel(ImageEncodingViewModel? selectedImageEncoding, ResolutionViewModel resolution, Action<ConversionOptionsViewModel> removeOptions)
        {
            this.selectedImageEncoding = selectedImageEncoding;
            this.resolution = resolution;
            RemoveConversionOptionCommand = new(() => removeOptions(this));
        }

        public Exception? Convert(string directoryToSaveInPath, string xamlFilename)
        {
            Exception? e = null;

            try
            {
                using FileStream s = new(xamlFilename, FileMode.Open);
                Control control = (Control)XamlReader.Load(s);
                s.Close();

                if (SelectedImageEncoding is null)
                {
                    throw new Exception("No encoding set for an image!");
                }

                Viewbox vb = new()
                {
                    Child = control,
                    Stretch = Resolution.PreserveAspectRatio ? System.Windows.Media.Stretch.UniformToFill : System.Windows.Media.Stretch.None,
                    Height = Resolution.Height,
                    Width = Resolution.Width
                };

                Rect rect = new() { Width = Resolution.Width, Height = Resolution.Height };
                vb.Arrange(rect);

                RenderTargetBitmap rtb = new((int)Resolution.Width, (int)Resolution.Height, 96, 96, System.Windows.Media.PixelFormats.Pbgra32);
                rtb.Render(vb);

                string fileName = $"{directoryToSaveInPath}\\{new FileInfo(xamlFilename).Name.Replace(".xaml", string.Empty)}_Hgt{Resolution.Height}_Wdt{Resolution.Width}{SelectedImageEncoding.FileExtension}";

                byte[] bytes = SelectedImageEncoding.Convert(rtb);

                File.WriteAllBytes(fileName, bytes);
            }
            catch (Exception ex)
            { 
                e = ex;
            }

            return e;
        }
    }
}
