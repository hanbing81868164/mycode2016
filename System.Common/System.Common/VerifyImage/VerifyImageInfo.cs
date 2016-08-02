using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace System
{
    /// <summary>
    /// 验证码图片信息
    /// </summary>
    public class VerifyImageInfo
    {
        private Bitmap image;
        private string contentType = "image/pjpeg";
        private ImageFormat imageFormat = ImageFormat.Jpeg;

        /// <summary>
        /// 生成出的图片
        /// </summary>
        public Bitmap Image
        {
            get { return image; }
            set { image = value; }
        }

        public Stream GetStream
        {
            get { return this.Image.ToMemoryStream(); }
        }

        public byte[] GetBytes
        {
            get { return this.GetStream.ToBytes(); }
        }

        /// <summary>
        /// 输出的图片类型，如 image/pjpeg
        /// </summary>
        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }

        /// <summary>
        /// 图片的格式
        /// </summary>
        public ImageFormat ImageFormat
        {
            get { return imageFormat; }
            set { imageFormat = value; }
        }
    }
}
