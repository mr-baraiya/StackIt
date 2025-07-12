using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackItAPIs.Models;
using Microsoft.EntityFrameworkCore;

namespace StackItAPIs.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly StackItContext _context;

        public UserController(StackItContext context)
        {
            _context = context;
        }

        #region Get All Users
        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to retrieve users.", Error = ex.Message });
            }
        }
        #endregion

        #region Get User by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return NotFound(new { Message = $"User with ID {id} not found." });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving user.", Error = ex.Message });
            }
        }
        #endregion

        #region Register (Add New User)
        [HttpPost]
        public async Task<ActionResult<User>> Register(User user)
        {
            try
            {
                user.CreatedAt = DateTime.UtcNow;
                user.UpdatedAt = DateTime.UtcNow;
                user.Reputation = 0;
                user.IsActive = true;
                user.IsBanned = false;
                user.Role ??= "User";

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error registering user.", Error = ex.Message });
            }
        }
        #endregion

        #region Update User
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUser(int id, User updatedUser)
        {
            if (id != updatedUser.Id)
                return BadRequest(new { Message = "User ID mismatch." });

            try
            {
                var existingUser = await _context.Users.FindAsync(id);
                if (existingUser == null)
                    return NotFound(new { Message = $"User with ID {id} not found." });

                // Update fields
                existingUser.Username = updatedUser.Username;
                existingUser.Email = updatedUser.Email;
                existingUser.FullName = updatedUser.FullName;
                existingUser.ProfilePictureUrl = updatedUser.ProfilePictureUrl;
                existingUser.Bio = updatedUser.Bio;
                existingUser.Role = updatedUser.Role;
                existingUser.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Ok(existingUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error updating user.", Error = ex.Message });
            }
        }
        #endregion

        #region Delete User
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return NotFound(new { Message = $"User with ID {id} not found." });

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error deleting user.", Error = ex.Message });
            }
        }
        #endregion

        #region Toggle User Active Status
        [HttpPatch("toggle-active/{id}")]
        public async Task<ActionResult> ToggleUserActive(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return NotFound(new { Message = $"User with ID {id} not found." });

                user.IsActive = !(user.IsActive ?? true);
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Ok(new { Message = "User active status updated.", user.IsActive });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error toggling status.", Error = ex.Message });
            }
        }
        #endregion

        #region Ban/Unban User
        [HttpPatch("toggle-ban/{id}")]
        public async Task<ActionResult> ToggleUserBan(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return NotFound(new { Message = $"User with ID {id} not found." });

                user.IsBanned = !(user.IsBanned ?? false);
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Ok(new { Message = "User ban status updated.", user.IsBanned });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error toggling ban.", Error = ex.Message });
            }
        }
        #endregion

        #region Filter Users (by username, email, role)
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<User>>> FilterUsers(
            [FromQuery] string? username,
            [FromQuery] string? email,
            [FromQuery] string? role)
        {
            try
            {
                var query = _context.Users.AsQueryable();

                if (!string.IsNullOrWhiteSpace(username))
                    query = query.Where(u => u.Username!.ToLower().Contains(username.ToLower()));

                if (!string.IsNullOrWhiteSpace(email))
                    query = query.Where(u => u.Email!.ToLower().Contains(email.ToLower()));

                if (!string.IsNullOrWhiteSpace(role))
                    query = query.Where(u => u.Role!.ToLower() == role.ToLower());

                var result = await query.OrderBy(u => u.Username).ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error filtering users.", Error = ex.Message });
            }
        }
        #endregion

    }
}
