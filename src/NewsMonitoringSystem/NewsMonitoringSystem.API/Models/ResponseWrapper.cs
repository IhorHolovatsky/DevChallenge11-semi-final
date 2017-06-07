using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsMonitoringSystem.API.Models
{
    public class ResponseWrapper<T>
    {
        public bool IsSuccess { get; set; }

        public T ResponseData { get; set; }

        public string ErrorMessage { get; set; }
    }
}