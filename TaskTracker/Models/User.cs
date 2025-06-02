using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "使用者名稱為必填欄位")]
        [StringLength(50, ErrorMessage = "使用者名稱不可超過50個字元")]
        [Display(Name = "使用者名稱")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "電子郵件為必填欄位")]
        [EmailAddress(ErrorMessage = "請輸入有效的電子郵件格式")]
        [StringLength(100, ErrorMessage = "電子郵件不可超過100個字元")]
        [Display(Name = "電子郵件")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "密碼為必填欄位")]
        [StringLength(255, ErrorMessage = "密碼不可超過255個字元")]
        [Display(Name = "密碼")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "角色為必填欄位")]
        [StringLength(20, ErrorMessage = "角色名稱不可超過20個字元")]
        [Display(Name = "角色")]
        public string Role { get; set; } = "User"; // User 或 Admin

        [StringLength(50, ErrorMessage = "部門名稱不可超過50個字元")]
        [Display(Name = "部門")]
        public string? Department { get; set; }

        [StringLength(50, ErrorMessage = "職位名稱不可超過50個字元")]
        [Display(Name = "職位")]
        public string? Position { get; set; }

        [Display(Name = "啟用狀態")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "建立日期")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // 導航屬性 - 一個使用者可負責多個任務
        public virtual ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
} 