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
        private readonly FriendCircleContext _friendCircleContext;
        public HomeController(ILogger<HomeController> logger,FriendCircleContext friendCircleContext)
        {
            _logger = logger;
            _friendCircleContext = friendCircleContext;
        }
        public IActionResult Newsfeed()
        {
            return View();
        }
        public IActionResult Index()
        {
            var userName = User.Identity.Name;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.userId = userId;
            ViewBag.userName = userName;
            return View();           
        }      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
