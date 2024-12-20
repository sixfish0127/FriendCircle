using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using 宏碁班專案_社交媒體平台MVC.Models;

namespace 宏碁班專案_社交媒體平台MVC.Controllers
{
    [Route("api/friends")]
    [ApiController]
    public class FriendsApiController : ControllerBase
    {
        private readonly FriendCircleContext _context;

        public FriendsApiController(FriendCircleContext context)
        {
            _context = context;
        }

        [HttpPost("send-request")]
        public async Task<IActionResult> SendFriendRequest([FromBody] int friendId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (userId == friendId)
            {
                return BadRequest("Cannot add yourself as a friend.");
            }

            var existingRequest = await _context.FriendShip
                .FirstOrDefaultAsync(f =>
                    (f.UserId1 == userId && f.UserId2 == friendId) ||
                    (f.UserId1 == friendId && f.UserId2 == userId));

            if (existingRequest != null)
            {
                return Conflict("好友請求已存在或你們已經是好友了。");
            }

            var friendRequest = new FriendShip
            {
                UserId1 = userId,
                UserId2 = friendId,
                Status = FriendshipStatus.Pending,
                CreatedAt = DateTime.Now
            };

            _context.FriendShip.Add(friendRequest);
            await _context.SaveChangesAsync();

            return Ok("Friend request sent successfully.");
        }
        // 回應好友請求
        [HttpPost("respond-request")]
        public async Task<IActionResult> RespondToFriendRequest(int requestId, string status)
        {
            var request = await _context.FriendShip.FirstOrDefaultAsync(f => f.Id == requestId);

            if (request == null)
            {
                return NotFound("Friend request not found.");
            }

            if (status != "Accepted" && status != "Rejected")
            {
                return BadRequest("Invalid status.");
            }

            request.Status = (FriendshipStatus)2;
            await _context.SaveChangesAsync();

            return Ok($"Friend request {status.ToLower()}.");
        }
        // 取得好友清單
        [HttpGet("list")]
        public async Task<IActionResult> GetFriendsList()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var friends = await _context.FriendShip
                .Where(f => f.UserId1 == userId && f.Status == (FriendshipStatus)2)
                .Select(f => new
                {
                    f.UserId2,
                    FriendName = _context.userInfo.FirstOrDefault(u => u.id == f.UserId2).name
                })
                .ToListAsync();

            return Ok(friends);
        }
    }

}
