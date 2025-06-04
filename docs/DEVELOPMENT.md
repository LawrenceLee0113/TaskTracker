# TaskTracker 開發指南

完整的開發環境設定與最佳實務指南

---

## 🛠️ 開發環境設定

### 必要工具

#### IDE 與編輯器
- **Visual Studio 2022** (推薦) - 完整的 .NET 開發環境
- **VS Code** - 輕量級跨平台編輯器
- **JetBrains Rider** - 專業的 .NET IDE

#### 資料庫工具
- **Azure Data Studio** (推薦) - 跨平台 SQL Server 管理工具
- **SQL Server Management Studio** (Windows) - 傳統 SQL Server 管理工具
- **DBeaver** - 通用資料庫管理工具

#### 容器管理
- **Docker Desktop** - 本地 Docker 開發環境
- **Docker Extension for VS Code** - VS Code Docker 支援

### VS Code 擴充功能

```json
{
  "recommendations": [
    "ms-dotnettools.csharp",
    "ms-dotnettools.csdevkit",
    "bradlc.vscode-tailwindcss",
    "ms-azuretools.vscode-docker",
    "ms-mssql.mssql",
    "formulahendry.auto-rename-tag",
    "ms-vscode.vscode-json"
  ]
}
```

---

## 🚀 專案設定

### 克隆與初始化

```bash
# 克隆專案
git clone <repository-url>
cd SmartPlanner2

# 檢查 .NET 版本
dotnet --version

# 檢查 Node.js 版本
node --version
npm --version

# 檢查 Docker 版本
docker --version
docker-compose --version
```

### 環境變數設定

建立 `.env` 檔案（如需要）：
```bash
# .env
SQL_SERVER_PASSWORD=TaskTracker123!
ASPNETCORE_ENVIRONMENT=Development
```

---

## 🗄️ 資料庫開發

### Entity Framework Core 開發流程

#### 模型優先開發 (Code First)

```bash
# 1. 修改 Models
# 2. 建立遷移
dotnet ef migrations add <描述性名稱>

# 3. 檢視遷移
dotnet ef migrations script

# 4. 套用遷移
dotnet ef database update

# 5. 驗證資料庫
docker exec -it tasktracker-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P TaskTracker123!
```

#### 常用 EF 指令

```bash
# 查看遷移狀態
dotnet ef migrations list

# 回滾到特定遷移
dotnet ef database update <MigrationName>

# 生成 SQL 腳本
dotnet ef script-migration

# 移除最後一個遷移
dotnet ef migrations remove

# 完全重置資料庫
dotnet ef database drop
dotnet ef database update
```

### 資料庫最佳實務

#### 模型設計

```csharp
public class Project
{
    [Key]
    public int ProjectId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string ProjectName { get; set; }
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    // 導航屬性
    public virtual ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
```

#### DbContext 配置

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // 設定關聯
    modelBuilder.Entity<Project>()
        .HasMany(p => p.Tasks)
        .WithOne(t => t.Project)
        .HasForeignKey(t => t.ProjectId)
        .OnDelete(DeleteBehavior.Cascade);
    
    // 設定索引
    modelBuilder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();
    
    // 設定種子資料
    SeedData(modelBuilder);
}
```

---

## 🎨 前端開發

### Tailwind CSS 開發流程

#### 監控模式開發

```bash
# 終端 1 - 啟動 .NET 應用程式
cd TaskTracker
dotnet watch run

# 終端 2 - 監控 Tailwind CSS
npx tailwindcss -i ./wwwroot/css/input.css -o ./wwwroot/css/output.css --watch
```

#### Tailwind 配置

```javascript
// tailwind.config.js
module.exports = {
  content: ["./Views/**/*.{html,cshtml}", "./wwwroot/**/*.js"],
  theme: {
    extend: {
      colors: {
        primary: '#3B82F6',
        secondary: '#6B7280'
      }
    }
  },
  plugins: []
}
```

### Razor 視圖最佳實務

#### 佈局結構

```html
<!-- _Layout.cshtml -->
<!DOCTYPE html>
<html lang="zh-TW">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - TaskTracker</title>
    <link rel="stylesheet" href="~/css/output.css" />
</head>
<body class="bg-gray-50">
    <nav class="bg-white shadow">
        <!-- 導航內容 -->
    </nav>
    
    <main class="container mx-auto px-4 py-8">
        @RenderBody()
    </main>
    
    <script src="~/js/site.js"></script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

#### 部分視圖

```html
<!-- _ValidationScriptsPartial.cshtml -->
<script src="~/lib/jquery-validation/dist/jquery.validate.min.js"></script>
<script src="~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js"></script>
```

