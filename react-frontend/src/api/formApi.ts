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
    api.get<PaginatedResult<FormDto>>(`/forms?page=${page}&pageSize=${pageSize}`).then(r => r.data),
  get: (id: string) =>
    api.get<FormDto>(`/forms/${id}`).then(r => r.data),
  getByToken: (token: string) =>
    api.get<FormDto>(`/forms/by-access-token/${token}`).then(r => r.data),
  create: (data: CreateFormDto) =>
    api.post<FormDto>('/forms', data).then(r => r.data),
  update: (id: string, data: UpdateFormDto) =>
    api.put<FormDto>(`/forms/${id}`, data).then(r => r.data),
  delete: (id: string) =>
    api.delete(`/forms/${id}`),
};

export const questionApi = {
  create: (data: CreateFormQuestionDto) =>
    api.post<FormQuestionDto>('/form-questions', data).then(r => r.data),
  update: (id: string, data: UpdateFormQuestionDto) =>
    api.put<FormQuestionDto>(`/form-questions/${id}`, data).then(r => r.data),
  delete: (id: string) =>
    api.delete(`/form-questions/${id}`),
};

export const optionApi = {
  create: (data: CreateFormQuestionOptionDto & { questionId: string }) =>
    api.post<FormQuestionOptionDto>('/form-question-options', data).then(r => r.data),
  update: (id: string, data: UpdateFormQuestionOptionDto) =>
    api.put<FormQuestionOptionDto>(`/form-question-options/${id}`, data).then(r => r.data),
  delete: (id: string) =>
    api.delete(`/form-question-options/${id}`),
};

export const responseApi = {
  submit: (data: SubmitFormResponseDto) =>
    api.post<FormResponseDto>('/form-responses', data).then(r => r.data),
  list: (formId: string) =>
    api.get<FormResponseDto[]>(`/form-responses?formId=${formId}`).then(r => r.data),
};
