using System.Security.Cryptography;
using System.Text;

namespace AdPang.FileManager.Shared.Common.Helper
{
    public class MD5Helper
    {
        /// <summary>
        /// 16位MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5Encrypt16(string password)
        {
            var md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(password)), 4, 8);
            t2 = t2.Replace("-", string.Empty);
            return t2;
        }

        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5Encrypt32(string password = "")
        {
            string pwd = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(password) && !string.IsNullOrWhiteSpace(password))
                {
                    MD5 md5 = MD5.Create(); //实例化一个md5对像
                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
                    byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                    // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
                    foreach (var item in s)
                    {
                        // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                        pwd = string.Concat(pwd, item.ToString("X2"));
                    }
                }
            }
            catch
            {
                throw new Exception($"错误的 password 字符串:【{password}】");
            }
            return pwd;
        }

        /// <summary>
        /// 64位MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5Encrypt64(string password)
        {
            // 实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(s);
        }



        public static string GetMD5HashCodeFromFile(string filePath)
        {
            try
            {
                using (FileStream fStream = new FileStream(filePath, System.IO.FileMode.Open))
                {
                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] retVal = md5.ComputeHash(fStream);

                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < retVal.Length; i++)
                    {
                        builder.Append(retVal[i].ToString("X2"));
                    }
                    return builder.ToString();
                };
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashCodeFromFile() fail,error:" + ex.Message);
            }
        }

        //public static string GetMD5HashCodeFromFileStream(FileStream fStream)
        //{
        //    try
        //    {
        //        MD5 md5 = new MD5CryptoServiceProvider();
        //        byte[] retVal = md5.ComputeHash(fStream);

        //        StringBuilder builder = new StringBuilder();
        //        for (int i = 0; i < retVal.Length; i++)
        //        {
        //            builder.Append(retVal[i].ToString("X2"));
        //        }
        //        return builder.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("GetMD5HashCodeFromFile() fail,error:" + ex.Message);
        //    }
        //}

        public static byte[] GetMD5HashBytesFromFile(string filePath)
        {
            try
            {
                using (FileStream fStream = new FileStream(filePath, System.IO.FileMode.Open))
                {
                    MD5 md5 = new MD5CryptoServiceProvider();
                    return md5.ComputeHash(fStream);
                };
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashBytesFromFile() fail,error:" + ex.Message);
            }
        }

    }
}
