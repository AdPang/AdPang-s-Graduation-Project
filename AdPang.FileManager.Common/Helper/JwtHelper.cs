using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AdPang.FileManager.Common.Helper
{
    public class JwtHelper
    {

        /// <summary>
        /// 颁发JWT字符串
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public static string IssueJwt(List<Claim> claims)
        {
            string iss = Appsettings.App(new string[] { "Audience", "Issuer" });
            string aud = Appsettings.App(new string[] { "Audience", "Audience" });
            string secret = Appsettings.App(new string[] { "Audience", "Secret" });

            claims.AddRange(new List<Claim>
            {
                new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(15)).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Iss,iss),
                new Claim(JwtRegisteredClaimNames.Aud,aud),
            });

            //秘钥 (SymmetricSecurityKey 对安全性的要求，密钥的长度太短会报出异常)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: iss,
                claims: claims,
                signingCredentials: creds);

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static string SerializeJwt(string authorStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            //var tokenModelJwt = new TokenModelJwt();
            string userIdStr = string.Empty;
            // token校验
            if (!string.IsNullOrEmpty(authorStr) && jwtHandler.CanReadToken(authorStr))
            {

                JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(authorStr);
                userIdStr = jwtToken.Payload[ClaimTypes.NameIdentifier] as string;


                var testRoleStr = jwtToken.Payload[ClaimTypes.Role] as string;
            }
            return userIdStr;
        }

        /// <summary>
        /// 授权解析jwt
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        //private static TokenModelJwt ParsingJwtToken(HttpContext httpContext)
        //{
        //    if (!httpContext.Request.Headers.ContainsKey("Authorization"))
        //        return null;
        //    var tokenHeader = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        //    TokenModelJwt tm = SerializeJwt(tokenHeader);
        //    return tm;
        //}

    }
}
