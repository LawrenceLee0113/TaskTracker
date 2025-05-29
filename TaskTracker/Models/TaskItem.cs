using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTracker.Models
{
    public class TaskItem
    {
        [Key]
        public int TaskId { get; set; }

        [Required(ErrorMessage = "任務名稱為必填欄位")]
        [StringLength(100, ErrorMessage = "任務名稱不可超過100個字元")]
        [Display(Name = "任務名稱")]
        public string TaskName { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "描述不可超過500個字元")]
        [Display(Name = "任務描述")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "專案為必填欄位")]
        [Display(Name = "所屬專案")]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "負責人為必填欄位")]
        [Display(Name = "負責人")]
        public int AssignedUserId { get; set; }

        [Required(ErrorMessage = "優先順序為必填欄位")]
        [StringLength(10)]
        [Display(Name = "優先順序")]
        public string Priority { get; set; } = "中";

        [Required(ErrorMessage = "任務狀態為必填欄位")]
        [StringLength(20)]
        [Display(Name = "任務狀態")]
        public string Status { get; set; } = "待辦";

        [Display(Name = "到期日")]
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        [Display(Name = "建立日期")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "完成日期")]
        public DateTime? CompletedDate { get; set; }

        // 導航屬性 - 多個任務屬於一個專案
        [ForeignKey("ProjectId")]
        public virtual Project? Project { get; set; }

        // 導航屬性 - 多個任務指派給一個使用者
        [ForeignKey("AssignedUserId")]
        public virtual User? AssignedUser { get; set; }
    }
} 