using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackItAPIs.Models;

namespace StackItAPIs.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly StackItContext _context;

        public VoteController(StackItContext context)
        {
            _context = context;
        }

        #region Get All Votes
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Vote>>> GetAllVotes()
        {
            try
            {
                var votes = await _context.Votes.ToListAsync();
                return Ok(votes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving votes.", Error = ex.Message });
            }
        }
        #endregion

        #region Get Vote by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Vote>> GetVoteById(int id)
        {
            try
            {
                var vote = await _context.Votes.FindAsync(id);
                if (vote == null)
                    return NotFound(new { Message = $"Vote with ID {id} not found." });

                return Ok(vote);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving vote.", Error = ex.Message });
            }
        }
        #endregion

        #region Create Vote
        [HttpPost]
        public async Task<ActionResult<Vote>> CreateVote(Vote vote)
        {
            try
            {
                vote.CreatedAt = DateTime.UtcNow;
                vote.UpdatedAt = DateTime.UtcNow;

                await _context.Votes.AddAsync(vote);
                await _context.SaveChangesAsync();

                return Ok(vote);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating vote.", Error = ex.Message });
            }
        }
        #endregion

        #region Update Vote
        [HttpPut("{id}")]
        public async Task<ActionResult<Vote>> UpdateVote(int id, Vote updatedVote)
        {
            if (id != updatedVote.Id)
                return BadRequest(new { Message = "Vote ID mismatch." });

            try
            {
                var existingVote = await _context.Votes.FindAsync(id);
                if (existingVote == null)
                    return NotFound(new { Message = $"Vote with ID {id} not found." });

                existingVote.VoteType = updatedVote.VoteType;
                existingVote.VotableType = updatedVote.VotableType;
                existingVote.VotableId = updatedVote.VotableId;
                existingVote.UserId = updatedVote.UserId;
                existingVote.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Ok(existingVote);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error updating vote.", Error = ex.Message });
            }
        }
        #endregion

        #region Delete Vote
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVote(int id)
        {
            try
            {
                var vote = await _context.Votes.FindAsync(id);
                if (vote == null)
                    return NotFound(new { Message = $"Vote with ID {id} not found." });

                _context.Votes.Remove(vote);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Vote deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error deleting vote.", Error = ex.Message });
            }
        }
        #endregion

        #region Filter Votes
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Vote>>> FilterVotes(
            [FromQuery] int? userId,
            [FromQuery] string? votableType,
            [FromQuery] int? votableId)
        {
            try
            {
                var query = _context.Votes.AsQueryable();

                if (userId != null)
                    query = query.Where(v => v.UserId == userId);

                if (!string.IsNullOrWhiteSpace(votableType))
                    query = query.Where(v => v.VotableType!.ToLower() == votableType.ToLower());

                if (votableId != null)
                    query = query.Where(v => v.VotableId == votableId);

                var result = await query.OrderByDescending(v => v.CreatedAt).ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error filtering votes.", Error = ex.Message });
            }
        }
        #endregion

    }
}
