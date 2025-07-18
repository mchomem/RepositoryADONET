namespace RepositoryADONET.App.Interfaces;

public interface IRepositoryBase<T> where T: class, new()
{
    T MapDataBaseToEntity(SqlDataReader reader);
    Task<T> GetrByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    Task<int> InserAsync(T entity);
    Task<int> UpdateAsync(T entity);
    Task<int> DeleteAsync(T entity);
}
