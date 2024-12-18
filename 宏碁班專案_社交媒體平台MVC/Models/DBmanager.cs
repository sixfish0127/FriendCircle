using Microsoft.Extensions.Hosting;

namespace 宏碁班專案_社交媒體平台MVC.Models
{
    public class DBmanager
    {
        //建立FriendCircleContext物件，連接資料庫
        private readonly FriendCircleContext _friendCircleContext;
        public DBmanager(FriendCircleContext friendCircleContext)
        {
            _friendCircleContext = friendCircleContext;
        }
        public userInfo getAccount(string email, string password)
        {
            //根據電子郵件和密碼檢索用戶資料
            var user = _friendCircleContext.userInfo.FirstOrDefault(u => u.email == email && u.password == password);
            return user;
        }

        public userInfo getAccountByEmail(string email)
        {
            //根據電子郵件檢索用戶資料
            return _friendCircleContext.userInfo.FirstOrDefault(u => u.email == email);
        }

        public void addAccount(userInfo user)
        {
            // 新增帳號
            _friendCircleContext.userInfo.Add(user);
            _friendCircleContext.SaveChanges();
        }

        public bool isUserExist(string email)
        {            
            //檢查信箱是否已存在
            var existUser = _friendCircleContext.userInfo.FirstOrDefault(u => u.email == email);
            return existUser != null;
        }

        // 添加更新用戶密碼的方法
        public void UpdatePassword(string email, string newPassword)
        {
            var user = _friendCircleContext.userInfo.FirstOrDefault(u => u.email == email);
            //如果用戶存在，則更新密碼
            if (user != null)
            {
                user.password = newPassword;
                _friendCircleContext.SaveChanges();
            }
        }
        // 尋找UserId
        public userInfo getUserById(int id)
        {            
            return _friendCircleContext.userInfo.FirstOrDefault(u => u.id == id);
        }
        // 更新用戶
        public void UpdateUser(userInfo user)
        {
            _friendCircleContext.userInfo.Update(user);
            _friendCircleContext.SaveChanges();
        }
        // 取得用戶的貼文
        public List<Posts> GetPostsByUserId(int userId)
        {
            return _friendCircleContext.Posts
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();
        }
    }
}
