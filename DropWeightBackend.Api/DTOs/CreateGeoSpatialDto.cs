namespace DropWeightBackend.Api.DTOs
{
    public class CreateGeoSpatialDto
    {
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        public int WorkoutId { get; set; }
    }
}
