using AutoMapper;
using DropWeightBackend.Api.DTOs;
using DropWeightBackend.Domain.Entities;

namespace DropWeightBackend.Api.Configuration {
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GeoSpatial, GeoSpatialDto>().ReverseMap();
            CreateMap<GeoSpatial, CreateGeoSpatialDto>().ReverseMap();
            CreateMap<GeoSpatial, UpdateGeoSpatialDto>().ReverseMap();

            CreateMap<Goal, GoalDto>().ReverseMap();

            CreateMap<User, LoginRequest>().ReverseMap();
            CreateMap<User, RegisterRequest>().ReverseMap();

            CreateMap<Workout, WorkoutDto>().ReverseMap();
            CreateMap<Workout, CreateWorkoutDto>().ReverseMap();
            CreateMap<Workout, UpdateWorkoutDto>().ReverseMap();

        }
    }
}