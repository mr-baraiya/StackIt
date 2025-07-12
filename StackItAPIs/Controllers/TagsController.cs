using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackItAPIs.Models;

namespace StackItAPIs.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly StackItContext _context;

        public TagController(StackItContext context)
        {
            _context = context;
        }

        #region Get All Tags
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Tag>>> GetAllTags()
        {
            try
            {
                var tags = await _context.Tags.ToListAsync();
                return Ok(tags);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving tags.", Error = ex.Message });
            }
        }
        #endregion

        #region Get Tag by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> GetTagById(int id)
        {
            try
            {
                var tag = await _context.Tags.FindAsync(id);
                if (tag == null)
                    return NotFound(new { Message = $"Tag with ID {id} not found." });

                return Ok(tag);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving tag.", Error = ex.Message });
            }
        }
        #endregion

        #region Create Tag
        [HttpPost]
        public async Task<ActionResult<Tag>> CreateTag(Tag tag)
        {
            try
            {
                tag.CreatedAt = DateTime.UtcNow;
                tag.UsageCount ??= 0;

                await _context.Tags.AddAsync(tag);
                await _context.SaveChangesAsync();

                return Ok(tag);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating tag.", Error = ex.Message });
            }
        }
        #endregion

        #region Update Tag
        [HttpPut("{id}")]
        public async Task<ActionResult<Tag>> UpdateTag(int id, Tag updatedTag)
        {
            if (id != updatedTag.Id)
                return BadRequest(new { Message = "Tag ID mismatch." });

            try
            {
                var tag = await _context.Tags.FindAsync(id);
                if (tag == null)
                    return NotFound(new { Message = $"Tag with ID {id} not found." });

                tag.Name = updatedTag.Name;
                tag.Description = updatedTag.Description;
                tag.Color = updatedTag.Color;
                tag.UsageCount = updatedTag.UsageCount;

                await _context.SaveChangesAsync();
                return Ok(tag);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error updating tag.", Error = ex.Message });
            }
        }
        #endregion

        #region Delete Tag
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTag(int id)
        {
            try
            {
                var tag = await _context.Tags.FindAsync(id);
                if (tag == null)
                    return NotFound(new { Message = $"Tag with ID {id} not found." });

                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Tag deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error deleting tag.", Error = ex.Message });
            }
        }
        #endregion

        #region Filter Tags by Name
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Tag>>> FilterTags([FromQuery] string? name)
        {
            try
            {
                var query = _context.Tags.AsQueryable();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    query = query.Where(t => t.Name!.ToLower().Contains(name.ToLower()));
                }

                var result = await query.OrderBy(t => t.Name).ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error filtering tags.", Error = ex.Message });
            }
        }
        #endregion

    }
}
