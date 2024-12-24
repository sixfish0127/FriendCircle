using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using 宏碁班專案_社交媒體平台MVC.Models;

namespace 宏碁班專案_社交媒體平台MVC.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsApiController : ControllerBase
    {
        private readonly FriendCircleContext _context;
        private readonly DBmanager _dbManager;
        private readonly NotificationService _notificationService;
        public CommentsApiController(FriendCircleContext context, DBmanager dbManager, NotificationService notificationService)
        {
            _context = context;
            _dbManager = dbManager;
            _notificationService = notificationService;
        }
        // 取得貼文的所有留言
        [HttpGet("{postId}")]
        public async Task<IActionResult> GetComments(int postId)
        {
            var comments = await _context.Comments
                .Where(c => c.PostId == postId)
                .Include(c => c.User)
                .OrderBy(c => c.CreatedAt)
                .Select(c => new
                {
                    c.ComentId,
                    c.Content,
                    userName = c.User.name,
                    Image = c.User.userimage,
                    CreatedAt = c.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
                })
                .ToListAsync();

            return Ok(comments);
        }
        // 新增留言
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] Comments comment)
        {
            if (ModelState.IsValid)
            {
                // 從登入的用戶取得用戶 ID
                var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                comment.UserId = int.Parse(UserId);
                // 從貼文 ID 取得貼文擁有者 ID
                var postOwnerId = _dbManager.GetPostOwnerId(comment.PostId);
                // 從用戶 ID 取得用戶名稱
                var userName = await _context.userInfo
                    .Where(u => u.id == comment.UserId)
                    .Select(u => u.name)
                    .FirstOrDefaultAsync();
                // 從用戶 ID 取得用戶頭像
                var userimgage = await _context.userInfo
                    .Where(u => u.id == comment.UserId)
                    .Select(u => u.userimage)
                    .FirstOrDefaultAsync();
                // 設定貼文的建立時間
                comment.CreatedAt = DateTime.Now;
                // 新增留言
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
                // 查詢留言者的名稱
                var commenterName = _context.userInfo
                    .Where(u => u.id == comment.UserId)
                    .Select(u => u.name)
                    .FirstOrDefault();
                // 產生通知
                var message = $"用戶 {commenterName} 對您的貼文留言: \"{comment.Content}\"";
                await _notificationService.CreateNotificationAsync(postOwnerId, message, comment.ComentId, NotificationsType.留言);
                try
                {
                    return Ok(new
                    {
                        comment.ComentId,
                        comment.Content,
                        User = userName ?? "Unknown",
                        Image = userimgage ?? "Unknown",
                        CreatedAt = comment.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
                    });
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine("Error occurred: " + ex.InnerException?.Message);
                    throw; //重新抛出異常
                }
            }
            return BadRequest(ModelState);
        }
    }
}
