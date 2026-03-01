using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Services.Domain;
using AlAshmar.Domain.Commons;
using AlAshmar.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlAshmar.Controllers.Teachers;

/// <summary>
/// Controller for teacher management operations including filtering, enrollment, and academic tracking.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TeacherManagementController : ControllerBase
{
    private readonly ITeacherManagementService _teacherService;

    public TeacherManagementController(ITeacherManagementService teacherService)
    {
        _teacherService = teacherService;
    }

    /// <summary>
    /// Get all teachers filtered by various criteria with support for OR operations.
    /// All filter parameters are optional - null values are ignored in filtering.
    /// </summary>
    /// <param name="filter">Filter parameters (pageNumber, pageSize, classId, semesterId, eventId)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of filtered teachers with their related data</returns>
    [HttpGet("filtered")]
    public async Task<IActionResult> GetAllTeachersFiltered(
        [FromQuery] TeacherFilterParameters filter,
        CancellationToken cancellationToken = default)
    {
        var result = await _teacherService.GetAllFilteredAsync(filter, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get a teacher by ID with full details including contact info, attachments, and academic records.
    /// </summary>
    /// <param name="id">Teacher ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTeacherById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _teacherService.GetByIdAsync(id, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Create a new teacher with basic info and optional contact details.
    /// </summary>
    /// <param name="dto">Teacher creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpPost]
    public async Task<IActionResult> CreateTeacher(
        [FromBody] CreateTeacherDto dto,
        CancellationToken cancellationToken = default)
    {
        // Generate username from email or nationality number
        var userName = dto.Email ?? dto.NationalityNumber ?? $"teacher_{Guid.NewGuid():N}".Substring(0, 20);
        var result = await _teacherService.CreateAsync(
            dto.Name, dto.FatherName, dto.MotherName,
            dto.NationalityNumber, dto.Email,
            userName, "defaultPassword123", cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Update an existing teacher's basic information.
    /// </summary>
    /// <param name="id">Teacher ID</param>
    /// <param name="dto">Updated teacher data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateTeacher(
        [FromRoute] Guid id,
        [FromBody] UpdateTeacherDto dto,
        CancellationToken cancellationToken = default)
    {
        var result = await _teacherService.UpdateAsync(
            id, dto.Name, dto.FatherName, dto.MotherName,
            dto.NationalityNumber, dto.Email, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Delete a teacher by ID.
    /// </summary>
    /// <param name="id">Teacher ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTeacher(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _teacherService.DeleteAsync(id, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get teacher's class enrollments.
    /// </summary>
    /// <param name="id">Teacher ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("{id:guid}/enrollments")]
    public async Task<IActionResult> GetClassEnrollments(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _teacherService.GetClassEnrollmentsAsync(id, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get teacher's attendance records.
    /// </summary>
    /// <param name="id">Teacher ID</param>
    /// <param name="filter">Attendance filter parameters (fromDate, toDate)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("{id:guid}/attendance")]
    public async Task<IActionResult> GetAttendanceRecords(
        [FromRoute] Guid id,
        [FromQuery] AttendanceFilterParameters filter,
        CancellationToken cancellationToken = default)
    {
        var result = await _teacherService.GetAttendanceRecordsAsync(id, filter, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get points given by teacher.
    /// </summary>
    /// <param name="id">Teacher ID</param>
    /// <param name="filter">Points filter parameters (semesterId)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("{id:guid}/points-given")]
    public async Task<IActionResult> GetPointsGiven(
        [FromRoute] Guid id,
        [FromQuery] PointsFilterParameters filter,
        CancellationToken cancellationToken = default)
    {
        var result = await _teacherService.GetPointsGivenAsync(id, filter, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Add attachment to a teacher's profile.
    /// </summary>
    /// <param name="id">Teacher ID</param>
    /// <param name="formFile">Attachment file</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpPost("{id:guid}/attachments")]
    public async Task<IActionResult> AddAttachment(
        [FromRoute] Guid id,
        IFormFile formFile,
        CancellationToken cancellationToken = default)
    {
        if (formFile == null || formFile.Length == 0)
            return BadRequest("No file provided");

        // Generate safe file name and path
        var fileName = $"{Guid.NewGuid():N}_{formFile.FileName}";
        var path = Path.Combine("uploads", "teachers", id.ToString(), fileName);

        var result = await _teacherService.AddAttachmentAsync(
            id,
            path,
            formFile.ContentType,
            fileName,
            formFile.FileName,
            null,
            cancellationToken
        );
        return result.ToActionResult();
    }

    /// <summary>
    /// Get teacher's attachments.
    /// </summary>
    /// <param name="id">Teacher ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("{id:guid}/attachments")]
    public async Task<IActionResult> GetAttachments(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _teacherService.GetAttachmentsAsync(id, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Add contact info to a teacher's profile.
    /// </summary>
    /// <param name="id">Teacher ID</param>
    /// <param name="parameters">Contact info parameters (number, email, isActive)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpPost("{id:guid}/contact-infos")]
    public async Task<IActionResult> AddContactInfo(
        [FromRoute] Guid id,
        [FromBody] ContactInfoParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var result = await _teacherService.AddContactInfoAsync(id, parameters, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get teacher's contact infos.
    /// </summary>
    /// <param name="id">Teacher ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("{id:guid}/contact-infos")]
    public async Task<IActionResult> GetContactInfos(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _teacherService.GetContactInfosAsync(id, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Remove contact info from a teacher's profile.
    /// </summary>
    /// <param name="id">Teacher ID</param>
    /// <param name="contactInfoId">Contact info ID to remove</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpDelete("{id:guid}/contact-infos/{contactInfoId:guid}")]
    public async Task<IActionResult> RemoveContactInfo(
        [FromRoute] Guid id,
        [FromRoute] Guid contactInfoId,
        CancellationToken cancellationToken = default)
    {
        var result = await _teacherService.RemoveContactInfoAsync(id, contactInfoId, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Assign a teacher to a class.
    /// </summary>
    /// <param name="id">Teacher ID</param>
    /// <param name="parameters">Class assignment parameters (classId, isMainTeacher)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpPost("{id:guid}/classes")]
    public async Task<IActionResult> AssignToClass(
        [FromRoute] Guid id,
        [FromBody] ClassAssignmentParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var result = await _teacherService.AssignToClassAsync(id, parameters, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Remove a teacher from a class.
    /// </summary>
    /// <param name="id">Teacher ID</param>
    /// <param name="classId">Class ID to remove from</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpDelete("{id:guid}/classes/{classId:guid}")]
    public async Task<IActionResult> RemoveFromClass(
        [FromRoute] Guid id,
        [FromRoute] Guid classId,
        CancellationToken cancellationToken = default)
    {
        var result = await _teacherService.RemoveFromClassAsync(id, classId, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Set a teacher as the main teacher for a class.
    /// </summary>
    /// <param name="id">Teacher ID</param>
    /// <param name="classId">Class ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpPut("{id:guid}/classes/{classId:guid}/main")]
    public async Task<IActionResult> SetAsMainTeacher(
        [FromRoute] Guid id,
        [FromRoute] Guid classId,
        CancellationToken cancellationToken = default)
    {
        var result = await _teacherService.SetAsMainTeacherAsync(id, classId, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get teacher statistics including total classes, students, and points given.
    /// </summary>
    /// <param name="id">Teacher ID</param>
    /// <param name="filter">Statistics filter parameters (semesterId)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("{id:guid}/statistics")]
    public async Task<IActionResult> GetStatistics(
        [FromRoute] Guid id,
        [FromQuery] PointsFilterParameters? filter = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _teacherService.GetStatisticsAsync(id, filter, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Search teachers by name (partial match).
    /// </summary>
    /// <param name="parameters">Search parameters (query, pageNumber, pageSize)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("search")]
    public async Task<IActionResult> SearchTeachers(
        [FromQuery] SearchParameters parameters,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(parameters.Query))
            return BadRequest("Search query is required");

        var result = await _teacherService.SearchByNameAsync(parameters, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get all students taught by a teacher (through class enrollments).
    /// </summary>
    /// <param name="id">Teacher ID</param>
    /// <param name="classId">Optional class filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("{id:guid}/students")]
    public async Task<IActionResult> GetStudents(
        [FromRoute] Guid id,
        [FromQuery] Guid? classId = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _teacherService.GetStudentsAsync(id, classId, cancellationToken);
        return result.ToActionResult();
    }
}
