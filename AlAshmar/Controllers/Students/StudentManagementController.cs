using AlAshmar.Application.DTOs;
using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Services.Domain;
using AlAshmar.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlAshmar.Controllers.Students;

/// <summary>
/// Controller for student management operations including filtering, enrollment, and academic tracking.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentManagementController : ControllerBase
{
    private readonly IStudentManagementService _studentService;

    public StudentManagementController(IStudentManagementService studentService)
    {
        _studentService = studentService;
    }

    /// <summary>
    /// Get all students filtered by various criteria with support for OR operations.
    /// All filter parameters are optional - null values are ignored in filtering.
    /// </summary>
    /// <param name="pageNumber">Page number for pagination (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="classId">Filter by class ID (nullable - supports OR operation)</param>
    /// <param name="semesterId">Filter by semester ID (nullable - supports OR operation)</param>
    /// <param name="eventId">Filter by event ID (nullable - supports OR operation)</param>
    /// <param name="teacherId">Filter by teacher ID (nullable - supports OR operation)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of filtered students with their related data</returns>
    [HttpGet("filtered")]
    public async Task<IActionResult> GetAllStudentFiltered(
        [FromQuery] int? pageNumber = null,
        [FromQuery] int? pageSize = null,
        [FromQuery] Guid? classId = null,
        [FromQuery] Guid? semesterId = null,
        [FromQuery] Guid? eventId = null,
        [FromQuery] Guid? teacherId = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _studentService.GetAllFilteredAsync(pageNumber, pageSize, classId, semesterId, eventId, teacherId, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get a student by ID with full details including contact info, attachments, and academic records.
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetStudentById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _studentService.GetByIdAsync(id, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Create a new student with basic info and optional contact details.
    /// </summary>
    /// <param name="dto">Student creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpPost]
    public async Task<IActionResult> CreateStudent(
        [FromBody] CreateStudentDto dto,
        CancellationToken cancellationToken = default)
    {
        // Generate username from email or nationality number
        var userName = dto.Email ?? dto.NationalityNumber ?? $"student_{Guid.NewGuid():N}".Substring(0, 20);
        // Note: Password should be hashed before sending
        var result = await _studentService.CreateAsync(
            dto.Name, dto.FatherName, dto.MotherName,
            dto.NationalityNumber, dto.Email,
            userName, "defaultPassword123", cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Update an existing student's basic information.
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <param name="dto">Updated student data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateStudent(
        [FromRoute] Guid id,
        [FromBody] UpdateStudentDto dto,
        CancellationToken cancellationToken = default)
    {
        var result = await _studentService.UpdateAsync(
            id, dto.Name, dto.FatherName, dto.MotherName,
            dto.NationalityNumber, dto.Email, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Delete a student by ID.
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteStudent(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _studentService.DeleteAsync(id, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get student's memorization progress (Hadith and Quran).
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("{id:guid}/memorization")]
    public async Task<IActionResult> GetMemorizationProgress(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _studentService.GetMemorizationProgressAsync(id, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get student's attendance records.
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <param name="fromDate">Optional from date</param>
    /// <param name="toDate">Optional to date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("{id:guid}/attendance")]
    public async Task<IActionResult> GetAttendanceRecords(
        [FromRoute] Guid id,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _studentService.GetAttendanceRecordsAsync(id, fromDate, toDate, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get student's points and achievements.
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <param name="semesterId">Optional semester filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("{id:guid}/points")]
    public async Task<IActionResult> GetPoints(
        [FromRoute] Guid id,
        [FromQuery] Guid? semesterId = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _studentService.GetPointsAsync(id, semesterId, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Enroll a student in a class.
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <param name="classId">Class ID to enroll in</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpPost("{id:guid}/enrollments")]
    public async Task<IActionResult> EnrollInClass(
        [FromRoute] Guid id,
        [FromBody] Guid classId,
        CancellationToken cancellationToken = default)
    {
        var result = await _studentService.EnrollInClassAsync(id, classId, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get student's class enrollments.
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("{id:guid}/enrollments")]
    public async Task<IActionResult> GetClassEnrollments(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _studentService.GetClassEnrollmentsAsync(id, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Add attachment to a student's profile.
    /// </summary>
    /// <param name="id">Student ID</param>
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
        var path = Path.Combine("uploads", "students", id.ToString(), fileName);

        var result = await _studentService.AddAttachmentAsync(
            id,
            path,
            formFile.ContentType,
            fileName,
            formFile.FileName,
            null, // ExtensionId can be added based on file type
            cancellationToken
        );
        return result.ToActionResult();
    }

    /// <summary>
    /// Get student's attachments.
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("{id:guid}/attachments")]
    public async Task<IActionResult> GetAttachments(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _studentService.GetAttachmentsAsync(id, cancellationToken);
        return result.ToActionResult();
    }
}
