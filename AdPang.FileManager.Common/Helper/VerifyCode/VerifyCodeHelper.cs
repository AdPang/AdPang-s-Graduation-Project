using System.Security.Cryptography;
using System.Text;

namespace AdPang.FileManager.Common.Helper.VerifyCode
{
    public static class VerifyCodeHelper
    {
        /// <summary>
        /// 生成随机验证码
        /// </summary>
        /// <param name="veriyCodeLength"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string CreatVeriyCode(int veriyCodeLength, Guid seed)
        {
            var md5 = EncryptMD5(seed.ToString());
            try
            {
                int index = (int)(md5.Sum(x => x) % 16 + md5.Average(x => x)) % 16;
                char c = md5[index];
                Random random = new Random(c);
                string result = c.ToString();
                for (int i = 0; i < veriyCodeLength - 1; i++)
                {
                    result += md5[random.Next() % 16].ToString();
                }
                return result;
            }
            catch
            {
                throw new Exception("生成验证码失败");
            }
        }
        private static string EncryptMD5(string s)
        {
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(s))).Replace("-", "");
        }
    }
}
