using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Diagnostics;
using System.Text.Json;
using 宏碁班專案_社交媒體平台MVC.Models;


namespace 宏碁班專案_社交媒體平台MVC.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationsApiController : Controller
    {
        private readonly FriendCircleContext _context;

        public NotificationsApiController(FriendCircleContext context)
        {
            _context = context;
        }        

        // 查詢用戶的所有通知
        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
             // 獲取目前登入用戶的 ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;            
            
            var notifications = await _context.Notifications
                .Where(n=>n.UserId==int.Parse(userId))
                .Include(n => n.Comment)  // 包括留言資訊
                .ThenInclude(c => c.User) // 包括留言者的用戶資訊                
                .OrderBy(n => n.CreatAt)
                .Select(n => new
                {
                    n.Id,
                    n.Type,
                    n.Message,
                    createdAt = n.CreatAt.ToString("yyyy-MM-ddTHH:mm:ss"),
                    n.IsRead,
                    commenterName = n.Comment.User.name, // 留言者名稱
                    commenterImage = n.Comment.User.userimage, // 留言者頭像
                    postOwnerName = n.User.name, // 貼文擁有者名稱（可選）
                    postOwnerImage = n.User.userimage, // 貼文擁有者頭像（可選）
                    friendName = _context.userInfo.Where(u=>u.id==n.FriendRequestId).Select(u => u.name).FirstOrDefault(), // 朋友名稱
                    friendImage = _context.userInfo.Where(u => u.id == n.FriendRequestId).Select(u => u.userimage).FirstOrDefault(), // 朋友頭像
                    friendId = n.FriendRequestId // 朋友ID    
                })
                .ToListAsync();

            return Ok(notifications);
        }
        

        // 標記通知為已讀
        [HttpPut("{id}/mark-read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            // 獲取通知
            var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
            if (notification == null)
            {
                return NotFound(new { success = false, message = "通知不存在" });
            }

            notification.IsRead = true;
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "通知已標記為已讀" });
        }            
    }
}


