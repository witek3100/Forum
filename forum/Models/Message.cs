using System.ComponentModel.DataAnnotations;

namespace Forum.Models;

public class Message
{
    [Key]
    public int id { get; set; }

    public int senderId { get; set; }

    public int receiverId { get; set; }

    public string content { get; set; } = default!;
}