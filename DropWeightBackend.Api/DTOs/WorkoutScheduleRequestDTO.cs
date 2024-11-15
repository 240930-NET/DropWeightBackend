using System;

namespace DropWeightBackend.Api.DTOs.WorkoutSchedule
{
    public class WorkoutScheduleRequest
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int WorkoutId { get; set; }
        public int UserId { get; set; }
    }
}
