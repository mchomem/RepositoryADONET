using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepositoryADONET.App.Models;
using RepositoryADONET.App.Repositories;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(configuration);
services.AddTransient<CustomerRepository>();

var serviceProvider = services.BuildServiceProvider();

var repository = serviceProvider.GetRequiredService<CustomerRepository>();
var customers = await repository.GetAllAsync(nameof(Customer));

int lengthSpaceColumn = 20;
var header = string.Join("".PadRight(lengthSpaceColumn, ' '), new List<string>() { "ID", "NAME", "EMAIL" });
Console.WriteLine(header);

foreach (var customer in customers)
{
    Console.Write($"{customer.Id}".PadRight(lengthSpaceColumn, ' '));
    Console.Write($"{customer.Name}".PadRight(lengthSpaceColumn, ' '));
    Console.Write($"{customer.Email}".PadRight(lengthSpaceColumn, ' '));
}
