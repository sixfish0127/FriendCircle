using Microsoft.AspNetCore.Mvc;
using 宏碁班專案_社交媒體平台MVC.Models;
namespace 宏碁班專案_社交媒體平台MVC.Components
{
    public class _LayoutViewComponent : ViewComponent
    {
        private readonly FriendCircleContext _friendCircleContext;
        public _LayoutViewComponent(FriendCircleContext friendCircleContext)
        {
            _friendCircleContext = friendCircleContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // 模擬取得用戶數據的邏輯
            var userId = UserClaimsPrincipal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Content("未登入");
            }
            var user = await _friendCircleContext.userInfo.FindAsync(int.Parse(userId));
            if (user == null)
            {
                return Content("用戶未找到");
            }
            return View(user);
        }
    }
}
