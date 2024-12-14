using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using 宏碁班專案_社交媒體平台MVC.Models;

namespace 宏碁班專案_社交媒體平台MVC.Controllers
{
    public class PostsController : Controller
    {
        private readonly ILogger<PostsController> _logger;
        private readonly DBmanager _dbManager;
        private readonly FriendCircleContext _friendCircleContext;
        public PostsController(ILogger<PostsController> logger, DBmanager dbManager,FriendCircleContext friendCircleContext)
        {
            _logger = logger;
            _dbManager = dbManager;
            _friendCircleContext = friendCircleContext;
        }
        public IActionResult Newsfeed()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _dbManager.getUserById(int.Parse(userId));
            
            return View(user);
        }
        // 顯示所有貼文
        public async Task<IActionResult> Index()
        {
            var posts = await _friendCircleContext.Posts.OrderByDescending(p => p.CreatedAt).ToListAsync();
            return View(posts);
        }
        // 處理新增貼文的請求
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Content")] Posts post)
        {
            if (ModelState.IsValid)
            {
                var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // 從登入的用戶取得用戶 ID
                post.UserId = int.Parse(UserId);
                _friendCircleContext.Add(post);
                await _friendCircleContext.SaveChangesAsync();
                return RedirectToAction("Newsfeed");
            }
            return View();
        }
    }
}
