using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackItAPIs.Models;

namespace StackItAPIs.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly StackItContext _context;

        public NotificationController(StackItContext context)
        {
            _context = context;
        }

        #region Get All Notifications
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetAll()
        {
            try
            {
                var notifications = await _context.Notifications
                    .Include(n => n.User)
                    .OrderByDescending(n => n.CreatedAt)
                    .ToListAsync();

                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving notifications.", Error = ex.Message });
            }
        }
        #endregion

        #region Get Notification by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetById(int id)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(id);
                if (notification == null)
                    return NotFound(new { Message = $"Notification with ID {id} not found." });

                return Ok(notification);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving notification.", Error = ex.Message });
            }
        }
        #endregion

        #region Create Notification
        [HttpPost]
        public async Task<ActionResult<Notification>> Create(Notification notification)
        {
            try
            {
                notification.CreatedAt = DateTime.UtcNow;
                notification.IsRead = false;

                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();

                return Ok(notification);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating notification.", Error = ex.Message });
            }
        }
        #endregion

        #region Mark as Read
        [HttpPatch("mark-read/{id}")]
        public async Task<ActionResult> MarkAsRead(int id)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(id);
                if (notification == null)
                    return NotFound(new { Message = $"Notification with ID {id} not found." });

                notification.IsRead = true;
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Notification marked as read." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error updating notification.", Error = ex.Message });
            }
        }
        #endregion

        #region Delete Notification
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(id);
                if (notification == null)
                    return NotFound(new { Message = $"Notification with ID {id} not found." });

                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Notification deleted." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error deleting notification.", Error = ex.Message });
            }
        }
        #endregion

        #region Filter Notifications (by UserId, IsRead)
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Notification>>> Filter(
            [FromQuery] int? userId,
            [FromQuery] bool? isRead)
        {
            try
            {
                var query = _context.Notifications.AsQueryable();

                if (userId != null)
                    query = query.Where(n => n.UserId == userId);

                if (isRead != null)
                    query = query.Where(n => n.IsRead == isRead);

                var result = await query
                    .OrderByDescending(n => n.CreatedAt)
                    .ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error filtering notifications.", Error = ex.Message });
            }
        }
        #endregion

    }
}
