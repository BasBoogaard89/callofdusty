namespace Application.Interfaces.Services;

public interface ITextTemplateService : IBaseService<TextTemplateDto>
{
    Task ApplyDescriptionsBatch(List<ChoreQuestDto> quests, int themeId);
}
