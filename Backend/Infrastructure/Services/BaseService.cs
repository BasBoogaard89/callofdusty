namespace Infrastructure.Services;

public class BaseService<T> : IBaseService<T> where T : BaseDto
{
    protected readonly dynamic repository;
    private readonly Type entityType;
    private readonly IMapper mapper;

    public BaseService(IServiceProvider sp, IMapper mapper)
    {
        this.mapper = mapper;

        var dtoType = typeof(T);
        var entityName = dtoType.Name.Replace("Dto", "");
        var fullEntityName = $"Domain.Entities.{entityName}";
        entityType = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .FirstOrDefault(x => x.Name == entityName && x.Namespace == "Domain.Entities")
            ?? throw new InvalidOperationException($"Entity type '{entityName}' not found");

        var repoType = typeof(IBaseRepository<>).MakeGenericType(entityType);
        repository = sp.GetRequiredService(repoType);
    }

    public virtual async Task<List<T>> GetAll()
    {
        var data = await repository.GetAll();

        return mapper.Map<List<T>>(data);
    }

    public virtual async Task<T> GetById(int id)
    {
        var data = await repository.GetById(id);

        return mapper.Map<T>(data);
    }

    public virtual async Task<T> Save(T dto)
    {
        var entity = mapper.Map(dto, typeof(T), entityType);

        var data = await repository.Save(entity);

        return mapper.Map<T>(data);
    }

    public virtual async Task<bool> Delete(int id)
    {
        var entity = await GetById(id);

        if (entity == null)
            return false;

        await repository.Delete(id);

        return true;
    }
}