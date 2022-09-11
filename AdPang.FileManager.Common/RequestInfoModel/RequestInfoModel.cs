namespace AdPang.FileManager.Common.RequestInfoModel
{
    public class RequestInfoModel
    {
        public Guid? CurrentOperaingUser { get; set; }
        public bool ImgVerifyCodeIsVerify { get; set; } = false;
        public bool MailVerifyCodeIsVerify { get; set; } = false;
    }
}
