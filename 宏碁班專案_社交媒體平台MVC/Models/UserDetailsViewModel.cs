namespace 宏碁班專案_社交媒體平台MVC.Models
{
    public class UserDetailsViewModel
    {
        public userInfo User { get; set; }// 用戶資訊
        public List<Posts> Posts { get; set; }// 貼文列表
    }
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string CreatedAt { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
    }
}
