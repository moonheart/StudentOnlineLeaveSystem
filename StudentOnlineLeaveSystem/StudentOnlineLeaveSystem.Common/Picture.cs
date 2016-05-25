using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentOnlineLeaveSystem.Common
{
    /// <summary>
    /// 图片相关
    /// </summary>
    public class Picture
    {
        /// <summary>
        /// 创建缩略图
        /// </summary>
        /// <param name="originalPicture">原图地址</param>
        /// <param name="thumbnail">缩略图地址</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <returns>是否成功</returns>
        public static bool CreateThumbnail(string originalPicture, string thumbnail, int width, int height)
        {
            //原图
            Image original = Image.FromFile(originalPicture);
            // 原图使用区域
            RectangleF originalArea = new RectangleF();
            //宽高比
            float ratio = (float)width / height;
            if (ratio > ((float)original.Width / original.Height))
            {
                originalArea.X = 0;
                originalArea.Width = original.Width;
                originalArea.Height = originalArea.Width / ratio;
                originalArea.Y = (original.Height - originalArea.Height) / 2;
            }
            else
            {
                originalArea.Y = 0;
                originalArea.Height = original.Height;
                originalArea.Width = originalArea.Height * ratio;
                originalArea.X = (original.Width - originalArea.Width) / 2;
            }
            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bitmap);
            //设置图片质量
            graphics.InterpolationMode = InterpolationMode.High;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            //绘制图片
            graphics.Clear(Color.Transparent);
            graphics.DrawImage(original, new RectangleF(0, 0, bitmap.Width, bitmap.Height), originalArea, GraphicsUnit.Pixel);
            //保存
            bitmap.Save(thumbnail);
            graphics.Dispose();
            original.Dispose();
            bitmap.Dispose();
            return true;
        }
    }
}
