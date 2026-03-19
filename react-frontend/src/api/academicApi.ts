import axios from 'axios';
import { API_URL } from '../config';
import { useAuthStore } from '../store/authStore';
import type {
  SemesterDto, CreateSemesterDto, UpdateSemesterDto,
  CourseDto, CreateCourseDto, UpdateCourseDto,
  HalaqaDto, CreateHalaqaDto, UpdateHalaqaDto,
} from '../types/academic';

const api = axios.create({ baseURL: API_URL });

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
    api.get<SemesterDto[]>('/semesters').then(r => r.data),
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
    api.get<CourseDto[]>('/courses').then(r => r.data),
  bySemester: (semesterId: string) =>
    api.get<CourseDto[]>(`/courses/by-semester/${semesterId}`).then(r => r.data),
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
    api.get<HalaqaDto[]>('/halaqas').then(r => r.data),
  byCourse: (courseId: string) =>
    api.get<HalaqaDto[]>(`/halaqas/by-course/${courseId}`).then(r => r.data),
  get: (id: string) =>
    api.get<HalaqaDto>(`/halaqas/${id}`).then(r => r.data),
  create: (data: CreateHalaqaDto) =>
    api.post<HalaqaDto>('/halaqas', data).then(r => r.data),
  update: (id: string, data: UpdateHalaqaDto) =>
    api.put<HalaqaDto>(`/halaqas/${id}`, data).then(r => r.data),
  delete: (id: string) =>
    api.delete(`/halaqas/${id}`),
};
