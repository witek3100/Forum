using System.ComponentModel.DataAnnotations;

namespace Forum.Models;

public class Tag
{
    [Key]
    public int id { get; set; }

    public string name { get; set; } = default!;
}