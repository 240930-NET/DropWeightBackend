using System.Collections.Generic;
using DropWeightBackend.Api.DTOs;
using DropWeightBackend.Domain.Enums;

namespace DropWeightBackend.Api.DTOs
{
    public class WorkoutDto
    {
        public int WorkoutId { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public WorkoutType Type { get; set; }
        public int Reps { get; set; }
        public List<GeoSpatialDto> GeoSpatials { get; set; }
    }
}
