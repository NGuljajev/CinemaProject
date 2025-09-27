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
    public class AdController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public AdController(CinemaDbContext context)
        {
            _context = context;
        }

        // GET: api/ad
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdDto>>> GetAds()
        {
            var list = await _context.Ads
                .Select(a => new AdDto
                {
                    Id = a.Id,
                    CinemaId = a.CinemaId,
                    Title = a.Title,
                    ClientName = a.ClientName,
                    MediaUrl = a.MediaUrl,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    Placement = a.Placement,
                    Price = a.Price,
                    Status = a.Status,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/ad/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdDto>> GetAd(long id)
        {
            var a = await _context.Ads.FindAsync(id);
            if (a == null)
                return NotFound();

            var dto = new AdDto
            {
                Id = a.Id,
                CinemaId = a.CinemaId,
                Title = a.Title,
                ClientName = a.ClientName,
                MediaUrl = a.MediaUrl,
                StartDate = a.StartDate,
                EndDate = a.EndDate,
                Placement = a.Placement,
                Price = a.Price,
                Status = a.Status,
                CreatedAt = a.CreatedAt
            };

            return Ok(dto);
        }

        // POST: api/ad
        [HttpPost]
        public async Task<ActionResult<AdDto>> CreateAd([FromBody] AdDto dto)
        {
            if (dto == null)
                return BadRequest("Request body is missing or invalid.");

            var entity = new Ad
            {
                CinemaId = dto.CinemaId,
                Title = dto.Title,
                ClientName = dto.ClientName,
                MediaUrl = dto.MediaUrl,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Placement = dto.Placement,
                Price = dto.Price,
                Status = dto.Status,
                CreatedAt = DateTime.UtcNow
            };

            _context.Ads.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            dto.CreatedAt = entity.CreatedAt;

            return CreatedAtAction(nameof(GetAd), new { id = dto.Id }, dto);
        }

        // PUT: api/ad/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAd(long id, [FromBody] AdDto dto)
        {
            if (dto == null || id != dto.Id)
                return BadRequest();

            var entity = await _context.Ads.FindAsync(id);
            if (entity == null)
                return NotFound();

            entity.CinemaId = dto.CinemaId;
            entity.Title = dto.Title;
            entity.ClientName = dto.ClientName;
            entity.MediaUrl = dto.MediaUrl;
            entity.StartDate = dto.StartDate;
            entity.EndDate = dto.EndDate;
            entity.Placement = dto.Placement;
            entity.Price = dto.Price;
            entity.Status = dto.Status;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/ad/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAd(long id)
        {
            var entity = await _context.Ads.FindAsync(id);
            if (entity == null)
                return NotFound();

            _context.Ads.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
