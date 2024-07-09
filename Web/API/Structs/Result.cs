using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrApps42.Web.API
{
    public struct Result<T>
    {
        public int StatusCode;
        public T? Value;
        public string? Error;

        public Result(int statusCode, T value, string error = "")
        {
            this.StatusCode = statusCode;
            this.Value = value;
            this.Error = error;
        }
    }
}
