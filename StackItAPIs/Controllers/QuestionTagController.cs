using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackItAPIs.Models;

namespace StackItAPIs.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuestionTagController : ControllerBase
    {
        private readonly StackItContext _context;

        public QuestionTagController(StackItContext context)
        {
            _context = context;
        }

        #region Get All Question-Tag Links
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<QuestionTag>>> GetAll()
        {
            try
            {
                var questionTags = await _context.QuestionTags.ToListAsync();
                return Ok(questionTags);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving question-tags.", Error = ex.Message });
            }
        }
        #endregion

        #region Get By ID
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionTag>> GetById(int id)
        {
            try
            {
                var link = await _context.QuestionTags.FindAsync(id);
                if (link == null)
                    return NotFound(new { Message = $"QuestionTag with ID {id} not found." });

                return Ok(link);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving record.", Error = ex.Message });
            }
        }
        #endregion

        #region Create Link (Assign Tag to Question)
        [HttpPost]
        public async Task<ActionResult<QuestionTag>> Create(QuestionTag questionTag)
        {
            try
            {
                questionTag.CreatedAt = DateTime.UtcNow;

                await _context.QuestionTags.AddAsync(questionTag);
                await _context.SaveChangesAsync();

                return Ok(questionTag);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating question-tag link.", Error = ex.Message });
            }
        }
        #endregion

        #region Delete Link by ID
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var link = await _context.QuestionTags.FindAsync(id);
                if (link == null)
                    return NotFound(new { Message = $"QuestionTag with ID {id} not found." });

                _context.QuestionTags.Remove(link);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Question-tag link deleted." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error deleting question-tag link.", Error = ex.Message });
            }
        }
        #endregion

        #region Filter by QuestionId or TagId
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<QuestionTag>>> Filter(
            [FromQuery] int? questionId,
            [FromQuery] int? tagId)
        {
            try
            {
                var query = _context.QuestionTags.AsQueryable();

                if (questionId != null)
                    query = query.Where(qt => qt.QuestionId == questionId);

                if (tagId != null)
                    query = query.Where(qt => qt.TagId == tagId);

                var result = await query.ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error filtering question-tags.", Error = ex.Message });
            }
        }
        #endregion

    }
}
