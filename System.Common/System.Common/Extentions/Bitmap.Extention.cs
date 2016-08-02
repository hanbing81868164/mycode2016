using System.IO;
using System.Drawing;


namespace System
{
    public static partial class Extention
    {

        public static MemoryStream ToMemoryStream(this Bitmap b)
        {
            MemoryStream ms = new MemoryStream();
            b.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms;
        }



    }
}
