using Xunit;
using AutoMapper;
using DropWeightBackend.Api.Configuration;
using DropWeightBackend.Api.DTOs;
using DropWeightBackend.Domain.Entities;
using DropWeightBackend.Domain.Enums;

namespace DropWeightBackend.Tests.Configuration
{
    public class MappingProfileTests
    {
        private readonly IMapper _mapper;

        public MappingProfileTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
        }

        

        [Fact]
        public void GeoSpatialToGeoSpatialDto_ShouldMapCorrectly()
        {
            // Arrange
            var geoSpatial = new GeoSpatial
            {
                GeoSpatialId = 1,
                Latitude = 40,
                Longitude = 50,
                WorkoutId = 1
            };

            // Act
            var dto = _mapper.Map<GeoSpatialDto>(geoSpatial);

            // Assert
            Assert.Equal(geoSpatial.Latitude, dto.Latitude);
            Assert.Equal(geoSpatial.Longitude, dto.Longitude);
        }

        [Fact]
        public void CreateGeoSpatialDtoToGeoSpatial_ShouldMapCorrectly()
        {
            // Arrange
            var createDto = new CreateGeoSpatialDto
            {
                Latitude = 40,
                Longitude = 50,
                WorkoutId = 1
            };

            // Act
            var geoSpatial = _mapper.Map<GeoSpatial>(createDto);

            // Assert
            Assert.Equal(createDto.Latitude, geoSpatial.Latitude);
            Assert.Equal(createDto.Longitude, geoSpatial.Longitude);
            Assert.Equal(createDto.WorkoutId, geoSpatial.WorkoutId);
        }

        [Fact]
        public void UpdateGeoSpatialDtoToGeoSpatial_ShouldMapCorrectly()
        {
            // Arrange
            var updateDto = new UpdateGeoSpatialDto
            {
                GeoSpatialId = 1,
                Latitude = 40,
                Longitude = 50
            };

            // Act
            var geoSpatial = _mapper.Map<GeoSpatial>(updateDto);

            // Assert
            Assert.Equal(updateDto.GeoSpatialId, geoSpatial.GeoSpatialId);
            Assert.Equal(updateDto.Latitude, geoSpatial.Latitude);
            Assert.Equal(updateDto.Longitude, geoSpatial.Longitude);
        }

        [Fact]
        public void GoalToGoalDto_ShouldMapCorrectly()
        {
            // Arrange
            var goal = new Goal
            {
                GoalId = 1,
                Type = GoalType.Custom,
                GoalName = "Test Goal",
                Description = "Test Description",
                StartingValue = 100,
                CurrentValue = 90,
                TargetValue = 80,
                Progress = 20,
                IsAchieved = false,
                UserId = 1
            };

            // Act
            var dto = _mapper.Map<GoalDto>(goal);

            // Assert
            Assert.Equal(goal.Type, dto.Type);
            Assert.Equal(goal.GoalName, dto.GoalName);
            Assert.Equal(goal.Description, dto.Description);
            Assert.Equal(goal.StartingValue, dto.StartingValue);
            Assert.Equal(goal.CurrentValue, dto.CurrentValue);
            Assert.Equal(goal.TargetValue, dto.TargetValue);
            Assert.Equal(goal.IsAchieved, dto.IsAchieved);
        }

        [Fact]
        public void LoginRequestToUser_ShouldMapCorrectly()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Username = "testuser",
                Password = "testpass"
            };

            // Act
            var user = _mapper.Map<User>(loginRequest);

            // Assert
            Assert.Equal(loginRequest.Username, user.Username);
        }

        [Fact]
        public void RegisterRequestToUser_ShouldMapCorrectly()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Username = "testuser",
                Password = "testpass",
                FirstName = "Test",
                LastName = "User"
            };

            // Act
            var user = _mapper.Map<User>(registerRequest);

            // Assert
            Assert.Equal(registerRequest.Username, user.Username);
            Assert.Equal(registerRequest.FirstName, user.FirstName);
            Assert.Equal(registerRequest.LastName, user.LastName);
        }

        [Fact]
        public void WorkoutToWorkoutDto_ShouldMapCorrectly()
        {
            // Arrange
            var workout = new Workout
            {
                WorkoutId = 1,
                Type = WorkoutType.Run,
                StartTime = 1000,
                EndTime = 2000,
                Reps = 10,
                UserId = 1,
                GeoSpatials = new List<GeoSpatial>
                {
                    new GeoSpatial { GeoSpatialId = 1, Latitude = 40, Longitude = 50 }
                }
            };

            // Act
            var dto = _mapper.Map<WorkoutDto>(workout);

            // Assert
            Assert.Equal(workout.WorkoutId, dto.WorkoutId);
            Assert.Equal(workout.Type, dto.Type);
            Assert.Equal(workout.StartTime, dto.StartTime);
            Assert.Equal(workout.EndTime, dto.EndTime);
            Assert.Equal(workout.Reps, dto.Reps);
            Assert.NotNull(dto.GeoSpatials);
            Assert.Single(dto.GeoSpatials);
        }

        [Fact]
        public void CreateWorkoutDtoToWorkout_ShouldMapCorrectly()
        {
            // Arrange
            var createDto = new CreateWorkoutDto
            {
                StartTime = 1000,
                EndTime = 2000,
                Type = WorkoutType.Run,
                Reps = 10,
                UserId = 1
            };

            // Act
            var workout = _mapper.Map<Workout>(createDto);

            // Assert
            Assert.Equal(createDto.StartTime, workout.StartTime);
            Assert.Equal(createDto.EndTime, workout.EndTime);
            Assert.Equal(createDto.Type, workout.Type);
            Assert.Equal(createDto.Reps, workout.Reps);
            Assert.Equal(createDto.UserId, workout.UserId);
        }

        [Fact]
        public void UpdateWorkoutDtoToWorkout_ShouldMapCorrectly()
        {
            // Arrange
            var updateDto = new UpdateWorkoutDto
            {
                WorkoutId = 1,
                StartTime = 1000,
                EndTime = 2000,
                Type = WorkoutType.Run,
                Reps = 10
            };

            // Act
            var workout = _mapper.Map<Workout>(updateDto);

            // Assert
            Assert.Equal(updateDto.WorkoutId, workout.WorkoutId);
            Assert.Equal(updateDto.StartTime, workout.StartTime);
            Assert.Equal(updateDto.EndTime, workout.EndTime);
            Assert.Equal(updateDto.Type, workout.Type);
            Assert.Equal(updateDto.Reps, workout.Reps);
        }
    }
} 