using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Models;

public class Message
{
    [Key]
    public int id { get; set; }
    
    [ForeignKey("User")]
    [EmailAddress]
    public string senderEmail { get; set; }
    
    [ForeignKey("User")]
    [EmailAddress]
    public string receiverEmail { get; set; }
    
    public string content { get; set; } = default!;
    
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
    public DateTime createdAt { get; set; } = DateTime.Now;
    
}