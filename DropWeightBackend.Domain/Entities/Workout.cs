using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DropWeightBackend.Domain.Enums;

namespace DropWeightBackend.Domain.Entities
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
        public ICollection<GeoSpatial> GeoSpatials { get; set;} = new HashSet<GeoSpatial>();

        public int UserId {get; set;} //FK
        [JsonIgnore]
        public User? User {get; set;} //Navigation Property

        public WorkoutSchedule? WorkoutSchedule { get; set; }
        //public Workout() {
        //    GeoSpatials = new HashSet<GeoSpatial>();
        //}
        
    }
}


