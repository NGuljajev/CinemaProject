using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using CinemaBackend.Data;
using CinemaProject.Controllers;
using CinemaProject.Models;
using CinemaProject.Dto;

namespace CinemaProject.UnitTests.ControllerTests
{
    public class FoodItemControllerTests
    {
        private CinemaDbContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<CinemaDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new CinemaDbContext(options);
        }

        [Fact]
        public async Task GetFoodItems_ReturnsAll()
        {
            var context = CreateContext(nameof(GetFoodItems_ReturnsAll));
            context.FoodItems.AddRange(new[]
            {
                new FoodItem { Name="Popcorn", Price=5.5m, IsVegetarian=true, IsActive=true, CreatedAt=DateTime.UtcNow },
                new FoodItem { Name="Hotdog", Price=7.0m, IsVegetarian=false, IsActive=true, CreatedAt=DateTime.UtcNow }
            });
            await context.SaveChangesAsync();
            var controller = new FoodItemController(context);

            var result = await controller.GetFoodItems();

            var ok = Assert.IsType<ActionResult<IEnumerable<FoodItemDto>>>(result);
            var items = Assert.IsType<OkObjectResult>(ok.Result).Value as IEnumerable<FoodItemDto>;
            Assert.Equal(2, items.Count());
        }

        [Fact]
        public async Task GetFoodItem_WhenFound_ReturnsDto()
        {
            var context = CreateContext(nameof(GetFoodItem_WhenFound_ReturnsDto));
            var entity = new FoodItem
            {
                CinemaId = 1,
                Name = "Nachos",
                Description = "Cheesy goodness",
                Price = 6.0m,
                IsVegetarian = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            context.FoodItems.Add(entity);
            await context.SaveChangesAsync();
            var controller = new FoodItemController(context);

            var action = await controller.GetFoodItem(entity.Id);

            var ok = Assert.IsType<ActionResult<FoodItemDto>>(action);
            var dto = Assert.IsType<OkObjectResult>(ok.Result).Value as FoodItemDto;
            Assert.Equal(entity.Id, dto.Id);
            Assert.Equal("Nachos", dto.Name);
        }

        [Fact]
        public async Task GetFoodItem_WhenNotFound_ReturnsNotFound()
        {
            var context = CreateContext(nameof(GetFoodItem_WhenNotFound_ReturnsNotFound));
            var controller = new FoodItemController(context);

            var action = await controller.GetFoodItem(999);

            var result = Assert.IsType<ActionResult<FoodItemDto>>(action);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateFoodItem_Valid_AddsEntityAndReturnsCreated()
        {
            var context = CreateContext(nameof(CreateFoodItem_Valid_AddsEntityAndReturnsCreated));
            var controller = new FoodItemController(context);
            var dto = new FoodItemDto
            {
                CinemaId = 2,
                Name = "Cola",
                Description = "Cold drink",
                Price = 3.0m,
                IsVegetarian = true,
                IsActive = true
            };

            var action = await controller.CreateFoodItem(dto);

            var result = Assert.IsType<CreatedAtActionResult>(action.Result);
            var returned = result.Value as FoodItemDto;
            Assert.NotEqual(0, returned.Id);
            Assert.Equal("Cola", returned.Name);
            Assert.True((DateTime.UtcNow - returned.CreatedAt).TotalSeconds < 5);

            var saved = await context.FoodItems.FindAsync(returned.Id);
            Assert.NotNull(saved);
        }

        [Fact]
        public async Task CreateFoodItem_NullDto_ReturnsBadRequest()
        {
            var context = CreateContext(nameof(CreateFoodItem_NullDto_ReturnsBadRequest));
            var controller = new FoodItemController(context);

            var result = await controller.CreateFoodItem(null);

            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task UpdateFoodItem_IdMismatch_ReturnsBadRequest()
        {
            var context = CreateContext(nameof(UpdateFoodItem_IdMismatch_ReturnsBadRequest));
            var controller = new FoodItemController(context);
            var dto = new FoodItemDto { Id = 1, Name = "Mismatch", Price = 4.0m };

            var result = await controller.UpdateFoodItem(2, dto);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateFoodItem_NullDto_ReturnsBadRequest()
        {
            var context = CreateContext(nameof(UpdateFoodItem_NullDto_ReturnsBadRequest));
            var controller = new FoodItemController(context);

            var result = await controller.UpdateFoodItem(1, null);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateFoodItem_NotFound_ReturnsNotFound()
        {
            var context = CreateContext(nameof(UpdateFoodItem_NotFound_ReturnsNotFound));
            var controller = new FoodItemController(context);
            var dto = new FoodItemDto { Id = 1, Name = "Nonexistent", Price = 10.0m };

            var result = await controller.UpdateFoodItem(1, dto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateFoodItem_ValidUpdate_ReturnsNoContent()
        {
            var context = CreateContext(nameof(UpdateFoodItem_ValidUpdate_ReturnsNoContent));
            var entity = new FoodItem
            {
                Name = "Old Burger",
                Description = "Beef burger",
                Price = 8.0m,
                IsVegetarian = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            context.FoodItems.Add(entity);
            await context.SaveChangesAsync();
            var controller = new FoodItemController(context);

            var dto = new FoodItemDto
            {
                Id = entity.Id,
                CinemaId = entity.CinemaId,
                Name = "Veggie Burger",
                Description = "Healthy option",
                Price = 9.0m,
                IsVegetarian = true,
                IsActive = true
            };

            var result = await controller.UpdateFoodItem(entity.Id, dto);

            Assert.IsType<NoContentResult>(result);
            var updated = await context.FoodItems.FindAsync(entity.Id);
            Assert.Equal("Veggie Burger", updated.Name);
            Assert.True(updated.IsVegetarian);
        }

        [Fact]
        public async Task DeleteFoodItem_NotFound_ReturnsNotFound()
        {
            var context = CreateContext(nameof(DeleteFoodItem_NotFound_ReturnsNotFound));
            var controller = new FoodItemController(context);

            var result = await controller.DeleteFoodItem(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteFoodItem_Exists_RemovesAndReturnsNoContent()
        {
            var context = CreateContext(nameof(DeleteFoodItem_Exists_RemovesAndReturnsNoContent));
            var entity = new FoodItem
            {
                Name = "RemoveMe",
                Price = 5.0m,
                IsVegetarian = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            context.FoodItems.Add(entity);
            await context.SaveChangesAsync();
            var controller = new FoodItemController(context);

            var result = await controller.DeleteFoodItem(entity.Id);

            Assert.IsType<NoContentResult>(result);
            Assert.Null(await context.FoodItems.FindAsync(entity.Id));
        }
    }
}
