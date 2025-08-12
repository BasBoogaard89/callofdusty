namespace Infrastructure.Repositories;

public class ThemeRepository(AppDbContext context) : BaseRepository<Theme>(context), IThemeRepository
{
}
