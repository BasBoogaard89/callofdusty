namespace Api.Controllers;

public class TextTemplateController(ITextTemplateService textTemplateService)
    : BaseController<TextTemplateDto, ITextTemplateService>(textTemplateService)
{
}
