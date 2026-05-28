# ?? Frontend Integration Guide - Course & Schedule API

## ?? API Base URL
```
https://localhost:7xxx/api
```

---

## ?? Các Endpoint Chính

### Courses (Khóa H?c)
```javascript
GET    /api/courses              // L?y t?t c?
GET    /api/courses/{id}         // L?y theo ID
POST   /api/courses              // T?o m?i
PUT    /api/courses/{id}         // C?p nh?t
DELETE /api/courses/{id}         // Xóa
```

### Classes (L?p H?c)
```javascript
GET    /api/classes              // L?y t?t c?
GET    /api/classes/{id}         // L?y theo ID
GET    /api/classes/course/{courseId}  // L?y l?p c?a khóa h?c
POST   /api/classes              // T?o m?i
PUT    /api/classes/{id}         // C?p nh?t
DELETE /api/classes/{id}         // Xóa
```

### Schedules (L?ch H?c)
```javascript
GET    /api/schedules            // L?y t?t c?
GET    /api/schedules/{id}       // L?y theo ID
GET    /api/schedules/class/{classId}  // L?y l?ch c?a l?p
GET    /api/schedules/day/{dayOfWeek}  // L?y l?ch theo ngày
POST   /api/schedules            // T?o m?i
PUT    /api/schedules/{id}       // C?p nh?t
DELETE /api/schedules/{id}       // Xóa
```

---

## ?? Ví D? Frontend (React)

### 1. L?y Danh Sách Khóa H?c

```javascript
// CoursesService.js
export const getCourses = async () => {
  try {
    const response = await fetch('https://localhost:7xxx/api/courses', {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json'
      }
    });
    const data = await response.json();
    if (data.success) {
      return data.data;
    } else {
      throw new Error(data.message);
    }
  } catch (error) {
    console.error('Error:', error);
    throw error;
  }
};

// Component
import { useEffect, useState } from 'react';
import { getCourses } from './services/CoursesService';

function CoursesList() {
  const [courses, setCourses] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    getCourses()
      .then(data => {
        setCourses(data);
        setLoading(false);
      })
      .catch(error => {
        console.error(error);
        setLoading(false);
      });
  }, []);

  if (loading) return <p>?ang t?i...</p>;

  return (
    <div>
      <h2>Danh Sách Khóa H?c</h2>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Tên Khóa H?c</th>
            <th>Mô T?</th>
            <th>Tín Ch?</th>
            <th>Tr?ng Thái</th>
          </tr>
        </thead>
        <tbody>
          {courses.map(course => (
            <tr key={course.courseID}>
              <td>{course.courseID}</td>
              <td>{course.courseName}</td>
              <td>{course.description}</td>
              <td>{course.credits}</td>
              <td>{course.status}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default CoursesList;
```

---

### 2. T?o Khóa H?c M?i

```javascript
// CoursesService.js
export const createCourse = async (courseData) => {
  try {
    const response = await fetch('https://localhost:7xxx/api/courses', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(courseData)
    });
    const data = await response.json();
    if (data.success) {
      return data.data;
    } else {
      throw new Error(data.message);
    }
  } catch (error) {
    console.error('Error:', error);
    throw error;
  }
};

// Component - Form t?o khóa h?c
import { useState } from 'react';
import { createCourse } from './services/CoursesService';

function CreateCourseForm() {
  const [formData, setFormData] = useState({
    courseName: '',
    description: '',
    credits: 0,
    status: 'Active'
  });

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const newCourse = await createCourse(formData);
      alert('T?o khóa h?c thành công!');
      setFormData({ courseName: '', description: '', credits: 0, status: 'Active' });
    } catch (error) {
      alert('L?i: ' + error.message);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <input
        placeholder="Tên khóa h?c"
        value={formData.courseName}
        onChange={e => setFormData({...formData, courseName: e.target.value})}
        required
      />
      <textarea
        placeholder="Mô t?"
        value={formData.description}
        onChange={e => setFormData({...formData, description: e.target.value})}
      />
      <input
        type="number"
        placeholder="Tín ch?"
        value={formData.credits}
        onChange={e => setFormData({...formData, credits: parseInt(e.target.value)})}
        required
      />
      <button type="submit">T?o Khóa H?c</button>
    </form>
  );
}

export default CreateCourseForm;
```

