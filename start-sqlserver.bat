@echo off
echo 正在啟動 SQL Server Docker 容器...
docker-compose up -d

echo.
echo 等待 SQL Server 容器完全啟動...
timeout /t 30 /nobreak > nul

echo.
echo 檢查 SQL Server 容器狀態...
docker-compose ps

echo.
echo SQL Server 已啟動完成！
echo 連接資訊：
echo   伺服器: localhost,1433
echo   使用者: sa
echo   密碼: TaskTracker123!
echo   資料庫: TaskTrackerDB (生產) / TaskTrackerDB_Dev (開發)
echo.
echo 若要停止 SQL Server，請執行: docker-compose down
echo 若要查看 SQL Server 日誌，請執行: docker-compose logs sqlserver
pause 