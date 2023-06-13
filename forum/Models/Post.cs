using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Models;

public class Post
{
    [Key]
    public int id { get; set; }

    [ForeignKey("User")]
    public int userId { get; set; }

    public string title { get; set; } = default!;

    public string content { get; set; } = default!;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
    public DateTime createdAt { get; set; } = DateTime.Now;
    
    [ForeignKey("Tag")]
    public int tagId { get; set; }
    
    public int likes { get; set; } = 0;

    public int dislikes { get; set; } = 0;
}