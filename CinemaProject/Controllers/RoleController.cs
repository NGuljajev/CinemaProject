using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaBackend.Data;
using CinemaProject.Models;
using CinemaProject.Dto;

namespace CinemaProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public RoleController(CinemaDbContext context)
        {
            _context = context;
        }

        // GET: api/role
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
        {
            var list = await _context.Roles
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/role/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetRole(int id)
        {
            var r = await _context.Roles.FindAsync(id);
            if (r == null) return NotFound();

            var dto = new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                CreatedAt = r.CreatedAt
            };

            return Ok(dto);
        }

        // POST: api/role
        [HttpPost]
        public async Task<ActionResult<RoleDto>> CreateRole([FromBody] RoleDto dto)
        {
            if (dto == null) return BadRequest();

            var entity = new Role
            {
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };

            _context.Roles.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            dto.CreatedAt = entity.CreatedAt;

            return CreatedAtAction(nameof(GetRole), new { id = dto.Id }, dto);
        }

        // PUT: api/role/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleDto dto)
        {
            if (dto == null || id != dto.Id) return BadRequest();

            var entity = await _context.Roles.FindAsync(id);
            if (entity == null) return NotFound();

            entity.Name = dto.Name;
            entity.Description = dto.Description;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/role/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var entity = await _context.Roles.FindAsync(id);
            if (entity == null) return NotFound();

            _context.Roles.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
