using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DropWeight.Domain.Entities
{
    public class GeoSpatial
    {
        [Key]
        public int GeoSpatialId { get; set; }

        [Required]
        public int Latitude { get; set; }

        [Required]
        public int Longitude { get; set; }

        public GeoSpatial() {}
        
    }
}


