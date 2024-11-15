using DropWeightBackend.Domain.Enums;

namespace DropWeightBackend.Api.DTOs
{
    public class UpdateWorkoutDto
    {
        public int WorkoutId { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public WorkoutType Type { get; set; }
        public int Reps { get; set; }
    }
}
