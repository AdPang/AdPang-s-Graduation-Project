namespace AdPang.FileManager.Common.Helper.Mail
{
    public static class MailPageHelper
    {
        public static async Task<string> GetMailPage(MailMsgOperaType operaType, string verifyCode)
        {
            string basePath = Path.GetDirectoryName(typeof(MailPageHelper).Assembly.Location);
            if (basePath == null) throw new DirectoryNotFoundException();
            var mailHead = await File.ReadAllTextAsync(@$"{basePath}/StaticResouce/MailSendPageHead.txt");
            var mailBody = await File.ReadAllTextAsync(@$"{basePath}/StaticResouce/MailSendPageBody.txt");
            var mailContent = mailHead + String.Format(mailBody, operaType.ToString(), verifyCode);
            return mailContent;
        }
    }


    public enum MailMsgOperaType
    {
        账号注销,
        账号注册,
        找回密码,
        重置密码,
    }
}
