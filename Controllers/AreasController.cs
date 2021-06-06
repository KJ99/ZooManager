using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooManager.Data.Contexts;
using ZooManager.Models.DTO.Requests;
using ZooManager.Models.DTO.Responses;
using ZooManager.Models.Entities;


namespace ZooManager.Controllers
{
    [ApiController]
    [Route("v1/areas")]
    public class AreasController : ZooApiController
    {
        public AreasController(IMapper mapper, ApiDbContext db, UserManager<IdentityUser> userManager) : base(mapper, db, userManager)
        {
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(AreaResponse[]))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "api.admin,zoo.manager,zoo.employee")]
        public async Task<ObjectResult> ListArea()
        {
           IQueryable<Area> query = _db.Areas;
           var list  = await query
               .Include(t => t.Animals)
               .ToListAsync();

           return Ok(Map<AreaResponse>(list.ToArray()));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "api.admin,zoo.manager,zoo.employee")]
        [HttpGet("{code}")]
        public async Task<ActionResult> GetArea(string code)
        {
            var area = await _db.Areas
                .Include(t => t.Animals)
                .Where(a => a.Code == code)
                .FirstOrDefaultAsync();

            if (area == null)
           {
               return NotFound();
           }

           return Ok(Map<AreaResponse>(area));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "api.admin,zoo.manager")]
        [HttpPost("")]
        [ProducesResponseType(200, Type = typeof(AreaResponse[]))]
        public async Task<ActionResult> Post([FromBody] AreaRequest request)
        { 
            if(!ModelState.IsValid)
            {
                return ProcessValidationViolations();
            }

            Area area = Map<Area>(request);
            _db.Areas.Add(area);
            await _db.SaveChangesAsync();

            Area created = await _db.Areas
              .Where(s => s.Code == area.Code)
              .Include(a => a.Animals)
              .FirstOrDefaultAsync();

            return Created("", Map<AreaResponse>(created));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "api.admin,zoo.manager")]
        [HttpPatch("{code}")]
        [ProducesResponseType(202, Type = typeof(AreaResponse[]))]
        public async Task<ActionResult> Patch([FromRoute] string code, [FromBody] AreaUpdateRequest request)
        {
            Area current = await _db.Areas
            .Where(s => s.Code == code)
            .FirstOrDefaultAsync();

            Area received = Map<Area>(request);

            if (current == null) 
            {
                return NotFound();
            }

            current.Name = received.Name;

            await _db.SaveChangesAsync();

            Area updated = await _db.Areas
           .Where(s => s.Code == code)
           .Include(a => a.Animals)
           .FirstOrDefaultAsync();

            return Accepted(Map<AreaResponse> (updated));
            
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "api.admin,zoo.manager")]
        [HttpDelete("{code}")]
        public IActionResult DeleteArea(string code)
        {
           var area = _db.Areas.Find(code);

           if(area == null)
           {
               return NotFound();
           }

           _db.Areas.Remove(area);
           _db.SaveChanges();

           return NoContent();

        }


    }
}
