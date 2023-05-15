using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FootballApi.Data;
using FootballApi.Models;

namespace FootballApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly FootballContext _context;

        public TeamsController(FootballContext context)
        {
            _context = context;
        }

        // GET: api/Teams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDTO>>> GetTeams()
        {
            return await _context.Teams
                .Select(t => new TeamDTO
                {
                    ID = t.ID,
                    Name = t.Name,
                    Budget = t.Budget,
                    LeagueName = t.League.Name
                })
                .ToListAsync();
        }

        //GET: api/Teams/IncPlayers
        [HttpGet("IncPlayers")]
        public async Task<ActionResult<IEnumerable<TeamDTO>>> GetTeamsIncPlayers()
        {
            var teamDTOs = await _context.Teams
                .Include(t => t.PlayerTeams).ThenInclude(p => p.Player)
                .Select(t => new TeamDTO
                {
                    ID = t.ID,
                    Name = t.Name,
                    Budget = t.Budget,
                    Players = t.PlayerTeams.Select(c => new PlayerDTO {
                        ID = c.PlayerId,
                        FirstName = c.Player.FirstName,
                        LastName = c.Player.LastName,
                    }).ToList(),
                    Playertotal = t.PlayerTeams.Select(c => new PlayerDTO
                    { ID = c.PlayerId}).ToList().Count,
                    LeagueName = t.League.Name
                })
                .ToListAsync();

                if (teamDTOs.Count() > 0)
                {
                    return teamDTOs;
                }
                else
                {
                    return NotFound(new { message = "Error: No Team records for that Player." });
                }

        }
        // GET: api/Teams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDTO>> GetTeam(int id)
        {
            var teamDTO = await _context.Teams
                .Include(t => t.PlayerTeams)
                .Select(t => new TeamDTO
                {
                    ID = t.ID,
                    Name = t.Name,
                    Budget = t.Budget,
                    LeagueID = t.LeagueID,
                    Players = t.PlayerTeams.Select(c => new PlayerDTO
                    {
                        ID = c.PlayerId,
                        FirstName = c.Player.FirstName,
                        LastName = c.Player.LastName,
                    }).ToList(),
                    Playertotal = t.PlayerTeams.Select(c => new PlayerDTO
                    { ID = c.PlayerId }).ToList().Count,

                    LeagueName = t.League.Name
                })
                .FirstOrDefaultAsync(x => x.ID == id);

            if (teamDTO == null)
            {
                return NotFound(new { message = "No Teams exists with that ID" });
            }

            return teamDTO;
        }

        // PUT: api/Teams/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(int id, TeamDTO teamDTO)
        {
            if (id != teamDTO.ID)
            {
                return BadRequest(new { message = "Error: no id for that Team." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var teamToUpdate = await _context.Teams.FindAsync(id);
            if (teamToUpdate == null)
            {
                return NotFound(new { message = "Error: team not found." });
            }

            teamToUpdate.ID = teamDTO.ID;
            teamToUpdate.Name = teamDTO.Name;
            teamToUpdate.Budget = teamDTO.Budget;
            teamToUpdate.LeagueID = teamDTO.League.ID;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
                {
                    return Conflict(new { message = "Concurrency Error: Team has been Removed." });
                }
                else
                {
                    return Conflict(new { message = "Concurrency Error: Team has been updated by another user.  Back out and try editing the record again." });
                }
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { Message = "Unable to save changes to the database.Try again, and if the problem persists see your system administrator." + ex.Message });
            }



        }

        // POST: api/Teams
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TeamDTO>> PostTeam(TeamDTO teamDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Team team = new Team
            {
                ID = teamDTO.ID,
                Name = teamDTO.Name,
                Budget = teamDTO.Budget,
                LeagueID = teamDTO.LeagueID,
                League = new League
                {
                    ID = teamDTO.League.ID,
                    Name = teamDTO.League.Name,
                }
            };
            try
            {
                _context.Teams.Add(team);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetTeam", new { id = team.ID }, teamDTO);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { Message = "Unable to save changes to the database.Try again, and if the problem persists see your system administrator." + ex.Message });
            }
           
        }

        // DELETE: api/Teams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound(new { message = "Delete Error: Team has already been removed." });
            }
            try
            {
                _context.Teams.Remove(team);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException )
            {
                return BadRequest(new { message = "Delete Error: Unable to delete Team." });
            }
           
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.ID == id);
        }
    }
}
