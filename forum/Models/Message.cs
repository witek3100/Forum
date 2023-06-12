using System.ComponentModel.DataAnnotations;

namespace Forum.Models;

public class Message
{
    [Key]
    public int id { get; set; }
    
    [EmailAddress]
    public string senderEmail { get; set; }
    
    [EmailAddress]
    public string receiverEmail { get; set; }
    
    public string content { get; set; } = default!;
}