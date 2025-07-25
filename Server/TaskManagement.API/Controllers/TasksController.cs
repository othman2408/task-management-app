using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Data;
using Task = TaskManagement.API.Models.Task;


namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly TaskManagementContext _context;

    public TasksController(TaskManagementContext context)
    {
        _context = context;
    }

    // GET: api/tasks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetTasks()
    {
        var tasks = await _context.Tasks
            .Include(t => t.Category)
            .Include(t => t.AssignedTo)
            .Select(t => new
            {
                t.Id,
                t.Title,
                t.Description,
                t.CreatedDate,
                t.DueDate,
                t.IsCompleted,
                t.CategoryId,
                t.AssignedToId,
                Category = t.Category != null ? new { t.Category.Id, t.Category.Name, t.Category.Description } : null,
                AssignedTo = t.AssignedTo != null ? new { t.AssignedTo.Id, t.AssignedTo.Name, t.AssignedTo.Email } : null
            })
            .ToListAsync();

        return tasks;
    }

    // GET: api/tasks/5
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetTask(int id)
    {
        var task = await _context.Tasks
            .Include(t => t.Category)
            .Include(t => t.AssignedTo)
            .Where(t => t.Id == id)
            .Select(t => new
            {
                t.Id,
                t.Title,
                t.Description,
                t.CreatedDate,
                t.DueDate,
                t.IsCompleted,
                t.CategoryId,
                t.AssignedToId,
                Category = t.Category != null ? new { t.Category.Id, t.Category.Name, t.Category.Description } : null,
                AssignedTo = t.AssignedTo != null ? new { t.AssignedTo.Id, t.AssignedTo.Name, t.AssignedTo.Email } : null
            })
            .FirstOrDefaultAsync();

        if (task == null)
        {
            return NotFound();
        }

        return task;
    }
    
    // GET: api/tasks/search
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<object>>> SearchTasks(
        [FromQuery] bool? isCompleted = null,
        [FromQuery] int? categoryId = null,
        [FromQuery] int? assignedToId = null)
    {
        var query = _context.Tasks
            .Include(t => t.Category)
            .Include(t => t.AssignedTo)
            .AsQueryable();

        if (isCompleted.HasValue)
            query = query.Where(t => t.IsCompleted == isCompleted.Value);

        if (categoryId.HasValue)
            query = query.Where(t => t.CategoryId == categoryId.Value);

        if (assignedToId.HasValue)
            query = query.Where(t => t.AssignedToId == assignedToId.Value);

        var tasks = await query
            .Select(t => new
            {
                t.Id,
                t.Title,
                t.Description,
                t.CreatedDate,
                t.DueDate,
                t.IsCompleted,
                t.CategoryId,
                t.AssignedToId,
                Category = t.Category != null ? new { t.Category.Id, t.Category.Name, t.Category.Description } : null,
                AssignedTo = t.AssignedTo != null ? new { t.AssignedTo.Id, t.AssignedTo.Name, t.AssignedTo.Email } : null
            })
            .ToListAsync();

        return tasks;
    }
    
    // POST: api/tasks
    [HttpPost]
    public async Task<ActionResult<Task>> CreateTask(Task task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    // PUT: api/tasks/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, Task task)
    {
        if (id != task.Id)
        {
            return BadRequest();
        }

        _context.Entry(task).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TaskExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/tasks/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TaskExists(int id)
    {
        return _context.Tasks.Any(e => e.Id == id);
    }
}
