using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Services.Domain;
using Microsoft.AspNetCore.Mvc;
using AlAshmar.Domain.Entities.Common;
using AlAshmar.Domain.Entities.Academic;

namespace AlAshmar.Controllers;

public class AllowableExtentionsController : BaseController<AllowableExtention, AllowableExtentionDto, CreateAllowableExtentionDto, UpdateAllowableExtentionDto, Guid>
{
    public AllowableExtentionsController(IRepositoryBase<AllowableExtention, Guid> repository, IMapper mapper) : base(repository, mapper) { }
    protected override Guid GetId(AllowableExtentionDto dto) => dto.Id;
}

public class AttachmentsController : BaseController<Attacment, AttacmentDto, CreateAttacmentDto, UpdateAttacmentDto, Guid>
{
    public AttachmentsController(IRepositoryBase<Attacment, Guid> repository, IMapper mapper) : base(repository, mapper) { }
    protected override Guid GetId(AttacmentDto dto) => dto.Id;
}

public class ContactInfosController : BaseController<ContactInfo, ContactInfoDto, CreateContactInfoDto, UpdateContactInfoDto, Guid>
{
    public ContactInfosController(IRepositoryBase<ContactInfo, Guid> repository, IMapper mapper) : base(repository, mapper) { }
    protected override Guid GetId(ContactInfoDto dto) => dto.Id;
}

public class BooksController : BaseController<Book, BookDto, CreateBookDto, UpdateBookDto, Guid>
{
    public BooksController(IRepositoryBase<Book, Guid> repository, IMapper mapper) : base(repository, mapper) { }
    protected override Guid GetId(BookDto dto) => dto.Id;
}

public class HadithsController : BaseController<Hadith, HadithDto, CreateHadithDto, UpdateHadithDto, Guid>
{
    public HadithsController(IRepositoryBase<Hadith, Guid> repository, IMapper mapper) : base(repository, mapper) { }
    protected override Guid GetId(HadithDto dto) => dto.Id;
}

public class SemestersController : BaseController<Semester, SemesterDto, CreateSemesterDto, UpdateSemesterDto, Guid>
{
    public SemestersController(IRepositoryBase<Semester, Guid> repository, IMapper mapper) : base(repository, mapper) { }
    protected override Guid GetId(SemesterDto dto) => dto.Id;
}

public class PointCategoriesController : BaseController<PointCategory, PointCategoryDto, CreatePointCategoryDto, UpdatePointCategoryDto, Guid>
{
    public PointCategoriesController(IRepositoryBase<PointCategory, Guid> repository, IMapper mapper) : base(repository, mapper) { }
    protected override Guid GetId(PointCategoryDto dto) => dto.Id;
}

public class PointsController : BaseController<Point, PointDto, CreatePointDto, UpdatePointDto, Guid>
{
    public PointsController(IRepositoryBase<Point, Guid> repository, IMapper mapper) : base(repository, mapper) { }
    protected override Guid GetId(PointDto dto) => dto.Id;
}
