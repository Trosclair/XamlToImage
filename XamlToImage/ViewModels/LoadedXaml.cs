using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using XamlToPNG.Utilities;
using XamlToPNG.ViewModels.Image.Encoding;

namespace XamlToPNG.ViewModels
{
    public class LoadedXaml : ObservableObject
    {
        private Control displayElement;
        private readonly Control exportElement;
        private readonly ImageEncodingViewModel? imageEncodingViewModel;
        private readonly static ObservableCollection<ImageEncodingViewModel> imageEncodings = [];

        public RelayCommand AddConversionCommand => new(AddConversion);

        public static ObservableCollection<ImageEncodingViewModel> ImageEncodings
        {
            get  
            {
                if (imageEncodings.Count == 0)
                {
                    foreach (ImageEncodingViewModel ie in ImageEncodingViewModel.GetEncodings())
                    {
                        imageEncodings.Add(ie);
                    }
                }
                return imageEncodings;
            } 
        }

        public string XamlFilename { get; }
        public Control DisplayElement { get => displayElement; set { displayElement = value; OnPropertyChanged(nameof(DisplayElement)); } }
        public ObservableCollection<ConversionOptionsViewModel> ConversionOptions { get; } = [];

        public LoadedXaml(string xamlFilename, ImageEncodingViewModel? defaultImageEncoding = null)
        {
            XamlFilename = xamlFilename;

            imageEncodingViewModel = defaultImageEncoding ?? ImageEncodings.FirstOrDefault(x => x.Name.Equals("PNG", StringComparison.OrdinalIgnoreCase));

            using FileStream s = new(xamlFilename, FileMode.Open);
            displayElement = (Control)XamlReader.Load(s);
            s.Close();

            AddConversion();
        }

        internal IEnumerable<Exception?> ConvertAll(string directoryToSaveInPath)
        {
            return ConversionOptions.Select(x => x.Convert(directoryToSaveInPath, XamlFilename));
        }

        private void AddConversion()
        {
            ConversionOptions.Add(new(imageEncodingViewModel, new(displayElement.Width, displayElement.Height), RemoveConversion));
        }

        private void RemoveConversion(ConversionOptionsViewModel conversionOptions)
        {
            ConversionOptions.Remove(conversionOptions);
        }
    }
}
