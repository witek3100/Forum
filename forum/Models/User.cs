using System.ComponentModel.DataAnnotations;

namespace Forum.Models;

public class User
{
    [Key]
    public int id { get; set; }

    [Display(Name = "Name")]
    public string name { get; set; } = default!;

    public string lastName { get; set; } = default!;

    public string email { get; set; }

    public string passwordHash { get; set; }

    public string token { get; set; }

    public string role { get; set; } = "user";
}