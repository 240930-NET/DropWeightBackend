using DropWeightBackend.Domain.Enums;

namespace DropWeightBackend.Api.DTOs
{
    public class CreateWorkoutDto
    {
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public WorkoutType Type { get; set; }
        public int Reps { get; set; }
        public int UserId { get; set; }
    }
}
