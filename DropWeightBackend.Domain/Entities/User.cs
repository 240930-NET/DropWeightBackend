using System.ComponentModel.DataAnnotations;

namespace DropWeightBackend.Domain.Entities
{
    public class User
    {
        [Key]
        public int UserId {get; set; }
        [Required]
        public string Username {get; set; }= "";
        [Required]
        public string PasswordHash {get; set; }= "";
        [Required]
        public string PasswordSalt {get;set;} = "";
        public string FirstName {get; set; }= "";
        public string LastName {get;set;} = "";

        public ICollection<Workout> Workouts {get;set;} = new List<Workout>();
        public ICollection<Goal> Goals {get;set;} = new List<Goal>();
        public ICollection<Nutrition> Nutritions { get; set; } = new List<Nutrition>();
        public ICollection<WorkoutSchedule> WorkoutSchedules { get; set; } = new List<WorkoutSchedule>();
    }
}


