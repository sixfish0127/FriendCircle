using System.Security.Claims;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using 宏碁班專案_社交媒體平台MVC.Models;

namespace 宏碁班專案_社交媒體平台MVC.Controllers
{
    [Route("api/friends")]
    [ApiController]
    public class FriendsApiController : ControllerBase
    {
        private readonly FriendCircleContext _context;
        private readonly NotificationService _notificationService;
        private readonly ILogger<FriendsApiController> _logger;
        private readonly MutualFriendService _mutualFriendService;
        public FriendsApiController(FriendCircleContext context, NotificationService notificationService, ILogger<FriendsApiController> logger, MutualFriendService mutualFriendService)
        {
            _context = context;
            _notificationService = notificationService;
            _logger = logger;
            _mutualFriendService = mutualFriendService;
        }

        //發送朋友邀請
        [HttpPost("send-request")]
        public async Task<IActionResult> SendFriendRequest([FromBody] FriendRequestDto friendRequest)
        {
            var senderId = friendRequest.SendId;
            var receiverId = friendRequest.ReceiverId;
            var senderIdName = await _context.userInfo
                    .Where(u => u.id == senderId)
                    .Select(u => u.name)
                    .FirstOrDefaultAsync();
            Console.WriteLine($"senderId:{friendRequest.SendId},receiverId:{friendRequest.ReceiverId}");
            var existingRequest = await _context.FriendShip
                .FirstOrDefaultAsync(f =>
                    (f.UserId1 == receiverId && f.UserId2 == senderId) ||
                    (f.UserId1 == senderId && f.UserId2 == receiverId));

            if (existingRequest.Status == FriendshipStatus.Accepted|| existingRequest.Status == FriendshipStatus.Pending)
            {
                return Conflict("好友請求已存在或你們已經是好友了。");
            }
            
            var friendShip = new FriendShip
            {
                UserId1 = receiverId,
                UserId2 = senderId,
                Status = FriendshipStatus.Pending,
                CreatedAt = DateTime.Now
            };

            _context.FriendShip.Add(friendShip);
            await _context.SaveChangesAsync();

            // 生成通知並保存到數據庫
            var message = $"{senderIdName}向您發送好友請求";
            await _notificationService.CreateNotificationAsync(receiverId, senderId, message, NotificationsType.朋友邀請);
            return Ok(new { message = "Friend request sent successfully." });
        }

        // 回應好友請求
        [HttpPut("{requestId}/response-request/{status}")]
        public async Task<IActionResult> RespondToFriendRequest(int requestId, string status)
        {
            var request = await _context.FriendShip.FirstOrDefaultAsync(f => f.UserId2 == requestId);
            
            // 設置好友請求的狀態
            request.Status = status == "Accepted" ? FriendshipStatus.Accepted : FriendshipStatus.NotFriend;

            // 生成通知並保存到數據庫
            var message = status == "Accepted" ? "您的好友請求已被接受" : "您的好友請求已被拒絕";
            try { 
                await _notificationService.CreateNotificationAsync(request.UserId2, request.UserId1, message, NotificationsType.朋友接受);
            
                await _context.SaveChangesAsync();
            }catch (Exception e)
            {
                // 記錄異常並返回伺服器錯誤
                _logger.LogError($"回覆好友請求時發生錯誤: {e.Message}");
                return StatusCode(500, "處理請求時發生錯誤。");
            }
            return Ok($"Friend request {status.ToLower()}.");
        }

        //取得好友請求
        [HttpGet("friend-requests")]
        public async Task<IActionResult> GetFriendRequests()
        {
            // 獲取目前登入用戶 ID
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // 過濾待確認的好友請求
            var friendRequests = await _context.FriendShip
                .Where(f => f.UserId1 == userId && f.Status == FriendshipStatus.Pending)
                .Join(
                    _context.userInfo,
                    f => f.UserId2,
                    u => u.id,
                    (f, u) => new
                    {
                        f.Id,
                        f.Status,
                        SenderId = u.id,
                        SenderName = u.name,
                        SenderImage = u.userimage
                    }
                )
                .ToListAsync();
            // 取得共同好友數量
            var result = new List<object>();
            foreach (var request in friendRequests)
            {
                var mutualFriendCount = await _mutualFriendService.GetMutualFriendCountAsync(userId, request.SenderId);
                result.Add(new
                {
                    request.Id,
                    request.SenderId,
                    request.SenderName,
                    request.SenderImage,
                    status = request.Status.ToString(),
                    MutualFriend = mutualFriendCount
                });
            }
            Console.WriteLine("回傳資料:",result);
            return Ok(result);
        }

        // 取得好友清單
        [HttpGet("list")]
        public async Task<IActionResult> GetFriendsList()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var friends = await _context.FriendShip
                .Where(f => f.UserId1 == userId && f.Status == FriendshipStatus.Accepted)
                .Join(_context.userInfo,
                    f => f.UserId2,
                    u => u.id,
                    (f, u) => new
                    {
                        u.id,
                        u.name,
                        u.userimage,
                        u.status,
                    })
                .ToListAsync();

            return Ok(friends);
        }
        public class FriendRequestDto
        {
            public int SendId { get; set; }       // 發送者ID
            public int ReceiverId { get; set; }  // 接收者ID

        }
    }

}
