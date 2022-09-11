using System.DrawingCore;
using System.DrawingCore.Drawing2D;
using System.DrawingCore.Imaging;
using AdPang.FileManager.Common.Helper.Redis;

namespace AdPang.FileManager.Common.Helper.VerifyCode
{
    public class ImgVerifyCodeHelper
    {
        private readonly RedisHelper redis;

        public ImgVerifyCodeHelper(RedisHelper redis)
        {
            this.redis = redis;
        }

        public string Creat4ImgVerfiyCode(Guid seed)
        {
            string result = VerifyCodeHelper.CreatVeriyCode(4, seed);
            var recored = GetVerifyCodeByRedisDb(seed);
            if (recored != null)
            {
                if (recored.IsUsed) throw new Exception("创建验证码错误！请重新获取");
                return result;
            }
            if (SaveVerifyCodeToRedisDb(seed, result))
                return result;
            else throw new Exception("发生错误！");
        }
        public string Creat6ImgVerfiyCode(Guid seed)
        {
            var result = VerifyCodeHelper.CreatVeriyCode(6, seed);
            var record = GetVerifyCodeByRedisDb(seed);
            if (record != null)
            {
                if (record.IsUsed) throw new Exception("创建验证码错误！请重新获取");
                return result;
            }
            if (SaveVerifyCodeToRedisDb(seed, result))
                return result;
            else throw new Exception("发生错误！");
        }



        /// <summary>
        /// 验证四位图形验证码
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public bool VerifyImgCode(Guid seed, string verifyCode)
        {
            if (verifyCode == null) throw new ArgumentException("parameter {verificationCode} is null!");
            try
            {
                var recode = redis.GetStringKey<ImgVerfiyRecord>(seed.ToString());
                if (recode.IsUsed) throw new Exception();
                if (verifyCode.ToUpper() == recode.Result.ToUpper())
                {
                    var isSaved = redis.SetStringKey(seed.ToString(), new ImgVerfiyRecord
                    {
                        Result = verifyCode,
                        Seed = seed,
                        IsUsed = true
                    }, 5);
                    if (!isSaved) return false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }





        /// <summary>
        /// 获取验证码记录
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>

        private ImgVerfiyRecord GetVerifyCodeByRedisDb(Guid seed)
        {
            return redis.GetStringKey<ImgVerfiyRecord>(seed.ToString());
        }
        /// <summary>
        /// 保存验证码到redis
        /// </summary>
        /// <returns></returns>
        private bool SaveVerifyCodeToRedisDb(Guid seed, string result)
        {
            var isSaved = redis.SetStringKey(seed.ToString(), new ImgVerfiyRecord
            {
                Result = result,
                Seed = seed
            }, 5);
            return isSaved;
        }



        public static byte[] CreateByteByImgVerifyCode(string verifyCode, int width, int height)
        {
            Font font = new Font("Arial", 14, FontStyle.Bold | FontStyle.Italic);
            Brush brush;
            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);
            SizeF totalSizeF = g.MeasureString(verifyCode, font);
            SizeF curCharSizeF;
            PointF startPointF = new PointF(0, (height - totalSizeF.Height) / 2);
            Random random = new Random(); //随机数产生器
            g.Clear(Color.White); //清空图片背景色  
            for (int i = 0; i < verifyCode.Length; i++)
            {
                brush = new LinearGradientBrush(new Point(0, 0), new Point(1, 1), Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)), Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)));
                g.DrawString(verifyCode[i].ToString(), font, brush, startPointF);
                curCharSizeF = g.MeasureString(verifyCode[i].ToString(), font);
                startPointF.X += curCharSizeF.Width;
            }
            //画图片的干扰线  
            for (int i = 0; i < 10; i++)
            {
                int x1 = random.Next(bitmap.Width);
                int x2 = random.Next(bitmap.Width);
                int y1 = random.Next(bitmap.Height);
                int y2 = random.Next(bitmap.Height);
                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }
            //画图片的前景干扰点  
            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(bitmap.Width);
                int y = random.Next(bitmap.Height);
                bitmap.SetPixel(x, y, Color.FromArgb(random.Next()));
            }
            g.DrawRectangle(new Pen(Color.Silver), 0, 0, bitmap.Width - 1, bitmap.Height - 1); //画图片的边框线  
            g.Dispose();
            //保存图片数据  
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Jpeg);
            //输出图片流  
            return stream.ToArray();
        }

    }

    public class ImgVerfiyRecord
    {
        public string Result { get; set; }
        public Guid Seed { get; set; }
        public bool IsUsed { get; set; }
    }


}
