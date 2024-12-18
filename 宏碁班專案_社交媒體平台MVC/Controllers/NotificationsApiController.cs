using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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
        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized("用戶未登入");

            var notifications = await _context.Notifications
                .Where(n => n.UserId == int.Parse(userId))
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new
                {
                    n.Title,
                    n.Message,
                    CreatedAt = n.CreatedAt.ToString("g"), // 格式化時間
                    n.ImageUrl // 用戶頭像或相關圖片
                })
                .Take(10) // 只取最新的 10 則通知
                .ToListAsync();

            return Ok(notifications);
        }
    }
}
}
