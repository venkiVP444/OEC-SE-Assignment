using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using RL.Data;
using RL.Data.DataModels;

namespace RL.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class PlanProcedureController : ControllerBase
{
    private readonly ILogger<PlanProcedureController> _logger;
    private readonly RLContext _context;

    public PlanProcedureController(ILogger<PlanProcedureController> logger, RLContext context)
    {
        _logger = logger;
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    // Get all plan procedures (OData)
    [HttpGet]
    [EnableQuery]
    public IEnumerable<PlanProcedure> Get()
    {
        return _context.PlanProcedures.Include(pp => pp.Procedure);
    }

    // Assign a user to a procedure
    [HttpPost("assign")]
    public async Task<IActionResult> AssignUser([FromBody] PlanProcedureUser model)
    {
        if (model.PlanProcedureId == 0)
        {
            return BadRequest("PlanProcedureId is required and cannot be 0.");
        }

        model.CreateDate = DateTime.UtcNow;
        model.UpdateDate = DateTime.UtcNow;

        _context.PlanProcedureUsers.Add(model);
        await _context.SaveChangesAsync();

        return Ok(model);
    }


    // Get users assigned to a procedure
    [HttpGet("byProcedure/{planProcedureId}")]
    public async Task<IActionResult> GetUsers(int planProcedureId)
    {
        var users = await _context.PlanProcedureUsers
            .Where(x => x.PlanProcedureId == planProcedureId)
            .Include(x => x.User) 
            .ToListAsync();

        return Ok(users);
    }


    // Remove a single user from a procedure
    [HttpDelete("remove/{id}")]
    public async Task<IActionResult> RemoveUser(int id)
    {
        var entry = await _context.PlanProcedureUsers.FindAsync(id);
        if (entry == null)
            return NotFound();

        _context.PlanProcedureUsers.Remove(entry);
        await _context.SaveChangesAsync();

        return Ok();
    }

    // Remove all users from a procedure
    [HttpDelete("removeAll/{planProcedureId}")]
    public async Task<IActionResult> RemoveAllUsers(int planProcedureId)
    {
        var entries = await _context.PlanProcedureUsers
            .Where(x => x.PlanProcedureId == planProcedureId)
            .ToListAsync();

        if (!entries.Any())
            return Ok();

        _context.PlanProcedureUsers.RemoveRange(entries);
        await _context.SaveChangesAsync();

        return Ok();
    }

    // Get procedures + users (optional)
    [HttpGet("withUsers")]
    public async Task<IActionResult> GetProceduresWithUsers()
    {
        var procedures = await _context.PlanProcedures
            .Include(pp => pp.Procedure)
            .Include(pp => pp.PlanProcedureUsers)
                .ThenInclude(ppu => ppu.User)
            .ToListAsync();

        return Ok(procedures);
    }
}
