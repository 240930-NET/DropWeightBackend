using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GreaterGradesBackend.Domain.Enums;

namespace DropWeight.Domain.Entities
{
    public class Workout
    {
        [Key]
        public int WorkoutId { get; set; }

        [Required]
        public int StartTime { get; set; }

        [Required]
        public int EndTime { get; set; }
        
        [Required]
        public WorkoutType Type { get; set; }
        public int Reps { get; set; }
        public ICollection<GeoSpatial> GeoSpatials { get; set;}

        public Workout() {
            GeoSpatials = new Hashset<GeoSpatial>();
        }
        
    }
}


