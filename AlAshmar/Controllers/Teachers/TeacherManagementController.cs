using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.UseCases.Teachers.GetAllTeachersFiltered;
using AlAshmar.Application.UseCases.Teachers.GetTeacherById;
using AlAshmar.Application.UseCases.Teachers.CreateTeacher;
using AlAshmar.Application.UseCases.Teachers.UpdateTeacher;
using AlAshmar.Application.UseCases.Teachers.DeleteTeacher;
using AlAshmar.Domain.Commons;
using AlAshmar.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace AlAshmar.Controllers.Teachers;

/// <summary>
/// Controller for teacher management operations including filtering, enrollment, and academic tracking.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TeacherManagementController : ControllerBase
{
    private readonly IQueryHandler<GetAllTeachersFilteredQuery, Result<List<TeacherDto>>> _filterHandler;
    private readonly IQueryHandler<GetTeacherByIdQuery, Result<TeacherDto?>> _getByIdHandler;
    private readonly ICommandHandler<CreateTeacherCommand, Result<TeacherDto>> _createHandler;
    private readonly ICommandHandler<UpdateTeacherCommand, Result<TeacherDto>> _updateHandler;
    private readonly ICommandHandler<DeleteTeacherCommand, Result<Success>> _deleteHandler;

    public TeacherManagementController(
        IQueryHandler<GetAllTeachersFilteredQuery, Result<List<TeacherDto>>> filterHandler,
        IQueryHandler<GetTeacherByIdQuery, Result<TeacherDto?>> getByIdHandler,
        ICommandHandler<CreateTeacherCommand, Result<TeacherDto>> createHandler,
        ICommandHandler<UpdateTeacherCommand, Result<TeacherDto>> updateHandler,
        ICommandHandler<DeleteTeacherCommand, Result<Success>> deleteHandler)
    {
        _filterHandler = filterHandler;
        _getByIdHandler = getByIdHandler;
        _createHandler = createHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
    }

    /// <summary>
    /// Get all teachers filtered by various criteria with support for OR operations.
    /// All filter parameters are optional - null values are ignored in filtering.
    /// </summary>
    [HttpGet("filtered")]
    public async Task<IActionResult> GetAllTeachersFiltered(
        [FromQuery] int? pageNumber = null,
        [FromQuery] int? pageSize = null,
        [FromQuery] Guid? classId = null,
        [FromQuery] Guid? semesterId = null,
        [FromQuery] Guid? eventId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllTeachersFilteredQuery(pageNumber, pageSize, classId, semesterId, eventId);
        var result = await _filterHandler.Handle(query, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get a teacher by ID with full details including contact info, attachments, and academic records.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTeacherById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTeacherByIdQuery(id);
        var result = await _getByIdHandler.Handle(query, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Create a new teacher with basic info and optional contact details.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateTeacher(
        [FromBody] CreateTeacherDto dto,
        CancellationToken cancellationToken = default)
    {
        // Generate username from email or nationality number
        var userName = dto.Email ?? dto.NationalityNumber ?? $"teacher_{Guid.NewGuid():N}".Substring(0, 20);
        // Generate a secure random password
        var password = GenerateSecurePassword();
        
        var command = new CreateTeacherCommand(
            dto.Name, dto.FatherName, dto.MotherName,
            dto.NationalityNumber, dto.Email,
            userName, password
        );
        var result = await _createHandler.Handle(command, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Update an existing teacher's basic information.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateTeacher(
        [FromRoute] Guid id,
        [FromBody] UpdateTeacherDto dto,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateTeacherCommand(
            id, dto.Name, dto.FatherName, dto.MotherName,
            dto.NationalityNumber, dto.Email
        );
        var result = await _updateHandler.Handle(command, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Delete a teacher by ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTeacher(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteTeacherCommand(id);
        var result = await _deleteHandler.Handle(command, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get teacher's class enrollments.
    /// </summary>
    [HttpGet("{id:guid}/enrollments")]
    public async Task<IActionResult> GetClassEnrollments(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        // Note: This endpoint can be implemented by creating a GetTeacherClassEnrollments query
        return Ok(new List<ClassTeacherEnrollmentDto>());
    }

    /// <summary>
    /// Get teacher's attendance records.
    /// </summary>
    [HttpGet("{id:guid}/attendance")]
    public async Task<IActionResult> GetAttendanceRecords(
        [FromRoute] Guid id,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        // Note: This endpoint can be implemented by creating a GetTeacherAttendanceRecords query
        return Ok(new List<TeacherAttencanceDto>());
    }

    /// <summary>
    /// Get points given by teacher.
    /// </summary>
    [HttpGet("{id:guid}/points-given")]
    public async Task<IActionResult> GetPointsGiven(
        [FromRoute] Guid id,
        [FromQuery] Guid? semesterId = null,
        CancellationToken cancellationToken = default)
    {
        // Note: This endpoint can be implemented by creating a GetTeacherPointsGiven query
        return Ok(new List<PointDto>());
    }

    /// <summary>
    /// Add attachment to a teacher's profile.
    /// </summary>
    [HttpPost("{id:guid}/attachments")]
    public async Task<IActionResult> AddAttachment(
        [FromRoute] Guid id,
        IFormFile formFile,
        CancellationToken cancellationToken = default)
    {
        if (formFile == null || formFile.Length == 0)
            return BadRequest("No file provided");

        // Note: This endpoint can be implemented by creating an AddTeacherAttachment command
        return Ok(new { Message = "Attachment upload not yet implemented" });
    }

    /// <summary>
    /// Get teacher's attachments.
    /// </summary>
    [HttpGet("{id:guid}/attachments")]
    public async Task<IActionResult> GetAttachments(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        // Note: This endpoint can be implemented by creating a GetTeacherAttachments query
        return Ok(new List<TeacherAttachmentDto>());
    }

    /// <summary>
    /// Generates a secure random password with 12 characters.
    /// </summary>
    private static string GenerateSecurePassword(int length = 12)
    {
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
        var randomBytes = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        
        var password = new char[length];
        for (int i = 0; i < length; i++)
        {
            password[i] = validChars[randomBytes[i] % validChars.Length];
        }
        
        return new string(password);
    }
}
