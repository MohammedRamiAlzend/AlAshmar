using AlAshmar.Application.Services.Domain;
using AlAshmar.Domain.Entities.Academic;
using AlAshmar.Domain.Entities.Common;

namespace AlAshmar.Controllers;

public class AllowableExtensionsController(IAllowableExtensionService service, IMapper mapper) :
    BaseController<AllowableExtension, AllowableExtensionDto, CreateAllowableExtensionDto, UpdateAllowableExtensionDto, Guid>(service, mapper)
{
    protected override Guid GetId(AllowableExtensionDto dto) => dto.Id;
}

public class ContactInfosController(IContactInfoService service, IMapper mapper) :
    BaseController<ContactInfo, ContactInfoDto, CreateContactInfoDto, UpdateContactInfoDto, Guid>(service, mapper)
{
    protected override Guid GetId(ContactInfoDto dto) => dto.Id;
}

public class BooksController(IBookService service, IMapper mapper) :
    BaseController<Book, BookDto, CreateBookDto, UpdateBookDto, Guid>(service, mapper)
{
    protected override Guid GetId(BookDto dto) => dto.Id;
}

public class HadithsController(IHadithService service, IMapper mapper) :
    BaseController<Hadith, HadithDto, CreateHadithDto, UpdateHadithDto, Guid>(service, mapper)
{
    protected override Guid GetId(HadithDto dto) => dto.Id;
}

public class SemestersController(ISemesterService service, IMapper mapper) :
    BaseController<Semester, SemesterDto, CreateSemesterDto, UpdateSemesterDto, Guid>(service, mapper)
{
    protected override Guid GetId(SemesterDto dto) => dto.Id;
}

public class PointCategoriesController(IPointCategoryService service, IMapper mapper) :
    BaseController<PointCategory, PointCategoryDto, CreatePointCategoryDto, UpdatePointCategoryDto, Guid>(service, mapper)
{
    protected override Guid GetId(PointCategoryDto dto) => dto.Id;
}

public class PointsController(IPointService service, IMapper mapper) :
    BaseController<Point, PointDto, CreatePointDto, UpdatePointDto, Guid>(service, mapper)
{
    protected override Guid GetId(PointDto dto) => dto.Id;
}
