using Microsoft.Identity.Client;

namespace DropWeight.Domain.Entities;

public class Goal {

    public int GoalId {get; set;}
    
    public string Type {get; set; } = "";

    public string Description {get; set; } = "";


    //Foreign Key
    public int UserId {get; set;}
}