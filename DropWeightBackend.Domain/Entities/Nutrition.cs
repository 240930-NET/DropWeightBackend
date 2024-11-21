using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DropWeightBackend.Domain.Entities
{
    public class Nutrition
    {
        [Key]
        public int NutritionId {get; set; }
        [Required]
        public DateTime Date { get; set; }
        
        //I made the nutrition following the api output from this api: https://calorieninjas.com/
        [Required]
        public required string Description {get; set; } 
        [Required]
        public double ServingSize {get; set;}
        [Required]
        public double Calories {get; set;}
        public double TotalFat {get; set;}
        public double SaturatedFat {get; set;}
        public double Cholesterol {get; set;}
        public double Sodium {get; set;}
        public double Carbohydrates {get; set;}
        public double Fiber {get; set;}
        public double Sugar {get; set;}
        public double Protein {get; set;}

        [Required]
        public int UserId {get; set;} //FK
        [JsonIgnore]
        public User? User {get; set;} //Navigation property (not sure we need it but its here)

    }
}
