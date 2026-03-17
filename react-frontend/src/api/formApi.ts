import axios from 'axios';
import { API_URL } from '../config';
import type {
  FormDto, CreateFormDto, UpdateFormDto,
  FormQuestionDto, CreateFormQuestionDto, UpdateFormQuestionDto,
  FormQuestionOptionDto, CreateFormQuestionOptionDto, UpdateFormQuestionOptionDto,
  FormResponseDto, SubmitFormResponseDto, PaginatedResult,
} from '../types/form';

const api = axios.create({ baseURL: API_URL });

export const formApi = {
  list: (page = 1, pageSize = 20) =>
    api.get<PaginatedResult<FormDto>>(`/api/forms?page=${page}&pageSize=${pageSize}`).then(r => r.data),
  get: (id: string) =>
    api.get<FormDto>(`/api/forms/${id}`).then(r => r.data),
  getByToken: (token: string) =>
    api.get<FormDto>(`/api/forms/by-access-token/${token}`).then(r => r.data),
  create: (data: CreateFormDto) =>
    api.post<FormDto>('/api/forms', data).then(r => r.data),
  update: (id: string, data: UpdateFormDto) =>
    api.put<FormDto>(`/api/forms/${id}`, data).then(r => r.data),
  delete: (id: string) =>
    api.delete(`/api/forms/${id}`),
};

export const questionApi = {
  create: (data: CreateFormQuestionDto) =>
    api.post<FormQuestionDto>('/api/form-questions', data).then(r => r.data),
  update: (id: string, data: UpdateFormQuestionDto) =>
    api.put<FormQuestionDto>(`/api/form-questions/${id}`, data).then(r => r.data),
  delete: (id: string) =>
    api.delete(`/api/form-questions/${id}`),
};

export const optionApi = {
  create: (data: CreateFormQuestionOptionDto & { questionId: string }) =>
    api.post<FormQuestionOptionDto>('/api/form-question-options', data).then(r => r.data),
  update: (id: string, data: UpdateFormQuestionOptionDto) =>
    api.put<FormQuestionOptionDto>(`/api/form-question-options/${id}`, data).then(r => r.data),
  delete: (id: string) =>
    api.delete(`/api/form-question-options/${id}`),
};

export const responseApi = {
  submit: (data: SubmitFormResponseDto) =>
    api.post<FormResponseDto>('/api/form-responses', data).then(r => r.data),
  list: (formId: string) =>
    api.get<FormResponseDto[]>(`/api/form-responses?formId=${formId}`).then(r => r.data),
};
