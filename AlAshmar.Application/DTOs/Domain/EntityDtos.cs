namespace AlAshmar.Application.DTOs.Domain;

public record AllowableExtensionDto(
    Guid Id,
    string ExtName
);

public record AttachmentDto(
    Guid Id,
    string Path,
    string Type,
    string SafeName,
    string OriginalName,
    Guid? ExtensionId,
    AllowableExtensionDto? Extension
);

public record ContactInfoDto(
    Guid Id,
    string Number,
    string? Email,
    bool IsActive
);

public record UserDto(
    Guid Id,
    string UserName,
    Guid? RoleId,
    RoleDto? Role
);

public record RoleDto(
    Guid Id,
    string Type,
    List<PermissionDto> Permissions
);

public record PermissionDto(
    Guid Id,
    string Name,
    string Description,
    string Resource,
    string Action
);

public record ManagerDto(
    Guid Id,
    string Name,
    Guid? UserId,
    UserDto? User,
    List<ManagerContactInfoDto> ManagerContactInfos,
    List<ManagerAttachmentDto> ManagerAttachments
);

public record ManagerContactInfoDto(
    Guid ManagerId,
    Guid ContactInfoId,
    ManagerDto? Manager,
    ContactInfoDto? ContactInfo
);

public record ManagerAttachmentDto(
    Guid ManagerId,
    Guid AttachmentId,
    ManagerDto? Manager,
    AttachmentDto? Attachment
);

public record TeacherDto(
    Guid Id,
    string Name,
    string FatherName,
    string MotherName,
    string? NationalityNumber,
    string? Email,
    Guid? UserId,
    UserDto? User,
    List<TeacherContactInfoDto> TeacherContactInfos,
    List<TeacherAttachmentDto> TeacherAttachments,
    List<ClassTeacherEnrollmentDto> ClassTeacherEnrollments
);

public record TeacherContactInfoDto(
    Guid TeacherId,
    Guid ContactInfoId,
    TeacherDto? Teacher,
    ContactInfoDto? ContactInfo
);

public record TeacherAttachmentDto(
    Guid TeacherId,
    Guid AttachmentId,
    TeacherDto? Teacher,
    AttachmentDto? Attachment
);

public record TeacherAttendanceDto(
    Guid Id,
    DateTime StartDate,
    DateTime EndDate,
    Guid ClassTeacherId
);

public record ClassTeacherEnrollmentDto(
    Guid Id,
    Guid TeacherId,
    TeacherDto? Teacher,
    bool IsMainTeacher,
    Guid ClassId
);

public record BookDto(
    Guid Id,
    string Name
);

public record HadithDto(
    Guid Id,
    string Text,
    Guid? BookId,
    BookDto? Book,
    string? Chapter
);

public record SemesterDto(
    Guid Id,
    DateTime StartDate,
    DateTime EndDate,
    string Name
);

public record CourseDto(
    Guid Id,
    string EventName,
    Guid SemesterId,
    SemesterDto? Semester,
    List<HalaqaDto> Halaqas
);

public record HalaqaDto(
    Guid Id,
    string ClassName,
    Guid CourseId,
    CourseDto? Course
);

public record PointCategoryDto(
    Guid Id,
    string Type
);

public record PointDto(
    Guid Id,
    Guid StudentId,
    Guid EventId,
    Guid ClassId,
    Guid SmesterId,
    int PointValue,
    Guid? CategoryId,
    PointCategoryDto? Category,
    Guid? GivenByTeacherId
);

public record CreateAllowableExtensionDto(string ExtName);
public record UpdateAllowableExtensionDto(string ExtName);

public record CreateAttachmentDto(string Path, string Type, string SafeName, string OriginalName, Guid? ExtensionId);
public record UpdateAttachmentDto(string Path, string Type, string SafeName, string OriginalName, Guid? ExtensionId);

