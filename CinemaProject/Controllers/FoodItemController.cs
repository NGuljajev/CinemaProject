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
    public class FoodItemController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public FoodItemController(CinemaDbContext context)
        {
            _context = context;
        }

        // GET: api/fooditem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodItemDto>>> GetFoodItems()
        {
            var list = await _context.FoodItems
                .Select(f => new FoodItemDto
                {
                    Id = f.Id,
                    CinemaId = f.CinemaId,
                    Name = f.Name,
                    Description = f.Description,
                    Price = f.Price,
                    IsVegetarian = f.IsVegetarian,
                    IsActive = f.IsActive,
                    CreatedAt = f.CreatedAt
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/fooditem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodItemDto>> GetFoodItem(int id)
        {
            var f = await _context.FoodItems.FindAsync(id);
            if (f == null) return NotFound();

            var dto = new FoodItemDto
            {
                Id = f.Id,
                CinemaId = f.CinemaId,
                Name = f.Name,
                Description = f.Description,
                Price = f.Price,
                IsVegetarian = f.IsVegetarian,
                IsActive = f.IsActive,
                CreatedAt = f.CreatedAt
            };

            return Ok(dto);
        }

        // POST: api/fooditem
        [HttpPost]
        public async Task<ActionResult<FoodItemDto>> CreateFoodItem([FromBody] FoodItemDto dto)
        {
            if (dto == null) return BadRequest();

            var entity = new FoodItem
            {
                CinemaId = dto.CinemaId,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                IsVegetarian = dto.IsVegetarian,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.FoodItems.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            dto.CreatedAt = entity.CreatedAt;

            return CreatedAtAction(nameof(GetFoodItem), new { id = dto.Id }, dto);
        }

        // PUT: api/fooditem/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFoodItem(int id, [FromBody] FoodItemDto dto)
        {
            if (dto == null || id != dto.Id) return BadRequest();

            var entity = await _context.FoodItems.FindAsync(id);
            if (entity == null) return NotFound();

            entity.CinemaId = dto.CinemaId;
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.Price = dto.Price;
            entity.IsVegetarian = dto.IsVegetarian;
            entity.IsActive = dto.IsActive;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/fooditem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodItem(int id)
        {
            var entity = await _context.FoodItems.FindAsync(id);
            if (entity == null) return NotFound();

            _context.FoodItems.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
