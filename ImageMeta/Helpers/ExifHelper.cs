using System.Collections.Generic;
using System.IO;
using System.Text;
using ExifLib;

namespace ImageMeta.Helpers
{
    public static class ExifHelper
    {
        public static Dictionary<string, object> GetExifInfosRaw(Stream fileStream)
        {
            var exifInfos = new Dictionary<string, object>();
            using (var exif = new ExifReader(fileStream, false))
            {
                object exifObject;
                if (exif.GetTagValue(ExifTags.ExifVersion, out exifObject))
                    exifInfos.Add("EXIF 版本", exifObject);
                if (exif.GetTagValue(ExifTags.DateTime, out exifObject))
                    exifInfos.Add("日期", exifObject);
                if (exif.GetTagValue(ExifTags.ColorSpace, out exifObject))
                    exifInfos.Add("颜色空间", exifObject);
                if (exif.GetTagValue(ExifTags.CameraOwnerName, out exifObject))
                    exifInfos.Add("相机所有者", exifObject);
                if (exif.GetTagValue(ExifTags.LensModel, out exifObject))
                    exifInfos.Add("镜头型号", exifObject);
                if (exif.GetTagValue(ExifTags.LensModel, out exifObject))
                    exifInfos.Add("镜头制造商", exifObject);
                if (exif.GetTagValue(ExifTags.LensSerialNumber, out exifObject))
                    exifInfos.Add("镜头序列号", exifObject);
                if (exif.GetTagValue(ExifTags.Make, out exifObject))
                    exifInfos.Add("相机制造商", exifObject);
                if (exif.GetTagValue(ExifTags.Model, out exifObject))
                    exifInfos.Add("相机型号", exifObject);
                if (exif.GetTagValue(ExifTags.BodySerialNumber, out exifObject))
                    exifInfos.Add("相机序列号", exifObject);
                if (exif.GetTagValue(ExifTags.Compression, out exifObject))
                    exifInfos.Add("压缩", exifObject);
                if (exif.GetTagValue(ExifTags.Software, out exifObject))
                    exifInfos.Add("应用程序", exifObject);
                if (exif.GetTagValue(ExifTags.Orientation, out exifObject))
                    exifInfos.Add("方向", exifObject);
                if (exif.GetTagValue(ExifTags.FocalLength, out exifObject))
                    exifInfos.Add("焦距", exifObject);
                if (exif.GetTagValue(ExifTags.DigitalZoomRatio, out exifObject))
                    exifInfos.Add("数码变焦", exifObject);
                if (exif.GetTagValue(ExifTags.WhiteBalance, out exifObject))
                    exifInfos.Add("白平衡", exifObject);
                if (exif.GetTagValue(ExifTags.Contrast, out exifObject))
                    exifInfos.Add("对比度", exifObject);
                if (exif.GetTagValue(ExifTags.FNumber, out exifObject))
                    exifInfos.Add("光圈", exifObject);
                if (exif.GetTagValue(ExifTags.ExposureBiasValue, out exifObject))
                    exifInfos.Add("曝光补偿", exifObject);
                if (exif.GetTagValue(ExifTags.ExposureMode, out exifObject))
                    exifInfos.Add("曝光模式", exifObject);
                if (exif.GetTagValue(ExifTags.ExposureProgram, out exifObject))
                    exifInfos.Add("曝光程序", exifObject);
                if (exif.GetTagValue(ExifTags.ExposureTime, out exifObject))
                    exifInfos.Add("曝光时间", exifObject);
                if (exif.GetTagValue(ExifTags.Flash, out exifObject))
                    exifInfos.Add("闪光灯", exifObject);
                if (exif.GetTagValue(ExifTags.FlashEnergy, out exifObject))
                    exifInfos.Add("闪光灯能量", exifObject);
                if (exif.GetTagValue(ExifTags.ISOSpeed, out exifObject))
                    exifInfos.Add("ISO 速度", exifObject);
                if (exif.GetTagValue(ExifTags.GPSLongitude, out exifObject))
                    exifInfos.Add("经度", exifObject);
                if (exif.GetTagValue(ExifTags.GPSLatitude, out exifObject))
                    exifInfos.Add("纬度", exifObject);
            }
            return exifInfos;
        }

        public static Dictionary<string, string> GetExifInfos(Stream fileStream)
        {
            var exifInfosRaw = GetExifInfosRaw(fileStream);
            var exifInfos = new Dictionary<string, string>();
            foreach (var exifKey in exifInfosRaw.Keys)
            {
                var exifValue = exifInfosRaw[exifKey];
                var type = exifValue.GetType();
                if (type == typeof(short) || type == typeof(int) || type == typeof(long) ||
                    type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong))
                {
                    exifInfos.Add(exifKey, exifValue.ToString());
                }
                else if (type == typeof(float) || type == typeof(double))
                {
                    exifInfos.Add(exifKey, exifValue.ToString());
                }
                else if (type == typeof(string))
                {
                    exifInfos.Add(exifKey, (string) exifValue);
                }
                else if (type == typeof(byte[]))
                {
                    exifInfos.Add(exifKey, Encoding.ASCII.GetString((byte[]) exifValue));
                }
                else if (type == typeof(float[]))
                {
                    var exifValues = (float[]) exifValue;
                    var exifStrings = new string[exifValues.Length];
                    for (var index = 0; index < exifValues.Length; index++)
                    {
                        exifStrings[index] = exifValues[index].ToString();
                    }
                    exifInfos.Add(exifKey, string.Join(",", exifStrings));
                }
                else if (type == typeof(double[]))
                {
                    var exifValues = (double[]) exifValue;
                    var exifStrings = new string[exifValues.Length];
                    for (var index = 0; index < exifValues.Length; index++)
                    {
                        exifStrings[index] = exifValues[index].ToString();
                    }
                    exifInfos.Add(exifKey, string.Join(",", exifStrings));
                }
                else
                {
                    exifInfos.Add(exifKey, exifValue.ToString());
                }
            }
            return exifInfos;
        }
    }
}