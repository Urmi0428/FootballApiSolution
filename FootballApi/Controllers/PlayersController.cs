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
    public class PlayersController : ControllerBase
    {
        private readonly FootballContext _context;

        public PlayersController(FootballContext context)
        {
            _context = context;
        }

        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDTO>>> GetPlayers()
        {
            return await _context.Players
                .Select(l => new PlayerDTO
                {
                    ID = l.ID,
                    FirstName = l.FirstName,
                    LastName = l.LastName,
                    Jersey = l.Jersey,
                    DOB = l.DOB,
                    FeePaid = l.FeePaid,
                    EMail = l.EMail,
                    RowVersion = l.RowVersion
                })
                .ToListAsync();
        }

        // GET: api/Players/IncTeams
        [HttpGet("IncTeams")]
        public async Task<ActionResult<IEnumerable<PlayerDTO>>> GetPlayersIncTeams()
        {
                return await _context.Players
                .Include(p => p.PlayerTeams).ThenInclude(t => t.Team)
                .Select(l => new PlayerDTO
                {
                       ID = l.ID,
                       FirstName = l.FirstName,
                       LastName = l.LastName,
                       Jersey = l.Jersey,
                       DOB = l.DOB,
                       FeePaid = l.FeePaid,
                       EMail = l.EMail,
                       RowVersion = l.RowVersion,
                       Teams = l.PlayerTeams.Select( c => new TeamDTO {
                            ID = c.TeamId,
                            Name = c.Team.Name
                       }).ToList(),
                       Teamtotal = l.PlayerTeams.Select(c => new TeamDTO
                       { ID = c.TeamId }).ToList().Count
                })
               .ToListAsync();

                //if (playerDTOs.Count() > 0)
                //{
                //    return playerDTOs;
                //}
                //else
                //{
                //    return NotFound(new { message = "Error: No Player records for that Team." });
                //}
        }
        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDTO>> GetPlayer(int id)
        {
            var playerDTO = await _context.Players
                        .Include(p => p.PlayerTeams)
                        .Select(l => new PlayerDTO
                        {
                            ID = l.ID,
                            FirstName = l.FirstName,
                            LastName = l.LastName,
                            Jersey = l.Jersey,
                            DOB = l.DOB,
                            FeePaid = l.FeePaid,
                            EMail = l.EMail,
                            RowVersion = l.RowVersion,
                            Teams = l.PlayerTeams.Select(c => new TeamDTO
                            {
                                ID = c.TeamId,
                                Name = c.Team.Name
                            }).ToList(),
                            Teamtotal = l.PlayerTeams.Select(c => new TeamDTO
                            { ID = c.TeamId }).ToList().Count
                        })
                        .FirstOrDefaultAsync(x => x.ID == id); 

            if (playerDTO == null)
            {
                return NotFound(new { message = "No league exists with that ID" });
            }

            return playerDTO;
        }

        // PUT: api/Players/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(int id, PlayerDTO playerDTO)
        {
            if (id != playerDTO.ID)
            {
                return BadRequest(new { message = "Error: no id for that Player." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var playerToUpdate = await _context.Players.FindAsync(id);

            if (playerToUpdate == null)
            {
                return NotFound(new { message = "Error: player not found." });
            }
            //concurrency check
            if (playerDTO.RowVersion != null)
            {
                if (!playerToUpdate.RowVersion.SequenceEqual(playerDTO.RowVersion))
                {
                    return Conflict(new { message = "Concurrency Error: Player has been changed by another user.  Try editing the record again." });
                }
            }


            playerToUpdate.ID = playerDTO.ID;
            playerToUpdate.FirstName = playerDTO.FirstName;
            playerToUpdate.LastName = playerDTO.LastName;
            playerToUpdate.Jersey = playerDTO.Jersey;
            playerToUpdate.DOB = playerDTO.DOB;
            playerToUpdate.FeePaid = playerDTO.FeePaid;
            playerToUpdate.EMail = playerDTO.EMail;
            playerToUpdate.RowVersion = playerDTO.RowVersion;

            //Put the original RowVersion value in the OriginalValues collection for the entity
            _context.Entry(playerToUpdate).Property("RowVersion").OriginalValue = playerDTO.RowVersion;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
                {
                    return Conflict(new { message = "Concurrency Error: Player has been Removed." });
                }
                else
                {
                    return Conflict(new { message = "Concurrency Error: Player has been updated by another user.  Back out and try editing the record again." });
                }
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { Message = "Unable to save changes to the database.Try again, and if the problem persists see your system administrator." + ex.Message });
            }

        }

        // POST: api/Players
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PlayerDTO>> PostPlayer(PlayerDTO playerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Player player = new Player
            {
                ID = playerDTO.ID,
                FirstName = playerDTO.FirstName,
                LastName = playerDTO.LastName,
                Jersey = playerDTO.Jersey,
                DOB = playerDTO.DOB,
                FeePaid = playerDTO.FeePaid,
                EMail = playerDTO.EMail,
                RowVersion = playerDTO.RowVersion
            };
            try
            {
                _context.Players.Add(player);
                await _context.SaveChangesAsync();
                //Assign Database Generated values back into the DTO
                playerDTO.ID = player.ID;
                playerDTO.RowVersion = player.RowVersion;

                return CreatedAtAction("GetPlayer", new { id = player.ID }, playerDTO);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { Message = "Unable to save changes to the database.Try again, and if the problem persists see your system administrator." + ex.Message });
            }

        }

        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound(new { message = "Delete Error: Player has already been removed." });
            }
            try
            {
                _context.Players.Remove(player);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException )
            {
                return BadRequest(new { message = "Delete Error: Unable to delete Player." });
            }
            
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.ID == id);
        }
    }
}
