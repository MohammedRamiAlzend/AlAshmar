# Front-End Implementation Tasks (Based on Current API Endpoints)

## Flutter Team (Students + Teachers)

### 1) Authentication and session bootstrap
- Build login flow using `POST /api/auth/login`.
- Persist token and role, then route users to student/teacher modules.

### 2) Students module
- Implement student list screen with filters and pagination from `GET /api/students/filtered`.
- Implement student details screen from `GET /api/students/{id}`.
- Implement create student form (multipart + optional photos) using `POST /api/students`.
- Implement edit student flow using `PUT /api/students/{id}`.
- Implement delete student action using `DELETE /api/students/{id}`.
- Implement reset/update password flow using `PUT /api/students/{id}/password`.

### 3) Student academic and profile assets
- Show student memorization progress from `GET /api/students/{id}/memorization`.
- Show student attendance history from `GET /api/students/{id}/attendance`.
- Show student points from `GET /api/students/{id}/points`.
- Implement class enrollment flow:
  - Enroll student via `POST /api/students/{id}/enrollments`.
  - List enrollments via `GET /api/students/{id}/enrollments`.
- Implement attachments flow:
  - Upload attachment via `POST /api/students/{id}/attachments`.
  - List attachments via `GET /api/students/{id}/attachments`.

### 4) Teachers module
- Implement teacher list screen with filters and pagination from `GET /api/teachers/filtered`.
- Implement teacher details screen from `GET /api/teachers/{id}`.
- Implement create teacher form using `POST /api/teachers`.
- Implement edit teacher flow using `PUT /api/teachers/{id}`.
- Implement delete teacher action using `DELETE /api/teachers/{id}`.
- Implement reset/update password flow using `PUT /api/teachers/{id}/password`.

### 5) Teacher academic and profile assets
- Show teacher enrollments from `GET /api/teachers/{id}/enrollments`.
- Show teacher attendance from `GET /api/teachers/{id}/attendance`.
- Show points given by teacher from `GET /api/teachers/{id}/points-given`.
- Implement attachments flow:
  - Upload attachment via `POST /api/teachers/{id}/attachments`.
  - List attachments via `GET /api/teachers/{id}/attachments`.

---

## React Team (Quizzes + Management)

### 1) Quiz/form builder and lifecycle
- Implement quiz list and pagination:
  - `GET /api/forms`
  - `GET /api/forms/paged?page=&pageSize=`
- Implement quiz details:
  - `GET /api/forms/{id}`
  - Public access link view `GET /api/forms/access/{accessToken}`
- Implement create/update/delete quiz:
  - `POST /api/forms`
  - `PUT /api/forms/{id}`
  - `DELETE /api/forms/{id}`

### 2) Quiz questions and options
- Implement question CRUD:
  - `GET /api/formquestions`
  - `GET /api/formquestions/{id}`
  - `GET /api/formquestions/by-form/{formId}`
  - `POST /api/formquestions`
  - `PUT /api/formquestions/{id}`
  - `DELETE /api/formquestions/{id}`
- Implement option CRUD:
  - `GET /api/formquestionoptions/{id}`
  - `GET /api/formquestionoptions/by-question/{questionId}`
  - `POST /api/formquestionoptions`
  - `PUT /api/formquestionoptions/{id}`
  - `DELETE /api/formquestionoptions/{id}`

### 3) Quiz responses and review
- Implement response review screens:
  - `GET /api/formresponses/{id}`
  - `GET /api/formresponses/by-form/{formId}`
- Integrate public submission endpoint for external respondents:
  - `POST /api/formresponses/submit`
- Implement response delete action:
  - `DELETE /api/formresponses/{id}`

### 4) Management module
- Implement manager CRUD and password update:
  - `GET /api/managers`
  - `GET /api/managers/{id}`
  - `POST /api/managers`
  - `PUT /api/managers/{id}`
  - `DELETE /api/managers/{id}`
  - `PUT /api/managers/{id}/password`
- Implement course management:
  - `GET /api/courses`
  - `GET /api/courses/by-semester/{semesterId}`
  - `GET /api/courses/{id}`
  - `POST /api/courses`
  - `PUT /api/courses/{id}`
  - `DELETE /api/courses/{id}`
- Implement halaqa management:
  - `GET /api/halaqas`
  - `GET /api/halaqas/by-course/{courseId}`
  - `GET /api/halaqas/{id}`
  - `POST /api/halaqas`
  - `PUT /api/halaqas/{id}`
  - `DELETE /api/halaqas/{id}`

### 5) Reporting dashboards (management-facing)
- Build report dashboards and export flows using `/api/reports/...` endpoints for:
  - students
  - teachers
  - attendance
  - points
  - classes
  - semester overview/statistics
