# TaskTracker Docker 部署指南

完整的容器化部署與管理指南

---

## 🐳 Docker 環境準備

### Docker Desktop 安裝

#### Windows
1. 下載 [Docker Desktop for Windows](https://www.docker.com/products/docker-desktop)
2. 執行安裝程式並重新啟動電腦
3. 確認 WSL 2 後端已啟用
4. 測試安裝：
```powershell
docker --version
docker-compose --version
```

#### macOS
```bash
# 使用 Homebrew 安裝
brew install --cask docker

# 或手動下載安裝
# https://www.docker.com/products/docker-desktop

# 啟動 Docker Desktop 應用程式
open -a Docker

# 測試安裝
docker --version
docker-compose --version
```

#### Linux (Ubuntu/Debian)
```bash
# 移除舊版本
sudo apt-get remove docker docker-engine docker.io containerd runc

# 安裝依賴
sudo apt-get update
sudo apt-get install ca-certificates curl gnupg lsb-release

# 新增 Docker 官方 GPG 金鑰
sudo mkdir -p /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg

# 設定儲存庫
echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

# 安裝 Docker Engine
sudo apt-get update
sudo apt-get install docker-ce docker-ce-cli containerd.io docker-compose-plugin

# 啟動 Docker 服務
sudo systemctl start docker
sudo systemctl enable docker

# 新增使用者到 docker 群組
sudo usermod -aG docker $USER
newgrp docker

# 測試安裝
docker --version
docker compose version
```

---

## 📦 SQL Server 容器部署

### Docker Compose 配置詳解

#### docker-compose.yml 結構
```yaml
version: '3.8'

services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: tasktracker-sqlserver
    hostname: sqlserver
    ports:
      - "1433:1433"
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=TaskTracker123!
      - MSSQL_PID=Express
    volumes:
      - sqlserver_data:/var/opt/mssql
      - ./backups:/var/opt/mssql/backup
    restart: unless-stopped
    healthcheck:
      test: ["CMD-SHELL", "/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P TaskTracker123! -Q 'SELECT 1'"]
      interval: 30s
      timeout: 10s
      retries: 5
      start_period: 60s
    networks:
      - tasktracker-network

volumes:
  sqlserver_data:
    driver: local

networks:
  tasktracker-network:
    driver: bridge
```

#### 環境變數說明
- **ACCEPT_EULA=Y** - 接受 SQL Server 授權條款
- **SA_PASSWORD** - SA 帳戶密碼（必須符合複雜性要求）
- **MSSQL_PID=Express** - 使用 SQL Server Express 版本
- **MSSQL_COLLATION** - 資料庫排序規則（可選）

### 一鍵部署腳本

#### Windows 批次檔 (start-sqlserver.bat)
```batch
@echo off
echo 正在啟動 TaskTracker SQL Server 容器...
echo.

REM 檢查 Docker 是否運行
docker version >nul 2>&1
if errorlevel 1 (
    echo 錯誤：Docker 未運行，請先啟動 Docker Desktop
    pause
    exit /b 1
)

REM 檢查並停止舊容器
docker-compose down

REM 啟動容器
echo 啟動 SQL Server 容器...
docker-compose up -d

REM 等待容器健康檢查
echo 等待 SQL Server 啟動完成...
:wait_loop
docker-compose ps | findstr "healthy" >nul
if errorlevel 1 (
    timeout /t 5 /nobreak >nul
    goto wait_loop
)

echo.
echo ✅ SQL Server 容器已成功啟動！
echo 📊 連接資訊：
echo    伺服器：localhost,1433
echo    使用者：sa
echo    密碼：TaskTracker123!
echo.
pause
```

#### Linux/macOS 腳本 (start-sqlserver.sh)
```bash
#!/bin/bash

echo "🚀 啟動 TaskTracker SQL Server 容器..."
echo

# 檢查 Docker 是否安裝
if ! command -v docker &> /dev/null; then
    echo "❌ 錯誤：未找到 Docker，請先安裝 Docker"
    exit 1
fi

# 檢查 Docker 是否運行
if ! docker info &> /dev/null; then
    echo "❌ 錯誤：Docker 未運行，請先啟動 Docker"
    exit 1
fi

# 檢查並停止舊容器
echo "🔄 清理舊容器..."
docker-compose down

# 啟動容器
echo "📦 啟動 SQL Server 容器..."
docker-compose up -d

# 等待健康檢查
echo "⏳ 等待 SQL Server 啟動完成..."
while ! docker-compose ps | grep -q "healthy"; do
    echo "   等待中..."
    sleep 5
done

echo
echo "✅ SQL Server 容器已成功啟動！"
echo "📊 連接資訊："
echo "   伺服器：localhost,1433"
echo "   使用者：sa"
echo "   密碼：TaskTracker123!"
echo
echo "🔗 測試連接："
echo "   docker exec -it tasktracker-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P TaskTracker123!"
```

---

## 🔧 容器管理

### 基本操作

#### 容器生命週期管理
```bash
# 啟動服務
docker-compose up -d

# 停止服務
docker-compose down

# 重啟服務
docker-compose restart

# 查看服務狀態
docker-compose ps

# 查看即時日誌
docker-compose logs -f sqlserver

# 進入容器
docker exec -it tasktracker-sqlserver bash
```

#### 容器監控
```bash
# 查看容器資源使用
docker stats tasktracker-sqlserver

# 查看容器詳細資訊
docker inspect tasktracker-sqlserver

# 檢查健康狀態
docker-compose ps --format "table {{.Name}}\t{{.Status}}"
```

### 進階操作

#### 容器配置修改
```bash
# 修改 docker-compose.yml 後重新建立
docker-compose down
docker-compose up -d

# 僅重新建立特定服務
docker-compose up -d --force-recreate sqlserver
```

#### 網路管理
```bash
# 查看網路
docker network ls

# 檢查網路詳情
docker network inspect smartplanner2_tasktracker-network

# 測試容器間連接
docker exec tasktracker-sqlserver ping host.docker.internal
```

---

## 💾 資料管理

### Volume 管理

#### 資料持久化
```bash
# 查看 Volume
docker volume ls

# 檢查 Volume 詳情
docker volume inspect smartplanner2_sqlserver_data

# 備份 Volume
docker run --rm -v smartplanner2_sqlserver_data:/data -v $(pwd):/backup alpine tar czf /backup/sqlserver_backup.tar.gz -C /data .

# 還原 Volume
docker run --rm -v smartplanner2_sqlserver_data:/data -v $(pwd):/backup alpine tar xzf /backup/sqlserver_backup.tar.gz -C /data
```

#### 資料庫備份與還原
```bash
# 備份資料庫
docker exec tasktracker-sqlserver /opt/mssql-tools/bin/sqlcmd \
  -S localhost -U sa -P TaskTracker123! \
  -Q "BACKUP DATABASE TaskTrackerDB_Dev TO DISK = '/var/opt/mssql/backup/TaskTracker_$(date +%Y%m%d_%H%M%S).bak' WITH FORMAT, INIT"

# 列出備份檔案
docker exec tasktracker-sqlserver ls -la /var/opt/mssql/backup/

# 還原資料庫
docker exec tasktracker-sqlserver /opt/mssql-tools/bin/sqlcmd \
  -S localhost -U sa -P TaskTracker123! \
  -Q "RESTORE DATABASE TaskTrackerDB_Dev FROM DISK = '/var/opt/mssql/backup/TaskTracker_backup.bak' WITH REPLACE"
```

### 資料庫管理工具連接

#### Azure Data Studio
```json
{
  "serverName": "localhost,1433",
  "authenticationType": "SqlLogin",
  "userName": "sa",
  "password": "TaskTracker123!",
  "database": "TaskTrackerDB_Dev",
  "trustServerCertificate": true
}
```

#### SQL Server Management Studio (SSMS)
```
伺服器名稱：localhost,1433
驗證：SQL Server 驗證
登入：sa
密碼：TaskTracker123!
加密：選用
信任伺服器憑證：是
```

---

## 🚀 生產部署

### 安全性配置

#### 生產環境 docker-compose.yml
```yaml
version: '3.8'

services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: tasktracker-sqlserver-prod
    ports:
      - "1433:1433"
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=${SA_PASSWORD}
      - MSSQL_PID=Express
    volumes:
      - sqlserver_prod_data:/var/opt/mssql
      - ./backups:/var/opt/mssql/backup
      - ./scripts:/var/opt/mssql/scripts
    restart: unless-stopped
    security_opt:
      - no-new-privileges:true
    healthcheck:
      test: ["CMD-SHELL", "/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P ${SA_PASSWORD} -Q 'SELECT 1'"]
      interval: 30s
      timeout: 10s
      retries: 5
      start_period: 60s
    networks:
      - tasktracker-network

volumes:
  sqlserver_prod_data:
    driver: local

networks:
  tasktracker-network:
    driver: bridge
    ipam:
      config:
        - subnet: 172.20.0.0/16
```

#### 環境變數檔案 (.env)
```bash
# 生產環境密碼
SA_PASSWORD=YourStrongPassword123!

# 資料庫設定
MSSQL_PID=Express
MSSQL_COLLATION=Chinese_Taiwan_Stroke_CI_AS

# 應用程式設定
ASPNETCORE_ENVIRONMENT=Production
```

### 效能優化

#### 記憶體限制
```yaml
services:
  sqlserver:
    # ... 其他設定
    deploy:
      resources:
        limits:
          memory: 2G
        reservations:
          memory: 1G
```

#### 日誌管理
```yaml
services:
  sqlserver:
    # ... 其他設定
    logging:
      driver: "json-file"
      options:
        max-size: "10m"
        max-file: "3"
```

---

## 🔍 監控與日誌

### 容器監控

#### Docker 統計資訊
```bash
# 即時監控
docker stats tasktracker-sqlserver --format "table {{.Container}}\t{{.CPUPerc}}\t{{.MemUsage}}\t{{.NetIO}}\t{{.BlockIO}}"

# 監控腳本
#!/bin/bash
while true; do
    clear
    echo "=== TaskTracker 容器監控 ==="
    echo "時間: $(date)"
    echo
    docker stats tasktracker-sqlserver --no-stream --format "table {{.Container}}\t{{.CPUPerc}}\t{{.MemUsage}}\t{{.NetIO}}"
    echo
    echo "按 Ctrl+C 停止監控"
    sleep 5
done
```

#### 健康檢查
```bash
# 檢查容器健康狀態
docker inspect --format='{{.State.Health.Status}}' tasktracker-sqlserver

# 查看健康檢查日誌
docker inspect --format='{{range .State.Health.Log}}{{.Output}}{{end}}' tasktracker-sqlserver
```

### 日誌管理

#### 查看和管理日誌
```bash
# 查看即時日誌
docker-compose logs -f sqlserver

# 查看最近 100 行日誌
docker-compose logs --tail=100 sqlserver

# 查看特定時間範圍的日誌
docker-compose logs --since="2024-01-01T00:00:00" --until="2024-01-02T00:00:00" sqlserver

# 匯出日誌到檔案
docker-compose logs sqlserver > sqlserver_logs_$(date +%Y%m%d).log
```

#### 日誌輪替設定
```yaml
services:
  sqlserver:
    logging:
      driver: "json-file"
      options:
        max-size: "50m"
        max-file: "5"
        labels: "service=sqlserver"
```

---

## 🔒 安全性最佳實務

### 密碼安全
```bash
# 生成強密碼
openssl rand -base64 32

# 使用環境變數存放密碼
export SA_PASSWORD=$(openssl rand -base64 32)
```

### 網路安全
```yaml
# 限制網路存取
services:
  sqlserver:
    networks:
      tasktracker-network:
        ipv4_address: 172.20.0.2
    # 僅允許本地連接
    ports:
      - "127.0.0.1:1433:1433"
```

### 檔案權限
```bash
# 設定適當的檔案權限
chmod 600 .env
chmod 644 docker-compose.yml
chmod +x start-sqlserver.sh
```

---

## 🛠️ 疑難排解

### 常見問題

#### 容器無法啟動
```bash
# 檢查 Docker 日誌
docker-compose logs sqlserver

# 檢查埠號衝突
netstat -tulpn | grep 1433

# 重新建立容器
docker-compose down -v
docker-compose up -d
```

#### 效能問題
```bash
# 檢查資源使用
docker stats tasktracker-sqlserver

# 調整記憶體限制
# 在 docker-compose.yml 中增加 deploy.resources 設定
```

#### 連接問題
```bash
# 測試連接
docker exec tasktracker-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P TaskTracker123! -Q "SELECT @@VERSION"

# 檢查網路
docker network inspect smartplanner2_tasktracker-network
```

---

## 📚 參考資源

- [SQL Server Docker 官方文件](https://hub.docker.com/_/microsoft-mssql-server)
- [Docker Compose 文件](https://docs.docker.com/compose/)
- [Docker 最佳實務](https://docs.docker.com/develop/dev-best-practices/)
- [SQL Server 容器化指南](https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker)

---

*最後更新：2024年12月* 