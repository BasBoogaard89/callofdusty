namespace Application.Interfaces.Repositories;

public interface IHistoryRepository : IBaseRepository<History>
{
    Task<Dictionary<int, DateTime>> GetLastCompletedPerChore();
}
