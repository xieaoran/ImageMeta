using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using ImageMeta.Annotations;
using ImageMeta.Configs;
using ImageMeta.Helpers;

namespace ImageMeta.Models
{
    public class ImageProperties : INotifyPropertyChanged
    {
        private double _dpiX;
        private double _dpiY;

        private Dictionary<string, string> _exifInfos;
        private int _pixelHeight;
        private int _pixelWidth;

        public ImageProperties(BitmapSource image, Stream fileStream = null)
        {
            PixelWidth = image.PixelWidth;
            PixelHeight = image.PixelHeight;
            DpiX = image.DpiX;
            DpiY = image.DpiY;
            if (fileStream == null) return;
            try
            {
                ExifInfos = ExifHelper.GetExifInfos(fileStream);
            }
            catch
            {
                ExifInfos = new Dictionary<string, string>();
            }
        }

        public int PixelWidth
        {
            get { return _pixelWidth; }
            set
            {
                if (_pixelWidth == value) return;
                _pixelWidth = value;
                OnPropertyChanged();
            }
        }

        public int PixelHeight
        {
            get { return _pixelHeight; }
            set
            {
                if (_pixelHeight == value) return;
                _pixelHeight = value;
                OnPropertyChanged();
            }
        }

        public double DpiX
        {
            get { return _dpiX; }
            set
            {
                if (Math.Abs(_dpiX - value) < StaticConfig.Tolerence) return;
                _dpiX = value;
                OnPropertyChanged();
            }
        }

        public double DpiY
        {
            get { return _dpiY; }
            set
            {
                if (Math.Abs(_dpiY - value) < StaticConfig.Tolerence) return;
                _dpiY = value;
                OnPropertyChanged();
            }
        }

        public Dictionary<string, string> ExifInfos
        {
            get { return _exifInfos; }
            set
            {
                if (_exifInfos == value) return;
                _exifInfos = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}