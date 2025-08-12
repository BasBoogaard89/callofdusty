namespace Api.Controllers;

public class ThemeController(IThemeService themeService)
    : BaseController<ThemeDto, IThemeService>(themeService)
{
}
