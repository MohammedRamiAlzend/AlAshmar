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
