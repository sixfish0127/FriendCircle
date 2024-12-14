using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using 宏碁班專案_社交媒體平台MVC.Models;

namespace 宏碁班專案_社交媒體平台MVC.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostsApiController : ControllerBase
    {
        private readonly FriendCircleContext _friendCircleContext;
        public PostsApiController(FriendCircleContext friendCircleContext)
        {
            _friendCircleContext = friendCircleContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _friendCircleContext.Posts
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new
                {
                    p.Id,
                    p.Content,
                    p.CreatedAt,
                    User = p.UserId // 替換為用戶名稱
                })
                .ToListAsync();

            return Ok(posts);
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] Posts post)
        {
            if (ModelState.IsValid)
            {
                // 檢查用戶是否已經登入或NameIdentifier為空
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized("User is not authenticated.");
                }
                // 從登入的用戶取得用戶 ID
                var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
                post.UserId = int.Parse(UserId);
                // 從用戶 ID 取得用戶名稱
                var userName = await _friendCircleContext.userInfo
                    .Where(u => u.id == post.UserId)
                    .Select(u => u.name)
                    .FirstOrDefaultAsync();
                // 設定貼文的建立時間
                post.CreatedAt = DateTime.Now;
                // 新增貼文
                _friendCircleContext.Add(post);
                await _friendCircleContext.SaveChangesAsync();
                // 回傳新增的貼文
                return Ok(new
                {
                    post.Id,
                    post.Content,
                    post.CreatedAt,
                    User = userName?? "Unknown"
                });
            }
            // 回傳錯誤訊息
            return BadRequest(ModelState);
        }
    }
}
