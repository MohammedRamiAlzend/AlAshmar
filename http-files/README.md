# HTTP Test Files

This directory contains `.http` files for testing the **AlAshmar API** endpoints using REST Client (VS Code extension, JetBrains IDE, or any compatible tool).

## Prerequisites

- **VS Code**: Install the [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) extension.
- **JetBrains Rider / IntelliJ**: Built-in HTTP client support.
- The API server must be running on `http://localhost:5107`.

## Authentication

All endpoints (except login and public form endpoints) require a **JWT Bearer token**.

**To get a token:**
1. Open `auth.http`
2. Run the `Login` request
3. Copy the `token` value from the JSON response
4. Paste it as the value of `@authToken` in the file you want to test

---

## Files

### Per-Controller Files

| File | Description |
|------|-------------|
| [`auth.http`](auth.http) | Login endpoint |
| [`students.http`](students.http) | Student CRUD, enrollments, attachments, and academic data |
| [`teachers.http`](teachers.http) | Teacher CRUD, enrollments, attachments, and academic data |
| [`managers.http`](managers.http) | Manager CRUD and attachments |
| [`courses.http`](courses.http) | Course (semester event) CRUD |
| [`halaqas.http`](halaqas.http) | Halaqa (class) CRUD |
| [`forms.http`](forms.http) | Forms, questions, question options, and responses |
| [`reports-students.http`](reports-students.http) | Student reports (daily / weekly / monthly / semester) |
| [`reports-teachers.http`](reports-teachers.http) | Teacher reports (daily / weekly / monthly / semester) |
| [`reports-semesters.http`](reports-semesters.http) | Semester overview, statistics, top students/teachers |
| [`reports-attendance.http`](reports-attendance.http) | Attendance reports (daily / weekly / monthly / overview) |
| [`reports-points.http`](reports-points.http) | Points reports (daily / weekly / monthly / overview) |
| [`reports-classes.http`](reports-classes.http) | Class reports (daily / weekly / monthly / semester) |

### Flow Files

End-to-end test flows that combine multiple API calls in a logical order:

| File | Description |
|------|-------------|
| [`flow-register-student.http`](flow-register-student.http) | Complete flow: login → create course → create halaqa → register student → enroll → verify |
| [`flow-daily-report.http`](flow-daily-report.http) | Complete flow: login → student daily report → teacher daily report → class daily report → attendance → points |
| [`flow-monthly-report.http`](flow-monthly-report.http) | Complete flow: login → student monthly → teacher monthly → class monthly → attendance monthly → points monthly → semester overview |

---

## Variables

Each file declares variables at the top. Update the placeholder GUIDs with real IDs from your database:

```http
@authToken  = YOUR_JWT_TOKEN_HERE      # Replace after login
@studentId  = 00000000-0000-...        # Replace with real student GUID
@teacherId  = 00000000-0000-...        # Replace with real teacher GUID
@classId    = 00000000-0000-...        # Replace with real halaqa GUID
@semesterId = 00000000-0000-...        # Replace with real semester GUID
```

---

## Quick Start

1. Start the API server (`dotnet run` from the `AlAshmar` project).
2. Open `auth.http` and run the **Login** request.
3. Copy the `token` from the response body.
4. Open any other `.http` file, paste the token into `@authToken`, then run requests.

For a full walkthrough, start with the **flow files** which guide you step-by-step.
