using DropWeightBackend.Domain.Enums;

namespace DropWeightBackend.Api.DTOs;

public class GoalDto {

    public GoalType Type { get; set; }
    public string? GoalName {get; set;}

    public bool? IsAchieved { get; set; }
    public string? Description { get; set; }

    public double? StartingValue { get; set; }
    public double? TargetValue { get; set; }
    public double? CurrentValue { get; set; }
    public int UserId {get; set; }
}