---

## 🐳 Docker 開發

### 本地 Docker 設定

#### docker-compose 開發配置

```yaml
# docker-compose.override.yml
version: '3.8'

services:
  sqlserver:
    environment:
      - SA_PASSWORD=DevPassword123!
    ports:
      - "1433:1433"
    volumes:
      - sqlserver_dev_data:/var/opt/mssql

volumes:
  sqlserver_dev_data:
```

#### 容器管理指令

```bash
# 開發環境啟動
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d

# 查看即時日誌
docker-compose logs -f sqlserver

# 進入容器
docker exec -it tasktracker-sqlserver bash

# 備份資料庫
docker exec tasktracker-sqlserver /opt/mssql-tools/bin/sqlcmd \
  -S localhost -U sa -P TaskTracker123! \
  -Q "BACKUP DATABASE TaskTrackerDB_Dev TO DISK = '/var/opt/mssql/backup/TaskTracker.bak'"
```

---

## 🧪 測試開發

### 單元測試設定

```bash
# 建立測試專案
dotnet new xunit -n TaskTracker.Tests
cd TaskTracker.Tests

# 新增專案參考
dotnet add reference ../TaskTracker/TaskTracker.csproj

# 新增測試套件
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Moq
```

### 測試範例

```csharp
[Fact]
public async Task GetProjects_ReturnsAllProjects()
{
    // Arrange
    var options = new DbContextOptionsBuilder<TaskTrackerContext>()
        .UseInMemoryDatabase(databaseName: "TestDb")
        .Options;
    
    using var context = new TaskTrackerContext(options);
    var controller = new ProjectsController(context);
    
    // Act
    var result = await controller.Index();
    
    // Assert
    var viewResult = Assert.IsType<ViewResult>(result);
    var model = Assert.IsAssignableFrom<IEnumerable<Project>>(viewResult.ViewData.Model);
}
```

---

## 🔧 偵錯技巧

### 常見問題診斷

#### 資料庫連接問題

```bash
# 檢查連接字串
cat TaskTracker/appsettings.json | grep ConnectionStrings

# 測試資料庫連接
docker exec -it tasktracker-sqlserver /opt/mssql-tools/bin/sqlcmd \
  -S localhost -U sa -P TaskTracker123! \
  -Q "SELECT @@VERSION"
```

#### Tailwind CSS 問題

```bash
# 檢查輸出檔案
ls -la TaskTracker/wwwroot/css/
cat TaskTracker/wwwroot/css/output.css | head -10

# 重新編譯
npx tailwindcss -i ./wwwroot/css/input.css -o ./wwwroot/css/output.css --verbose
```

### VS Code 偵錯設定

```json
// .vscode/launch.json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": ".NET Core Launch (web)",
      "type": "coreclr",
      "request": "launch",
      "preLaunchTask": "build",
      "program": "${workspaceFolder}/TaskTracker/bin/Debug/net8.0/TaskTracker.dll",
      "args": [],
      "cwd": "${workspaceFolder}/TaskTracker",
      "stopAtEntry": false,
      "serverReadyAction": {
        "action": "openExternally",
        "pattern": "\\bNow listening on:\\s+(https?://\\S+)"
      },
      "env": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  ]
}
```

---

## 📈 效能優化

### Entity Framework 優化

```csharp
// 預載入關聯資料
var projects = await context.Projects
    .Include(p => p.Tasks)
    .ToListAsync();

// 分頁查詢
var pagedTasks = await context.Tasks
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();

// 異步操作
public async Task<IActionResult> Index()
{
    var projects = await _context.Projects.ToListAsync();
    return View(projects);
}
```

### Tailwind CSS 優化

```bash
# 生產環境編譯（移除未使用的樣式）
npx tailwindcss -i ./wwwroot/css/input.css -o ./wwwroot/css/output.css --minify
```

---

## 🔒 安全性考量

### 資料驗證

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("ProjectName,Description")] Project project)
{
    if (ModelState.IsValid)
    {
        _context.Add(project);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    return View(project);
}
```

### 環境設定

```json
// appsettings.Production.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=prod-server;Database=TaskTrackerDB;..."
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  }
}
```

---

## 📚 參考資源

- [ASP.NET Core 官方文件](https://docs.microsoft.com/aspnet/core/)
- [Entity Framework Core 文件](https://docs.microsoft.com/ef/core/)
- [Tailwind CSS 文件](https://tailwindcss.com/docs)
- [Docker 官方文件](https://docs.docker.com/)

---

*最後更新：2024年12月* 