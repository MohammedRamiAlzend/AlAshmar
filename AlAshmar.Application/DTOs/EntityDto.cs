namespace AlAshmar.Application.DTOs;



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
