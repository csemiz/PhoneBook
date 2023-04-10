using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookEntityLayer.Mappings.ResultModels
{
    public class Result : IResult
    {
        public Result()
        {

        }
        public bool IsSuccess { get; set ; }
        public string Message { get ; set ; }

        public Result(bool success)
        {
            IsSuccess = success;
        }

        public Result(bool success, string message):this(success)
        {
            Message = message;
            //IsSuccess = success;
        }
    }
}
