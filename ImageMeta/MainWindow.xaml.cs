using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ImageMeta.Helpers;
using ImageMeta.Huffman;
using ImageMeta.Models;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using Telerik.Windows.Media.Imaging;

namespace ImageMeta
{
    /// <summary>
    ///     MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : RadRibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();
            SetDataContext();
        }

        private MainWindowViewModel ViewModel { get; }

        public void SetDataContext()
        {
            ImageRenderArea.DataContext = ViewModel;
            ImageProperties.DataContext = ViewModel;
            MetaColorsView.ItemsSource = ViewModel.MetaColors;
            MetaCountSlider.DataContext = ViewModel.MetaArgs;
            MetaErrorSlider.DataContext = ViewModel.MetaArgs;
        }

        private void OpenImage(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "位图文件(*.bmp)|*.bmp|JPEG文件(*.jpg;*.jpeg)|*.jpg;*.jpeg|PNG文件(*.png)|*.png"
            };
            var result = openDialog.ShowDialog();
            if (result != null && result.Value)
            {
                ViewModel.Image = new RadBitmap(openDialog.OpenFile());
                ViewModel.Properties = new ImageProperties(ViewModel.Image.Bitmap, openDialog.OpenFile());
                ExifTree.Items.Clear();
                foreach (var key in ViewModel.Properties.ExifInfos.Keys)
                {
                    ExifTree.Items.Add(new RadTreeViewItem() { Header = $"{key} - {ViewModel.Properties.ExifInfos[key]}" });
                }
            }
        }

        private void SaveImage(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog()
            {
                CheckPathExists = true,
                Filter = "位图文件(*.bmp)|*.bmp"
            };
            var result = saveDialog.ShowDialog();
            if (result != null && result.Value)
            {
                var encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(ViewModel.ImageSource));
                encoder.Save(saveDialog.OpenFile());
            }
        }

        private void OpenCompressed(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "压缩位图文件(*.cbmp)|*.cbmp"
            };
            var result = openDialog.ShowDialog();
            if (result != null && result.Value)
            {
                var huffmanFile = new HuffmanFile(openDialog.FileName + "_key");
                var imageBytes = huffmanFile.ReadEncodedFile(openDialog.FileName);
                var imageStream = new MemoryStream(imageBytes);
                ViewModel.Image = new RadBitmap(imageStream);
                ViewModel.Properties = new ImageProperties(ViewModel.Image.Bitmap, openDialog.OpenFile());
                ExifTree.Items.Clear();
                foreach (var key in ViewModel.Properties.ExifInfos.Keys)
                {
                    ExifTree.Items.Add(new RadTreeViewItem() { Header = $"{key} - {ViewModel.Properties.ExifInfos[key]}" });
                }
            }
        }

        private void SaveCompressed(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog()
            {
                CheckPathExists = true,
                Filter = "压缩位图文件(*.cbmp)|*.cbmp"
            };
            var result = saveDialog.ShowDialog();
            if (result != null && result.Value)
            {
                var encoder = new BmpBitmapEncoder();
                var destStream = new MemoryStream();
                encoder.Frames.Add(BitmapFrame.Create(ViewModel.ImageSource));
                encoder.Save(destStream);
                var imageBytes = destStream.ToArray();
                var huffmanFile = new HuffmanFile(imageBytes);
                huffmanFile.EncodeFile(saveDialog.FileName, saveDialog.FileName + "_key");
            }
        }

        private void GetMetaColors(object sender, RoutedEventArgs e)
        {
            var imageLength = ViewModel.Properties.PixelHeight*ViewModel.Properties.PixelWidth*3;
            var imagePtr = Marshal.AllocHGlobal(imageLength);
            var outColorsPtr = Marshal.AllocHGlobal(ViewModel.MetaArgs.MetaCount*3);
            var pixels = ViewModel.Image.GetPixels();
            for (var index = 0; index < pixels.Length; index++)
            {
                var pixel = pixels[index];
                Marshal.WriteByte(imagePtr, index*3 + 2, (byte) pixel); // B
                Marshal.WriteByte(imagePtr, index*3 + 1, (byte) (pixel >> 8)); // G
                Marshal.WriteByte(imagePtr, index*3, (byte) (pixel >> 16)); // R
            }
            MetaHelper.meta_calculate(imagePtr, imageLength, ViewModel.MetaArgs.MetaCount, outColorsPtr,
                ViewModel.MetaArgs.MaxError);
            ViewModel.MetaColors.Clear();
            for (var index = 0; index < ViewModel.MetaArgs.MetaCount; index++)
            {
                var r = Marshal.ReadByte(outColorsPtr, index*3);
                var g = Marshal.ReadByte(outColorsPtr, index*3 + 1);
                var b = Marshal.ReadByte(outColorsPtr, index*3 + 2);
                ViewModel.MetaColors.Add(Color.FromRgb(r, g, b));
            }
            Marshal.FreeHGlobal(imagePtr);
            Marshal.FreeHGlobal(outColorsPtr);
        }
    }
}