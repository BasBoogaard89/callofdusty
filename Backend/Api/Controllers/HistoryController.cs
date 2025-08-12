namespace Api.Controllers;

public class HistoryController(IHistoryService historyService)
    : BaseController<HistoryDto, IHistoryService>(historyService)
{
}
