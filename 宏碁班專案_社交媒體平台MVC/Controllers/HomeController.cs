using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using 宏碁班專案_社交媒體平台MVC.Models;

namespace 宏碁班專案_社交媒體平台MVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {       
        private readonly ILogger<HomeController> _logger;
        private readonly DBmanager _dbManager;
        private readonly FriendCircleContext _friendCircleContext;
        public HomeController(ILogger<HomeController> logger, DBmanager dbManager,FriendCircleContext friendCircleContext)
        {
            _logger = logger;
            _dbManager = dbManager;
            _friendCircleContext = friendCircleContext;
        }
       
        public IActionResult Details()
        {
            var userName = User.Identity.Name;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _dbManager.getUserById(int.Parse(userId));            
            return View(user);           
        }
        [HttpPost]
        public IActionResult UpdateUser(userInfo user)
        {
            
            return RedirectToAction("Index");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
