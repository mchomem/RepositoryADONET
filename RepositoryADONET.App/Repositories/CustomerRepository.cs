namespace RepositoryADONET.App.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IRepositoryBase<Customer> _repositoryBase;

    public CustomerRepository(IRepositoryBase<Customer> repositoryBase)
    {
        _repositoryBase = repositoryBase;
    }

    public async Task<int> DeleteAsync(Customer entity)
    {
        var result = await _repositoryBase.DeleteAsync(entity);
        return result;
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        var customers = await _repositoryBase.GetAllAsync();
        return customers;
    }

    public async Task<Customer> GetByIdAsync(int id)
    {
        var customer = await _repositoryBase.GetrByIdAsync(id);
        return customer;
    }

    public async Task<int> InsertAsync(Customer entity)
    {
        var result = await _repositoryBase.InserAsync(entity);
        return result;
    }

    public async Task<int> UpdateAsync(Customer entity)
    {
        var result = await _repositoryBase.UpdateAsync(entity);
        return result;
    }
}
