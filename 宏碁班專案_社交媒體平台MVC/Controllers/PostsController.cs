using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
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
            if (user == null)
            {
                return NotFound("無法找到用戶資訊");
            }
            return View(user);
        }
    }
}
