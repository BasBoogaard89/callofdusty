namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class StatsController(IStatsService statsService) : ControllerBase
{
    //[HttpPost("GetAllFiltered")]
    //[ProducesResponseType(typeof(List<RoomDto>), (int)HttpStatusCode.OK)]
    //public async Task<IActionResult> GetAllFiltered([FromBody] RoomFilterDto filters)
    //{
    //    var data = await statsService.GetAllFiltered(filters);

    //    return Ok(data);
    //}
}
