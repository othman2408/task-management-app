using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.Models;

public class Category
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string? Description { get; set; }
    
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
}