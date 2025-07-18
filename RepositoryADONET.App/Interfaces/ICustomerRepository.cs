namespace RepositoryADONET.App.Interfaces;

public interface ICustomerRepository
{
    Task<Customer> GetByIdAsync(int id);
    Task<List<Customer>> GetAllAsync();
    Task<int> InsertAsync(Customer entity);
    Task<int> UpdateAsync(Customer entity);
    Task<int> DeleteAsync(Customer entity);
}
