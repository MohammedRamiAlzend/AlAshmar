namespace AlAshmar.Application.DTOs.Domain;

// ==================== STUDENT RESPONSE DTOs ====================
// Organized by usage scenario to avoid null values and improve API clarity

// ------------------- Basic Info DTOs (Lightweight, for lists) -------------------

/// <summary>
/// Minimal student information for dropdowns, quick references, and basic lookups.
/// </summary>
public record StudentBasicInfoDto(
    Guid Id,
    string Name
);

/// <summary>
/// Student information for list views with essential identifying information.
/// Used in get-all scenarios without detailed navigation properties.
/// </summary>
public record StudentListItemDto(
    Guid Id,
    string Name,
    string FatherName,
    string MotherName,
    string? NationalityNumber,
    string? Email,
    string? UserName,
    string? RoleType
);

/// <summary>
/// Student information with user details for administrative list views.
/// </summary>
public record StudentWithUserListItemDto(
    Guid Id,
    string Name,
    string FatherName,
    string MotherName,
    string? NationalityNumber,
    string? Email,
    UserBasicInfoDto User
);

// ------------------- Detail DTOs (Full student information) -------------------

/// <summary>
/// Complete student details including all navigation properties.
/// Used for single student detail views where all related data is needed.
/// </summary>
public record StudentDetailDto(
    Guid Id,
    string Name,
    string FatherName,
    string MotherName,
    string? NationalityNumber,
    string? Email,
    Guid? UserId,
    UserDetailDto? User,
    List<StudentContactInfoDetailDto> ContactInfos,
    List<StudentAttachmentDetailDto> Attachments,
    List<StudentHadithDetailDto> Hadiths,
    List<StudentQuraanPageDetailDto> QuranPages,
    List<StudentClassEventsPointDetailDto> ClassEventsPoints,
    List<PointDetailDto> Points
);

/// <summary>
/// Student summary with counts and aggregated information.
/// Used for dashboard views and overview screens.
/// </summary>
public record StudentSummaryDto(
    Guid Id,
    string Name,
    string FatherName,
    string MotherName,
    string? NationalityNumber,
    string? Email,
    int ContactInfoCount,
    int AttachmentCount,
    int MemorizedHadithCount,
    int MemorizedQuranPageCount,
    int TotalPoints,
    int ClassEnrollmentCount
);

// ------------------- Academic Progress DTOs -------------------

/// <summary>
/// Student memorization progress for Hadith and Quran.
/// </summary>
public record StudentMemorizationProgressDto(
    Guid StudentId,
    List<StudentHadithSummaryDto> Hadiths,
    List<StudentQuraanPageSummaryDto> QuranPages,
    int TotalHadithsMemorized,
    int TotalQuranPagesMemorized
);

/// <summary>
/// Student attendance records with date ranges.
/// Used for individual student attendance queries.
/// </summary>
public record StudentAttendanceDto(
    Guid Id,
    DateTime StartDate,
    DateTime EndDate,
    Guid ClassStudentId
);

/// <summary>
/// Student points and achievements with category information.
/// </summary>
public record StudentPointDto(
    Guid Id,
    Guid StudentId,
    Guid EventId,
    Guid ClassId,
    Guid SemesterId,
    int PointValue,
    Guid? CategoryId,
    PointCategorySummaryDto? Category,
    Guid? GivenByTeacherId
);

/// <summary>
/// Aggregated student points summary.
/// </summary>
public record StudentPointsSummaryDto(
    Guid StudentId,
    int TotalPoints,
    int QuranPoints,
    int HadithPoints,
    int AttendancePoints,
    int BehaviorPoints,
    List<StudentPointDto> Points,
    List<StudentClassEventsPointSummaryDto> ClassEventsPoints
);

// ------------------- Enrollment DTOs -------------------

/// <summary>
/// Student class enrollment information.
/// </summary>
public record StudentEnrollmentDto(
    Guid Id,
    Guid StudentId,
    string StudentName,
    Guid ClassId,
    string? ClassName,
    DateTime EnrollmentDate
);

