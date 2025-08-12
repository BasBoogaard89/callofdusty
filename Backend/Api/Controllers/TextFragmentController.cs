namespace Api.Controllers;

public class TextFragmentController(ITextFragmentService textFragmentService)
    : BaseController<TextFragmentDto, ITextFragmentService>(textFragmentService)
{
}
