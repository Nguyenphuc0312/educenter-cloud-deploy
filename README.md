# EduCenter Cloud Deploy

Folder gom project để chuẩn bị đưa lên cloud server.

## Cấu trúc

```text
educenter-cloud-deploy/
  frontend/                         Vue frontend, lấy từ nhánh feature/payment-ui
  backend/
    student-attendance-service/      Backend nhóm 2 hiện có
    course-schedule-service/         Chờ copy backend nhóm 1
    payment-report-service/          Chờ copy backend nhóm 3
```

## Chạy frontend

```powershell
cd frontend
npm install
npm run dev -- --host 0.0.0.0
```

## Chạy backend nhóm 2

```powershell
cd backend/student-attendance-service
dotnet restore
dotnet run --urls http://0.0.0.0:5002
```

## Lưu ý khi đưa lên cloud

- Không dùng `localhost` trong `.env.production` của frontend.
- API phải trỏ về IP/domain public của cloud server.
- Mở firewall/security group cho các port cần dùng, ví dụ `5173`, `5001`, `5002`, `5003`, hoặc dùng `80/443`.
- Nếu frontend public bằng HTTPS thì API cũng nên có HTTPS.
