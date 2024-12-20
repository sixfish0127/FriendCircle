using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> GetPosts(int page=1,int pageSize=10,bool userOnly = false,int?uid = null)
        {            
            if (page<1||pageSize<1) return BadRequest("無效的請求");

            var query = _friendCircleContext.Posts
                .Include(p => p.User)
                .OrderBy(p => p.CreatedAt);

            
            if (uid.HasValue)
            {
                //PostDetails頁面需要過濾特定使用者的貼文
                query = query.Where(p => p.UserId == uid.Value).OrderBy(p => p.CreatedAt);
            }
            else if(userOnly)
            {
                // Details頁面過濾只顯示當前使用者的貼文
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;                
                query = query.Where(p => p.UserId == int.Parse(userId))
                 .OrderBy(p => p.CreatedAt); // 重新排序
            }
            var posts = await query                                
                .Skip((page - 1) * pageSize)//跳過前面的資料
                .Take(pageSize)//取得指定數量的資料
                .Select(p => new
                {
                    p.Id,
                    p.Content,
                    CreatedAt = p.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss"),
                    user = p.User != null ? p.User.name : "Unknown User",
                    image = p.User != null ? p.User.userimage : "/images/Default.jpg",
                    uid = p.User.id
                })
                .ToListAsync();
            //確認是否有取得資料            
            Console.WriteLine($"Page: {page}, PageSize: {pageSize}, Total Posts: {posts.Count}");
            foreach (var post in posts)
            {
                Console.WriteLine($"Id: {post.Id}, Content: {post.Content}, User: {post.user},CreatAt:{post.CreatedAt}");
            }

            return Ok(posts);
        }        
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] Posts post)
        {
            if (ModelState.IsValid)
            {                               
                // 從登入的用戶取得用戶 ID
                var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
                post.UserId = int.Parse(UserId);                
                // 從用戶 ID 取得用戶名稱
                var userName = await _friendCircleContext.userInfo
                    .Where(u => u.id == post.UserId)
                    .Select(u => u.name)
                    .FirstOrDefaultAsync();
                // 從用戶 ID 取得用戶頭像
                var userimgage = await _friendCircleContext.userInfo
                    .Where(u => u.id == post.UserId)
                    .Select(u => u.userimage)
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
                    User = userName?? "Unknown",
                    Image = userimgage ?? "Unknown",                       
                });
            }
            // 回傳錯誤訊息
            return BadRequest(ModelState);
        }
    }
}
