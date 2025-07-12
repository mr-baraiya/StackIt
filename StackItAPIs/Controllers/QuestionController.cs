using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackItAPIs.Models;

namespace StackItAPIs.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly StackItContext _context;

        public QuestionController(StackItContext context)
        {
            _context = context;
        }

        #region Get All Questions
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Question>>> GetAllQuestions()
        {
            try
            {
                var questions = await _context.Questions
                    .Include(q => q.User)
                    .Include(q => q.QuestionTags)
                    .ThenInclude(qt => qt.Tag)
                    .ToListAsync();

                return Ok(questions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving questions.", Error = ex.Message });
            }
        }
        #endregion

        #region Get Question by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestionById(int id)
        {
            try
            {
                var question = await _context.Questions
                    .Include(q => q.User)
                    .Include(q => q.Answers)
                    .Include(q => q.QuestionTags)
                    .ThenInclude(qt => qt.Tag)
                    .FirstOrDefaultAsync(q => q.Id == id);

                if (question == null)
                    return NotFound(new { Message = $"Question with ID {id} not found." });

                return Ok(question);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving question.", Error = ex.Message });
            }
        }
        #endregion

        #region Create Question
        [HttpPost]
        public async Task<ActionResult<Question>> CreateQuestion(Question question)
        {
            try
            {
                question.CreatedAt = DateTime.UtcNow;
                question.UpdatedAt = DateTime.UtcNow;
                question.ViewCount ??= 0;
                question.VoteScore ??= 0;
                question.AnswerCount ??= 0;
                question.IsClosed ??= false;

                await _context.Questions.AddAsync(question);
                await _context.SaveChangesAsync();

                return Ok(question);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating question.", Error = ex.Message });
            }
        }
        #endregion

        #region Update Question
        [HttpPut("{id}")]
        public async Task<ActionResult<Question>> UpdateQuestion(int id, Question updatedQuestion)
        {
            if (id != updatedQuestion.Id)
                return BadRequest(new { Message = "Question ID mismatch." });

            try
            {
                var question = await _context.Questions.FindAsync(id);
                if (question == null)
                    return NotFound(new { Message = $"Question with ID {id} not found." });

                question.Title = updatedQuestion.Title;
                question.Description = updatedQuestion.Description;
                question.IsClosed = updatedQuestion.IsClosed;
                question.ClosedReason = updatedQuestion.ClosedReason;
                question.AcceptedAnswerId = updatedQuestion.AcceptedAnswerId;
                question.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Ok(question);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error updating question.", Error = ex.Message });
            }
        }
        #endregion

        #region Delete Question
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuestion(int id)
        {
            try
            {
                var question = await _context.Questions.FindAsync(id);
                if (question == null)
                    return NotFound(new { Message = $"Question with ID {id} not found." });

                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Question deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error deleting question.", Error = ex.Message });
            }
        }
        #endregion

        #region Filter Questions (by title or user)
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Question>>> FilterQuestions(
            [FromQuery] string? title,
            [FromQuery] int? userId)
        {
            try
            {
                var query = _context.Questions.AsQueryable();

                if (!string.IsNullOrWhiteSpace(title))
                    query = query.Where(q => q.Title!.ToLower().Contains(title.ToLower()));

                if (userId != null)
                    query = query.Where(q => q.UserId == userId);

                var result = await query
                    .Include(q => q.User)
                    .Include(q => q.QuestionTags).ThenInclude(qt => qt.Tag)
                    .OrderByDescending(q => q.CreatedAt)
                    .ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error filtering questions.", Error = ex.Message });
            }
        }
        #endregion

    }
}
