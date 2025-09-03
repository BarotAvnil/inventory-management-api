@echo off
echo Testing Inventory Management API
echo ================================

echo.
echo 1. Testing Login...
curl -X POST "http://localhost:5083/api/Auth/login" ^
     -H "Content-Type: application/json" ^
     -d "{\"email\": \"admin@inventorymanagement.com\", \"password\": \"Admin123!\"}"

echo.
echo.
echo 2. Testing if server is running...
curl -X GET "http://localhost:5083/" -I

echo.
echo.
echo Instructions:
echo 1. Make sure the API is running with: dotnet run
echo 2. Open browser to: http://localhost:5083
echo 3. Use Postman with the collection file for full testing
echo.
pause
