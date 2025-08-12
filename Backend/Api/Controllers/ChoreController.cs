namespace Api.Controllers;

public class ChoreController(IChoreService choreService)
    : BaseController<ChoreDto, IChoreService>(choreService)
{
    [HttpPost("GetAllFiltered")]
    [ProducesResponseType(typeof(List<ChoreDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllFiltered([FromBody] ChoreFilterDto filters)
    {
        var data = await choreService.GetAllFiltered(filters);

        return Ok(data);
    }

    [HttpGet("Quest")]
    [ProducesResponseType(typeof(ChoreQuestDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetQuest([FromQuery] int themeId)
    {
        var data = await choreService.GetQuest(themeId);

        return Ok(data);
    }

    [HttpGet("AllQuests")]
    [ProducesResponseType(typeof(List<ChoreQuestDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllQuests([FromQuery] int themeId)
    {
        var data = await choreService.GetAllQuests(themeId);

        return Ok(data);
    }
}
