using System;

namespace DropWeightBackend.Domain.Entities
{
    public class WorkoutSchedule
    {
        public int WorkoutScheduleId { get; set; }
        
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int WorkoutId { get; set; }
        public Workout? Workout { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
