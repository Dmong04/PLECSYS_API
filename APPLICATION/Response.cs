using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION
{
    public class Response<T>
    {
        public T? Data { get; set; }

        public bool Success { get; set; }

        public required string Message { get; set; }

        public Response() { }

        public Response(T? data, bool success, string message)
        {
            Data = data;
            Success = success;
            Message = message;
        }
    }
}
