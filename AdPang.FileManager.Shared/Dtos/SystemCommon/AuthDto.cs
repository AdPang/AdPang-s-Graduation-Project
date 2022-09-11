namespace AdPang.FileManager.Shared.Dtos.SystemCommon
{
    public class AuthDto
    {
        /// <summary>
        /// 是否成功登录
        /// </summary>
        public bool IsSuccess { get; set; } = false;
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// Jwt令牌
        /// </summary>
        public string JwtStr { get; set; } = string.Empty;
    }
}
