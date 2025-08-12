namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public abstract class BaseController<TDto, TService>(TService service) : ControllerBase
    where TDto : BaseDto
    where TService : IBaseService<TDto>
{
    [HttpGet]
    public virtual async Task<IActionResult> GetAll()
    {
        var data = await service.GetAll();

        return Ok(data);
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetById(int id)
    {
        var data = await service.GetById(id);

        return Ok(data);
    }

    [HttpPost]
    public virtual async Task<IActionResult> Save([FromBody] TDto dto)
    {
        var data = await service.Save(dto);

        return Ok(data);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public virtual async Task<IActionResult> Delete(int id)
    {
        var result = await service.Delete(id);

        return Ok(result);
    }
}
