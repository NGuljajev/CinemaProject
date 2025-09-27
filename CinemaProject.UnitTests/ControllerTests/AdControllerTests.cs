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
    public class AdControllerTests
    {
        private CinemaDbContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<CinemaDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new CinemaDbContext(options);
        }

        [Fact]
        public async Task GetAds_ReturnsAllItems()
        {
            var context = CreateContext(nameof(GetAds_ReturnsAllItems));
            context.Ads.AddRange(new[]
            {
                new Ad { Title="Ad1", ClientName="Client1", MediaUrl="url1", StartDate=DateTime.UtcNow, EndDate=DateTime.UtcNow.AddDays(1), Placement="Screen", Price=100, Status="Active", CreatedAt=DateTime.UtcNow },
                new Ad { Title="Ad2", ClientName="Client2", MediaUrl="url2", StartDate=DateTime.UtcNow, EndDate=DateTime.UtcNow.AddDays(2), Placement="Lobby", Price=200, Status="Inactive", CreatedAt=DateTime.UtcNow }
            });
            await context.SaveChangesAsync();
            var controller = new AdController(context);

            var result = await controller.GetAds();

            var ok = Assert.IsType<ActionResult<IEnumerable<AdDto>>>(result);
            var items = Assert.IsType<OkObjectResult>(ok.Result).Value as IEnumerable<AdDto>;
            Assert.Equal(2, items.Count());
        }

        [Fact]
        public async Task GetAd_WhenFound_ReturnsDto()
        {
            var context = CreateContext(nameof(GetAd_WhenFound_ReturnsDto));
            var entity = new Ad
            {
                Title = "AdTest",
                ClientName = "Client",
                MediaUrl = "url",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Placement = "Placement",
                Price = 50,
                Status = "Active",
                CreatedAt = DateTime.UtcNow
            };
            context.Ads.Add(entity);
            await context.SaveChangesAsync();
            var controller = new AdController(context);

            var action = await controller.GetAd(entity.Id);

            var ok = Assert.IsType<ActionResult<AdDto>>(action);
            var dto = Assert.IsType<OkObjectResult>(ok.Result).Value as AdDto;
            Assert.Equal(entity.Id, dto.Id);
            Assert.Equal("AdTest", dto.Title);
        }

        [Fact]
        public async Task GetAd_WhenNotFound_ReturnsNotFound()
        {
            var context = CreateContext(nameof(GetAd_WhenNotFound_ReturnsNotFound));
            var controller = new AdController(context);

            var action = await controller.GetAd(123);

            var result = Assert.IsType<ActionResult<AdDto>>(action);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateAd_AddsEntityAndReturnsCreated()
        {
            var context = CreateContext(nameof(CreateAd_AddsEntityAndReturnsCreated));
            var controller = new AdController(context);
            var dto = new AdDto
            {
                Title = "NewAd",
                ClientName = "Client",
                MediaUrl = "url",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(2),
                Placement = "Screen",
                Price = 150,
                Status = "Active"
            };

            var action = await controller.CreateAd(dto);

            var result = Assert.IsType<CreatedAtActionResult>(action.Result);
            var returned = result.Value as AdDto;
            Assert.NotEqual(0, returned.Id);
            Assert.Equal("NewAd", returned.Title);
            Assert.True((DateTime.UtcNow - returned.CreatedAt).TotalSeconds < 5);

            var saved = await context.Ads.FindAsync(returned.Id);
            Assert.NotNull(saved);
        }

        [Fact]
        public async Task UpdateAd_IdMismatch_ReturnsBadRequest()
        {
            var context = CreateContext(nameof(UpdateAd_IdMismatch_ReturnsBadRequest));
            var controller = new AdController(context);
            var dto = new AdDto { Id = 1 };

            var result = await controller.UpdateAd(2, dto);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateAd_NotFound_ReturnsNotFound()
        {
            var context = CreateContext(nameof(UpdateAd_NotFound_ReturnsNotFound));
            var controller = new AdController(context);
            var dto = new AdDto { Id = 1 };

            var result = await controller.UpdateAd(1, dto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateAd_ValidUpdate_ReturnsNoContent()
        {
            var context = CreateContext(nameof(UpdateAd_ValidUpdate_ReturnsNoContent));
            var entity = new Ad
            {
                Title = "OldTitle",
                ClientName = "Client",
                MediaUrl = "url",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Placement = "Lobby",
                Price = 100,
                Status = "Active",
                CreatedAt = DateTime.UtcNow
            };
            context.Ads.Add(entity);
            await context.SaveChangesAsync();
            var controller = new AdController(context);

            var dto = new AdDto
            {
                Id = entity.Id,
                Title = "UpdatedTitle",
                ClientName = "NewClient",
                MediaUrl = "newurl",
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Placement = "UpdatedPlacement",
                Price = 200,
                Status = "Inactive"
            };

            var result = await controller.UpdateAd(entity.Id, dto);

            Assert.IsType<NoContentResult>(result);
            var updated = await context.Ads.FindAsync(entity.Id);
            Assert.Equal("UpdatedTitle", updated.Title);
            Assert.Equal("NewClient", updated.ClientName);
        }

        [Fact]
        public async Task DeleteAd_NotFound_ReturnsNotFound()
        {
            var context = CreateContext(nameof(DeleteAd_NotFound_ReturnsNotFound));
            var controller = new AdController(context);

            var result = await controller.DeleteAd(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteAd_Exists_RemovesAndReturnsNoContent()
        {
            var context = CreateContext(nameof(DeleteAd_Exists_RemovesAndReturnsNoContent));
            var entity = new Ad
            {
                Title = "DeleteMe",
                ClientName = "Client",
                MediaUrl = "url",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Placement = "Screen",
                Price = 99,
                Status = "Active",
                CreatedAt = DateTime.UtcNow
            };
            context.Ads.Add(entity);
            await context.SaveChangesAsync();
            var controller = new AdController(context);

            var result = await controller.DeleteAd(entity.Id);

            Assert.IsType<NoContentResult>(result);
            Assert.Null(await context.Ads.FindAsync(entity.Id));
        }
    }
}
