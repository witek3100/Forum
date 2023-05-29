using Azure.Core.Pipeline;

namespace forum.Models;

public class Users
{
    public int id { get; set; }

    public string name { get; set; }

    public string surname { get; set; }

    public string email { get; set; }
    
    public string password_hash { get; set; }
    
    public string role { get; set; }
}