namespace Application.Dtos;

public class ChoreQuestDto
{
    public ChoreDto Chore { get; set; } = null!;
    public string QuestDescription { get; set; } = string.Empty;
    public string ChoreDescription { get; set; } = string.Empty;
    public string RoomDescription { get; set; } = string.Empty;

    public double Score { get; set; }
    public double Urgency { get; set; }
    public double DaysSince { get; set; }
}
