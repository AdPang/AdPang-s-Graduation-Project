using System.Text;
using System.Security.Cryptography;
using System.DrawingCore;
using System.DrawingCore.Drawing2D;
using System.DrawingCore.Imaging;

namespace AdPang.FileManager.Common.Helper
{
    public class VerifyCodeHelper 
    {
        private readonly RedisHelper redis;

        public VerifyCodeHelper(RedisHelper redis)
        {
            this.redis = redis;
        }

        public string Creat4VerfiyCode(Guid seed)
        {
            var md5 = EncryptMD5(seed.ToString());
            try
            {
                int index = (int)(md5.Sum(x => x) % 16 + md5.Average(x => x)) % 16;
                char c = md5[index];
                Random random = new Random(c);
                string result = c.ToString();
                for (int i = 0; i < 3; i++)
                {
                    result += md5[random.Next() % 16].ToString();
                }
                var recode = redis.GetStringKey<VerfiyRecord>(seed.ToString());
                if (recode != null) throw new Exception("生成验证码失败");
                var isSaved = redis.SetStringKey<VerfiyRecord>(seed.ToString(), new VerfiyRecord
                {
                    Result = result,
                    Seed = seed,
                }, 5);
                if (!isSaved) throw new Exception("生成验证码失败");
                return result;
            }
            catch
            {
                throw new Exception("生成验证码失败");
            }
        }



        public bool Verify4Capt(Guid seed, string verifyCode)
        {
            if (verifyCode == null) throw new ArgumentException("parameter {verificationCode} is null!");
            //var md5 = EncryptMD5(seed.ToString());
            try
            {
                var recode = redis.GetStringKey<VerfiyRecord>(seed.ToString());
                if (recode.IsUsed) throw new Exception();
                if (verifyCode.ToUpper() == recode.Result.ToUpper())
                {
                    var isSaved = redis.SetStringKey<VerfiyRecord>(seed.ToString(), new VerfiyRecord
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




        private string EncryptMD5(string s)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            return BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(s))).Replace("-", "");
        }
        public static byte[] CreateByteByImgVerifyCode(string verifyCode, int width, int height)
        {
            Font font = new Font("Arial", 14, (FontStyle.Bold | FontStyle.Italic));
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

    public class VerfiyRecord
    {
        public string Result { get; set; }
        public Guid Seed { get; set; }
        public bool IsUsed { get; set; }
    }
}
