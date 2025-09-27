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
    public class EventController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public EventController(CinemaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents()
        {
            var list = await _context.Events
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    CinemaId = e.CinemaId,
                    Name = e.Name,
                    Description = e.Description,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    DiscountPercent = e.DiscountPercent,
                    Active = e.Active,
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();

            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEvent(int id)
        {
            var e = await _context.Events.FindAsync(id);
            if (e == null) return NotFound();

            return Ok(new EventDto
            {
                Id = e.Id,
                CinemaId = e.CinemaId,
                Name = e.Name,
                Description = e.Description,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                DiscountPercent = e.DiscountPercent,
                Active = e.Active,
                CreatedAt = e.CreatedAt
            });
        }

        [HttpPost]
        public async Task<ActionResult<EventDto>> CreateEvent(EventDto dto)
        {
            var entity = new Event
            {
                CinemaId = dto.CinemaId,
                Name = dto.Name,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                DiscountPercent = dto.DiscountPercent,
                Active = dto.Active,
                CreatedAt = DateTime.UtcNow
            };

            _context.Events.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            dto.CreatedAt = entity.CreatedAt;

            return CreatedAtAction(nameof(GetEvent), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, EventDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var entity = await _context.Events.FindAsync(id);
            if (entity == null) return NotFound();

            entity.CinemaId = dto.CinemaId;
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.StartDate = dto.StartDate;
            entity.EndDate = dto.EndDate;
            entity.DiscountPercent = dto.DiscountPercent;
            entity.Active = dto.Active;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var entity = await _context.Events.FindAsync(id);
            if (entity == null) return NotFound();

            _context.Events.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
