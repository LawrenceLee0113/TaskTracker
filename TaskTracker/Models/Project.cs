using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Models
{
    public class Project
    {
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "專案名稱為必填欄位")]
        [StringLength(100, ErrorMessage = "專案名稱不可超過100個字元")]
        [Display(Name = "專案名稱")]
        public string ProjectName { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "描述不可超過500個字元")]
        [Display(Name = "專案描述")]
        public string? Description { get; set; }

        [Display(Name = "開始日期")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "結束日期")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "專案狀態為必填欄位")]
        [StringLength(20)]
        [Display(Name = "專案狀態")]
        public string Status { get; set; } = "進行中";

        [Display(Name = "建立日期")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // 導航屬性 - 一個專案可包含多個任務
        public virtual ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
} 