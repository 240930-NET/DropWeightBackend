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

    [Required]
    public string Description {get; set; } = "";

    [Required]
    public bool Status;

    public double? TargetWeight { get; set; }
    public double? CurrentWeight { get; set; }

    //Foreign Key
    public int UserId {get; set;}

    [JsonIgnore]
    public required User User {get; set;} //Navigation Property

}
