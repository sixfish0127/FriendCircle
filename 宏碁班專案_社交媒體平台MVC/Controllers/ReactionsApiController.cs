using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using 宏碁班專案_社交媒體平台MVC.Models;

namespace 宏碁班專案_社交媒體平台MVC.Controllers
{
    [Route("api/reactions")]
    [ApiController]
    public class ReactionsApiController : ControllerBase
    {
        private readonly FriendCircleContext _context;
        private readonly DBmanager _dbManager;
        private readonly NotificationService _notificationService;

        public ReactionsApiController(FriendCircleContext context, DBmanager dbManager, NotificationService notificationService)
        {
            _context = context;
            _dbManager = dbManager;
            _notificationService = notificationService;
        }
        // 取得每篇貼文的反應數
        [HttpGet("{postId}")]
        public async Task<IActionResult> GetReactions(int postId)
        {
            var reactions = await _context.ReactionType
                .Where(r => r.PostId == postId)
                .GroupBy(r => r.ReactionType1)
                .Select(r => new
                {
                    Type = r.Key.ToString(),//接列舉轉換成字串
                    Count = r.Count()
                })
                .ToListAsync();
            return Ok(reactions);
        }
        // 新增或更新反應
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateReaction([FromBody] ReactionType reaction)
        {
            if (ModelState.IsValid)
            {
                // 從登入的用戶取得用戶 ID
                var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                reaction.UserId = int.Parse(UserId);                

                // 檢查用戶是否已經對這篇貼文按過反應
                var existingReaction = await _context.ReactionType
                    .Where(r => r.PostId == reaction.PostId && r.UserId == reaction.UserId)
                    .FirstOrDefaultAsync();
                if (existingReaction != null)
                {                                        
                    // 更新反應
                    existingReaction.ReactionType1 = reaction.ReactionType1;                    
                    existingReaction.CreatedAt = DateTime.Now;
                }
                else
                {                    
                    // 新增反應
                    reaction.CreatedAt = DateTime.Now;
                    _context.ReactionType.Add(reaction);
                }
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }
    }
}
