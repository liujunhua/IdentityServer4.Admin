using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Security.Cryptography;
using SkiaSharp;
using System.Linq;
using System.IO;

namespace Skoruba.IdentityServer4.STS.Identity.Helpers
{
    /// <summary>
    /// 验证码帮助类
    /// </summary>
    public class YZMHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static byte[] GetCode(out string code)
        {
            #region 反射SK支持的全部颜色
            //List<SKColor> colors = new List<SKColor>();           
            //var skcolors = new SKColors();
            //var type = skcolors.GetType();
            //foreach (FieldInfo field in type.GetFields())
            //{
            //    colors.Add( (SKColor)field.GetValue(skcolors));
            //}
            #endregion

            //int maxcolorindex = colors.Count-1;
            byte[] imageBytes = null;
            code = GenerateRandomNumber(4, DateTime.Now.Second);
            var zu = code.ToList();
            SKBitmap bmp = new SKBitmap(80, 30);
            using (SKCanvas canvas = new SKCanvas(bmp))
            {
                //背景色
                canvas.DrawColor(SKColors.White);

                using (SKPaint sKPaint = new SKPaint())
                {
                    sKPaint.TextSize = 16;//字体大小
                    sKPaint.IsAntialias = true;//开启抗锯齿                   
                    sKPaint.Typeface = SKTypeface.FromFamilyName("微软雅黑", SKTypefaceStyle.Bold);//字体
                    SKRect size = new SKRect();
                    sKPaint.MeasureText(zu[0].ToString(), ref size);//计算文字宽度以及高度

                    float temp = (bmp.Width / 4 - size.Size.Width) / 2;
                    float temp1 = bmp.Height - (bmp.Height - size.Size.Height) / 2;
                    Random random = new Random();

                    for (int i = 0; i < 4; i++)
                    {

                        sKPaint.Color = new SKColor((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
                        canvas.DrawText(zu[i].ToString(), temp + 20 * i, temp1, sKPaint);//画文字
                    }
                    //干扰线
                    for (int i = 0; i < 5; i++)
                    {
                        sKPaint.Color = new SKColor((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
                        canvas.DrawLine(random.Next(0, 40), random.Next(1, 29), random.Next(41, 80), random.Next(1, 29), sKPaint);
                    }
                }
                //页面展示图片
                using (SKImage img = SKImage.FromBitmap(bmp))
                {
                    using (SKData p = img.Encode())
                    {
                        imageBytes = p.ToArray();
                    }
                }
                return imageBytes;
            }
        }
        #region 生成随机数
        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="Length">长度</param>
        /// <param name="index">随机种子</param>
        /// <returns></returns>
        public static string GenerateRandomNumber(int Length, int index)
        {
            char[] constant ={'0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F','G','H','J','K','L','M','N','P','Q','R','S','T','U','V','W','X','Y','Z','O','I'
                                //,'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'
                              };
            StringBuilder newRandom = new StringBuilder(36);
            Random rd = new Random(unchecked((int)DateTime.Now.Ticks + index));
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(36)]);
            }
            return newRandom.ToString();
        }

        #endregion
    }
}
