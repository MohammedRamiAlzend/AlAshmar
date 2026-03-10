namespace AlAshmar.Application.DTOs;

// Simple DTOs for basic operations
// Note: Full DTOs with navigation properties are in AlAshmar.Application.DTOs.Domain namespace

public record TeacherDto(
    Guid Id,
    string Name,
    string FatherName,
    string MotherName,
    string? NationalityNumber,
    string? Email,
    Guid? UserId
);

public record ManagerDto(
    Guid Id,
    string Name,
    Guid? UserId
);
