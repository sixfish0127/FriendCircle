using Microsoft.EntityFrameworkCore;

namespace 宏碁班專案_社交媒體平台MVC.Models
{
    public class MutualFriendService
    {
        private readonly FriendCircleContext _friendCircleContext;
        public MutualFriendService(FriendCircleContext friendCircleContext)
        {
            _friendCircleContext = friendCircleContext;
        }
        public async Task<int> GetMutualFriendCountAsync(int userId1, int userId2)
        {
            // 獲取用戶 A 的好友列表
            var friendsOfUser1 = await _friendCircleContext.FriendShip
                .Where(f => (f.UserId1 == userId1 || f.UserId2 == userId1) && f.Status == FriendshipStatus.Accepted)
                .Select(f => f.UserId1 == userId1 ? f.UserId2 : f.UserId1)
                .ToListAsync();

            // 獲取用戶 B 的好友列表
            var friendsOfUser2 = await _friendCircleContext.FriendShip
                .Where(f => (f.UserId1 == userId2 || f.UserId2 == userId2) && f.Status == FriendshipStatus.Accepted)
                .Select(f => f.UserId1 == userId2 ? f.UserId2 : f.UserId1)
                .ToListAsync();

            // 計算共同好友數量
            var mutualFriends = friendsOfUser1.Intersect(friendsOfUser2).Count();
            return mutualFriends;
        }
    }
}
