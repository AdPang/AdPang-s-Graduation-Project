﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Common.RequestInfoModel
{
    public class RequestInfoModel
    {
        public Guid? CurrentOperaingUser { get; set; }
        public bool ImgVerifyCodeIsVerify { get; set; } = false;
        public bool MailVerifyCodeIsVerify { get; set; } = false;
    }
}
