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
    public class ScreeningController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public ScreeningController(CinemaDbContext context)
        {
            _context = context;
        }

        // GET: api/screening
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScreeningDto>>> GetScreenings()
        {
            var list = await _context.Screenings
                .Select(s => new ScreeningDto
                {
                    Id = s.Id,
                    CinemaId = s.CinemaId,
                    HallId = s.HallId,
                    FilmId = s.FilmId,
                    StartAt = s.StartAt,
                    EndAt = s.EndAt,
                    Price = s.Price,
                    Language = s.Language,
                    Format = s.Format,
                    SeatsTotal = s.SeatsTotal,
                    SeatsAvailable = s.SeatsAvailable,
                    IsActive = s.IsActive,
                    CreatedAt = s.CreatedAt
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/screening/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ScreeningDto>> GetScreening(int id)
        {
            var s = await _context.Screenings.FindAsync(id);
            if (s == null) return NotFound();

            var dto = new ScreeningDto
            {
                Id = s.Id,
                CinemaId = s.CinemaId,
                HallId = s.HallId,
                FilmId = s.FilmId,
                StartAt = s.StartAt,
                EndAt = s.EndAt,
                Price = s.Price,
                Language = s.Language,
                Format = s.Format,
                SeatsTotal = s.SeatsTotal,
                SeatsAvailable = s.SeatsAvailable,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt
            };

            return Ok(dto);
        }

        // POST: api/screening
        [HttpPost]
        public async Task<ActionResult<ScreeningDto>> CreateScreening([FromBody] ScreeningDto dto)
        {
            if (dto == null)
                return BadRequest("Request body is missing or invalid.");

            if (dto.StartAt >= dto.EndAt)
                return BadRequest("StartAt must be earlier than EndAt.");

            if (dto.Price < 0)
                return BadRequest("Price must be non-negative.");

            // Validate related entities
            var cinemaExists = await _context.Cinemas.AnyAsync(c => c.Id == dto.CinemaId);
            if (!cinemaExists)
                return BadRequest($"Cinema with id {dto.CinemaId} does not exist.");

            var hallExists = await _context.Halls.AnyAsync(h => h.Id == dto.HallId);
            if (!hallExists)
                return BadRequest($"Hall with id {dto.HallId} does not exist.");

            var filmExists = await _context.Films.AnyAsync(f => f.Id == dto.FilmId);
            if (!filmExists)
                return BadRequest($"Film with id {dto.FilmId} does not exist.");

            var entity = new Screening
            {
                CinemaId = dto.CinemaId,
                HallId = dto.HallId,
                FilmId = dto.FilmId,
                StartAt = dto.StartAt,
                EndAt = dto.EndAt,
                Price = dto.Price,
                Language = dto.Language,
                Format = dto.Format,
                SeatsTotal = dto.SeatsTotal,
                SeatsAvailable = dto.SeatsAvailable,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.Screenings.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            dto.CreatedAt = entity.CreatedAt;

            return CreatedAtAction(nameof(GetScreening), new { id = dto.Id }, dto);
        }

        // PUT: api/screening/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScreening(int id, [FromBody] ScreeningDto dto)
        {
            if (dto == null || id != dto.Id)
                return BadRequest();

            if (dto.StartAt >= dto.EndAt)
                return BadRequest("StartAt must be earlier than EndAt.");

            if (dto.Price < 0)
                return BadRequest("Price must be non-negative.");

            var entity = await _context.Screenings.FindAsync(id);
            if (entity == null) return NotFound();

            // Validate related entities
            var cinemaExists = await _context.Cinemas.AnyAsync(c => c.Id == dto.CinemaId);
            if (!cinemaExists)
                return BadRequest($"Cinema with id {dto.CinemaId} does not exist.");

            var hallExists = await _context.Halls.AnyAsync(h => h.Id == dto.HallId);
            if (!hallExists)
                return BadRequest($"Hall with id {dto.HallId} does not exist.");

            var filmExists = await _context.Films.AnyAsync(f => f.Id == dto.FilmId);
            if (!filmExists)
                return BadRequest($"Film with id {dto.FilmId} does not exist.");

            entity.CinemaId = dto.CinemaId;
            entity.HallId = dto.HallId;
            entity.FilmId = dto.FilmId;
            entity.StartAt = dto.StartAt;
            entity.EndAt = dto.EndAt;
            entity.Price = dto.Price;
            entity.Language = dto.Language;
            entity.Format = dto.Format;
            entity.SeatsTotal = dto.SeatsTotal;
            entity.SeatsAvailable = dto.SeatsAvailable;
            entity.IsActive = dto.IsActive;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/screening/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScreening(int id)
        {
            var entity = await _context.Screenings.FindAsync(id);
            if (entity == null) return NotFound();

            _context.Screenings.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
