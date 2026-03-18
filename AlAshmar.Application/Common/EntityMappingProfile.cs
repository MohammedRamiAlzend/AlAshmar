using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Domain.Entities.Academic;
using AlAshmar.Domain.Entities.Common;
using AlAshmar.Domain.Entities.Forms;
using AlAshmar.Domain.Entities.Managers;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Domain.Entities.Teachers;
using AlAshmar.Domain.Entities.Users;

namespace AlAshmar.Application.Common;

public class EntityMappingProfile : Profile
{
    public EntityMappingProfile()
    {

        CreateMap<AllowableExtension, AllowableExtensionDto>().ReverseMap();
        CreateMap<CreateAllowableExtensionDto, AllowableExtension>();
        CreateMap<UpdateAllowableExtensionDto, AllowableExtension>();

        CreateMap<Attachment, AttachmentDto>().ReverseMap();
        CreateMap<CreateAttachmentDto, Attachment>();
        CreateMap<UpdateAttachmentDto, Attachment>();

        CreateMap<ContactInfo, ContactInfoDto>().ReverseMap();
        CreateMap<CreateContactInfoDto, ContactInfo>();
        CreateMap<UpdateContactInfoDto, ContactInfo>();

        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>();

        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<Permission, PermissionDto>().ReverseMap();

        CreateMap<Manager, ManagerDto>().ReverseMap();
        CreateMap<CreateManagerDto, Manager>();
        CreateMap<UpdateManagerDto, Manager>();

        CreateMap<ManagerContactInfo, ManagerContactInfoDto>().ReverseMap();
        CreateMap<ManagerAttachment, ManagerAttachmentDto>().ReverseMap();

        CreateMap<Teacher, TeacherDto>().ReverseMap();
        CreateMap<CreateTeacherDto, Teacher>();
        CreateMap<UpdateTeacherDto, Teacher>();

        CreateMap<TeacherContactInfo, TeacherContactInfoDto>().ReverseMap();
        CreateMap<TeacherAttachment, TeacherAttachmentDto>().ReverseMap();
        CreateMap<TeacherAttendance, TeacherAttendanceDto>().ReverseMap();
        CreateMap<ClassTeacherEnrollment, ClassTeacherEnrollmentDto>().ReverseMap();

        CreateMap<CreateStudentDto, Student>();
        CreateMap<UpdateStudentDto, Student>();

        CreateMap<StudentAttendance, StudentAttendanceDto>().ReverseMap();

        CreateMap<Book, BookDto>().ReverseMap();
        CreateMap<CreateBookDto, Book>();
        CreateMap<UpdateBookDto, Book>();

        CreateMap<Hadith, HadithDto>().ReverseMap();
        CreateMap<CreateHadithDto, Hadith>();
        CreateMap<UpdateHadithDto, Hadith>();

        CreateMap<Semester, SemesterDto>().ReverseMap();
        CreateMap<CreateSemesterDto, Semester>();
        CreateMap<UpdateSemesterDto, Semester>();

        CreateMap<Course, CourseDto>().ReverseMap();
        CreateMap<CreateCourseDto, Course>();
        CreateMap<UpdateCourseDto, Course>();

        CreateMap<Halaqa, HalaqaDto>().ReverseMap();
        CreateMap<CreateHalaqaDto, Halaqa>();
        CreateMap<UpdateHalaqaDto, Halaqa>();

        CreateMap<PointCategory, PointCategoryDto>().ReverseMap();
        CreateMap<CreatePointCategoryDto, PointCategory>();
        CreateMap<UpdatePointCategoryDto, PointCategory>();

        CreateMap<Point, PointDto>().ReverseMap();
        CreateMap<CreatePointDto, Point>();
        CreateMap<UpdatePointDto, Point>();

        CreateMap<Form, FormDto>()
            .ReverseMap()
            .ForMember(dest => dest.AccessToken, opt => opt.Ignore())
            .ForMember(dest => dest.Questions, opt => opt.Ignore());
        CreateMap<CreateFormDto, FormDto>();
        CreateMap<UpdateFormDto, FormDto>();
        CreateMap<CreateFormDto, Form>()
            .ForMember(dest => dest.AccessToken, opt => opt.Ignore());
        CreateMap<UpdateFormDto, Form>()
            .ForMember(dest => dest.AccessToken, opt => opt.Ignore());


        CreateMap<FormQuestion, FormQuestionDto>().ReverseMap();
        CreateMap<CreateFormQuestionDto, FormQuestionDto>();
        CreateMap<CreateFormQuestionDto, FormQuestion>()
            .ForMember(dest => dest.Options, opt => opt.Ignore());
        CreateMap<UpdateFormQuestionDto, FormQuestionDto>();
        CreateMap<UpdateFormQuestionDto, FormQuestion>();

        CreateMap<FormQuestionOption, FormQuestionOptionDto>().ReverseMap();
        CreateMap<CreateFormQuestionOptionDto, FormQuestionOptionDto>();
        CreateMap<CreateFormQuestionOptionDto, FormQuestionOption>();
        CreateMap<UpdateFormQuestionOptionDto, FormQuestionOptionDto>();
        CreateMap<UpdateFormQuestionOptionDto, FormQuestionOption>();

        CreateMap<FormResponse, FormResponseDto>().ReverseMap();

        CreateMap<FormAnswer, FormAnswerDto>()
            .ForMember(dest => dest.SelectedOptionIds,
                opt => opt.MapFrom(src => src.SelectedOptions.Select(so => so.FormQuestionOptionId).ToList()))
            .ReverseMap()
            .ForMember(dest => dest.SelectedOptions, opt => opt.Ignore());
    }
}
