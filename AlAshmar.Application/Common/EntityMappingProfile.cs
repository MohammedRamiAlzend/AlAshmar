using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Domain.Entities.Academic;
using AlAshmar.Domain.Entities.Common;
using AlAshmar.Domain.Entities.Managers;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Domain.Entities.Teachers;
using AlAshmar.Domain.Entities.Users;

namespace AlAshmar.Application.Common;

public class EntityMappingProfile : Profile
{
    public EntityMappingProfile()
    {
        // Common Domain
        CreateMap<AllowableExtention, AllowableExtentionDto>().ReverseMap();
        CreateMap<CreateAllowableExtentionDto, AllowableExtention>();
        CreateMap<UpdateAllowableExtentionDto, AllowableExtention>();

        CreateMap<Attacment, AttacmentDto>().ReverseMap();
        CreateMap<CreateAttacmentDto, Attacment>();
        CreateMap<UpdateAttacmentDto, Attacment>();

        CreateMap<ContactInfo, ContactInfoDto>().ReverseMap();
        CreateMap<CreateContactInfoDto, ContactInfo>();
        CreateMap<UpdateContactInfoDto, ContactInfo>();

        // Users Domain
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>();

        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<Permission, PermissionDto>().ReverseMap();

        // Managers Domain
        CreateMap<Manager, ManagerDto>().ReverseMap();
        CreateMap<CreateManagerDto, Manager>();
        CreateMap<UpdateManagerDto, Manager>();

        CreateMap<ManagerContactInfo, ManagerContactInfoDto>().ReverseMap();
        CreateMap<ManagerAttachment, ManagerAttachmentDto>().ReverseMap();

        // Teachers Domain
        CreateMap<Teacher, TeacherDto>().ReverseMap();
        CreateMap<CreateTeacherDto, Teacher>();
        CreateMap<UpdateTeacherDto, Teacher>();

        CreateMap<TeacherContactInfo, TeacherContactInfoDto>().ReverseMap();
        CreateMap<TeacherAttachment, TeacherAttachmentDto>().ReverseMap();
        CreateMap<TeacherAttencance, TeacherAttencanceDto>().ReverseMap();
        CreateMap<ClassTeacherEnrollment, ClassTeacherEnrollmentDto>().ReverseMap();

        // Students Domain
        // Note: Student entity uses specialized DTOs (StudentListItemDto, StudentDetailDto, etc.)
        // Direct mapping is not used. Use manual mapping in handlers.
        CreateMap<CreateStudentDto, Student>();
        CreateMap<UpdateStudentDto, Student>();

        // StudentAttendance uses the StudentAttendanceDto for CRUD operations
        CreateMap<StudentAttendance, StudentAttendanceDto>().ReverseMap();
        
        // Academic Domain
        CreateMap<Book, BookDto>().ReverseMap();
        CreateMap<CreateBookDto, Book>();
        CreateMap<UpdateBookDto, Book>();

        CreateMap<Hadith, HadithDto>().ReverseMap();
        CreateMap<CreateHadithDto, Hadith>();
        CreateMap<UpdateHadithDto, Hadith>();

        CreateMap<Semester, SemesterDto>().ReverseMap();
        CreateMap<CreateSemesterDto, Semester>();
        CreateMap<UpdateSemesterDto, Semester>();

        CreateMap<PointCategory, PointCategoryDto>().ReverseMap();
        CreateMap<CreatePointCategoryDto, PointCategory>();
        CreateMap<UpdatePointCategoryDto, PointCategory>();

        CreateMap<Point, PointDto>().ReverseMap();
        CreateMap<CreatePointDto, Point>();
        CreateMap<UpdatePointDto, Point>();
    }
}