---

### 3. L?y Danh Sách L?p c?a Khóa H?c

```javascript
// ClassesService.js
export const getClassesByCourse = async (courseId) => {
  try {
    const response = await fetch(`https://localhost:7xxx/api/classes/course/${courseId}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json'
      }
    });
    const data = await response.json();
    if (data.success) {
      return data.data;
    } else {
      throw new Error(data.message);
    }
  } catch (error) {
    console.error('Error:', error);
    throw error;
  }
};

// Component
import { useEffect, useState } from 'react';
import { getClassesByCourse } from './services/ClassesService';

function CourseClasses({ courseId }) {
  const [classes, setClasses] = useState([]);

  useEffect(() => {
    getClassesByCourse(courseId)
      .then(data => setClasses(data))
      .catch(error => console.error(error));
  }, [courseId]);

  return (
    <div>
      <h3>L?p c?a Khóa H?c {courseId}</h3>
      <ul>
        {classes.map(cls => (
          <li key={cls.classID}>
            {cls.classCode} - {cls.instructor} ({cls.enrolledStudents}/{cls.capacity})
          </li>
        ))}
      </ul>
    </div>
  );
}

export default CourseClasses;
```

---

### 4. L?y L?ch H?c c?a L?p

```javascript
// SchedulesService.js
export const getSchedulesByClass = async (classId) => {
  try {
    const response = await fetch(`https://localhost:7xxx/api/schedules/class/${classId}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json'
      }
    });
    const data = await response.json();
    if (data.success) {
      return data.data;
    } else {
      throw new Error(data.message);
    }
  } catch (error) {
    console.error('Error:', error);
    throw error;
  }
};

// Component
import { useEffect, useState } from 'react';
import { getSchedulesByClass } from './services/SchedulesService';

function ClassSchedules({ classId }) {
  const [schedules, setSchedules] = useState([]);

  useEffect(() => {
    getSchedulesByClass(classId)
      .then(data => setSchedules(data))
      .catch(error => console.error(error));
  }, [classId]);

  return (
    <div>
      <h3>L?ch H?c</h3>
      <table>
        <thead>
          <tr>
            <th>Ngày</th>
            <th>Th?i Gian</th>
            <th>Phòng</th>
          </tr>
        </thead>
        <tbody>
          {schedules.map(schedule => (
            <tr key={schedule.scheduleID}>
              <td>{schedule.dayOfWeek}</td>
              <td>{schedule.startTime} - {schedule.endTime}</td>
              <td>{schedule.room}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default ClassSchedules;
```

---

### 5. S? d?ng axios (Alternative)

```javascript
// services/apiClient.js
import axios from 'axios';

const apiClient = axios.create({
  baseURL: 'https://localhost:7xxx/api',
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json'
  }
});

// Interceptor ?? x? lý response
apiClient.interceptors.response.use(
  response => response.data,
  error => {
    console.error('API Error:', error.response?.data);
    throw error;
  }
);

export default apiClient;

// S? d?ng
import apiClient from './services/apiClient';

// L?y t?t c? khóa h?c
const courses = await apiClient.get('/courses');

// T?o khóa h?c m?i
const newCourse = await apiClient.post('/courses', {
  courseName: 'New Course',
  description: 'Description',
  credits: 3,
  status: 'Active'
});

// C?p nh?t khóa h?c
const updated = await apiClient.put('/courses/1', {
  courseID: 1,
  courseName: 'Updated Course',
  description: 'Updated Description',
  credits: 4,
  status: 'Active'
});

// Xóa khóa h?c
await apiClient.delete('/courses/1');
```

---

### 6. S? d?ng React Query (Best Practice)

```javascript
// hooks/useCourses.js
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import apiClient from '../services/apiClient';

export const useCourses = () => {
  return useQuery({
    queryKey: ['courses'],
    queryFn: () => apiClient.get('/courses')
  });
};

export const useCourseById = (id) => {
  return useQuery({
    queryKey: ['courses', id],
    queryFn: () => apiClient.get(`/courses/${id}`)
  });
};

export const useCreateCourse = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (courseData) => apiClient.post('/courses', courseData),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['courses'] });
    }
  });
};

export const useUpdateCourse = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, data }) => apiClient.put(`/courses/${id}`, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['courses'] });
    }
  });
};

export const useDeleteCourse = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id) => apiClient.delete(`/courses/${id}`),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['courses'] });
    }
  });
};

// S? d?ng trong component
import { useCourses, useCreateCourse } from '../hooks/useCourses';

function CoursesPage() {
  const { data: courses, isLoading } = useCourses();
  const { mutate: createCourse } = useCreateCourse();

  if (isLoading) return <p>?ang t?i...</p>;

  return (
    <div>
      <h2>Khóa H?c</h2>
      <button onClick={() => createCourse({
        courseName: 'New Course',
        description: 'Description',
        credits: 3,
        status: 'Active'
      })}>
        T?o Khóa H?c
      </button>
      {/* Hi?n th? danh sách */}
    </div>
  );
}

export default CoursesPage;
```

---

## ??? CORS Configuration

API ?ã ???c c?u hình CORS cho phép t?t c? origins. N?u v?n g?p CORS error, thêm vào frontend:

```javascript
// N?u s? d?ng fetch
fetch('https://localhost:7xxx/api/courses', {
  method: 'GET',
  headers: {
    'Content-Type': 'application/json'
  },
  mode: 'cors',
  credentials: 'include'
});
```

---

## ?? JWT Authentication (Future)

Khi backend thêm JWT, frontend c?n:

```javascript
// L?u token sau khi login
localStorage.setItem('token', response.token);

// Thêm token vào header
const headers = {
  'Content-Type': 'application/json',
  'Authorization': `Bearer ${localStorage.getItem('token')}`
};

fetch('https://localhost:7xxx/api/courses', {
  method: 'GET',
  headers: headers
});
```

---

## ?? Data Models

### Course
```javascript
{
  courseID: 1,
  courseName: "C# Basics",
  description: "Learn C# fundamentals",
  credits: 3,
  status: "Active",
  createdDate: "2024-01-15T10:30:00",
  updatedDate: "2024-01-15T10:30:00",
  classes: []
}
```

### Class
```javascript
{
  classID: 1,
  classCode: "CS001",
  courseID: 1,
  capacity: 30,
  enrolledStudents: 25,
  instructor: "Nguy?n V?n A",
  status: "Active",
  createdDate: "2024-01-15T10:30:00",
  updatedDate: "2024-01-15T10:30:00",
  course: { ... },
  schedules: []
}
```

### Schedule
```javascript
{
  scheduleID: 1,
  classID: 1,
  startDate: "2024-01-15T00:00:00",
  endDate: "2024-05-15T00:00:00",
  dayOfWeek: "Monday",
  startTime: "08:00:00",
  endTime: "10:00:00",
  room: "A101",
  status: "Active",
  createdDate: "2024-01-15T10:30:00",
  updatedDate: "2024-01-15T10:30:00",
  class: { ... }
}
```

---

## ?? Deployment Notes

1. Thay `https://localhost:7xxx` b?ng URL th?c t? c?a backend
2. S? d?ng environment variables ?? qu?n lý URL
3. Thêm error handling và retry logic
4. Implement loading states và user feedback

---

**Chúc b?n phát tri?n thành công! ??**
