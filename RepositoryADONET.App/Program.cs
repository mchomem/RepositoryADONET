var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(configuration);
services.AddTransient(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
services.AddTransient<ICustomerRepository, CustomerRepository>();

var serviceProvider = services.BuildServiceProvider();

await InsertAsync();
await UpdateAsync();
await ReadAllAsync();

async Task ReadAllAsync()
{
    var repository = serviceProvider.GetRequiredService<ICustomerRepository>();
    var customers = await repository.GetAllAsync();
    int lengthSpaceColumn = 20;
    var header = string.Join("".PadRight(lengthSpaceColumn, ' '), new List<string>() { "ID", "NAME", "EMAIL" });

    Console.WriteLine(header);

    foreach (var customer in customers)
    {
        Console.Write($"{customer.Id}".PadRight(lengthSpaceColumn, ' '));
        Console.Write($"{customer.Name}".PadRight(lengthSpaceColumn, ' '));
        Console.Write($"{customer.Email}".PadRight(lengthSpaceColumn, ' '));
        Console.WriteLine();
    }
}

async Task InsertAsync()
{
    var repository = serviceProvider.GetRequiredService<ICustomerRepository>();
    var customer = new Customer($"John Doe {Guid.NewGuid()}", "john.doe@email.com");
    await repository.InsertAsync(customer);
}

async Task UpdateAsync()
{
    var repository = serviceProvider.GetRequiredService<ICustomerRepository>();
    var customer = await repository.GetByIdAsync(2);    
    customer.Update("Jane Doe 2", customer.Email);

    await repository.UpdateAsync(customer);
}
