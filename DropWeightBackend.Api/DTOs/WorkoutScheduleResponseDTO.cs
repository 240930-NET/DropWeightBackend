using System;

namespace DropWeightBackend.Api.DTOs.WorkoutSchedule
{
    public class WorkoutScheduleResponse
    {
        public int WorkoutScheduleId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string WorkoutName { get; set; }  // Optional for better user experience
        public int UserId { get; set; }
    }
}