/// <summary>
/// Class enrollment with student details.
/// </summary>
public record ClassEnrollmentWithStudentDto(
    Guid Id,
    Guid ClassId,
    string ClassName,
    Guid StudentId,
    string StudentName,
    bool IsActive
);

// ==================== STUDENT RELATED ENTITY DTOs ====================

// ------------------- Contact Info -------------------

public record StudentContactInfoDetailDto(
    Guid StudentId,
    Guid ContactInfoId,
    string Number,
    string? Email,
    bool IsActive
);

// ------------------- Attachments -------------------

public record StudentAttachmentDetailDto(
    Guid StudentId,
    Guid AttachmentId,
    AttachmentDetailDto Attachment
);

public record AttachmentDetailDto(
    Guid Id,
    string Path,
    string Type,
    string SafeName,
    string OriginalName,
    Guid? ExtentionId,
    AllowableExtentionDto? Extention
);

// ------------------- Hadith -------------------

public record StudentHadithDetailDto(
    Guid Id,
    Guid HadithId,
    Guid StudentId,
    Guid? TeacherId,
    Guid? ClassId,
    DateTime? MemorizedAt,
    string? Status,
    string? Notes,
    HadithSummaryDto? Hadith
);

public record HadithSummaryDto(
    Guid Id,
    string Text,
    Guid? BookId,
    string? BookName,
    string? Chapter
);

public record StudentHadithSummaryDto(
    Guid Id,
    Guid HadithId,
    string? HadithText,
    string? BookName,
    string? Chapter,
    DateTime? MemorizedAt,
    string? Status
);

// ------------------- Quran Pages -------------------

public record StudentQuraanPageDetailDto(
    Guid Id,
    int PageNumber,
    Guid StudentId,
    Guid? TeacherId,
    Guid? ClassId,
    DateTime? MemorizedAt,
    string? Status,
    string? Notes
);

public record StudentQuraanPageSummaryDto(
    Guid Id,
    int PageNumber,
    Guid StudentId,
    DateTime? MemorizedAt,
    string? Status
);

// ------------------- Class Events Points -------------------

public record StudentClassEventsPointDetailDto(
    Guid Id,
    Guid StudentId,
    Guid ClassId,
    Guid SemesterId,
    Guid EventId,
    int QuranPoints,
    int HadithPoints,
    int AttendancePoints,
    int BehaviorPoints,
    int TotalPoints
);

public record StudentClassEventsPointSummaryDto(
    Guid Id,
    Guid StudentId,
    Guid ClassId,
    Guid SemesterId,
    Guid EventId,
    int TotalPoints
);

// ------------------- Points -------------------

public record PointDetailDto(
    Guid Id,
    Guid StudentId,
    Guid EventId,
    Guid ClassId,
    Guid SemesterId,
    int PointValue,
    Guid? CategoryId,
    PointCategorySummaryDto? Category,
    Guid? GivenByTeacherId
);

public record PointCategorySummaryDto(
    Guid Id,
    string Type
);

// ==================== USER DTOs (Simplified versions) ====================

public record UserBasicInfoDto(
    Guid Id,
    string UserName
);

public record UserDetailDto(
    Guid Id,
    string UserName,
    Guid? RoleId,
    RoleSummaryDto? Role
);

public record RoleSummaryDto(
    Guid Id,
    string Type
);

// ==================== CREATE/UPDATE DTOs ====================
// Separated by operation type for better validation and API clarity

// ------------------- Student Creation -------------------

/// <summary>
/// DTO for creating a new student with basic information.
/// </summary>
public record CreateStudentDto(
    string Name,
    string FatherName,
    string MotherName,
    string NationalityNumber,
    string? Email
);

/// <summary>
/// DTO for creating a student with account credentials.
/// Used when student needs a user account.
/// </summary>
public record CreateStudentWithAccountDto(
    string Name,
    string FatherName,
    string MotherName,
    string NationalityNumber,
    string? Email,
    string UserName,
    string Password
);

