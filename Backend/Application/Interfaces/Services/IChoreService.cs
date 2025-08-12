namespace Application.Interfaces.Services;

public interface IChoreService : IBaseService<ChoreDto>
{
    Task<List<ChoreDto>> GetAllFiltered(ChoreFilterDto filters);
    Task<List<ChoreQuestDto>> GetRankedChores();
    Task<ChoreQuestDto> GetQuest(int themeId);
    Task<List<ChoreQuestDto>> GetAllQuests(int themeId);
}
