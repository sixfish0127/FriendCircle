namespace 宏碁班專案_社交媒體平台MVC.Models
{
    public class NotificationService
    {
        private readonly FriendCircleContext _context;
        private readonly HttpClient _httpClient;

        public NotificationService(FriendCircleContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        // 生成通知並保存到數據庫
        public async Task<Notifications> CreateNotificationAsync(int userId, string message, int commentId, NotificationsType type)
        {
            var notification = new Notifications
            {
                UserId = userId,
                Message = message,
                Type = type,
                CreatAt = DateTime.Now,
                CommentId = commentId,
                IsRead = false
            };
            // 確認 postOwnerId 和 comment.ComentId 是否有效
            if (!_context.userInfo.Any(u => u.id == userId))
            {
                throw new Exception($"用戶 ID {userId} 在 Users 表中不存在。");
            }

            if (!_context.Comments.Any(c => c.ComentId == commentId))
            {
                throw new Exception($"留言 ID {commentId} 在 Comments 表中不存在。");
            }

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();           

            return notification;
        }
        public async Task<Notifications> CreateNotificationAsync(int userId, int friendid ,string message, NotificationsType type)
        {
            var notification = new Notifications
            {
                UserId = userId,
                Message = message,
                Type = type,
                CreatAt = DateTime.Now, 
                FriendRequestId = friendid,
                IsRead = false
            };
            // 確認 postOwnerId 和 comment.ComentId 是否有效
            if (!_context.userInfo.Any(u => u.id == userId))
            {
                throw new Exception($"用戶 ID {userId} 在 Users 表中不存在。");
            }            

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            
            return notification;
        }        
    }
}
