namespace AlAshmar.Application.DTOs.Domain;

// ==================== COMMON DOMAIN ====================

public record AllowableExtentionDto(
    Guid Id,
    string ExtName
);

public record AttacmentDto(
    Guid Id,
    string Path,
    string Type,
    string SafeName,
    string OriginalName,
    Guid? ExtentionId,
    AllowableExtentionDto? Extention
);

public record ContactInfoDto(
    Guid Id,
    string Number,
    string? Email,
    bool IsActive
);

// ==================== USERS DOMAIN ====================

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

// ==================== MANAGERS DOMAIN ====================

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
    AttacmentDto? Attachment
);

// ==================== TEACHERS DOMAIN ====================

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
    AttacmentDto? Attachment
);

public record TeacherAttencanceDto(
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

// ==================== ACADEMIC DOMAIN ====================

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

// ==================== CREATE/UPDATE DTOs ====================

// Common
public record CreateAllowableExtentionDto(string ExtName);
public record UpdateAllowableExtentionDto(string ExtName);

public record CreateAttacmentDto(string Path, string Type, string SafeName, string OriginalName, Guid? ExtentionId);
public record UpdateAttacmentDto(string Path, string Type, string SafeName, string OriginalName, Guid? ExtentionId);

public record CreateContactInfoDto(string Number, string? Email, bool IsActive);
public record UpdateContactInfoDto(string Number, string? Email, bool IsActive);

// Users
public record CreateUserDto(string UserName, string Password, Guid? RoleId);
public record UpdateUserDto(string UserName, Guid? RoleId);

// Managers
public record CreateManagerDto(string Name, Guid? UserId);
public record UpdateManagerDto(string Name, Guid? UserId);

// Teachers
public record CreateTeacherDto(string Name, string FatherName, string MotherName, string? NationalityNumber, string? Email, Guid? UserId);
public record UpdateTeacherDto(string Name, string FatherName, string MotherName, string? NationalityNumber, string? Email);

// Academic
public record CreateBookDto(string Name);
public record UpdateBookDto(string Name);

public record CreateHadithDto(string Text, Guid? BookId, string? Chapter);
public record UpdateHadithDto(string Text, string? Chapter);

public record CreateSemesterDto(DateTime StartDate, DateTime EndDate, string Name);
public record UpdateSemesterDto(DateTime StartDate, DateTime EndDate, string Name);

public record CreatePointCategoryDto(string Type);
public record UpdatePointCategoryDto(string Type);

public record CreatePointDto(Guid StudentId, Guid EventId, Guid ClassId, Guid SmesterId, int PointValue, Guid? CategoryId, Guid? GivenByTeacherId);
public record UpdatePointDto(int PointValue, Guid? CategoryId, Guid? GivenByTeacherId);
