using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LeaveSystem.Web.Common
{
    public class Security
    {
        private static Random _random = new Random();
        ///<summary>
        /// 创建验证码字符
        /// </summary>
        /// <param name="length">字符长度</param>
        /// <returns>验证码字符</returns>
        public static string CreateVerificationText(int length)
        {
            char[] verification = new char[length];
            char[] dictionary =
            {
                //'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
            };
            for (int i = 0; i < length; i++) { verification[i] = dictionary[_random.Next(dictionary.Length - 1)]; }
            return new string(verification);
        }

        /// <summary>
        /// 创建验证码图片
        /// </summary>
        /// <param name="verificationText">验证码字符串</param>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片长度</param>
        /// <returns>图片</returns>
        public static byte[] CreateVerificationImage(string verificationText, int width, int height)
        {
            Pen pen = new Pen(Color.Black);
            Font font = new Font("Arial", 14, FontStyle.Bold);
            Brush brush = null;
            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);
            SizeF totalSizeF = g.MeasureString(verificationText, font);
            SizeF curCharSizeF;
            PointF startPointF = new PointF((width - totalSizeF.Width) / 2, (height - totalSizeF.Height) / 2);
            g.Clear(Color.White);
            for (int i = 0; i < verificationText.Length; i++)
            {
                brush = new LinearGradientBrush(new Point(0, 0), new Point(1, 1), Color.FromArgb(_random.Next(255), _random.Next(255), _random.Next(255)), Color.FromArgb(_random.Next(255), _random.Next(255), _random.Next(255)));
                g.DrawString(verificationText[i].ToString(), font, brush, startPointF);
                curCharSizeF = g.MeasureString(verificationText[i].ToString(), font);
                startPointF.X += curCharSizeF.Width;
            }
            g.Dispose();
            var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Jpeg);
            return stream.ToArray();
        }

        public static byte[] GetVerifImage(int length, int width, int height, out string verifCode)
        {
            verifCode = CreateVerificationText(length);
            return CreateVerificationImage(verifCode, width, height);
        }

        /// <summary>
        /// 256位散列加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns>密文</returns>
        public static string Sha256(string plainText)
        {
            SHA256Managed sha256 = new SHA256Managed();
            byte[] cipherText = sha256.ComputeHash(Encoding.Default.GetBytes(plainText));
            return Convert.ToBase64String(cipherText);
        }
    }
}