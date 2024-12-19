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

            // 發送通知到 Python
            //await SendNotificationToPython(notification);

            return notification;
        }

        //// 發送通知到 Python API
        //private async Task SendNotificationToPython(Notifications notification)
        //{
        //    var payload = new
        //    {
        //        id = notification.Id,
        //        userId = notification.UserId,
        //        commentId = notification.CommentId,
        //        message = notification.Message,
        //        createdAt = notification.CreatAt,
        //        isRead = notification.IsRead
        //    };

        //    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        //    try
        //    {
        //        var response = await _httpClient.PostAsync("https://localhost:7281//api/process-notifications", content);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            Console.WriteLine("成功傳送至python!");
        //        }
        //        else
        //        {
        //            Console.WriteLine($"傳送至python失敗: {response.StatusCode}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"傳送至python時錯誤: {ex.Message}");
        //    }
        //}
    }
}
