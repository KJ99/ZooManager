using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ZooManager.Data.Contexts;
using ZooManager.Models.DTO.Requests;
using ZooManager.Models.DTO.Responses;
using ZooManager.Models.Entities;

namespace ZooManager.Controllers
{
    [ApiController]
    [Route("v1/animals")]
    public class AnimalsController : ZooApiController
    {
        public AnimalsController(IMapper mapper, ApiDbContext db, UserManager<IdentityUser> userManager) : base(mapper, db, userManager)
        {
        }

        [HttpGet("")]
        [ProducesResponseType(200, Type = typeof(AnimalResponse[]))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "api.admin,zoo.manager,zoo.employee")]
        public async Task<ObjectResult> List()
        {
            IQueryable<Animal> query = _db.Animals.AsQueryable<Animal>();
            var list = await query
                .Include(a => a.Species)
                .Include(a => a.Area)
                .ToListAsync();
            return Ok(Map<AnimalResponse>(list.ToArray()));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(AnimalResponse[]))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "api.admin,zoo.manager,zoo.employee")]
        public async Task<ActionResult> Get([FromRoute] int id)
        {
            Animal animal = await _db.Animals
                .Include(a => a.Area)
                .Include(a => a.Species)
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();

            if (animal == null)
            {
                return NotFound();
            }

            return Ok(Map<AnimalResponse>(animal));
        }

        [HttpPost("")]
        [ProducesResponseType(200, Type = typeof(AnimalResponse[]))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "api.admin,zoo.manager")]
        public async Task<ActionResult> Post([FromBody] AnimalRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ProcessValidationViolations();
            }

            Animal animal = Map<Animal>(request);
            _db.Animals.Add(animal);
            await _db.SaveChangesAsync();

            Animal created = await _db.Animals
                .Include(a => a.Area)
                .Include(a => a.Species)
                .Where(t => t.Id == animal.Id)
                .FirstOrDefaultAsync();

            return Created("", Map<AnimalResponse>(created));
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(202, Type = typeof(AnimalResponse[]))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "api.admin,zoo.manager")]
        public async Task<ActionResult> Patch([FromRoute] int id, [FromBody] AnimalRequest request)
        {
            Animal current = await _db.Animals
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync();

            Animal received = Map<Animal>(request);

            if (current == null)
            {
                return NotFound();
            }

            current.Name = received.Name;
            current.SpeciesId = received.SpeciesId;
            current.AreaCode = received.AreaCode;

            await _db.SaveChangesAsync();

            Animal updated = await _db.Animals
             .Include(a => a.Area)
             .Include(a => a.Species)
             .Where(t => t.Id == current.Id)
             .FirstOrDefaultAsync();

            return Accepted(Map<AnimalResponse>(updated));

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(202)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "api.admin,zoo.manager")]
        public async Task<ActionResult> DeleteAnimal ([FromRoute] int id)
        {
            Animal animals = await _db.Animals
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();

            if (animals == null)
            {
                return NotFound();
            }

            _db.Animals.Remove(animals);
            await _db.SaveChangesAsync();
            return NoContent();
        }


    }
}