import axios from 'axios';
import { API_URL } from '../config';
import { useAuthStore } from '../store/authStore';
import type {
  SemesterDto, CreateSemesterDto, UpdateSemesterDto,
  CourseDto, CreateCourseDto, UpdateCourseDto,
  HalaqaDto, CreateHalaqaDto, UpdateHalaqaDto,
  StudentDto, CreateStudentDto, UpdateStudentDto,
  TeacherDto, CreateTeacherDto, UpdateTeacherDto,
  EnrollmentDto, CreateEnrollmentDto,
} from '../types/academic';

const api = axios.create({ baseURL: API_URL });

function normalizeArrayResponse<T>(payload: unknown): T[] {
  if (Array.isArray(payload)) {
    return payload as T[];
  }

  if (payload && typeof payload === 'object') {
    const record = payload as Record<string, unknown>;
    const candidate = record.items ?? record.data ?? record.result ?? record.value;
    if (Array.isArray(candidate)) {
      return candidate as T[];
    }
  }

  return [];
}

api.interceptors.request.use(config => {
  const token = useAuthStore.getState().token;
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

api.interceptors.response.use(
  res => res,
  err => {
    if (err.response?.status === 401) {
      useAuthStore.getState().logout();
      window.location.href = '/login';
    }
    return Promise.reject(err);
  }
);

export const semesterApi = {
  list: () =>
    api.get<unknown>('/semesters').then(r => normalizeArrayResponse<SemesterDto>(r.data)),
  get: (id: string) =>
    api.get<SemesterDto>(`/semesters/${id}`).then(r => r.data),
  create: (data: CreateSemesterDto) =>
    api.post<SemesterDto>('/semesters', data).then(r => r.data),
  update: (id: string, data: UpdateSemesterDto) =>
    api.put<SemesterDto>(`/semesters/${id}`, data).then(r => r.data),
  delete: (id: string) =>
    api.delete(`/semesters/${id}`),
};

export const courseApi = {
  list: () =>
    api.get<unknown>('/courses').then(r => normalizeArrayResponse<CourseDto>(r.data)),
  bySemester: (semesterId: string) =>
    api.get<unknown>(`/courses/by-semester/${semesterId}`).then(r => normalizeArrayResponse<CourseDto>(r.data)),
  get: (id: string) =>
    api.get<CourseDto>(`/courses/${id}`).then(r => r.data),
  create: (data: CreateCourseDto) =>
    api.post<CourseDto>('/courses', data).then(r => r.data),
  update: (id: string, data: UpdateCourseDto) =>
    api.put<CourseDto>(`/courses/${id}`, data).then(r => r.data),
  delete: (id: string) =>
    api.delete(`/courses/${id}`),
};

export const halaqaApi = {
  list: () =>
    api.get<unknown>('/halaqas').then(r => normalizeArrayResponse<HalaqaDto>(r.data)),
  byCourse: (courseId: string) =>
    api.get<unknown>(`/halaqas/by-course/${courseId}`).then(r => normalizeArrayResponse<HalaqaDto>(r.data)),
  get: (id: string) =>
    api.get<HalaqaDto>(`/halaqas/${id}`).then(r => r.data),
  create: (data: CreateHalaqaDto) =>
    api.post<HalaqaDto>('/halaqas', data).then(r => r.data),
  update: (id: string, data: UpdateHalaqaDto) =>
    api.put<HalaqaDto>(`/halaqas/${id}`, data).then(r => r.data),
  delete: (id: string) =>
    api.delete(`/halaqas/${id}`),
};

export const studentApi = {
  list: () =>
    api.get<unknown>('/students').then(r => normalizeArrayResponse<StudentDto>(r.data)),
  get: (id: string) =>
    api.get<StudentDto>(`/students/${id}`).then(r => r.data),
  create: (data: CreateStudentDto) =>
    api.post<StudentDto>('/students', data).then(r => r.data),
  update: (id: string, data: UpdateStudentDto) =>
    api.put<StudentDto>(`/students/${id}`, data).then(r => r.data),
  delete: (id: string) =>
    api.delete(`/students/${id}`),
};

export const teacherApi = {
  list: () =>
    api.get<unknown>('/teachers').then(r => normalizeArrayResponse<TeacherDto>(r.data)),
  get: (id: string) =>
    api.get<TeacherDto>(`/teachers/${id}`).then(r => r.data),
  create: (data: CreateTeacherDto) =>
    api.post<TeacherDto>('/teachers', data).then(r => r.data),
  update: (id: string, data: UpdateTeacherDto) =>
    api.put<TeacherDto>(`/teachers/${id}`, data).then(r => r.data),
  delete: (id: string) =>
    api.delete(`/teachers/${id}`),
};

export const enrollmentApi = {
  list: () =>
    api.get<unknown>('/enrollments').then(r => normalizeArrayResponse<EnrollmentDto>(r.data)),
  byStudent: (studentId: string) =>
    api.get<unknown>(`/enrollments/by-student/${studentId}`).then(r => normalizeArrayResponse<EnrollmentDto>(r.data)),
  byHalaqa: (halaqaId: string) =>
    api.get<unknown>(`/enrollments/by-halaqa/${halaqaId}`).then(r => normalizeArrayResponse<EnrollmentDto>(r.data)),
  create: (data: CreateEnrollmentDto) =>
    api.post<EnrollmentDto>('/enrollments', data).then(r => r.data),
  delete: (id: string) =>
    api.delete(`/enrollments/${id}`),
};
