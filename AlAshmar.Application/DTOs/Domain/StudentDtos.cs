namespace AlAshmar.Application.DTOs.Domain;









public record StudentBasicInfoDto(
    Guid Id,
    string Name
);





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




public record StudentWithUserListItemDto(
    Guid Id,
    string Name,
    string FatherName,
    string MotherName,
    string? NationalityNumber,
    string? Email,
    UserBasicInfoDto User
);







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






public record StudentMemorizationProgressDto(
    Guid StudentId,
    List<StudentHadithSummaryDto> Hadiths,
    List<StudentQuraanPageSummaryDto> QuranPages,
    int TotalHadithsMemorized,
    int TotalQuranPagesMemorized
);





public record StudentAttendanceDto(
    Guid Id,
    DateTime StartDate,
    DateTime EndDate,
    Guid ClassStudentId
);




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






public record StudentEnrollmentDto(
    Guid Id,
    Guid StudentId,
    string StudentName,
    Guid ClassId,
    string? ClassName,
    DateTime EnrollmentDate
);




public record ClassEnrollmentWithStudentDto(
    Guid Id,
    Guid ClassId,
    string ClassName,
    Guid StudentId,
    string StudentName,
    bool IsActive
);





public record StudentContactInfoDetailDto(
    Guid StudentId,
    Guid ContactInfoId,
    string Number,
    string? Email,
    bool IsActive
);



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









public record CreateStudentDto(
    string Name,
    string FatherName,
    string MotherName,
    string NationalityNumber,
    string? Email
);





public record CreateStudentWithAccountDto(
    string Name,
    string FatherName,
    string MotherName,
    string NationalityNumber,
    string? Email,
    string UserName,
    string Password
);




public record CreateStudentContactInfoDto(
    Guid StudentId,
    string Number,
    string? Email,
    bool IsActive = true
);




public record CreateStudentAttachmentDto(
    Guid StudentId,
    string Path,
    string Type,
    string SafeName,
    string OriginalName,
    Guid? ExtentionId
);






public record UpdateStudentDto(
    string Name,
    string FatherName,
    string MotherName,
    string NationalityNumber,
    string? Email
);




public record UpdateStudentContactInfoDto(
    string Number,
    string? Email,
    bool IsActive
);




public record UpdateStudentAttachmentDto(
    string Path,
    string Type,
    string SafeName,
    string OriginalName,
    Guid? ExtentionId
);






public record RecordStudentHadithDto(
    Guid StudentId,
    Guid HadithId,
    Guid? TeacherId,
    Guid? ClassId,
    string? Status,
    string? Notes
);




public record RecordStudentQuranPageDto(
    Guid StudentId,
    int PageNumber,
    Guid? TeacherId,
    Guid? ClassId,
    string? Status,
    string? Notes
);




public record UpdateMemorizationStatusDto(
    string Status,
    string? Notes
);






public record AddStudentPointDto(
    Guid StudentId,
    Guid EventId,
    Guid ClassId,
    Guid SemesterId,
    int PointValue,
    Guid? CategoryId,
    Guid? GivenByTeacherId
);




public record UpdateStudentClassEventsPointDto(
    int QuranPoints,
    int HadithPoints,
    int AttendancePoints,
    int BehaviorPoints
);






public record EnrollStudentInClassDto(
    Guid StudentId,
    Guid ClassId
);







public record StudentFilterDto(
    Guid? ClassId = null,
    Guid? SemesterId = null,
    Guid? EventId = null,
    Guid? TeacherId = null,
    string? SearchTerm = null,
    string? NationalityNumber = null,
    string? Email = null
);




public record PaginationDto(
    int PageNumber = 1,
    int PageSize = 10
);




public record StudentQueryRequestDto(
    StudentFilterDto? Filters = null,
    PaginationDto? Pagination = null
);






public record PagedResponse<T>(
    List<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages,
    bool HasPreviousPage,
    bool HasNextPage
);




public record StudentPagedResponse(
    List<StudentListItemDto> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages,
    bool HasPreviousPage,
    bool HasNextPage
);
