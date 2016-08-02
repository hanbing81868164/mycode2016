using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;


namespace System
{
    public static partial class Extention
    {
        #region System.Drawing.Image

        /// <summary>
        /// 将图像转换为字节格式
        /// </summary>
        /// <param name="image">要转换的图像</param>
        /// <returns>转换后的字节数组</returns>
        public static byte[] ToBytes(this Image image)
        {
            return ToBytes(image, ImageFormat.Png);
        }

        /// <summary>
        /// 将图像转换为字节格式
        /// </summary>
        /// <param name="image">要转换的图像</param>
        /// <returns>转换后的字节数组</returns>
        public static byte[] ToBytes(this Image image, System.Drawing.Imaging.ImageFormat format)
        {
            if (image == null) return null;

            using (var ms = new IO.MemoryStream())
            {
                image.Save(ms, format);
                ms.Close();

                return ms.ToArray();
            }
        }

        /// <summary>
        /// 使用质量90将图片保存到指定位置为JPEG图片
        /// </summary>
        /// <param name="image">要保存的图片</param>
        /// <param name="path">保存的路径</param>
        public static void SaveAsJpeg(this Image image, string path)
        {
            SaveAsJpeg(image, path, 90);
        }

        /// <summary>
        /// 使用指定的图片质量将图片保存到指定位置为JPEG图片
        /// </summary>
        /// <param name="image">要保存的图片</param>
        /// <param name="path">保存的路径</param>
        /// <param name="quality">质量</param>
        public static void SaveAsJpeg(this Image image, string path, int quality)
        {
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ((long)quality));

            ImageCodecInfo myImageCodecInfo = (from p in ImageCodecInfo.GetImageEncoders() where p.MimeType == "image/jpeg" select p).Single<ImageCodecInfo>();
            image.Save(path, myImageCodecInfo, parameters);
        }

        /// <summary>
        /// 由原始的小图创建一个居中的大图
        /// </summary>
        /// <param name="image">原始图像</param>
        /// <param name="width">新图像的宽度</param>
        /// <param name="height">新图像的高度</param>
        /// <returns><see cref="T:System.Drawing.Image"/></returns>
        public static Image ResizeWithMargin(this Image image, int width, int height)
        {
            if (image == null || image.Width >= width || image.Height >= height) return image;

            var nimg = new Bitmap(width, height);
            using (var g = Graphics.FromImage(nimg))
            {
                g.DrawImage(image, (width - image.Width) / 2, (height - image.Height) / 2, image.Height, image.Width);
            }

            return nimg;
        }

        #endregion


    }
}
