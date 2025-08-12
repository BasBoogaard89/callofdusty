using System.Text.RegularExpressions;

namespace Infrastructure.Services;

public class TextTemplateService(
    IServiceProvider sp,
    IMapper mapper,
    ITextFragmentService textFragmentService) : BaseService<TextTemplateDto>(sp, mapper), ITextTemplateService
{
    private static readonly Regex PlaceholderRx = new Regex(@"@(\w+)", RegexOptions.Compiled);

    public override async Task<TextTemplateDto> Save(TextTemplateDto dto)
    {
        var entity = await repository.GetById(dto.Id) ?? new TextTemplate();

        entity.Description = dto.Description;
        entity.ThemeId = dto.ThemeId;
        entity.CategoryType = dto.CategoryType;

        if (dto.CategoryType == CategoryType.Chore || dto.CategoryType == CategoryType.Room)
        {
            entity.CategoryId = dto.CategoryId;
        } else
        {
            entity.CategoryId = 0;
        }

        var data = await repository.Save(entity);

        return mapper.Map<TextTemplateDto>(data);
    }

    //public async Task ApplyDescriptionsToQuest(ChoreQuestDto quest, int themeId)
    //{
    //    quest.QuestDescription = await ResolveTemplate(themeId, CategoryType.Intro);
    //    quest.ChoreDescription = await ResolveTemplate(themeId, CategoryType.Chore, quest.Chore.CategoryId);
    //    quest.RoomDescription = await ResolveTemplate(themeId, CategoryType.Room, quest.Chore.Room.CategoryId);
    //}

    //private async Task<string> ResolveTemplate(int themeId, CategoryType type, int? categoryId = null)
    //{
    //    var template = await repository.GetRandomTextTemplate(themeId, type, categoryId);
    //    var fragments = await textFragmentService.GetAllFiltered(new TextFragmentFilterDto { TextTemplateId = template.Id });

    //    var values = fragments
    //        .GroupBy(f => f.Key)
    //        .ToDictionary(g => g.Key, g => GetRandomValue(g));

    //    return ReplacePlaceholders(template.Description, values);
    //}

    //private static string ReplacePlaceholders(string template, Dictionary<string, string> values)
    //{
    //    return Regex.Replace(template, @"@(\w+)", match =>
    //    {
    //        var key = match.Groups[1].Value;
    //        return values.TryGetValue(key, out var val) ? val : match.Value;
    //    });
    //}

    //private static string GetRandomValue(IEnumerable<TextFragmentDto> options)
    //{
    //    var list = options.ToList();
    //    return list.Count == 0 ? "" : list[new Random().Next(list.Count)].Value;
    //}

    public async Task ApplyDescriptionsBatch(List<ChoreQuestDto> quests, int themeId)
    {
        if (quests == null || quests.Count == 0) return;

        // verzamel categorie-ids
        var choreCatIds = quests.Select(q => q.Chore.CategoryId).Distinct().ToList();
        var roomCatIds = quests.Select(q => q.Chore.Room.CategoryId).Distinct().ToList();

        // batch-templates
        List<TextTemplate> introTpls = await repository.GetAllFiltered(new TextTemplateFilterDto { ThemeId = themeId, CategoryType = CategoryType.Intro });
        List<TextTemplate> choreTpls = await repository.GetAllFiltered(new TextTemplateFilterDto { ThemeId = themeId, CategoryType = CategoryType.Chore, CategoryId = choreCatIds });
        List<TextTemplate> roomTpls = await repository.GetAllFiltered(new TextTemplateFilterDto { ThemeId = themeId, CategoryType = CategoryType.Room, CategoryId = roomCatIds });

        // alle templateIds voor fragmenten
        var allTplIds = introTpls.Select(t => t.Id)
            .Concat(choreTpls.Select(t => t.Id))
            .Concat(roomTpls.Select(t => t.Id))
            .Distinct()
            .ToList();

        var allFrags = allTplIds.Count > 0
            ? await textFragmentService.GetAllFiltered(new TextFragmentFilterDto { TextTemplateIds = allTplIds })
            : new List<TextFragmentDto>();

        // indexen
        var introByCat = introTpls
            .GroupBy(t => t.CategoryId)
            .ToDictionary(g => g.Key, g => g.ToList());
        var choreByCat = choreTpls
            .GroupBy(t => t.CategoryId)
            .ToDictionary(g => g.Key, g => g.ToList());
        var roomByCat = roomTpls
            .GroupBy(t => t.CategoryId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var fragsByTpl = allFrags.GroupBy(f => f.TextTemplateId)
            .ToDictionary(
                g => g.Key,
                g => g.GroupBy(x => x.Key)
                      .ToDictionary(gg => gg.Key, gg => gg.Select(x => x.Value).ToList())
            );

        var rnd = Random.Shared;

        foreach (var q in quests)
        {
            q.QuestDescription = Build(introByCat, fragsByTpl, rnd, 0);
            q.ChoreDescription = Build(choreByCat, fragsByTpl, rnd, q.Chore.CategoryId);
            q.RoomDescription = Build(roomByCat, fragsByTpl, rnd, q.Chore.Room.CategoryId);
        }
    }

    private static string Build(
        Dictionary<int, List<TextTemplate>> tplByCat,
        Dictionary<int, Dictionary<string, List<string>>> fragsByTpl,
        Random rnd,
        int? categoryId)
    {
        var cat = categoryId ?? 0;
        if (!tplByCat.TryGetValue(cat, out var list) || list.Count == 0)
            return string.Empty;

        var tpl = list[rnd.Next(list.Count)];

        if (!fragsByTpl.TryGetValue(tpl.Id, out var fragMap) || fragMap.Count == 0)
            return tpl.Description;

        var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var (key, options) in fragMap)
        {
            values[key] = options.Count == 0 ? "" : options[rnd.Next(options.Count)];
        }

        return ReplacePlaceholders(tpl.Description, values);
    }

    private static string ReplacePlaceholders(string template, Dictionary<string, string> values)
        => PlaceholderRx.Replace(template, m =>
        {
            var key = m.Groups[1].Value;
            return values.TryGetValue(key, out var val) ? val : m.Value;
        });
}
