using AdPang.FileManager.Common.Helper.Mail;
using AdPang.FileManager.Common.Helper.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Common.Helper.VerifyCode
{
    public class MailVerifyCodeHelper
    {
        private readonly RedisHelper redis;

        public MailVerifyCodeHelper(RedisHelper redis)
        {
            this.redis = redis;
        }

        public string Creat6MailVerfiyCode(string email, MailMsgOperaType operaType)
        {
            var result = VerifyCodeHelper.CreatVeriyCode(6,Guid.NewGuid());
            var record = GetVerifyCodeByRedisDb(email, operaType);
            if (record != null)
            {
                if (record.IsUsed)
                    throw new Exception("请求过于频繁！请稍后重试！");
                return record.Result;
            }
            if (SaveVerifyCodeToRedisDb(email, operaType, result)) return result;
            else throw new Exception("发生错误！");

        }

        /// <summary>
        /// 获取record
        /// </summary>
        /// <param name="email"></param>
        /// <param name="operaType"></param>
        /// <returns></returns>
        private MailVerfiyRecord GetVerifyCodeByRedisDb(string email, MailMsgOperaType operaType)
        {
            return redis.GetStringKey<MailVerfiyRecord>(email + operaType);
        }

        private bool SaveVerifyCodeToRedisDb(string email, MailMsgOperaType operaType,string result)
        {
            var isSaved = redis.SetStringKey(email+operaType, new MailVerfiyRecord
            {
                Result = result,
                Email = email,
                OperaType = operaType
            }, 10);
            return isSaved;
        }

        private bool SaveVerifyCodeToRedisDb(MailVerfiyRecord record)
        {
            record.IsUsed = true;
            var isSaved = redis.SetStringKey(record.Email + record.OperaType, record, 10);
            return isSaved;
        }

        public bool VerifyImgCode(string email, string verifyCode, MailMsgOperaType operaType)
        {
            var record = GetVerifyCodeByRedisDb(email, operaType);
            if (record == null || record.IsUsed || !record.Result.ToUpper().Equals(verifyCode.ToUpper())) return false;
            record.IsUsed = true;
            if(SaveVerifyCodeToRedisDb(record)) return true;
            return false;
        }
    }

    public class MailVerfiyRecord
    {
        public string Email { get; set; }
        public string Result { get; set; }
        public bool IsUsed { get; set; }
        public MailMsgOperaType OperaType { get; set; }
    }
}
