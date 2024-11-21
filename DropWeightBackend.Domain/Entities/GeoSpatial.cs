using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DropWeightBackend.Domain.Entities
{
    public class GeoSpatial
    {
        [Key]
        public int GeoSpatialId { get; set; }

        [Required]
        public int Latitude { get; set; }

        [Required]
        public int Longitude { get; set; }

        [Required]
        public int WorkoutId {get; set; } //FK
        [JsonIgnore]
        public Workout? Workout {get; set;} //Navigation Property

        
    }
}


