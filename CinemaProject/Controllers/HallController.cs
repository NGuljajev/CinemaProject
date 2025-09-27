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
    public class HallController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public HallController(CinemaDbContext context)
        {
            _context = context;
        }

        // GET: api/hall
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HallDto>>> GetHalls()
        {
            var list = await _context.Halls
                .Select(h => new HallDto
                {
                    Id = h.Id,
                    CinemaId = h.CinemaId,
                    Name = h.Name,
                    SeatCount = h.SeatCount,
                    Description = h.Description,
                    Is3D = h.Is3D,
                    CreatedAt = h.CreatedAt,
                    UpdatedAt = h.UpdatedAt
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/hall/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HallDto>> GetHall(int id)
        {
            var h = await _context.Halls.FindAsync(id);
            if (h == null) return NotFound();

            var dto = new HallDto
            {
                Id = h.Id,
                CinemaId = h.CinemaId,
                Name = h.Name,
                SeatCount = h.SeatCount,
                Description = h.Description,
                Is3D = h.Is3D,
                CreatedAt = h.CreatedAt,
                UpdatedAt = h.UpdatedAt
            };

            return Ok(dto);
        }

        // POST: api/hall
        [HttpPost]
        public async Task<ActionResult<HallDto>> CreateHall([FromBody] HallDto dto)
        {
            if (dto == null) return BadRequest("Request body is missing or invalid.");

            var entity = new Hall
            {
                CinemaId = dto.CinemaId,
                Name = dto.Name,
                SeatCount = dto.SeatCount,
                Description = dto.Description,
                Is3D = dto.Is3D,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            _context.Halls.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            dto.CreatedAt = entity.CreatedAt;
            dto.UpdatedAt = entity.UpdatedAt;

            return CreatedAtAction(nameof(GetHall), new { id = dto.Id }, dto);
        }

        // PUT: api/hall/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHall(int id, [FromBody] HallDto dto)
        {
            if (dto == null || id != dto.Id) return BadRequest();

            var entity = await _context.Halls.FindAsync(id);
            if (entity == null) return NotFound();

            entity.CinemaId = dto.CinemaId;
            entity.Name = dto.Name;
            entity.SeatCount = dto.SeatCount;
            entity.Description = dto.Description;
            entity.Is3D = dto.Is3D;
            entity.UpdatedAt = DateTime.UtcNow;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/hall/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHall(int id)
        {
            var entity = await _context.Halls.FindAsync(id);
            if (entity == null) return NotFound();

            _context.Halls.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
