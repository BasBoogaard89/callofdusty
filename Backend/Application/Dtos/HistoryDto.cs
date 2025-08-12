namespace Application.Dtos;

public class HistoryDto : BaseDto
{
    [Required]
    public int ChoreId { get; set; }

    [Required]
    public DateTime DateStarted { get; set; }

    [Required]
    public DateTime DateCompleted { get; set; }
}
