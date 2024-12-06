using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using XamlToPNG.Utilities;

namespace XamlToPNG.ViewModels.Image.Encoding
{
    public abstract class ImageEncodingViewModel(string name, string fileExtension) : ObservableObject
    {
        public string Name { get; private set; } = name;
        public string FileExtension { get; private set; } = fileExtension;
        public abstract byte[] Convert(RenderTargetBitmap bitmap);

        public static IEnumerable<ImageEncodingViewModel> GetEncodings()
        {
            List<ImageEncodingViewModel> imageEncodings = [];

            Assembly thisAssembly = Assembly.GetExecutingAssembly();

            Type[] types = thisAssembly.GetTypes();

            foreach (Type type in types.Where(x => !x.IsAbstract && x.BaseType == typeof(ImageEncodingViewModel)))
            {
                if (Activator.CreateInstance(type) is ImageEncodingViewModel ie)
                {
                    imageEncodings.Add(ie);
                }
                else
                {
                    MessageBox.Show($"Unable to load encoding for type: {type.Name}");
                }
            } 

            return imageEncodings;
        }
    }
}
