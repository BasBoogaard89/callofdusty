namespace Domain.Entities;

public class History : BaseEntity
{
    public int ChoreId { get; set; }
    public DateTime DateStarted { get; set; }
    public DateTime DateCompleted { get; set; }
}
