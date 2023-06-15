using System.ComponentModel.DataAnnotations;

namespace Forum.Models;

public class User
{
    [Key]
    public int id { get; set; }

    [Required]
    [StringLength(30)]
    [Display(Name = "First name: ")]
    public string name { get; set; } = default!;

    [Required]
    [StringLength(30)]
    [Display(Name = "Last name: ")]
    public string lastName { get; set; } = default!;

    [Required]
    [EmailAddress]
    [StringLength(40)]
    [Display(Name = "Email Address: ")]
    public string email { get; set; }

    public string passwordHash { get; set; }

    public string? token { get; set; }

    public string role { get; set; } = "user";
}