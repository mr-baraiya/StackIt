using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackItAPIs.Models;

namespace StackItAPIs.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly StackItContext _context;

        public CommentController(StackItContext context)
        {
            _context = context;
        }

        #region Get All Comments
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAll()
        {
            try
            {
                var comments = await _context.Comments
                    .Include(c => c.User)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync();

                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving comments.", Error = ex.Message });
            }
        }
        #endregion

        #region Get Comment by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetById(int id)
        {
            try
            {
                var comment = await _context.Comments.FindAsync(id);
                if (comment == null)
                    return NotFound(new { Message = $"Comment with ID {id} not found." });

                return Ok(comment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving comment.", Error = ex.Message });
            }
        }
        #endregion

        #region Create Comment
        [HttpPost]
        public async Task<ActionResult<Comment>> Create(Comment comment)
        {
            try
            {
                comment.CreatedAt = DateTime.UtcNow;
                comment.UpdatedAt = DateTime.UtcNow;
                comment.VoteScore ??= 0;

                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();

                return Ok(comment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating comment.", Error = ex.Message });
            }
        }
        #endregion

        #region Update Comment
        [HttpPut("{id}")]
        public async Task<ActionResult<Comment>> Update(int id, Comment updatedComment)
        {
            if (id != updatedComment.Id)
                return BadRequest(new { Message = "Comment ID mismatch." });

            try
            {
                var comment = await _context.Comments.FindAsync(id);
                if (comment == null)
                    return NotFound(new { Message = $"Comment with ID {id} not found." });

                comment.Content = updatedComment.Content;
                comment.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Ok(comment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error updating comment.", Error = ex.Message });
            }
        }
        #endregion

        #region Delete Comment
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var comment = await _context.Comments.FindAsync(id);
                if (comment == null)
                    return NotFound(new { Message = $"Comment with ID {id} not found." });

                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Comment deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error deleting comment.", Error = ex.Message });
            }
        }
        #endregion

        #region Filter Comments
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Comment>>> Filter(
            [FromQuery] int? userId,
            [FromQuery] string? commentableType,
            [FromQuery] int? commentableId)
        {
            try
            {
                var query = _context.Comments.AsQueryable();

                if (userId != null)
                    query = query.Where(c => c.UserId == userId);

                if (!string.IsNullOrWhiteSpace(commentableType))
                    query = query.Where(c => c.CommentableType!.ToLower() == commentableType.ToLower());

                if (commentableId != null)
                    query = query.Where(c => c.CommentableId == commentableId);

                var result = await query
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error filtering comments.", Error = ex.Message });
            }
        }
        #endregion

    }
}
