using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Drawing2D;

namespace System
{
    public class Thumbnail
    {
        #region 属性
        //private System.Drawing.Image image;

        public enum WatermarkType
        {
            /// <summary>
            /// 表示添加图片水印
            /// </summary>
            WM_IMAGE = 0,
            /// <summary>
            /// 表示添加文字水印
            /// </summary>
            WM_TEXT = 1
        }

        /// <summary>
        /// 缩放属性
        /// </summary>
        public enum ZoomType
        {
            /// <summary>
            /// 宽为标准
            /// </summary>
            W = 0,
            /// <summary>
            /// 高为标准
            /// </summary>
            H = 1,
            /// <summary>
            /// 等比缩放
            /// </summary>
            Cut = 2,
            /// <summary>
            /// 指定高宽缩放（可能变形）    
            /// </summary>
            HW = 3,
            /// <summary>
            /// 默认，给程序处理
            /// </summary>
            Default = 4

        }

        private int _addWaterMark_w = 0;
        private int _addWaterMark_h = 0;
        private ZoomType _model = ZoomType.W;
        public ZoomType Model
        {
            get { return this._model; }
            set { this._model = value; }
        }

        /// <summary>
        /// 水印类型
        /// </summary>
        public WatermarkType WaterMarkType
        {
            get;
            set;
        }

        public string MarkText { get; set; }
        public string MarkImagePath { get; set; }

        /// <summary>
        /// 加水印后新图片宽度
        /// </summary>
        public int Width
        {
            get { return this._addWaterMark_w; }
            set { this._addWaterMark_w = value; }
        }
        /// <summary>
        /// 加水印后新图的高度
        /// </summary>
        public int Height
        {
            get { return this._addWaterMark_h; }
            set { this._addWaterMark_h = value; }
        }


        private bool _ShowTransparentLayer = false;

        /// <summary>
        /// 是否加一个半透明的层
        /// </summary>
        public bool ShowTransparentLayer
        {
            get { return this._ShowTransparentLayer; }
            set { this._ShowTransparentLayer = value; }
        }

        private bool highquality = false;
        /// <summary>
        /// 生成高质量缩略图,但图片会变大,默认为flase
        /// </summary>
        public bool HighQuality
        {
            get { return this.highquality; }
            set { this.highquality = value; }
        }

        private int? _X = null;
        private int? _Y = null;

        /// <summary>
        /// 原图开始截取X位置
        /// </summary>
        public int? X
        {
            get { return this._X; }
            set { this._X = value; }
        }

        /// <summary>
        /// 原图开始截取Y位置
        /// </summary>
        public int? Y
        {
            get { return this._Y; }
            set { this._Y = value; }
        }

        float _Transparency = 0.9f;
        /// <summary>
        /// 透明度
        /// </summary>
        public float Transparency
        {
            get { return this._Transparency; }
            set { this._Transparency = value; }
        }

        #endregion



        #region 转换
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        #endregion



