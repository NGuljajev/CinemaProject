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
    public class EventControllerTests
    {
        private CinemaDbContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<CinemaDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new CinemaDbContext(options);
        }

        [Fact]
        public async Task GetEvents_ReturnsAllItems()
        {
            var context = CreateContext(nameof(GetEvents_ReturnsAllItems));
            context.Events.AddRange(new[]
            {
                new Event { Name="Event1", Description="Desc1", StartDate=DateTime.UtcNow, EndDate=DateTime.UtcNow.AddDays(1), DiscountPercent=10, Active=true, CreatedAt=DateTime.UtcNow },
                new Event { Name="Event2", Description="Desc2", StartDate=DateTime.UtcNow, EndDate=DateTime.UtcNow.AddDays(2), DiscountPercent=20, Active=false, CreatedAt=DateTime.UtcNow }
            });
            await context.SaveChangesAsync();
            var controller = new EventController(context);

            var result = await controller.GetEvents();

            var ok = Assert.IsType<ActionResult<IEnumerable<EventDto>>>(result);
            var items = Assert.IsType<OkObjectResult>(ok.Result).Value as IEnumerable<EventDto>;
            Assert.Equal(2, items.Count());
        }

        [Fact]
        public async Task GetEvent_WhenFound_ReturnsDto()
        {
            var context = CreateContext(nameof(GetEvent_WhenFound_ReturnsDto));
            var entity = new Event
            {
                Name = "Premiere",
                Description = "Big event",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(3),
                DiscountPercent = 15,
                Active = true,
                CreatedAt = DateTime.UtcNow
            };
            context.Events.Add(entity);
            await context.SaveChangesAsync();
            var controller = new EventController(context);

            var action = await controller.GetEvent(entity.Id);

            var ok = Assert.IsType<ActionResult<EventDto>>(action);
            var dto = Assert.IsType<OkObjectResult>(ok.Result).Value as EventDto;
            Assert.Equal(entity.Id, dto.Id);
            Assert.Equal("Premiere", dto.Name);
        }

        [Fact]
        public async Task GetEvent_WhenNotFound_ReturnsNotFound()
        {
            var context = CreateContext(nameof(GetEvent_WhenNotFound_ReturnsNotFound));
            var controller = new EventController(context);

            var action = await controller.GetEvent(999);

            var result = Assert.IsType<ActionResult<EventDto>>(action);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateEvent_AddsEntityAndReturnsCreated()
        {
            var context = CreateContext(nameof(CreateEvent_AddsEntityAndReturnsCreated));
            var controller = new EventController(context);
            var dto = new EventDto
            {
                Name = "NewEvent",
                Description = "Desc",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(5),
                DiscountPercent = 5,
                Active = true
            };

            var action = await controller.CreateEvent(dto);

            var result = Assert.IsType<CreatedAtActionResult>(action.Result);
            var returned = result.Value as EventDto;
            Assert.NotEqual(0, returned.Id);
            Assert.Equal("NewEvent", returned.Name);
            Assert.True((DateTime.UtcNow - returned.CreatedAt).TotalSeconds < 5);

            var saved = await context.Events.FindAsync(returned.Id);
            Assert.NotNull(saved);
        }

        [Fact]
        public async Task UpdateEvent_IdMismatch_ReturnsBadRequest()
        {
            var context = CreateContext(nameof(UpdateEvent_IdMismatch_ReturnsBadRequest));
            var controller = new EventController(context);
            var dto = new EventDto { Id = 1, Name = "Mismatch" };

            var result = await controller.UpdateEvent(2, dto);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateEvent_NotFound_ReturnsNotFound()
        {
            var context = CreateContext(nameof(UpdateEvent_NotFound_ReturnsNotFound));
            var controller = new EventController(context);
            var dto = new EventDto { Id = 1, Name = "DoesNotExist" };

            var result = await controller.UpdateEvent(1, dto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateEvent_ValidUpdate_ReturnsNoContent()
        {
            var context = CreateContext(nameof(UpdateEvent_ValidUpdate_ReturnsNoContent));
            var entity = new Event
            {
                Name = "OldName",
                Description = "OldDesc",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                DiscountPercent = 0,
                Active = false,
                CreatedAt = DateTime.UtcNow
            };
            context.Events.Add(entity);
            await context.SaveChangesAsync();
            var controller = new EventController(context);

            var dto = new EventDto
            {
                Id = entity.Id,
                Name = "UpdatedName",
                Description = "UpdatedDesc",
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                DiscountPercent = 25,
                Active = true
            };

            var result = await controller.UpdateEvent(entity.Id, dto);

            Assert.IsType<NoContentResult>(result);
            var updated = await context.Events.FindAsync(entity.Id);
            Assert.Equal("UpdatedName", updated.Name);
            Assert.Equal(25, updated.DiscountPercent);
            Assert.True(updated.Active);
        }

        [Fact]
        public async Task DeleteEvent_NotFound_ReturnsNotFound()
        {
            var context = CreateContext(nameof(DeleteEvent_NotFound_ReturnsNotFound));
            var controller = new EventController(context);

            var result = await controller.DeleteEvent(123);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteEvent_Exists_RemovesAndReturnsNoContent()
        {
            var context = CreateContext(nameof(DeleteEvent_Exists_RemovesAndReturnsNoContent));
            var entity = new Event
            {
                Name = "DeleteMe",
                Description = "Will be deleted",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(2),
                DiscountPercent = 10,
                Active = false,
                CreatedAt = DateTime.UtcNow
            };
            context.Events.Add(entity);
            await context.SaveChangesAsync();
            var controller = new EventController(context);

            var result = await controller.DeleteEvent(entity.Id);

            Assert.IsType<NoContentResult>(result);
            Assert.Null(await context.Events.FindAsync(entity.Id));
        }
    }
}
