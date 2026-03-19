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

const canRetryWithFallback = (error: unknown) => {
  if (!axios.isAxiosError(error)) return false;
  const status = error.response?.status;
  return status === 404 || status === 405;
};

async function getWithFallback<T>(paths: string[]): Promise<T> {
  let lastError: unknown;
  for (const path of paths) {
    try {
      const response = await api.get<T>(path);
      return response.data;
    } catch (error) {
      lastError = error;
      if (!canRetryWithFallback(error)) throw error;
    }
  }
  throw lastError;
}

async function postWithFallback<TResponse, TBody>(paths: string[], data: TBody): Promise<TResponse> {
  let lastError: unknown;
  for (const path of paths) {
    try {
      const response = await api.post<TResponse>(path, data);
      return response.data;
    } catch (error) {
      lastError = error;
      if (!canRetryWithFallback(error)) throw error;
    }
  }
  throw lastError;
}

async function putWithFallback<TResponse, TBody>(paths: string[], data: TBody): Promise<TResponse> {
  let lastError: unknown;
  for (const path of paths) {
    try {
      const response = await api.put<TResponse>(path, data);
      return response.data;
    } catch (error) {
      lastError = error;
      if (!canRetryWithFallback(error)) throw error;
    }
  }
  throw lastError;
}

async function deleteWithFallback(paths: string[]): Promise<void> {
  let lastError: unknown;
  for (const path of paths) {
    try {
      await api.delete(path);
      return;
    } catch (error) {
      lastError = error;
      if (!canRetryWithFallback(error)) throw error;
    }
  }
  throw lastError;
}

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
    getWithFallback<unknown>(['/students', '/student']).then(payload => normalizeArrayResponse<StudentDto>(payload)),
  get: (id: string) =>
    getWithFallback<StudentDto>([`/students/${id}`, `/student/${id}`]),
  create: (data: CreateStudentDto) =>
    postWithFallback<StudentDto, CreateStudentDto>(['/students', '/student'], data),
  update: (id: string, data: UpdateStudentDto) =>
    putWithFallback<StudentDto, UpdateStudentDto>([`/students/${id}`, `/student/${id}`], data),
  delete: (id: string) =>
    deleteWithFallback([`/students/${id}`, `/student/${id}`]),
};

export const teacherApi = {
  list: () =>
    getWithFallback<unknown>(['/teachers', '/teacher']).then(payload => normalizeArrayResponse<TeacherDto>(payload)),
  get: (id: string) =>
    getWithFallback<TeacherDto>([`/teachers/${id}`, `/teacher/${id}`]),
  create: (data: CreateTeacherDto) =>
    postWithFallback<TeacherDto, CreateTeacherDto>(['/teachers', '/teacher'], data),
  update: (id: string, data: UpdateTeacherDto) =>
    putWithFallback<TeacherDto, UpdateTeacherDto>([`/teachers/${id}`, `/teacher/${id}`], data),
  delete: (id: string) =>
    deleteWithFallback([`/teachers/${id}`, `/teacher/${id}`]),
};

export const enrollmentApi = {
  list: () =>
    getWithFallback<unknown>(['/enrollments', '/enrollment']).then(payload => normalizeArrayResponse<EnrollmentDto>(payload)),
  byStudent: (studentId: string) =>
    getWithFallback<unknown>([
      `/enrollments/by-student/${studentId}`,
      `/enrollment/by-student/${studentId}`,
    ]).then(payload => normalizeArrayResponse<EnrollmentDto>(payload)),
  byHalaqa: (halaqaId: string) =>
    getWithFallback<unknown>([
      `/enrollments/by-halaqa/${halaqaId}`,
      `/enrollment/by-halaqa/${halaqaId}`,
    ]).then(payload => normalizeArrayResponse<EnrollmentDto>(payload)),
  create: (data: CreateEnrollmentDto) =>
    postWithFallback<EnrollmentDto, CreateEnrollmentDto>(['/enrollments', '/enrollment'], data),
  delete: (id: string) =>
    deleteWithFallback([`/enrollments/${id}`, `/enrollment/${id}`]),
};
