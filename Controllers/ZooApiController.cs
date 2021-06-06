using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooManager.Data.Contexts;
using ZooManager.Models.DTO.Responses;

namespace ZooManager.Controllers
{
    public class ZooApiController : ControllerBase
    {
        protected IMapper _mapper;

        protected ApiDbContext _db;

        protected UserManager<IdentityUser> _userManager;

        public ZooApiController(IMapper mapper, ApiDbContext db, UserManager<IdentityUser> userManager)
        {
            _mapper = mapper;
            _db = db;
            _userManager = userManager;
        }


        protected List<T> Map<T>(object[] list)
        {
            List<T> result = new List<T>();

            foreach(object obj in list)
            {
                result.Add(Map<T>(obj));
            }

            return result;
        }

        protected T Map<T>(object obj)
        {
            return _mapper.Map<T>(obj);
        }

        protected ActionResult ProcessValidationViolations()
        {
            List<ValidationErrorResponse> violations = new List<ValidationErrorResponse>();
            foreach(string key in ModelState.Keys)
            {
                foreach (var error in ModelState[key].Errors)
                {
                    violations.Add(new()
                    {
                        Path = key,
                        Message = error.ErrorMessage
                    });
                }
            }
            return BadRequest(violations);
        }
    }
}
