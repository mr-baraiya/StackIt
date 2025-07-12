using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackItAPIs.Models;

namespace StackItAPIs.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly StackItContext _context;

        public AnswerController(StackItContext context)
        {
            _context = context;
        }

        #region Get All Answers
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Answer>>> GetAll()
        {
            try
            {
                var answers = await _context.Answers
                    .Include(a => a.User)
                    .Include(a => a.Question)
                    .OrderByDescending(a => a.CreatedAt)
                    .ToListAsync();

                return Ok(answers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving answers.", Error = ex.Message });
            }
        }
        #endregion

        #region Get Answer by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Answer>> GetById(int id)
        {
            try
            {
                var answer = await _context.Answers
                    .Include(a => a.User)
                    .Include(a => a.Question)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (answer == null)
                    return NotFound(new { Message = $"Answer with ID {id} not found." });

                return Ok(answer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving answer.", Error = ex.Message });
            }
        }
        #endregion

        #region Create Answer
        [HttpPost]
        public async Task<ActionResult<Answer>> Create(Answer answer)
        {
            try
            {
                answer.CreatedAt = DateTime.UtcNow;
                answer.UpdatedAt = DateTime.UtcNow;
                answer.VoteScore ??= 0;
                answer.IsAccepted ??= false;

                await _context.Answers.AddAsync(answer);
                await _context.SaveChangesAsync();

                return Ok(answer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating answer.", Error = ex.Message });
            }
        }
        #endregion

        #region Update Answer
        [HttpPut("{id}")]
        public async Task<ActionResult<Answer>> Update(int id, Answer updatedAnswer)
        {
            if (id != updatedAnswer.Id)
                return BadRequest(new { Message = "Answer ID mismatch." });

            try
            {
                var answer = await _context.Answers.FindAsync(id);
                if (answer == null)
                    return NotFound(new { Message = $"Answer with ID {id} not found." });

                answer.Content = updatedAnswer.Content;
                answer.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Ok(answer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error updating answer.", Error = ex.Message });
            }
        }
        #endregion

        #region Delete Answer
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var answer = await _context.Answers.FindAsync(id);
                if (answer == null)
                    return NotFound(new { Message = $"Answer with ID {id} not found." });

                _context.Answers.Remove(answer);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Answer deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error deleting answer.", Error = ex.Message });
            }
        }
        #endregion

        #region Filter Answers
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Answer>>> Filter(
            [FromQuery] int? questionId,
            [FromQuery] int? userId)
        {
            try
            {
                var query = _context.Answers.AsQueryable();

                if (questionId != null)
                    query = query.Where(a => a.QuestionId == questionId);

                if (userId != null)
                    query = query.Where(a => a.UserId == userId);

                var result = await query
                    .Include(a => a.User)
                    .Include(a => a.Question)
                    .OrderByDescending(a => a.CreatedAt)
                    .ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error filtering answers.", Error = ex.Message });
            }
        }
        #endregion

        #region Mark as Accepted
        [HttpPatch("accept/{id}")]
        public async Task<ActionResult> MarkAsAccepted(int id)
        {
            try
            {
                var answer = await _context.Answers.FindAsync(id);
                if (answer == null)
                    return NotFound(new { Message = $"Answer with ID {id} not found." });

                // Unaccept all other answers for this question
                var otherAnswers = await _context.Answers
                    .Where(a => a.QuestionId == answer.QuestionId)
                    .ToListAsync();

                foreach (var a in otherAnswers)
                    a.IsAccepted = false;

                // Accept this one
                answer.IsAccepted = true;
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Answer marked as accepted." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error marking answer as accepted.", Error = ex.Message });
            }
        }
        #endregion

    }
}
