# EduCenter Frontend

Vue 3 frontend chung cho de tai he thong quan ly khoa hoc va hoc vien trung tam dao tao.

## Tech Stack

- Vue 3
- Vite
- Ant Design Vue
- Axios

## Module Ownership

- Team 1 - Course & Schedule Service: `src/modules/courses`
- Team 2 - Student, Attendance & Results Service: `src/modules/students`
- Team 3 - Payment & Report Service: `src/modules/payments`

Dung chung:

- App shell / menu: `src/App.vue`
- Home page: `src/views/HomeView.vue`
- API base URL: `src/config/apiBaseUrls.js`
- HTTP client: `src/services/httpClient.js`
- Global styles: `src/assets/main.css`

## API Base URL

Mac dinh frontend goi thang tung service:

```env
VITE_COURSE_API_URL=http://localhost:5001/api
VITE_STUDENT_API_URL=http://localhost:5002/api
VITE_PAYMENT_API_URL=http://localhost:5003/api
```

Neu demo tren nhieu may, doi `localhost` thanh IP LAN tuong ung va luu vao `.env.local`.

## Run

```sh
npm install
npm run dev -- --host 0.0.0.0
```

## Build

```sh
npm run build
```

## Collaboration Rules

- Moi nhom chi code trong module cua minh.
- Khong sua `src/App.vue`, `src/config/apiBaseUrls.js`, `src/services/httpClient.js` neu chua thong nhat ca 3 nhom.
- Khong commit `node_modules`, `dist`, file log, hoac `.env.local`.
