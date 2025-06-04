# TaskTracker 疑難排解指南

完整的問題診斷與解決方案

---

## 🐳 Docker & SQL Server 問題

### 容器無法啟動

#### 問題現象
```bash
$ docker-compose up -d
ERROR: Cannot start service sqlserver: driver failed programming external connectivity
```

#### 解決方案
```bash
# 1. 檢查埠號佔用
netstat -an | grep 1433
lsof -i :1433  # macOS/Linux

# 2. 終止佔用程序
sudo kill -9 <PID>

# 3. 重新啟動容器
docker-compose down
docker-compose up -d

# 4. 使用其他埠號（如需要）
# 編輯 docker-compose.yml，改為 "1434:1433"
```

### SQL Server 連接失敗

#### 問題現象
```
Microsoft.Data.SqlClient.SqlException: A network-related or instance-specific error occurred
```

#### 診斷步驟
```bash
# 1. 檢查容器狀態
docker-compose ps

# 2. 查看容器日誌
docker-compose logs sqlserver

# 3. 檢查 SQL Server 是否完全啟動
docker exec -it tasktracker-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P TaskTracker123!
```

#### 解決方案
```bash
# 等待 SQL Server 完全啟動（可能需要 30-60 秒）
docker-compose logs -f sqlserver

# 看到以下訊息表示啟動成功：
# "SQL Server is now ready for client connections"

# 如果持續失敗，重新建立容器
docker-compose down -v
docker-compose up -d
```

### 容器資料遺失

#### 問題現象
每次重啟容器後資料消失

#### 解決方案
```bash
# 檢查 Volume 設定
docker volume ls
docker volume inspect smartplanner2_sqlserver_data

# 如果 Volume 不存在，重新建立
docker-compose down
docker volume create smartplanner2_sqlserver_data
docker-compose up -d
```

---

## 🗄️ Entity Framework 問題

### 遷移失敗

#### 問題現象
```
The model backing the context has changed since the database was created
```

#### 解決方案
```bash
# 1. 檢查當前遷移狀態
dotnet ef migrations list

# 2. 移除有問題的遷移
dotnet ef migrations remove

# 3. 重新建立遷移
dotnet ef migrations add InitialCreate

# 4. 更新資料庫
dotnet ef database update
```

### 模型變更警告

#### 問題現象
```
An operation was scaffolded that may result in the loss of data
```

#### 解決方案
```bash
# 1. 檢視遷移檔案內容
cat Migrations/*_<MigrationName>.cs

# 2. 手動編輯遷移檔案（如需要）
# 3. 備份重要資料
# 4. 套用遷移
dotnet ef database update
```

### 連接字串錯誤

#### 問題現象
```
Invalid connection string format
```

#### 檢查清單
```bash
# 1. 檢查 appsettings.json
cat TaskTracker/appsettings.json

# 2. 確認格式正確
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=TaskTrackerDB_Dev;User Id=sa;Password=TaskTracker123!;TrustServerCertificate=true;"
  }
}

# 3. 檢查特殊字元轉義
# 密碼中的特殊字元需要適當處理
```

---

## 🎨 前端問題

### Tailwind CSS 樣式未載入

#### 問題現象
網頁顯示無樣式或樣式不正確

#### 診斷步驟
```bash
# 1. 檢查輸出檔案
ls -la TaskTracker/wwwroot/css/
cat TaskTracker/wwwroot/css/output.css | head -20

# 2. 檢查 input.css 是否存在
cat TaskTracker/wwwroot/css/input.css

# 3. 檢查 tailwind.config.js
cat TaskTracker/tailwind.config.js
```

#### 解決方案
```bash
# 1. 重新安裝依賴
cd TaskTracker
npm install

# 2. 重新編譯 CSS
npx tailwindcss -i ./wwwroot/css/input.css -o ./wwwroot/css/output.css

# 3. 檢查 _Layout.cshtml 中的 CSS 連結
grep "output.css" Views/Shared/_Layout.cshtml

# 4. 清除瀏覽器快取
# Ctrl+F5 或清除瀏覽器快取
```

### Node.js 依賴問題

#### 問題現象
```bash
npm ERR! Could not resolve dependency
```

#### 解決方案
```bash
# 1. 清除 npm 快取
npm cache clean --force

# 2. 刪除 node_modules
rm -rf node_modules package-lock.json

# 3. 重新安裝
npm install

# 4. 如果仍有問題，使用 yarn
npm install -g yarn
yarn install
```

### 監控模式無效

#### 問題現象
Tailwind CSS 監控模式不會自動重新編譯

