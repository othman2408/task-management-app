using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.Models;

public class TeamMember
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string? Email { get; set; }
    
    public ICollection<Task> AssignedTasks { get; set; } = new List<Task>();
}