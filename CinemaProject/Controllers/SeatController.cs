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
    public class SeatController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public SeatController(CinemaDbContext context)
        {
            _context = context;
        }

        // GET: api/seat
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeatDto>>> GetSeats()
        {
            var list = await _context.Seats
                .Select(s => new SeatDto
                {
                    Id = s.Id,
                    HallId = s.HallId,
                    RowLabel = s.RowLabel,
                    SeatNumber = s.SeatNumber,
                    SeatCode = s.SeatCode,
                    SeatType = s.SeatType,
                    IsActive = s.IsActive,
                    CreatedAt = s.CreatedAt
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/seat/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SeatDto>> GetSeat(int id)
        {
            var s = await _context.Seats.FindAsync(id);
            if (s == null) return NotFound();

            var dto = new SeatDto
            {
                Id = s.Id,
                HallId = s.HallId,
                RowLabel = s.RowLabel,
                SeatNumber = s.SeatNumber,
                SeatCode = s.SeatCode,
                SeatType = s.SeatType,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt
            };

            return Ok(dto);
        }

        // POST: api/seat
        [HttpPost]
        public async Task<ActionResult<SeatDto>> CreateSeat([FromBody] SeatDto dto)
        {
            if (dto == null)
                return BadRequest("Request body is missing or invalid.");

            if (string.IsNullOrWhiteSpace(dto.SeatType))
                return BadRequest("SeatType is required.");

            var hallExists = await _context.Halls.AnyAsync(h => h.Id == dto.HallId);
            if (!hallExists)
                return BadRequest($"Hall with id {dto.HallId} does not exist.");

            var entity = new Seat
            {
                HallId = dto.HallId,
                RowLabel = dto.RowLabel,
                SeatNumber = dto.SeatNumber,
                SeatCode = dto.SeatCode,
                SeatType = dto.SeatType,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.Seats.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            dto.CreatedAt = entity.CreatedAt;

            return CreatedAtAction(nameof(GetSeat), new { id = dto.Id }, dto);
        }

        // PUT: api/seat/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeat(int id, [FromBody] SeatDto dto)
        {
            if (dto == null || id != dto.Id)
                return BadRequest();

            var entity = await _context.Seats.FindAsync(id);
            if (entity == null) return NotFound();

            if (string.IsNullOrWhiteSpace(dto.SeatType))
                return BadRequest("SeatType is required.");

            var hallExists = await _context.Halls.AnyAsync(h => h.Id == dto.HallId);
            if (!hallExists)
                return BadRequest($"Hall with id {dto.HallId} does not exist.");

            entity.HallId = dto.HallId;
            entity.RowLabel = dto.RowLabel;
            entity.SeatNumber = dto.SeatNumber;
            entity.SeatCode = dto.SeatCode;
            entity.SeatType = dto.SeatType;
            entity.IsActive = dto.IsActive;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/seat/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeat(int id)
        {
            var entity = await _context.Seats.FindAsync(id);
            if (entity == null) return NotFound();

            _context.Seats.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
