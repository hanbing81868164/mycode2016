using System.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;

namespace TestProject1
{
    
    
    /// <summary>
    ///这是 ThumbnailTest 的测试类，旨在
    ///包含所有 ThumbnailTest 单元测试
    ///</summary>
    [TestClass()]
    public class ThumbnailTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///Thumbnail 构造函数 的测试
        ///</summary>
        [TestMethod()]
        public void ThumbnailConstructorTest()
        {
            Thumbnail target = new Thumbnail();
            Assert.Inconclusive("TODO: 实现用来验证目标的代码");
        }

        /// <summary>
        ///CreateThumbnails 的测试
        ///</summary>
        [TestMethod()]
        public void CreateThumbnailsTest()
        {
            string imagePath = @"E:\my-code2012\System.Common\TestProject1\bin\Debug\image\2000.png"; // TODO: 初始化为适当的值
            string savePath = @"E:\my-code2012\System.Common\TestProject1\bin\Debug\image\2000_01.png";// TODO: 初始化为适当的值
            int width = 200; // TODO: 初始化为适当的值
            int height = 200; // TODO: 初始化为适当的值
            Thumbnail.CreateThumbnails(imagePath, savePath, width, height);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///GetEncoderInfo 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("System.Common.dll")]
        public void GetEncoderInfoTest()
        {
            string mimeType = string.Empty; // TODO: 初始化为适当的值
            ImageCodecInfo expected = null; // TODO: 初始化为适当的值
            ImageCodecInfo actual;
            actual = Thumbnail_Accessor.GetEncoderInfo(mimeType);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///MakeThumbnail 的测试
        ///</summary>
        [TestMethod()]
        public void MakeThumbnailTest()
        {
            Thumbnail target = new Thumbnail() { Model= Thumbnail.ZoomType.Cut, Width=150, Height=150 }; // TODO: 初始化为适当的值
            string filePath = @"E:\my-code2012\System.Common\TestProject1\bin\Debug\image\2000.png"; // TODO: 初始化为适当的值
            string savePath = @"E:\my-code2012\System.Common\TestProject1\bin\Debug\image\2000_01.png";// TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.MakeThumbnail(filePath, savePath);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///MakeThumbnail 的测试
        ///</summary>
        [TestMethod()]
        public void MakeThumbnailTest1()
        {
            Thumbnail target = new Thumbnail(); // TODO: 初始化为适当的值
            byte[] bytes = null; // TODO: 初始化为适当的值
            string savePath = string.Empty; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.MakeThumbnail(bytes, savePath);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///MakeThumbnail 的测试
        ///</summary>
        [TestMethod()]
        public void MakeThumbnailTest2()
        {
            Thumbnail target = new Thumbnail(); // TODO: 初始化为适当的值
            Stream stream = null; // TODO: 初始化为适当的值
            string savePath = string.Empty; // TODO: 初始化为适当的值
            target.MakeThumbnail(stream, savePath);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///addWaterMark 的测试
        ///</summary>
        [TestMethod()]
        public void addWaterMarkTest()
        {
            Thumbnail target = new Thumbnail(); // TODO: 初始化为适当的值
            byte[] bytes = null; // TODO: 初始化为适当的值
            string savePath = string.Empty; // TODO: 初始化为适当的值
            int xpos = 0; // TODO: 初始化为适当的值
            int ypos = 0; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.addWaterMark(bytes, savePath, xpos, ypos);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///addWaterMark 的测试
        ///</summary>
        [TestMethod()]
        public void addWaterMarkTest1()
        {
            Thumbnail target = new Thumbnail(); // TODO: 初始化为适当的值
            Stream stream = null; // TODO: 初始化为适当的值
            string savePath = string.Empty; // TODO: 初始化为适当的值
            int xpos = 0; // TODO: 初始化为适当的值
            int ypos = 0; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.addWaterMark(stream, savePath, xpos, ypos);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///addWaterMark 的测试
        ///</summary>
        [TestMethod()]
        public void addWaterMarkTest2()
        {
            Thumbnail target = new Thumbnail() { Width = 150, Height = 150, WaterMarkType = Thumbnail.WatermarkType.WM_IMAGE, MarkImagePath = @"E:\my-code2012\System.Common\TestProject1\bin\Debug\image\mark.png", HighQuality = true }; // TODO: 初始化为适当的值


            string imagePath = @"E:\my-code2012\System.Common\TestProject1\bin\Debug\image\2000.png"; // TODO: 初始化为适当的值
            string savePath = @"E:\my-code2012\System.Common\TestProject1\bin\Debug\image\2000_01.png";// TODO: 初始化为适当的值

            int xpos = 0; // TODO: 初始化为适当的值
            int ypos = 0; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.addWaterMark(imagePath, savePath, xpos, ypos);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///addWatermarkImage 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("System.Common.dll")]
        public void addWatermarkImageTest()
        {
            Thumbnail_Accessor target = new Thumbnail_Accessor(); // TODO: 初始化为适当的值
            Graphics graphics = null; // TODO: 初始化为适当的值
            string markImagePath = string.Empty; // TODO: 初始化为适当的值
            string markPosition = string.Empty; // TODO: 初始化为适当的值
            int width = 0; // TODO: 初始化为适当的值
            int height = 0; // TODO: 初始化为适当的值
            int xpos = 0; // TODO: 初始化为适当的值
            int ypos = 0; // TODO: 初始化为适当的值
            target.addWatermarkImage(graphics, markImagePath, markPosition, width, height, xpos, ypos);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///addWatermarkText 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("System.Common.dll")]
        public void addWatermarkTextTest()
        {
            Thumbnail_Accessor target = new Thumbnail_Accessor(); // TODO: 初始化为适当的值
            Graphics graphics = null; // TODO: 初始化为适当的值
            string markText = string.Empty; // TODO: 初始化为适当的值
            string markPosition = string.Empty; // TODO: 初始化为适当的值
            int width = 0; // TODO: 初始化为适当的值
            int height = 0; // TODO: 初始化为适当的值
            target.addWatermarkText(graphics, markText, markPosition, width, height);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///Y 的测试
        ///</summary>
        [TestMethod()]
        public void YTest()
        {
            Thumbnail target = new Thumbnail(); // TODO: 初始化为适当的值
            Nullable<int> expected = new Nullable<int>(); // TODO: 初始化为适当的值
            Nullable<int> actual;
            target.Y = expected;
            actual = target.Y;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
