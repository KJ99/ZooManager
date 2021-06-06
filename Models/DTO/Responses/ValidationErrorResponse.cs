using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZooManager.Models.DTO.Responses
{
    public class ValidationErrorResponse
    {
        public string Path { get; set; }

        public string Message { get; set; }
    }
}
