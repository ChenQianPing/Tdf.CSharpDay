using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter13
{
    public class ImageHelper
    {
        public static void TestMethod1()
        {
            var str = @"博学云";

            var rect = new Rectangle(0, 0, (int) 480, (int) 762);

            /*
             * 得到Bitmap(传入Rectangle.Empty自动计算宽高)
             * Font：Arial、叶根友毛笔行书2.0版
             */
            Bitmap bmp = TextToBitmap(str, new Font("叶根友毛笔行书2.0版", 32), rect, Color.Black, Color.White);

            // 保存到桌面save.jpg
            var directory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
            bmp.Save(directory + "\\save.png", ImageFormat.Png);
        }

        #region 把文字转换才Bitmap
        /// <summary>
        /// 把文字转换才Bitmap
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="rect">用于输出的矩形，文字在这个矩形内显示，为空时自动计算</param>
        /// <param name="fontcolor">字体颜色</param>
        /// <param name="backColor">背景颜色</param>
        /// <returns></returns>
        public static Bitmap TextToBitmap(string text, Font font, Rectangle rect, Color fontcolor, Color backColor)
        {
            Graphics g;
            Bitmap bmp;
            var format = new StringFormat(StringFormatFlags.NoClip);
            if (rect == Rectangle.Empty)
            {
                bmp = new Bitmap(1, 1);
                g = Graphics.FromImage(bmp);
                // 计算绘制文字所需的区域大小（根据宽度计算长度），重新创建矩形区域绘图
                SizeF sizef = g.MeasureString(text, font, PointF.Empty, format);

                var width = (int)(sizef.Width + 1);
                var height = (int)(sizef.Height + 1);
                rect = new Rectangle(0, 0, width, height);
                bmp.Dispose();

                bmp = new Bitmap(width, height);
            }
            else
            {
                bmp = new Bitmap(rect.Width, rect.Height);
            }

            g = Graphics.FromImage(bmp);

            // 使用ClearType字体功能
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.FillRectangle(new SolidBrush(backColor), rect);

            format.LineAlignment= StringAlignment.Center; // 垂直居中
            format.Alignment= StringAlignment.Center;     // 水平居中


            var img = Image.FromFile($@"E:\src\Tdf.CSharpDay\Tdf.CSharpDay\Chapter13\logo-v2.png");

            var xx = (rect.Width - img.Width) / 2;
            var yy = (rect.Height - img.Height) / 2;

            // 居中
            g.DrawImage(img, new Rectangle(xx, yy, rect.Width, rect.Height),
                new Rectangle(0, 0, rect.Width, rect.Height), GraphicsUnit.Pixel);

            // g.DrawImage(img, 0, 0, img.Width, img.Height);

            g.DrawString(text, font, Brushes.Black, rect, format);

            return bmp;
        }
        #endregion

        /// <summary>
        /// 将指向图像按指定的填充模式绘制到目标图像上
        /// </summary>
        /// <param name="sourceBmp">要控制填充模式的源图</param>
        /// <param name="targetBmp">要绘制到的目标图</param>
        /// <param name="fillMode">填充模式</param>
        /// <remarks></remarks>
        public void ImageFillRect(Bitmap sourceBmp, Bitmap targetBmp, FillMode fillMode)
        {
            try
            {
                switch (fillMode)
                {
                    case FillMode.Title:
                        using (TextureBrush txbrus = new TextureBrush(sourceBmp))
                        {
                            txbrus.WrapMode = System.Drawing.Drawing2D.WrapMode.Tile;
                            using (Graphics g = Graphics.FromImage(targetBmp))
                            {
                                g.FillRectangle(txbrus, new Rectangle(0, 0, targetBmp.Width - 1, targetBmp.Height - 1));
                            }
                        }
                        break;

                    case FillMode.Center:
                        using (Graphics g = Graphics.FromImage(targetBmp))
                        {
                            var xx = (targetBmp.Width - sourceBmp.Width) / 2;
                            var yy = (targetBmp.Height - sourceBmp.Height) / 2;
                            g.DrawImage(sourceBmp, new Rectangle(xx, yy, sourceBmp.Width, sourceBmp.Height), new Rectangle(0, 0, sourceBmp.Width, sourceBmp.Height), GraphicsUnit.Pixel);
                        }
                        break;

                    case FillMode.Struk:
                        using (var g = Graphics.FromImage(targetBmp))
                        {
                            g.DrawImage(sourceBmp, new Rectangle(0, 0, targetBmp.Width, targetBmp.Height), new Rectangle(0, 0, sourceBmp.Width, sourceBmp.Height), GraphicsUnit.Pixel);
                        }
                        break;

                    case FillMode.Zoom:
                        double tm = 0.0;
                        double w = sourceBmp.Width;
                        double h = sourceBmp.Height;
                        if (w > targetBmp.Width)
                        {
                            tm = targetBmp.Width / sourceBmp.Width;
                            w = w * tm;
                            h = h * tm;
                        }
                        if (h > targetBmp.Height)
                        {
                            tm = targetBmp.Height / h;
                            w = w * tm;
                            h = h * tm;
                        }
                        using (var tmpBp = new Bitmap((int)w, (int)h))
                        {
                            using (var g2 = Graphics.FromImage(tmpBp))
                            {
                                g2.DrawImage(sourceBmp, new Rectangle(0, 0, (int)w, (int)h), new Rectangle(0, 0, sourceBmp.Width, sourceBmp.Height), GraphicsUnit.Pixel);
                                using (var g = Graphics.FromImage(targetBmp))
                                {
                                    double xx = (targetBmp.Width - w) / 2;
                                    double yy = (targetBmp.Height - h) / 2;
                                    g.DrawImage(tmpBp, new Rectangle((int)xx, (int)yy, (int)w, (int)h), new Rectangle(0, 0, (int)w, (int)h), GraphicsUnit.Pixel);
                                }
                            }
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
