namespace Api.Controllers;

public class RoomCategoryController(IRoomCategoryService roomCategoryService)
    : BaseController<RoomCategoryDto, IRoomCategoryService>(roomCategoryService)
{
}
