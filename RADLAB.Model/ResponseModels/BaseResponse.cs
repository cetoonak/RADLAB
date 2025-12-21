using System;
using System.Collections.Generic;
using System.Text;

namespace RADLAB.Model.ResponseModels
{
    public class BaseResponse
    {
        public BaseResponse()
        {
            Success = true;
        }

        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public int RowCount { get; set; }

        public void SetException(Exception Exception)
        {
            Success = false;
            Message = Exception.Message;
        }
    }
}