        #region 生成缩略图方法

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="thumbnailPath">源图路径（物理路径）</param>
        /// <param name="savePath">新图的保存路径</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式('W')</param>
        public bool MakeThumbnail(string filePath, string savePath)
        {
            byte[] bytes = FileHelper.GetBytes(filePath);
            MakeThumbnail(bytes, savePath);

            return true;
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="thumbnailPath">源图路径（物理路径）</param>
        /// <param name="savePath">新图的保存路径</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式('W')</param>
        public bool MakeThumbnail(byte[] bytes, string savePath)
        {
            Stream stm = bytes.ToStream();
            MakeThumbnail(stm, savePath);
            stm.Close();
            stm.Dispose();
            return true;
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="thumbnailPath">源图路径（物理路径）</param>
        /// <param name="savePath">新图的保存路径</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式('W')</param>
        public void MakeThumbnail(Stream stream, string savePath)
        {
            using (System.Drawing.Image originalImage = System.Drawing.Image.FromStream(stream))
            {

                if (originalImage.Width < this.Width && originalImage.Height < this.Height)
                {
                    originalImage.Save(savePath);
                    originalImage.Dispose();
                }
                else
                {
                    if (this.Model == ZoomType.Default)
                    {
                        if (originalImage.Width > originalImage.Height)
                        {
                            this.Model = ZoomType.W;
                        }
                        else
                        {
                            if (originalImage.Width == originalImage.Height)
                            {
                                this.Model = this.Width > this.Height ? ZoomType.W : ZoomType.H;
                            }
                            else
                            {
                                this.Model = ZoomType.H;
                            }
                        }
                    }

                    int towidth = this.Width;
                    int toheight = this.Height;

                    int x = 0;
                    int y = 0;
                    int ow = originalImage.Width;
                    int oh = originalImage.Height;

                    switch (this.Model)
                    {
                        case ZoomType.HW://指定高宽缩放（可能变形）                
                            break;
                        case ZoomType.W://指定宽，高按比例                    
                            toheight = originalImage.Height * this.Width / originalImage.Width;
                            break;
                        case ZoomType.H://指定高，宽按比例
                            towidth = originalImage.Width * this.Height / originalImage.Height;
                            break;
                        case ZoomType.Cut://指定高宽裁减（不变形）                
                            if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                            {
                                oh = originalImage.Height;
                                ow = originalImage.Height * towidth / toheight;
                                y = 0;
                                x = (originalImage.Width - ow) / 2;
                            }
                            else
                            {
                                ow = originalImage.Width;
                                oh = originalImage.Width * this.Height / towidth;
                                x = 0;
                                y = (originalImage.Height - oh) / 2;
                            }
                            //hanbing修改
                            //x = y = 0;//表示从左上角开始裁

                            break;
                        default:
                            break;
                    }

                    #region 2008-9-27来自网络新方法

                    this._X = this._X == null ? x : this._X;
                    this._Y = this._Y == null ? y : this._Y;

                    ////////////////////////////////////////////////////////////////////
                    Bitmap img = new Bitmap(towidth, toheight);
                    img.SetResolution(72f, 72f);

                    Graphics gdiobj = Graphics.FromImage(img);
                    gdiobj.CompositingQuality = CompositingQuality.HighQuality;
                    gdiobj.SmoothingMode = SmoothingMode.HighQuality;
                    gdiobj.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    gdiobj.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    gdiobj.FillRectangle(new SolidBrush(Color.White), 0, 0, towidth, toheight);
                    Rectangle destrect = new Rectangle(0, 0, towidth, toheight); ;
                    gdiobj.DrawImage(originalImage, destrect, int.Parse(this.X.ToString()), int.Parse(this.Y.ToString()), ow, oh, GraphicsUnit.Pixel);


                    System.Drawing.Imaging.EncoderParameters ep = new System.Drawing.Imaging.EncoderParameters(1);
                    ep.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);

                    System.Drawing.Imaging.ImageCodecInfo ici = GetEncoderInfo("image/jpeg");
                    if (this.HighQuality)
                    {
                        img.Save(savePath, ici, ep);//高质量的大图
                    }
                    else
                    {
                        img.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }

                    try
                    {
                        originalImage.Dispose();
                        img.Dispose();
                        gdiobj.Dispose();
                    }
                    catch { }
                    ///////////////////////////////////////////////////
                    #endregion
                }

            }
        }

        /// <summary>
        /// 对图片进行等比缩小后生成缩略图
        /// </summary>
        /// <param name="imagePath">原图片地址</param>
        /// <param name="savePath">新图片地址</param>
        /// <param name="width">缩略图的宽</param>
        /// <param name="height">缩略图的高</param>
        public static void CreateThumbnails(string imagePath, string savePath, int width, int height)
        {
            using (Image image = System.Drawing.Image.FromFile(imagePath))
            {
                double bl = 1d;
                if ((image.Width <= image.Height) && (width >= height))
                {
                    bl = Convert.ToDouble(image.Height) / Convert.ToDouble(height);
                }
                else if ((image.Width > image.Height) && (width < height))
                {
                    bl = Convert.ToDouble(image.Width) / Convert.ToDouble(width);
                }
                else

                    if ((image.Width <= image.Height) && (width <= height))
                    {
                        if (image.Height / height >= image.Width / width)
                        {
                            bl = Convert.ToDouble(image.Width) / Convert.ToDouble(width);
                        }
                        else
                        {
                            bl = Convert.ToDouble(image.Height) / Convert.ToDouble(height);
                        }
                    }
                    else
                    {
                        if (image.Height / height >= image.Width / width)
                        {
                            bl = Convert.ToDouble(image.Height) / Convert.ToDouble(height);
                        }
                        else
                        {
                            bl = Convert.ToDouble(image.Width) / Convert.ToDouble(width);
                        }
                    }

                Bitmap b = new Bitmap(image, Convert.ToInt32(image.Width / bl), Convert.ToInt32(image.Height / bl));
                b.Save(savePath);
                b.Dispose();
                image.Dispose();
            }
        }

        #endregion

        #region 添加水印方法

        ///// <summary>
        ///// 直接添加水印并保存文件
        ///// </summary>
        ///// <param name="bytes">图片内容</param>
        ///// <param name="savePath">保存地址</param>
        ///// <param name="watermarkType">加水印类型</param>
        ///// <param name="markText">加文字水印的文字</param>
        ///// <param name="markImagePath">加图片水印的图片</param>
        ///// <returns></returns>
        //public bool addWaterMark(byte[] bytes, string savePath, WatermarkType watermarkType, string markText, string markImagePath, int xpos, int ypos)
        //{
        //    bool tf = false;
        //    try
        //    {
        //        Stream m = bytes.ToStream();
        //        System.Drawing.Image image = System.Drawing.Image.FromStream(m);

        //        Bitmap b = new Bitmap(m);
        //        Graphics g = Graphics.FromImage(b);

        //        g.Clear(Color.White);

        //        g.SmoothingMode = SmoothingMode.HighQuality;
        //        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //        g.CompositingQuality = CompositingQuality.HighQuality;
        //        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;


        //        g.DrawImage(image, 0, 0, image.Width, image.Height);

        //        switch (watermarkType)//水印类型
        //        { //是图片的话               
        //            case WatermarkType.WM_IMAGE:
        //                this.addWatermarkImage(g, markImagePath, "WM_BOTTOM_RIGHT", image.Width, image.Height, xpos, xpos);
        //                break; //如果是文字                    
        //            case WatermarkType.WM_TEXT:
        //                this.addWatermarkText(g, markText, "WM_BOTTOM_RIGHT", image.Width, image.Height);
        //                break;
        //        }

        //        b.Save(savePath);
        //        b.Dispose();
        //        g.Dispose();
        //        image.Dispose();
        //        m.Close();
        //        m.Dispose();
        //        tf = true;
        //    }
        //    catch { }
        //    return tf;
        //}

        /// <summary>
        /// 直接添加水印并保存文件
        /// </summary>
        /// <param name="bytes">图片内容</param>
        /// <param name="savePath">保存地址</param>
        /// <param name="watermarkType">加水印类型</param>
        /// <param name="markText">加文字水印的文字</param>
        /// <param name="markImagePath">加图片水印的图片</param>
        /// <returns></returns>
        public bool addWaterMark(byte[] bytes, string savePath,  int xpos = 0, int ypos = 0)
        {
            return addWaterMark(bytes.ToStream(), savePath, xpos, ypos);
        }

        /// <summary>
        /// 添加水印
        /// </summary>
        /// <param name="imagePath">旧图片地址</param>
        /// <param name="savePath">加水印后图片保存地址</param>
        /// <param name="watermarkType">是加图片水印还是文字水印,WM_IMAGE表示图片,WM_TEXT表示文字</param>
        /// <param name="markText">加文字水印时的文字</param>
        /// <param name="markImagePath">加图片水印时的水印图片地址</param>
        public bool addWaterMark(string imagePath, string savePath,  int xpos = 0, int ypos = 0)
        {
            return addWaterMark(FileHelper.GetStream(imagePath), savePath,  xpos, ypos);
        }


        /// <summary>
        /// 添加水印
        /// </summary>
        /// <param name="oldpath">旧图片地址</param>
        /// <param name="savePath">加水印后图片保存地址</param>
        /// <param name="watermarkType">是加图片水印还是文字水印,WM_IMAGE表示图片,WM_TEXT表示文字</param>
        /// <param name="markText">加文字水印时的文字</param>
        /// <param name="markImagePath">加图片水印时的水印图片地址</param>
        public bool addWaterMark(Stream stream, string savePath,  int xpos = 0, int ypos = 0)
        {
            using (Image image = Image.FromStream(stream))
            {
                int w_ = image.Width;//原图宽度
                if (this.Width != 0)
                {
                    if (image.Width >= this.Width)//当原图宽度大于设定宽度时进行缩放
                    {
                        w_ = this.Width;
                    }
                }

                int h_ = image.Height;//原图高度
                if (this.Height != 0)
                {
                    if (image.Height >= this.Height)//当原图高度大于设定高度时进行缩放
                    {
                        h_ = this.Height;
                    }
                }


                if (this.Model == ZoomType.Default)
                {
                    if (image.Width > image.Height)
                    {
                        this.Model = ZoomType.W;
                    }
                    else
                    {
                        if (image.Width == image.Height)
                        {
                            this.Model = this.Width > this.Height ? ZoomType.W : ZoomType.H;
                        }
                        else
                        {
                            this.Model = ZoomType.H;
                        }
                    }
                }

                //Bitmap b = new Bitmap(stm);
                //Graphics g = Graphics.FromImage(b);

                //g.Clear(Color.White);

                //g.SmoothingMode = SmoothingMode.HighQuality;
                //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //g.CompositingQuality = CompositingQuality.HighQuality;
                //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                //g.DrawImage(image, 0, 0, w_, h_);


                //****************************************************************2008-11-13******************
                //string model = image.Width > image.Height ? "W" : "H";


                int ow = image.Width;
                int oh = image.Height;
                int x = 0;
                int y = 0;

                switch (this.Model)
                {
                    case ZoomType.HW://指定高宽缩放（可能变形）                
                        break;
                    case ZoomType.W://指定宽，高按比例                    
                        h_ = image.Height * w_ / image.Width;
                        break;
                    case ZoomType.H://指定高，宽按比例
                        w_ = image.Width * h_ / image.Height;
                        break;
                    case ZoomType.Cut://指定高宽裁减（不变形）                
                        if ((double)image.Width / (double)image.Height > (double)w_ / (double)h_)
                        {
                            oh = image.Height;
                            ow = image.Height * w_ / h_;
                            y = 0;
                            x = (image.Width - ow) / 2;
                        }
                        else
                        {
                            ow = image.Width;
                            oh = image.Width * h_ / w_;
                            x = 0;
                            y = (image.Height - oh) / 2;
                        }
                        //hanbing修改
                        //x = y = 0;//表示从左上角开始裁

                        break;
                    default:
                        break;
                }

                #region 之前方法
                ////新建一个bmp图片
                //System.Drawing.Image b = new System.Drawing.Bitmap(w_, h_);
                ////新建一个画板
                //Graphics g = System.Drawing.Graphics.FromImage(b);
                ////设置高质量插值法
                //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                ////设置高质量,低速度呈现平滑程度
                //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                ////清空画布并以透明背景色填充
                //g.Clear(Color.Transparent);
                ////在指定位置并且按指定大小绘制原图片的指定部分
                //g.DrawImage(image, new Rectangle(0, 0, w_, h_), new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);
                ////****************************************************************2008-11-13******************
                #endregion

                #region 2008-11-18新加方法
                this._X = this._X == null ? x : this._X;
                this._Y = this._Y == null ? y : this._Y;



                Bitmap b = new Bitmap(w_, h_);//新建一个bmp图片
                b.SetResolution(72f, 72f);
                Graphics g = Graphics.FromImage(b);
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.FillRectangle(new SolidBrush(Color.White), 0, 0, w_, h_);
                Rectangle destrect = new Rectangle(0, 0, w_, h_);

                // Forum.BLL.Other.Files.WriteLine(AppDomain.CurrentDomain.BaseDirectory+"log.txt",this.X.ToString()+"=="+this.Y.ToString());

                g.DrawImage(image, destrect, int.Parse(this.X.ToString()), int.Parse(this.Y.ToString()), ow, oh, GraphicsUnit.Pixel);
                System.Drawing.Imaging.EncoderParameters ep = new System.Drawing.Imaging.EncoderParameters(1);
                ep.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);
                System.Drawing.Imaging.ImageCodecInfo ici = GetEncoderInfo("image/jpeg");
                #endregion

                switch (this.WaterMarkType)//水印类型
                { //是图片的话               
                    case WatermarkType.WM_IMAGE:
                        this.addWatermarkImage(g,this.MarkImagePath, "WM_BOTTOM_RIGHT", w_, h_, xpos, ypos);
                        break; //如果是文字                    
                    case WatermarkType.WM_TEXT:
                        this.addWatermarkText(g,this.MarkText, "WM_BOTTOM_RIGHT", w_, h_);
                        break;
                }

                if (this.HighQuality)
                {
                    b.Save(savePath, ici, ep);//高质量的大图
                }
                else
                {
                    b.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                b.Dispose();
                g.Dispose();
                image.Dispose();
            }
            return true;
        }


        #region
        /// <summary>
        ///  加水印文字
        /// </summary>
        /// <param name="graphics">imge 对象</param>
        /// <param name="markText">水印文字内容</param>
        /// <param name="markPosition">水印位置</param>
        /// <param name="width">被加水印图片的宽</param>
        /// <param name="height">被加水印图片的高</param>
        void addWatermarkText(Graphics graphics, string markText, string markPosition, int width, int height)
        {
            int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };
            Font crFont = null;
            SizeF crSize = new SizeF();

            crFont = new Font(new FontFamily("Arial"), 16, FontStyle.Regular);
            crSize = graphics.MeasureString(markText, crFont);

            float xpos = 0;
            float ypos = 0;

            switch (markPosition)
            {
                case "WM_TOP_LEFT":
                    xpos = ((float)width * (float).01) + (crSize.Width / 2);
                    ypos = (float)height * (float).01;
                    break;
                case "WM_TOP_RIGHT":
                    xpos = ((float)width * (float).99) - (crSize.Width / 2);
                    ypos = (float)height * (float).01;
                    break;
                case "WM_BOTTOM_RIGHT":
                    xpos = ((float)width * (float).99) - (crSize.Width / 2);
                    ypos = ((float)height * (float).99) - crSize.Height;
                    break;
                case "WM_BOTTOM_LEFT":
                    xpos = ((float)width * (float).01) + (crSize.Width / 2);
                    ypos = ((float)height * (float).99) - crSize.Height;
                    break;
            }

            if (this.ShowTransparentLayer)
            {
                //画一个半透明的矩形作为背景
                Color redAlpha50Percent = Color.FromArgb(100, 255, 255, 255); //半透明红色
                graphics.FillRectangle(new SolidBrush(redAlpha50Percent), 0, ypos, width, 40);//
                //画一个半透明的矩形作为背景
            }

            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;

            SolidBrush semiTransBrush = new SolidBrush(Color.Red);

            graphics.DrawString(markText, crFont, semiTransBrush, xpos, ypos + 5, StrFormat);
            semiTransBrush.Dispose();
        }

