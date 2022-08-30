using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
