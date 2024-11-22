using System;
using System.ComponentModel.DataAnnotations;

namespace DropWeightBackend.API.DTOs
{
    public class WorkoutScheduleRequest
    {
        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public int WorkoutId { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