        /// <summary>
        ///  加水印图片
        /// </summary>
        /// <param name="graphics">imge 对象</param>
        /// <param name="markImagePath">水印图片的地址</param>
        /// <param name="markPosition">水印位置</param>
        /// <param name="width">被加水印图片的宽</param>
        /// <param name="height">被加水印图片的高</param>
        void addWatermarkImage(Graphics graphics, string markImagePath, string markPosition, int width, int height, int xpos = 0, int ypos = 0)
        {
            System.Drawing.Image watermark = new Bitmap(markImagePath);

            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();

            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };
            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);
            //不透明
            //float[][] colorMatrixElements = {
            //                                       new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
            //                                        new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
            //                                       new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
            //                                        new float[] {0.0f,  0.0f,  0.0f,  1.0f, 0.0f},
            //                                        new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
            //                                };
            if (this.ShowTransparentLayer)
            {
                //50%透明(改第四行的0.5f就可以)
                float[][] colorMatrixElements = {
                                               new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                               new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  Transparency, 0.0f},//这里的0.9f就是表示90%透明 ,Tmz值为透明度 如 0.5f
                                                new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                            };
                ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
                imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);




                //画一个半透明的矩形作为背景
                //Color redAlpha50Percent = Color.FromArgb(128, 255, 255, 255); //半透明红色
                Color redAlpha50Percent = Color.FromArgb(100, 255, 255, 255); //半透明红色
                graphics.FillRectangle(new SolidBrush(redAlpha50Percent), 0, height - watermark.Height - 4, width, watermark.Height + 4);//
                //画一个半透明的矩形作为背景
            }

            //int xpos = 0;
            //int ypos = 0;
            int WatermarkWidth = 0;
            int WatermarkHeight = 0;
            double bl = 1d;
            //计算水印图片的比率
            //取背景的1/4宽度来比较
            if ((width > watermark.Width * 4) && (height > watermark.Height * 4))
            {
                bl = 1;
            }
            else if ((width > watermark.Width * 4) && (height < watermark.Height * 4))
            {
                bl = Convert.ToDouble(height / 4) / Convert.ToDouble(watermark.Height);

            }
            else

                if ((width < watermark.Width * 4) && (height > watermark.Height * 4))
                {
                    bl = Convert.ToDouble(width / 4) / Convert.ToDouble(watermark.Width);
                }
                else
                {
                    if ((width * watermark.Height) > (height * watermark.Width))
                    {
                        bl = Convert.ToDouble(height / 4) / Convert.ToDouble(watermark.Height);

                    }
                    else
                    {
                        bl = Convert.ToDouble(width / 4) / Convert.ToDouble(watermark.Width);

                    }

                }
            bl = 1;

            WatermarkWidth = Convert.ToInt32(watermark.Width * bl);
            WatermarkHeight = Convert.ToInt32(watermark.Height * bl);

            if (xpos == 0 && ypos == 0)
            {
                switch (markPosition)
                {
                    case "WM_TOP_LEFT":
                        xpos = 10;
                        ypos = 10;
                        break;
                    case "WM_TOP_RIGHT":
                        xpos = width - WatermarkWidth - 20;
                        ypos = 10;
                        break;
                    case "WM_BOTTOM_RIGHT":
                        xpos = width - WatermarkWidth - 10;
                        ypos = height - WatermarkHeight - 10;
                        break;
                    case "WM_BOTTOM_LEFT":
                        xpos = 10;
                        ypos = height - WatermarkHeight - 10;
                        break;
                }
            }
            graphics.DrawImage(watermark, new Rectangle(xpos, ypos + 8, WatermarkWidth, WatermarkHeight), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);

            watermark.Dispose();
            imageAttributes.Dispose();
        }
        #endregion
        #endregion
    }
}
