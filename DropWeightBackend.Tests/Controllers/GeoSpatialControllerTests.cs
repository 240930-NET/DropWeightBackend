using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using DropWeightBackend.Api.Controllers;
using DropWeightBackend.Api.Services.Interfaces;
using DropWeightBackend.Api.DTOs;

namespace DropWeightBackend.Tests
{
    public class GeoSpatialControllerTests
    {
        private readonly Mock<IGeoSpatialService> _mockGeoSpatialService;
        private readonly GeoSpatialController _controller;

        public GeoSpatialControllerTests()
        {
            _mockGeoSpatialService = new Mock<IGeoSpatialService>();
            _controller = new GeoSpatialController(_mockGeoSpatialService.Object);
        }

        [Fact]
        public async Task GetGeoSpatialById_ShouldReturnOk_WhenExists()
        {
            // Arrange
            var geoSpatialDto = new GeoSpatialDto {};
            _mockGeoSpatialService.Setup(service => service.GetGeoSpatialByIdAsync(1))
                .ReturnsAsync(geoSpatialDto);

            // Act
            var result = await _controller.GetGeoSpatialById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedGeoSpatial = Assert.IsType<GeoSpatialDto>(okResult.Value);
            //Assert.Equal(geoSpatialDto.GeoSpatialId, returnedGeoSpatial.GeoSpatialId);
        }

        [Fact]
        public async Task GetGeoSpatialById_ShouldReturnNotFound_WhenDoesNotExist()
        {
            // Arrange
            _mockGeoSpatialService.Setup(service => service.GetGeoSpatialByIdAsync(999))
                .ReturnsAsync((GeoSpatialDto)null);

            // Act
            var result = await _controller.GetGeoSpatialById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAllGeoSpatials_ShouldReturnOk_WithGeoSpatials()
        {
            // Arrange
            var geoSpatials = new List<GeoSpatialDto>
            {
                new GeoSpatialDto(),
                new GeoSpatialDto()
            };
            _mockGeoSpatialService.Setup(service => service.GetAllGeoSpatialsAsync())
                .ReturnsAsync(geoSpatials);

            // Act
            var result = await _controller.GetAllGeoSpatials();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedGeoSpatials = Assert.IsAssignableFrom<IEnumerable<GeoSpatialDto>>(okResult.Value);
            Assert.Equal(2, returnedGeoSpatials.Count());
        }

        [Fact]
        public async Task GetGeoSpatialsByWorkoutId_ShouldReturnOk_WithGeoSpatials()
        {
            // Arrange
            var workoutId = 1;
            var geoSpatials = new List<GeoSpatialDto>
            {
                new GeoSpatialDto {}
            };
            _mockGeoSpatialService.Setup(service => service.GetGeoSpatialsByWorkoutIdAsync(workoutId))
                .ReturnsAsync(geoSpatials);

            // Act
            var result = await _controller.GetGeoSpatialsByWorkoutId(workoutId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedGeoSpatials = Assert.IsAssignableFrom<IEnumerable<GeoSpatialDto>>(okResult.Value);
            Assert.Single(returnedGeoSpatials);
        }

        [Fact]
        public async Task AddGeoSpatial_ShouldReturnCreatedAtAction_WhenModelIsValid()
        {
            // Arrange
            var createDto = new CreateGeoSpatialDto { WorkoutId = 1 };
            _mockGeoSpatialService.Setup(service => service.AddGeoSpatialAsync(createDto))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddGeoSpatial(createDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            //Assert.Equal(nameof(GeoSpatialController.GetGeoSpatialById), createdAtActionResult.ActionName);
            //Assert.Equal(createDto.WorkoutId, createdAtActionResult.RouteValues["id"]);
        }

        [Fact]
        public async Task AddGeoSpatial_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var createDto = new CreateGeoSpatialDto();
            _controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await _controller.AddGeoSpatial(createDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateGeoSpatial_ShouldReturnNoContent_WhenModelIsValid()
        {
            // Arrange
            var updateDto = new UpdateGeoSpatialDto { GeoSpatialId = 1 };
            _mockGeoSpatialService.Setup(service => service.UpdateGeoSpatialAsync(updateDto))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateGeoSpatial(updateDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateGeoSpatial_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var updateDto = new UpdateGeoSpatialDto();
            _controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await _controller.UpdateGeoSpatial(updateDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteGeoSpatial_ShouldReturnNoContent()
        {
            // Arrange
            _mockGeoSpatialService.Setup(service => service.DeleteGeoSpatialAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteGeoSpatial(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
} 