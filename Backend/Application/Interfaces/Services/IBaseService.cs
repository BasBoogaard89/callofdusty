namespace Application.Interfaces.Services;

public interface IBaseService<T> where T : BaseDto
{
    Task<List<T>> GetAll();
    Task<T> GetById(int id);
    Task<T> Save(T dto);
    Task<bool> Delete(int id);
}