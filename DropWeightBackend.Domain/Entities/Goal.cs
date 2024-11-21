using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DropWeightBackend.Domain.Enums;
using Microsoft.Identity.Client;

namespace DropWeightBackend.Domain.Entities;

public class Goal {

    [Key]
    public int GoalId {get; set;}

    [Required]
    public GoalType Type { get; set; } = GoalType.Custom;
    public string? GoalName {get; set;}
    public double? Progress {get; set;}

    public bool? IsAchieved {get; set;}
    public string? Description {get; set; }

    public double? StartingValue {get; set;}
    public double? TargetValue { get; set; }
    public double? CurrentValue { get; set; }

    //Foreign Key
    public int UserId {get; set;}

    [JsonIgnore]
    public User? User {get; set;} //Navigation Property

}
