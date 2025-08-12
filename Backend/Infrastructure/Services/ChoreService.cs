using System;

namespace Infrastructure.Services;

public class ChoreService(
    IServiceProvider sp,
    IMapper mapper,
    IHistoryRepository historyRepository,
    ITextTemplateService textTemplateService) : BaseService<ChoreDto>(sp, mapper), IChoreService
{
    public override async Task<ChoreDto> Save(ChoreDto dto)
    {
        var entity = await repository.GetById(dto.Id) ?? new Chore();

        entity.CategoryId = dto.CategoryId;
        entity.RoomId = dto.RoomId;
        entity.Description = dto.Description;
        entity.DurationMinutes = dto.DurationMinutes;
        entity.FrequencyDays = dto.FrequencyDays;
        entity.DirtinessFactor = dto.DirtinessFactor;

        var data = await repository.Save(entity);

        return mapper.Map<ChoreDto>(data);
    }

    public async Task<List<ChoreDto>> GetAllFiltered(ChoreFilterDto filters)
    {
        var data = await repository.GetAllFiltered(filters);

        return mapper.Map<List<ChoreDto>>(data);
    }

    public async Task<List<ChoreQuestDto>> GetRankedChores()
    {
        List<Chore> allChores = await repository.GetAll();

        var lastCompleted = await historyRepository.GetLastCompletedPerChore();

        var now = DateTime.UtcNow;

        var ranked = allChores
            .Select(chore =>
            {
                var lastDone = lastCompleted.TryGetValue(chore.Id, out var d) ? d : DateTime.MinValue;
                var daysSince = (now - lastDone).TotalDays;
                var urgency = daysSince / (chore.FrequencyDays == 0 ? 1 : chore.FrequencyDays);
                var score = urgency * (int)chore.DirtinessFactor;
                
                return new ChoreQuestDto {
                    Chore = mapper.Map<ChoreDto>(chore),
                    Score = score,
                    Urgency = urgency,
                    DaysSince = daysSince
                };
            })
            .OrderByDescending(x => x.Score)
            .ToList();

        return ranked;
    }

    public async Task<ChoreQuestDto> GetQuest(int themeId)
    {
        var rankedChores = await GetRankedChores();

        var quest = rankedChores.Take(1).ToList();
        await textTemplateService.ApplyDescriptionsBatch(quest, themeId);

        return quest.FirstOrDefault();
    }

    public async Task<List<ChoreQuestDto>> GetAllQuests(int themeId)
    {
        var rankedChores = await GetRankedChores();

        await textTemplateService.ApplyDescriptionsBatch(rankedChores, themeId);

        return rankedChores;
    }
}
