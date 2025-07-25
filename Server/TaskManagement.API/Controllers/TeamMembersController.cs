using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Data;
using TeamMember = TaskManagement.API.Models.TeamMember;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamMembersController : ControllerBase
{
    private readonly TaskManagementContext _context;

    public TeamMembersController(TaskManagementContext context)
    {
        _context = context;
    }

    // GET: api/teammembers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamMember>>> GetTeamMembers()
    {
        return await _context.TeamMembers.ToListAsync();
    }

    // GET: api/teammembers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TeamMember>> GetTeamMember(int id)
    {
        var teamMember = await _context.TeamMembers.FindAsync(id);

        if (teamMember == null)
        {
            return NotFound();
        }

        return teamMember;
    }

    // POST: api/teammembers
    [HttpPost]
    public async Task<ActionResult<TeamMember>> CreateTeamMember(TeamMember teamMember)
    {
        _context.TeamMembers.Add(teamMember);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTeamMember), new { id = teamMember.Id }, teamMember);
    }

    // PUT: api/teammembers/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeamMember(int id, TeamMember teamMember)
    {
        if (id != teamMember.Id)
        {
            return BadRequest();
        }

        _context.Entry(teamMember).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TeamMemberExists(id))
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

    // DELETE: api/teammembers/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeamMember(int id)
    {
        var teamMember = await _context.TeamMembers.FindAsync(id);
        if (teamMember == null)
        {
            return NotFound();
        }

        // Check if team member has assigned tasks
        if (await _context.Tasks.AnyAsync(t => t.AssignedToId == id))
        {
            return BadRequest("Cannot delete team member with assigned tasks.");
        }

        _context.TeamMembers.Remove(teamMember);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TeamMemberExists(int id)
    {
        return _context.TeamMembers.Any(e => e.Id == id);
    }
}