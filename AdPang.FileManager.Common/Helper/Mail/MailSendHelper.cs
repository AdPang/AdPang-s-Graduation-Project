using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace AdPang.FileManager.Common.Helper.Mail
{
    public class MailSendHelper
    {
        static IConfiguration Configuration { get; set; }
        public MailSendHelper(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public static async Task<bool> SendVerfiyCodeMessageMail(string sendTo, MailMsgOperaType operaType, string verifyCode)
        {
            try
            {
                MailEntity mails = new MailEntity()
                {
                    AuthCode = Configuration["MailConfigs:AuthCode"],
                    MailBody = await MailPageHelper.GetMailPage(MailMsgOperaType.账号注册, verifyCode),
                    FromPerson = Configuration["MailConfigs:FromPerson"],
                    Host = "smtp.qq.com",
                    IsbodyHtml = true,
                    MailTitle = "AdPang文件管理器",
                    RecipientArry = new string[] { sendTo }
                };

                //将发件人邮箱带入MailAddress中初始化
                MailAddress mailAddress = new MailAddress(mails.FromPerson);
                //创建Email的Message对象
                MailMessage mailMessage = new MailMessage();

                //判断收件人数组中是否有数据
                if (mails.RecipientArry.Count() > 0)
                {
                    //循环添加收件人地址
                    foreach (var item in mails.RecipientArry)
                    {
                        if (!string.IsNullOrEmpty(item))
                            mailMessage.To.Add(item.ToString());
                    }
                }
                //发件人邮箱
                mailMessage.From = mailAddress;
                //标题
                mailMessage.Subject = mails.MailTitle;
                //编码
                mailMessage.SubjectEncoding = Encoding.UTF8;
                //正文
                mailMessage.Body = mails.MailBody;
                //正文编码
                mailMessage.BodyEncoding = Encoding.Default;
                //邮件优先级
                mailMessage.Priority = MailPriority.High;
                //正文是否是html格式
                mailMessage.IsBodyHtml = mails.IsbodyHtml;

                //实例化一个Smtp客户端
                SmtpClient smtp = new SmtpClient();
                //将发件人的邮件地址和客户端授权码带入以验证发件人身份
                smtp.Credentials = new System.Net.NetworkCredential(mails.FromPerson, mails.AuthCode);
                //指定SMTP邮件服务器
                smtp.Host = mails.Host;

                //邮件发送到SMTP服务器
                smtp.Send(mailMessage);
                smtp.Dispose();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
    public class MailEntity
    {
        /// <summary>
        /// 发送人
        /// </summary>
        public string FromPerson { get; set; }

        /// <summary>
        /// 收件人地址(多人)
        /// </summary>
        public IEnumerable<string> RecipientArry { get; set; }


        /// <summary>
        /// 标题
        /// </summary>
        public string MailTitle { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        public string MailBody { get; set; }

        /// <summary>
        /// 客户端授权码(可存在配置文件中)
        /// </summary>
        public string AuthCode { get; set; }

        /// <summary>
        /// SMTP邮件服务器
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 正文是否是html格式
        /// </summary>
        public bool IsbodyHtml { get; set; }
        ///// <summary>
        ///// 接收文件
        ///// </summary>
        //public List<IFormFile> files { get; set; }
    }
}
