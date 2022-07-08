using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Shared
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            
        }
        public ApiResponse(string message, bool satus = false)
        {
            this.Message = message;
            this.Status = satus;
        }
        public ApiResponse(bool status, object reuslt)
        {
            this.Status = status;
            this.Result = reuslt;
        }
        public string Message { get; set; }
        public bool Status { get; set; }
        public object Result { get; set; }
    }

    public class ApiResponse<T>
    {
        public ApiResponse()
        {

        }
        public ApiResponse(bool status, string msg)
        {
            Status = status;
            Message = msg;
        }
        public ApiResponse(bool status, T result)
        {
            Status = status;
            Result = result;
        }
        public string Message { get; set; }

        public bool Status { get; set; }

        public T Result { get; set; }
    }
}
