using System.Drawing;
using System.Windows.Forms;

namespace System.Common
{
    public partial class MyImage : PictureBox
    {
        Image defaultImg = null;

        public MyImage()
        {
            InitializeComponent();

            this.MouseEnter += MyImage_MouseEnter;
            this.MouseLeave += MyImage_MouseLeave;
            this.MouseDown += MyImage_MouseDown;
            this.MouseUp += MyImage_MouseUp;
            this.EnabledChanged += MyImage_EnabledChanged;
        }



        void SetImage(int x, int y)
        {
            if (this.defaultImg == null)
                return;

            using (Bitmap bitmap = new Bitmap(this.defaultImg))
            {
                var newbitmap = Utils.Cut(bitmap, x, y, this.Width, this.Height);
                this.Image = Image.FromStream(newbitmap.ToMemoryStream());
            }
        }

        /// <summary>
        /// 不可用时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MyImage_EnabledChanged(object sender, EventArgs e)
        {
            if (this.Enabled)
            {
                SetImage(this.EnabledX, 0);
            }
            else
            {
                SetImage(0, 0);
            }
        }

        /// <summary>
        /// 抬起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MyImage_MouseUp(object sender, MouseEventArgs e)
        {
            SetImage(0, 0);
        }

        /// <summary>
        /// 按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MyImage_MouseDown(object sender, MouseEventArgs e)
        {
            SetImage(this.DownX, 0);
        }

        /// <summary>
        /// 移出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MyImage_MouseLeave(object sender, EventArgs e)
        {
            SetImage(0, 0);
        }

        /// <summary>
        /// 移入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MyImage_MouseEnter(object sender, EventArgs e)
        {
            SetImage(this.OverX, 0);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);


            if (defaultImg == null)
                this.defaultImg = this.Image;
        }

        public int OverX { get; set; }

        public int DownX { get; set; }

        public int EnabledX { get; set; }
    }
}
