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
    [Route("v1/species")]
    public class SpeciesController : ZooApiController
    {
        public SpeciesController(IMapper mapper, ApiDbContext db, UserManager<IdentityUser> userManager) : base(mapper, db, userManager)
        {
        }

        [HttpGet("")]
        [ProducesResponseType(200, Type = typeof(SpeciesResponse[]))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "api.admin,zoo.manager,zoo.employee")]
        public async Task<ObjectResult> List()
        {
            IQueryable<Species> query = _db.Species;
            var list = await query
                .Include(t =>t.Animals)
                .ToListAsync();
            return Ok(Map<SpeciesResponse>(list.ToArray()));
                
        }
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(SpeciesResponse[]))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "api.admin,zoo.manager,zoo.employee")]
        public async Task<ActionResult> Get([FromRoute] int id)
        {
            Species species = await _db.Species
                 .Where(t => t.Id == id)
                 .Include(t => t.Animals)
                .FirstOrDefaultAsync();

            if (species == null)
            {
                return NotFound();
            }

            return Ok(Map<SpeciesResponse>(species));
        }

        [HttpPost("")]
        [ProducesResponseType(200, Type = typeof(SpeciesResponse[]))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "api.admin,zoo.manager")]
        public async Task<ActionResult> Post([FromBody] SpeciesRequest request)
        { 
            if(!ModelState.IsValid)
            {
                return ProcessValidationViolations();
            }

            Species species = Map<Species>(request);
            _db.Species.Add(species);
            await _db.SaveChangesAsync();

            Species created = await _db.Species
            .Where(s => s.Id == species.Id)
            .Include(s => s.Animals)
            .FirstOrDefaultAsync();
            return Created("", Map<SpeciesResponse>(created));
        }
        [HttpPatch("{id}")]
        [ProducesResponseType(202, Type = typeof(SpeciesResponse[]))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "api.admin,zoo.manager")]
        public async Task<ActionResult> Patch([FromRoute] int id, [FromBody] SpeciesRequest request)
        {
            Species current = await _db.Species
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync();

            Species received = Map<Species>(request);

            if (current == null) 
            {
                return NotFound();
            }

            current.Name = received.Name;
            current.Description = received.Description;

            await _db.SaveChangesAsync();

            Species updated = await _db.Species
            .Where(s => s.Id == current.Id)
            .Include(s => s.Animals)
            .FirstOrDefaultAsync();
            return Accepted(Map<SpeciesResponse> (updated));
            
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(202)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "api.admin,zoo.manager")]
        public async Task<ActionResult> Delete([FromRoute] int id)

        {
            Species species = await _db.Species
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();

            
            if (species == null)
            {
                return NotFound();
            }

            _db.Species.Remove(species);
            await _db.SaveChangesAsync();

            return NoContent();

        }


    }
}