public record CreateContactInfoDto(string Number, string? Email, bool IsActive);
public record UpdateContactInfoDto(string Number, string? Email, bool IsActive);

public record UpdatePasswordDto(string NewPassword);

public record CreateUserDto(string UserName, string Password, Guid? RoleId);
public record UpdateUserDto(string UserName, Guid? RoleId);

public record CreateManagerDto(string Name, Guid? UserId);
public record UpdateManagerDto(string Name, Guid? UserId);

public record CreateTeacherDto(
    string Name,
    string FatherName,
    string MotherName,
    string NationalityNumber,
    string? Email,
    Guid? UserId,
    List<CreateTeacherContactInfoDto>? ContactInfos = null
);
public record UpdateTeacherDto(string Name, string FatherName, string MotherName, string NationalityNumber, string? Email);
public record CreateTeacherContactInfoDto(string Number, string? Email, bool IsActive = true);

public record CreateBookDto(string Name);
public record UpdateBookDto(string Name);

public record CreateHadithDto(string Text, Guid? BookId, string? Chapter);
public record UpdateHadithDto(string Text, string? Chapter);

public record CreateSemesterDto(DateTime StartDate, DateTime EndDate, string Name);
public record UpdateSemesterDto(DateTime StartDate, DateTime EndDate, string Name);

public record CreateCourseDto(string EventName, Guid SemesterId);
public record UpdateCourseDto(string EventName);

public record CreateHalaqaDto(string ClassName, Guid CourseId);
public record UpdateHalaqaDto(string ClassName);

public record CreatePointCategoryDto(string Type);
public record UpdatePointCategoryDto(string Type);

public record CreatePointDto(Guid StudentId, Guid EventId, Guid ClassId, Guid SmesterId, int PointValue, Guid? CategoryId, Guid? GivenByTeacherId);
public record UpdatePointDto(int PointValue, Guid? CategoryId, Guid? GivenByTeacherId);

public record StudentDto(
    Guid Id,
    string Name,
    string FatherName,
    string MotherName,
    string? NationalityNumber,
    string? Email,
    Guid? UserId,
    UserDto? User,
    List<StudentContactInfoDto> StudentContactInfos,
    List<StudentAttachmentDto> StudentAttachments,
    List<StudentHadithDto> StudentHadiths,
    List<StudentQuraanPageDto> StudentQuraanPages,
    List<StudentClassEventsPointDto> StudentClassEventsPoints,
    List<PointDto> Points
);

public record StudentContactInfoDto(
    Guid StudentId,
    Guid ContactInfoId,
    StudentDto? Student,
    ContactInfoDto? ContactInfo
);

public record StudentAttachmentDto(
    Guid StudentId,
    Guid AttachmentId,
    StudentDto? Student,
    AttachmentDto? Attachment
);

public record StudentHadithDto(
    Guid Id,
    Guid HadithId,
    Guid StudentId,
    Guid? TeacherId,
    Guid? ClassId,
    DateTime? MemorizedAt,
    string? Status,
    string? Notes
);

public record StudentQuraanPageDto(
    Guid Id,
    int PageNumber,
    Guid StudentId,
    Guid? TeacherId,
    Guid? ClassId,
    DateTime? MemorizedAt,
    string? Status,
    string? Notes
);

public record StudentClassEventsPointDto(
    Guid Id,
    Guid StudentId,
    Guid ClassId,
    Guid SmesterId,
    Guid EventId,
    int QuranPoints,
    int HadithPoints,
    int AttendancePoints,
    int BehaviorPoints,
    int TotalPoints
);

public record ClassStudentEnrollmentDto(
    Guid Id,
    Guid StudentId,
    StudentDto? Student,
    Guid ClassId
);

public record TeacherStatisticsDto(
    Guid TeacherId,
    int TotalClasses,
    int IsMainTeacherCount,
    int TotalPointsGiven,
    int TotalPointsCount,
    int TotalContactInfos
);
