namespace Api.Controllers;

public class ChoreCategoryController(IChoreCategoryService choreCategoryService)
    : BaseController<ChoreCategoryDto, IChoreCategoryService>(choreCategoryService)
{
}
