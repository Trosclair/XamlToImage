using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlToPNG.Utilities;

namespace XamlToPNG.ViewModels
{
    public class ResolutionViewModel : ObservableObject
    {
        private double width, height, aspectRatio;
        private bool preserveAspectRatio = true;

        public double Width { get => width; set { width = value; XChanged(); OnPropertyChanged(nameof(Width)); } }
        public double Height { get => height; set { height = value; YChanged(); OnPropertyChanged(nameof(Height)); } }
        public double AspectRatio { get => aspectRatio; set { aspectRatio = value; OnPropertyChanged(nameof(AspectRatio)); } }
        public bool PreserveAspectRatio { get => preserveAspectRatio; set { preserveAspectRatio = value; OnPropertyChanged(nameof(PreserveAspectRatio)); } }

        public ResolutionViewModel(double width, double height)
        {
            Width = width;
            Height = height;
            AspectRatio = width / height;
        }

        private void XChanged()
        {
            if (PreserveAspectRatio)
            {
                height = Width * AspectRatio;
                OnPropertyChanged(nameof(Height));
            }
        }

        private void YChanged()
        {
            if (PreserveAspectRatio)
            {
                width = height / AspectRatio;
                OnPropertyChanged(nameof(Width));
            }
        }
    }
}
