namespace Infrastructure.Repositories;

public class ChoreCategoryRepository(AppDbContext context) : BaseRepository<ChoreCategory>(context), IChoreCategoryRepository
{
}
