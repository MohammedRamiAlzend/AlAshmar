export interface SemesterDto {
  id: string;
  name: string;
  startDate: string;
  endDate: string;
}

export interface CourseDto {
  id: string;
  eventName: string;
  semesterId: string;
  semester?: SemesterDto;
  halaqas: HalaqaDto[];
}

export interface HalaqaDto {
  id: string;
  className: string;
  courseId: string;
  course?: CourseDto;
}

export interface StudentDto {
  id: string;
  name: string;
  email: string;
  phone: string;
  nationalId: string;
}

export interface TeacherDto {
  id: string;
  name: string;
  email: string;
  phone: string;
  nationalId: string;
}

export interface EnrollmentDto {
  id: string;
  studentId: string;
  halaqaId: string;
  student?: StudentDto;
  halaqa?: HalaqaDto;
}

export interface CreateSemesterDto {
  name: string;
  startDate: string;
  endDate: string;
}

export interface UpdateSemesterDto {
  name: string;
  startDate: string;
  endDate: string;
}

export interface CreateCourseDto {
  eventName: string;
  semesterId: string;
}

export interface UpdateCourseDto {
  eventName: string;
}

export interface CreateHalaqaDto {
  className: string;
  courseId: string;
}

export interface UpdateHalaqaDto {
  className: string;
}

export interface CreateStudentDto {
  name: string;
  email: string;
  phone: string;
  nationalId: string;
}

export interface UpdateStudentDto {
  name: string;
  email: string;
  phone: string;
  nationalId: string;
}

export interface CreateTeacherDto {
  name: string;
  email: string;
  phone: string;
  nationalId: string;
}

export interface UpdateTeacherDto {
  name: string;
  email: string;
  phone: string;
  nationalId: string;
}

export interface CreateEnrollmentDto {
  studentId: string;
  halaqaId: string;
}
