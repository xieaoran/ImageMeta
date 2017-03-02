using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ImageMeta.Annotations;
using ImageMeta.Configs;

namespace ImageMeta.Helpers
{
    public class MetaArgs : INotifyPropertyChanged
    {
        private int _maxError;
        private int _metaCount;

        public MetaArgs()
        {
            MetaCount = StaticConfig.DefaultMetaCount;
            MaxError = 0;
        }

        public int MetaCount
        {
            get { return _metaCount; }
            set
            {
                if (_metaCount == value) return;
                _metaCount = value;
                OnPropertyChanged();
            }
        }

        public int MaxError
        {
            get { return _maxError; }
            set
            {
                if (_maxError == value) return;
                _maxError = value;
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

    public static class MetaHelper
    {
        [DllImport("ImageMetaNative.dll")]
        public static extern void meta_calculate(IntPtr decodedImageRgbBytes, int byteLength, int metaCount,
            IntPtr outColors, int maxError);
    }
}