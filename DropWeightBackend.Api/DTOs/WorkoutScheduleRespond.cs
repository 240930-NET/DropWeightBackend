using System;

namespace DropWeightBackend.API.DTOs
{
    public class WorkoutScheduleResponse
    {
        public int WorkoutScheduleId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int WorkoutId { get; set; }
        public string? WorkoutType { get; set; } // Optional: To display workout type
        public int Reps { get; set; } // Optional: Number of repetitions
        public int UserId { get; set; }
    }
}
