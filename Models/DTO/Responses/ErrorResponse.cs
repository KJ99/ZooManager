using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooManager.Models.DTO.Responses
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public string Code { get; set; }
    }
}
