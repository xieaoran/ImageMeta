using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ImageMeta.Annotations;
using ImageMeta.Helpers;
using ImageMeta.Models;
using Telerik.Windows.Media.Imaging;

namespace ImageMeta
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private RadBitmap _image;

        private BitmapSource _imageSource;

        private ImageProperties _properties;

        public MainWindowViewModel()
        {
            MetaColors = new ObservableCollection<Color>();
            MetaArgs = new MetaArgs();
        }

        public RadBitmap Image
        {
            get { return _image; }
            set
            {
                if (_image == value) return;
                _image = value;
                ImageSource = value.Bitmap;
                MetaColors.Clear();
                OnPropertyChanged();
            }
        }

        public BitmapSource ImageSource
        {
            get { return _imageSource; }
            set
            {
                if (Equals(_imageSource, value)) return;
                _imageSource = value;
                OnPropertyChanged();
            }
        }

        public ImageProperties Properties
        {
            get { return _properties; }
            set
            {
                if (_properties == value) return;
                _properties = value;
                OnPropertyChanged();
            }
        }

        public MetaArgs MetaArgs { get; set; }

        public ObservableCollection<Color> MetaColors { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}