#### 解決方案
```bash
# 1. 終止現有程序
pkill -f tailwindcss

# 2. 檢查檔案權限
ls -la wwwroot/css/

# 3. 重新啟動監控
npx tailwindcss -i ./wwwroot/css/input.css -o ./wwwroot/css/output.css --watch --verbose

# 4. 確認監控路徑正確
# tailwind.config.js 中的 content 路徑
```

---

## 🔧 .NET 應用程式問題

### 編譯錯誤

#### CS0246: 找不到類型或命名空間

```bash
# 1. 檢查 using 語句
# 2. 確認套件參考
dotnet list package

# 3. 還原套件
dotnet restore

# 4. 清除並重建
dotnet clean
dotnet build
```

#### CS1061: 不包含定義

```bash
# 檢查物件類型和可用方法
# 確認 Entity Framework 模型是否正確設定
```

### 執行時錯誤

#### HTTP 500 內部伺服器錯誤

```bash
# 1. 檢查應用程式日誌
dotnet run --verbosity detailed

# 2. 檢查 Developer Exception Page
# 在瀏覽器中查看詳細錯誤訊息

# 3. 檢查 Program.cs 設定
# 確認服務註冊和中介軟體配置
```

#### 資料庫相關錯誤

```bash
# 1. 檢查連接字串
# 2. 確認資料庫是否存在
docker exec -it tasktracker-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P TaskTracker123! -Q "SELECT name FROM sys.databases"

# 3. 檢查遷移狀態
dotnet ef migrations list
```

---

## 🌐 網路問題

### 無法存取 localhost:5166

#### 檢查清單
```bash
# 1. 確認應用程式正在運行
ps aux | grep dotnet

# 2. 檢查埠號
netstat -an | grep 5166

# 3. 檢查防火牆設定
# Windows: Windows Defender 防火牆
# macOS: 系統偏好設定 > 安全性與隱私權 > 防火牆

# 4. 嘗試其他埠號
dotnet run --urls "https://localhost:5167;http://localhost:5168"
```

### HTTPS 憑證問題

#### 問題現象
瀏覽器顯示「連線不安全」

#### 解決方案
```bash
# 開發環境信任憑證
dotnet dev-certs https --trust

# 清除並重新產生憑證
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

---

## 📱 跨平台問題

### Windows 特定問題

#### PowerShell 執行原則
```powershell
# 設定執行原則
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser

# 執行啟動腳本
.\start-sqlserver.bat
```

#### 路徑分隔符號
```bash
# 使用正確的路徑分隔符號
# Windows: \
# Unix/Linux: /
```

### macOS 特定問題

#### Docker Desktop 權限
```bash
# 確認 Docker Desktop 正在運行
docker version

# 如果權限不足
sudo chmod +x start-sqlserver.sh
./start-sqlserver.sh
```

### Linux 特定問題

#### 套件管理
```bash
# Ubuntu/Debian
sudo apt update
sudo apt install dotnet-sdk-8.0

# CentOS/RHEL
sudo yum install dotnet-sdk-8.0

# Arch Linux
sudo pacman -S dotnet-sdk
```

---

## 🔍 診斷工具

### 日誌檢查

#### ASP.NET Core 日誌
```csharp
// 在 Program.cs 中啟用詳細日誌
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
```

#### Entity Framework 日誌
```csharp
// 在 DbContext 中啟用
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
}
```

### 健康檢查

#### 快速診斷腳本
```bash
#!/bin/bash
echo "=== TaskTracker 診斷報告 ==="
echo "Date: $(date)"
echo ""

echo "1. Docker 狀態:"
docker --version
docker-compose ps

echo ""
echo "2. .NET 狀態:"
dotnet --version
cd TaskTracker && dotnet build --verbosity quiet

echo ""
echo "3. SQL Server 連接:"
docker exec tasktracker-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P TaskTracker123! -Q "SELECT @@VERSION" 2>/dev/null && echo "✓ 連接成功" || echo "✗ 連接失敗"

echo ""
echo "4. 檔案檢查:"
[ -f "TaskTracker/wwwroot/css/output.css" ] && echo "✓ CSS 檔案存在" || echo "✗ CSS 檔案遺失"

echo ""
echo "=== 診斷完成 ==="
```

---

## 📞 獲得協助

### 官方資源
- [ASP.NET Core 疑難排解](https://docs.microsoft.com/aspnet/core/test/troubleshoot)
- [Entity Framework 疑難排解](https://docs.microsoft.com/ef/core/miscellaneous/logging)
- [Docker 疑難排解](https://docs.docker.com/config/daemon/troubleshoot/)

### 社群資源
- [Stack Overflow](https://stackoverflow.com/questions/tagged/asp.net-core)
- [Reddit r/dotnet](https://reddit.com/r/dotnet)
- [GitHub Issues](https://github.com/dotnet/aspnetcore/issues)

---

*最後更新：2024年12月* 