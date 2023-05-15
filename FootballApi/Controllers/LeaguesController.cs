using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FootballApi.Data;
using FootballApi.Models;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.X86;
using System.Threading.Channels;

namespace FootballApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaguesController : ControllerBase
    {
        private readonly FootballContext _context;

        public LeaguesController(FootballContext context)
        {
            _context = context;
        }

        // GET: api/Leagues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeagueDTO>>> GetLeagues()
        {
            return await _context.Leagues
                .Select(l => new LeagueDTO
                {
                    ID = l.ID,
                    Name = l.Name,
                })
                .ToListAsync();
        }

        //GET: api/Leagues/Teaminc
        [HttpGet("TeamsInc")]
        public async Task<ActionResult<IEnumerable<LeagueDTO>>> GetLeagueTeamInc()
        {
            return await _context.Leagues
                 .Include(_t => _t.Teams)
                 .Select(l => new LeagueDTO
                 {
                     ID = l.ID,
                     Name = l.Name,
                     Teamtotal = l.Teams.Count,
                     Teams = l.Teams.Select(lTeam => new TeamDTO
                     {
                         ID = lTeam.ID,
                         Name = lTeam.Name,
                         Budget = lTeam.Budget, 
                     }).ToList()
                 })
                 .ToListAsync();
        }

        // GET: api/Leagues/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LeagueDTO>> GetLeague(string id)
        {
            var league = await _context.Leagues
                .Include(t => t.Teams)
                .Select(l => new LeagueDTO
                {
                    ID = l.ID,
                    Name = l.Name,
                    Teamtotal = l.Teams.Count,
                    Teams = l.Teams.Select(lTeam => new TeamDTO
                    {
                        ID = lTeam.ID,
                        Name = lTeam.Name,
                        Budget = lTeam.Budget,
                        
                    }).ToList()
                })
                .FirstOrDefaultAsync(l =>l.ID == id);

            if (league == null)
            {
                return NotFound(new { message = "Error: League record not found" });
            }

            return league;
        }


        // PUT: api/Leagues/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeague(string id, LeagueDTO leagueDTO)
        {
            if (id != leagueDTO.ID)
            {
                return BadRequest(new { message = "Error: ID does not match League" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Get the record you want to update
            var leagueToUpdate = await _context.Leagues.FindAsync(id);

            //Check that you got it
            if (leagueToUpdate == null)
            {
                return NotFound(new { message = "Error: League record not found." });
            }

            leagueToUpdate.ID = leagueDTO.ID;
            leagueToUpdate.Name = leagueDTO.Name;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeagueExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { Message = "Unable to save changes to the database.Try again, and if the problem persists see your system administrator." + ex.Message });
            }
          


        }

        // POST: api/Leagues
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LeagueDTO>> PostLeague(LeagueDTO leagueDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            League league = new League
            { 
                ID=leagueDTO.ID,
                Name=leagueDTO.Name,
                
            };
            try
            {
                _context.Leagues.Add(league);
                await _context.SaveChangesAsync();
                //Assign Database Generated values back into the DTO
                leagueDTO.ID = league.ID;
                return CreatedAtAction(nameof(GetLeague), new { id = league.ID }, leagueDTO);
            }
            catch (DbUpdateException)
            {
                return BadRequest(new {Message = "Unable to save changes to the database.Try again, and if the problem persists see your system administrator." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Unable to save changes to the database." + ex.Message });
            }

        }

        // DELETE: api/Leagues/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeague(string id)
        {
            var league = await _context.Leagues.FindAsync(id);
            if (league == null)
            {
                return NotFound(new { message = "Delete Error: League has already been removed." });
            }
            try 
            {
                _context.Leagues.Remove(league);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Delete Error: Unable to delete League." });
            }
        }

        private bool LeagueExists(string id)
        {
            return _context.Leagues.Any(e => e.ID == id);
        }
    }
}
