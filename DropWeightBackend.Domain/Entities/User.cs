using System.Collections.Generic;


namespace DropWeight.Domain.Entities
{
    public class User
    {
        [Key]
        public int UserId {get; set; }
        public string Username {get; set; }= "";
        public string PasswordHash {get; set; }= "";
        public string PasswordSalt {get;set;} = "";
        public string FirstName {get; set; }= "";
        public string LastName {get;set;} = "";

        public ICollection<Workout> Workouts {get;set;} = new List<Workout>();
        public ICollection<Goal> Goals {get;set;} = new List<Goal>();
        public ICollection<Nutrition> Nutritions = new List<Workout>();
    }
}


