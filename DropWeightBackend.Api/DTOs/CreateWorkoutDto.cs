using DropWeightBackend.Domain.Enums;
using DropWeightBackend.Domain.Entities;

namespace DropWeightBackend.Api.DTOs
{
    public class CreateWorkoutDto
    {
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public WorkoutType Type { get; set; }
        public int Reps { get; set; }
        public int UserId { get; set; }
        
        public List<GeoSpatialDto>? GeoSpatials { get; set; } = null;
    }
}
