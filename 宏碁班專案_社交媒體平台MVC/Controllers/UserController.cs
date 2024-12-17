using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using 宏碁班專案_社交媒體平台MVC.Models;
using Microsoft.AspNetCore.Authorization;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;


namespace 宏碁班專案_社交媒體平台MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly DBmanager _dbManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        //建構子注入
        public UserController(ILogger<UserController> logger, DBmanager dbManager, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _dbManager = dbManager;
            _webHostEnvironment = webHostEnvironment;
        }        
        public IActionResult AccountInfo()
        {            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _dbManager.getUserById(int.Parse(userId));
            // 列印模型屬性值
            //Console.WriteLine("綁定的模型屬性值：");
            //Console.WriteLine($"Name: {user.name}");
            //Console.WriteLine($"Birthday: {user.birthday}");
            //Console.WriteLine($"Email: {user.email}");
            //Console.WriteLine($"Phone: {user.phone}");
            //Console.WriteLine($"Address: {user.address}");
            //Console.WriteLine($"Description: {user.description}");
            //Console.WriteLine($"UserImage: {user.userimage}");
            if (user == null)
            {
                return NotFound("無法找到用戶資訊");
            }
            return View(user);
        }
        [HttpPost]
        //用戶資訊頁面      
        public IActionResult AccountInfo(userInfo users)
        {
            // 從 Claims 中取得用戶 ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // 透過 ID 取得用戶資料
            var user = _dbManager.getUserById(int.Parse(userId));
            //// 列印模型屬性值
            //Console.WriteLine("綁定的模型屬性值：");
            //Console.WriteLine($"Name: {user.name}");
            //Console.WriteLine($"Birthday: {user.birthday}");
            //Console.WriteLine($"Email: {user.email}");
            //Console.WriteLine($"Phone: {user.phone}");
            //Console.WriteLine($"Address: {user.address}");
            //Console.WriteLine($"Description: {user.description}");
            //Console.WriteLine($"UserImage: {user.userimage}");
            if (user == null)
            {
                return NotFound("無法找到用戶資訊");
            }

            user.name = users.name;
            user.birthday = users.birthday;
            user.phone = users.phone;
            user.address = users.address;
            user.description = users.description;
            _dbManager.UpdateUser(user);
            return View(user);//將用戶資料回傳到前端
        }

        //上傳使用者頭像
        [HttpPost]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                Console.WriteLine("文件為空");
                return RedirectToAction("AccountInfo");
            }
            ////檢查文件類型
            //if (!file.ContentType.StartsWith("image/"))
            //{
            //    ViewBag.ErrorMessage = "只能上傳圖片文件。";
            //    return View("AccountInfo");
            //}            
            // 儲存檔案到 wwwroot/image 資料夾
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            // 查詢當前用戶的舊圖片路徑
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _dbManager.getUserById(int.Parse(userId));
            // 刪除舊圖片（如果存在）
            if (user.userimage != null)
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, user.userimage.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            // 產生唯一檔名
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            // 使用 SixLabors.ImageSharp 套件處理圖片
            using (var stream = file.OpenReadStream())
            using (var image = Image.Load(stream))
            {
                // 調整圖片大小至指定寬高，例如 300x300
                image.Mutate(x => x.Resize(200, 200));                
                // 儲存圖片
                image.Save(filePath);
            }            
            // 將圖片連結保存到資料庫
            user.userimage = $"/images/{uniqueFileName}";
            _dbManager.UpdateUser(user);          
            return RedirectToAction("AccountInfo");
        }
        //登出
        public IActionResult Logout()
        {
            //登出，清除身份驗證Cookie
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        //設定頁面
        public IActionResult Setting()
        {
            return View();
        }
        //忘記密碼頁面
        public IActionResult Forgot()
        {
            return View();
        }
        //忘記密碼
        [HttpPost]
        public IActionResult Forgot(string email, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError("email", "電子郵件不能為空!");
                return View();
            }

            var user = _dbManager.getAccountByEmail(email);
            if (user == null)
            {
                ModelState.AddModelError("email", "該電子郵件未註冊!");
                return View();
            }
            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                ModelState.AddModelError("password", "密碼不能為空!");
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("confirmPassword", "密碼不一致!");
                return View();
            }
            _dbManager.UpdatePassword(email, newPassword);
            TempData["Message"] = "密碼已成功重置!";
            return RedirectToAction("Login");
        }
        // 登入頁面
        public IActionResult Login()
        {
            return View();
        }
        //透過post方法登入，回傳非同步處理任務
        [HttpPost]
        public async Task<IActionResult> Login(userInfo user, bool remeberMe)
        {   //驗證成功        
            if (ModelState.IsValid)
            {                
                if (string.IsNullOrWhiteSpace(user.email) || string.IsNullOrWhiteSpace(user.password))
                {
                    ModelState.AddModelError("email", "電子郵件和密碼不能為空!");
                    return View(user);
                }
                var loginUser = _dbManager.getAccount(user.email, user.password);                
                if (loginUser == null)
                {
                    //登入失敗，顯示錯誤訊息
                    ModelState.AddModelError("email", "信箱或密碼錯誤!");
                    //清除密碼，方便下次輸入
                    user.password = "";
                    return View(user);
                }
                //登入成功，建立身份驗證Cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email,loginUser.email),
                    new Claim(ClaimTypes.NameIdentifier, loginUser.id.ToString()),                     
                };                
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = remeberMe,//登入狀態(勾選就是記住)                                                            
                    ExpiresUtc = remeberMe ? DateTimeOffset.UtcNow.AddDays(30) :null
                };
                //設定身分驗證，將資訊(Claim)儲存在Cookie中，以便後續身分驗證和授權
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                         new ClaimsPrincipal(claimsIdentity),
                                         authProperties);
                //登入成功，重定向到首頁
                return RedirectToAction("Newsfeed", "Posts");
            }
            else
            {
                _logger.LogWarning("ModelState 驗證失敗。錯誤數量: {0}", ModelState.ErrorCount);
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("ModelState 錯誤: {0}", error.ErrorMessage);
                }
            }
            // 返回登入頁面
            return View(user);
        }
        //註冊頁面
        public IActionResult Register()
        {
            return View();
        }
        //透過post方法註冊新用戶
        [HttpPost]
        public IActionResult Register(userInfo user)
        {          
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(user.email) || string.IsNullOrWhiteSpace(user.password))
                {
                    ModelState.AddModelError("confirmPassword", "所有欄位都必須填寫!");
                    return View(user);
                }
                
                if (user.password != user.confirmPassword)
                {
                    ModelState.AddModelError("confirmPassword", "密碼不一致!");
                    //清除確認密碼方便下次輸入
                    user.confirmPassword = "";
                    return View(user);
                }
                //保存新用戶到資料庫
                _dbManager.addAccount(user);
                //註冊成功，重定向到登入頁面
                return RedirectToAction("Login");
            }
            // 返回註冊頁面
            return View(user);            
        }
        //檢查信箱是否重複
        [HttpPost]
        public IActionResult CheckRepeataccount([FromBody] EmailRequest request)
        {
            Console.WriteLine($"檢測信箱:{request.Email}");
            if (_dbManager.isUserExist(request.Email))
            {   
                //信箱已被使用時，不回傳布林值，阻擋表單sumbit
                return Json($"{request.Email}已被使用");
            }
            // 信箱未被使用時，回傳布林值確保表單可以正常sumbit
            return Json(false);
        }
        
    }
    // 使用類別來封裝資料，才能使用[FromBody]來讀取資料
    public class EmailRequest
    {
        public string Email { get; set; }
    }
}
