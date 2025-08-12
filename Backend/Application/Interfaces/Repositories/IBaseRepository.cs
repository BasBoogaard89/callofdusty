namespace Application.Interfaces.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<List<T>> GetAll();
    Task<T> GetById(int id);
    Task<T> Save(T updatedEntity);
    Task<bool> Delete(int id);
}
