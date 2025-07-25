using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.Models;

public class Task
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; } = false;
    
    // Foreign keys
    public int? CategoryId { get; set; }
    public int? AssignedToId { get; set; }
    
    // Navigation properties
    public Category? Category { get; set; }
    public TeamMember? AssignedTo { get; set; }
}