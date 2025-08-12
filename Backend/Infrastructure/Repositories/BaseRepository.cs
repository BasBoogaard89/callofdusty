namespace Infrastructure.Repositories;

public class BaseRepository<T>(AppDbContext context) : IBaseRepository<T> where T : BaseEntity
{
    public async Task<List<T>> GetAll()
    {
        return await context.Set<T>().ToListAsync();
    }

    public async Task<T> GetById(int id)
    {
        var entity = await context.Set<T>().FindAsync(id);

        return entity;
    }

    public async Task<T> Save(T updatedEntity)
    {
        EntityEntry entry;

        if (updatedEntity.Id == 0)
        {
            entry = await context.Set<T>().AddAsync(updatedEntity);
        } else
        {
            entry = context.Set<T>().Update(updatedEntity);
        }

        await SaveChanges();

        return (T)entry.Entity;
    }

    public async Task<bool> Delete(int id)
    {
        var entity = await context.Set<T>().FindAsync(id);

        if (entity != null)
        {
            context.Set<T>().Remove(entity);
            await SaveChanges();
        }

        return entity != null;
    }

    public async Task SaveChanges()
    {
        await context.SaveChangesAsync();
    }
}
