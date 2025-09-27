// Controllers/CinemaController.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaProject.Models;
using CinemaBackend.Data;
using CinemaProject.Dto;

namespace CinemaProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CinemaController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public CinemaController(CinemaDbContext context)
        {
            _context = context;
        }

        // GET: api/cinema
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CinemaDto>>> GetCinemas()
        {
            var list = await _context.Cinemas
                .Select(c => new CinemaDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Address = c.Address,
                    City = c.City,
                    Phone = c.Phone,
                    Email = c.Email,
                    Website = c.Website,
                    Timezone = c.Timezone,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt ?? DateTime.MinValue
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/cinema/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CinemaDto>> GetCinema(long id)
        {
            var c = await _context.Cinemas.FindAsync(id);
            if (c == null)
                return NotFound();

            var dto = new CinemaDto
            {
                Id = c.Id,
                Name = c.Name,
                Address = c.Address,
                City = c.City,
                Phone = c.Phone,
                Email = c.Email,
                Website = c.Website,
                Timezone = c.Timezone,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt ?? DateTime.MinValue
            };

            return Ok(dto);
        }

        // POST: api/cinema
        [HttpPost]
        public async Task<ActionResult<CinemaDto>> CreateCinema(CinemaDto dto)
        {
            var entity = new Cinema
            {
                Name = dto.Name,
                Address = dto.Address,
                City = dto.City,
                Phone = dto.Phone,
                Email = dto.Email,
                Website = dto.Website,
                Timezone = dto.Timezone,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Cinemas.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            dto.CreatedAt = entity.CreatedAt;
            dto.UpdatedAt = entity.UpdatedAt ?? DateTime.MinValue;

            return CreatedAtAction(nameof(GetCinema), new { id = dto.Id }, dto);
        }

        // PUT: api/cinema/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCinema(long id, CinemaDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var entity = await _context.Cinemas.FindAsync(id);
            if (entity == null)
                return NotFound();

            entity.Name = dto.Name;
            entity.Address = dto.Address;
            entity.City = dto.City;
            entity.Phone = dto.Phone;
            entity.Email = dto.Email;
            entity.Website = dto.Website;
            entity.Timezone = dto.Timezone;
            entity.UpdatedAt = DateTime.UtcNow;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/cinema/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCinema(long id)
        {
            var entity = await _context.Cinemas.FindAsync(id);
            if (entity == null)
                return NotFound();

            _context.Cinemas.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
