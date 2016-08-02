using System.Drawing;


namespace System
{
    /// <summary>
    /// 验证码图片页面类
    /// </summary>
    public class VerifyImagePage : System.Web.UI.Page
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="e"></param>
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);


            string bgcolor = Utils.GetQueryString("bgcolor").Trim();
            int textcolor = Utils.GetQueryInt("textcolor", 1);


            int w = Utils.GetQueryInt("w", 120);
            int h = Utils.GetQueryInt("h", 40);

            string type = Utils.GetQueryString("type");
            type = (type.IsNullOrEmpty() ? "int" : type);


            string[] bgcolorArray = bgcolor.Split(',');

            Color bg = Color.White;
            if (bgcolorArray.Length == 1 && bgcolor != string.Empty)
                bg = bgcolor.ToColor();
            else if (bgcolorArray.Length == 3 && bgcolorArray.IsNumericArray())
                bg = Color.FromArgb(bgcolorArray[0].ToInt32(255), bgcolorArray[1].ToInt32(255), bgcolorArray[2].ToInt32(255));

            VerifyImageInfo verifyimg = VerifyImageProvider.GetInstance("VerifyImage").GenerateImage(null, w, h, bg, textcolor, type);
            Bitmap image = verifyimg.Image;

            System.Web.HttpContext.Current.Response.ContentType = verifyimg.ContentType;
            image.Save(this.Response.OutputStream, verifyimg.ImageFormat);
        }
    }
}
