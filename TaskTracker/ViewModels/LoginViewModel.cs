using System.ComponentModel.DataAnnotations;

namespace TaskTracker.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "電子郵件為必填欄位")]
        [EmailAddress(ErrorMessage = "請輸入有效的電子郵件格式")]
        [Display(Name = "電子郵件")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "密碼為必填欄位")]
        [Display(Name = "密碼")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "記住我")]
        public bool RememberMe { get; set; }
    }
} 