/// <summary>
/// DTO for adding contact information to a student.
/// </summary>
public record CreateStudentContactInfoDto(
    Guid StudentId,
    string Number,
    string? Email,
    bool IsActive = true
);

/// <summary>
/// DTO for adding an attachment to a student.
/// </summary>
public record CreateStudentAttachmentDto(
    Guid StudentId,
    string Path,
    string Type,
    string SafeName,
    string OriginalName,
    Guid? ExtentionId
);

// ------------------- Student Updates -------------------

/// <summary>
/// DTO for updating student basic information.
/// </summary>
public record UpdateStudentDto(
    string Name,
    string FatherName,
    string MotherName,
    string NationalityNumber,
    string? Email
);

/// <summary>
/// DTO for updating student contact information.
/// </summary>
public record UpdateStudentContactInfoDto(
    string Number,
    string? Email,
    bool IsActive
);

/// <summary>
/// DTO for updating student attachment information.
/// </summary>
public record UpdateStudentAttachmentDto(
    string Path,
    string Type,
    string SafeName,
    string OriginalName,
    Guid? ExtentionId
);

// ------------------- Student Memorization -------------------

/// <summary>
/// DTO for recording student hadith memorization.
/// </summary>
public record RecordStudentHadithDto(
    Guid StudentId,
    Guid HadithId,
    Guid? TeacherId,
    Guid? ClassId,
    string? Status,
    string? Notes
);

/// <summary>
/// DTO for recording student Quran page memorization.
/// </summary>
public record RecordStudentQuranPageDto(
    Guid StudentId,
    int PageNumber,
    Guid? TeacherId,
    Guid? ClassId,
    string? Status,
    string? Notes
);

/// <summary>
/// DTO for updating student memorization status.
/// </summary>
public record UpdateMemorizationStatusDto(
    string Status,
    string? Notes
);

// ------------------- Student Points -------------------

/// <summary>
/// DTO for adding points to a student.
/// </summary>
public record AddStudentPointDto(
    Guid StudentId,
    Guid EventId,
    Guid ClassId,
    Guid SemesterId,
    int PointValue,
    Guid? CategoryId,
    Guid? GivenByTeacherId
);

/// <summary>
/// DTO for updating student class events points.
/// </summary>
public record UpdateStudentClassEventsPointDto(
    int QuranPoints,
    int HadithPoints,
    int AttendancePoints,
    int BehaviorPoints
);

// ------------------- Student Enrollment -------------------

/// <summary>
/// DTO for enrolling a student in a class.
/// </summary>
public record EnrollStudentInClassDto(
    Guid StudentId,
    Guid ClassId
);

// ==================== QUERY/FILTER DTOs ====================

/// <summary>
/// Filter criteria for querying students.
/// All properties are nullable - null values are ignored in filtering.
/// </summary>
public record StudentFilterDto(
    Guid? ClassId = null,
    Guid? SemesterId = null,
    Guid? EventId = null,
    Guid? TeacherId = null,
    string? SearchTerm = null,
    string? NationalityNumber = null,
    string? Email = null
);

/// <summary>
/// Pagination parameters for list queries.
/// </summary>
public record PaginationDto(
    int PageNumber = 1,
    int PageSize = 10
);

/// <summary>
/// Combined filter and pagination request for student queries.
/// </summary>
public record StudentQueryRequestDto(
    StudentFilterDto? Filters = null,
    PaginationDto? Pagination = null
);

// ==================== RESPONSE WRAPPERS ====================

/// <summary>
/// Paginated response for student list queries.
/// </summary>
public record PagedResponse<T>(
    List<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages,
    bool HasPreviousPage,
    bool HasNextPage
);

/// <summary>
/// Paginated response specifically for student list items.
/// </summary>
public record StudentPagedResponse(
    List<StudentListItemDto> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages,
    bool HasPreviousPage,
    bool HasNextPage
);
