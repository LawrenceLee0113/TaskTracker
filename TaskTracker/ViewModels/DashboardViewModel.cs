namespace TaskTracker.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalProjects { get; set; }
        public int TotalTasks { get; set; }
        public int TotalUsers { get; set; }
        public int CompletedTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int PendingTasks { get; set; }
        public int CompletionRate { get; set; }
        public bool IsAdmin { get; set; }
        public string CurrentUserName { get; set; } = string.Empty;
    }
} 