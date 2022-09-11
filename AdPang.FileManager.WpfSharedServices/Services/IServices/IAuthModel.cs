namespace AdPang.FileManager.Application_WPF.Services.IServices
{
    public interface IAuthModel
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? JwtStr { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
