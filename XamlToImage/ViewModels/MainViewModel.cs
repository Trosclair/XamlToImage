using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using XamlToPNG.Utilities;

namespace XamlToPNG.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private bool hasXamlFiles = false;

        public RelayCommand OpenFilesCommand => new(OpenFiles);
        public RelayCommand OpenFoldersCommand => new(OpenFolders);
        public RelayCommand ExportImagesCommand => new(ExportImages);

        public ObservableCollection<LoadedXaml> LoadedXamls { get; } = [];
        public ObservableCollection<string> Issues { get; } = [];

        public bool HasXamlFiles { get => hasXamlFiles; set { hasXamlFiles = value; OnPropertyChanged(nameof(HasXamlFiles)); } }

        private void OpenFiles()
        {
            OpenFileDialog ofd = new()
            {
                Filter = "Xaml Files (*.xaml)|*.xaml|All Files (*.*)|*.*",
                Multiselect = true
            };

            if (ofd.ShowDialog() == true)
            {
                IEnumerable<string> files = ofd.FileNames.Except(LoadedXamls.Select(x => x.XamlFilename));

                LoadedXamls.AddRange(files.Select(x => new LoadedXaml(x)));
            }

            HasXamlFiles = LoadedXamls.Count > 0;
        }

        private void OpenFolders()
        {
            OpenFolderDialog ofd = new()
            {
                Multiselect = true
            };

            if (ofd.ShowDialog() == true)
            {
                IEnumerable<string> files = 
                    ofd.FolderNames
                    .Select(x => new DirectoryInfo(x).GetFiles())
                    .SelectMany(x => x)
                    .Where(x => x.Extension.EndsWith("xaml", StringComparison.OrdinalIgnoreCase))
                    .Select(x => x.FullName)
                    .Except(LoadedXamls.Select(x => x.XamlFilename));

                LoadedXamls.AddRange(files.Select(x => new LoadedXaml(x)));
            }

            HasXamlFiles = LoadedXamls.Count > 0;
        }

        private void ExportImages()
        {
            OpenFolderDialog ofd = new();

            if (ofd.ShowDialog() == true)
            {
                string outputDirectoryPath = Path.Combine(ofd.FolderName, "Exported Xaml Images\\");
                int i = 1;
                while (Directory.Exists(outputDirectoryPath))
                {
                    outputDirectoryPath = Path.Combine(ofd.FolderName, $"Exported Xaml Images({i})\\");
                    i++;
                }

                Directory.CreateDirectory(outputDirectoryPath);

                Issues.AddRange(LoadedXamls.Select(x => x.ConvertAll(outputDirectoryPath)).SelectMany(x => x).OfType<Exception>().Select(x => x.Message));
            }
        }
    }
}
