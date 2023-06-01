using System.ComponentModel.DataAnnotations;

namespace Forum.Models;

public class Comment
{
    [Key]
    public int id { get; set; }

    public int userId { get; set; }

    public int postId { get; set; }

    public string content { get; set; } = default!;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
    public DateTime createdAt { get; set; } = DateTime.Now;
}