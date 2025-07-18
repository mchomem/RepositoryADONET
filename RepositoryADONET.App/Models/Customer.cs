namespace RepositoryADONET.App.Models;

public class Customer
{
    public Customer() { }

    public Customer(string name, string email)
    {
        Name = name;
        Email = email;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private  set; }

    public void Update(string name, string email)
    {
        Name = name;
        Email = email;
    }
}
