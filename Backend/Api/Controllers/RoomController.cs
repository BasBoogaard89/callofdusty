namespace Api.Controllers;

public class RoomController(IRoomService roomService)
    : BaseController<RoomDto, IRoomService>(roomService)
{
    [HttpPost("GetAllFiltered")]
    [ProducesResponseType(typeof(List<RoomDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllFiltered([FromBody] RoomFilterDto filters)
    {
        var data = await roomService.GetAllFiltered(filters);

        return Ok(data);
    }
}
