using AlAshmar.Domain.Entities.Common;
using AlAshmar.Domain.Entities.Academic;

namespace AlAshmar.Controllers;

public class AllowableExtentionsController(IRepositoryBase<AllowableExtention, Guid> repository, IMapper mapper) :
    BaseController<AllowableExtention, AllowableExtentionDto, CreateAllowableExtentionDto, UpdateAllowableExtentionDto, Guid>(repository, mapper)
{
    protected override Guid GetId(AllowableExtentionDto dto) => dto.Id;
}

//public class AttachmentsController(IRepositoryBase<Attacment, Guid> repository, IMapper mapper) :
//    BaseController<Attacment, AttacmentDto, CreateAttacmentDto, UpdateAttacmentDto, Guid>(repository, mapper)
//{
//    protected override Guid GetId(AttacmentDto dto) => dto.Id;
//}

public class ContactInfosController(IRepositoryBase<ContactInfo, Guid> repository, IMapper mapper) :
    BaseController<ContactInfo, ContactInfoDto, CreateContactInfoDto, UpdateContactInfoDto, Guid>(repository, mapper)
{
    protected override Guid GetId(ContactInfoDto dto) => dto.Id;
}

public class BooksController(IRepositoryBase<Book, Guid> repository, IMapper mapper) :
    BaseController<Book, BookDto, CreateBookDto, UpdateBookDto, Guid>(repository, mapper)
{
    protected override Guid GetId(BookDto dto) => dto.Id;
}

public class HadithsController(IRepositoryBase<Hadith, Guid> repository, IMapper mapper) :
    BaseController<Hadith, HadithDto, CreateHadithDto, UpdateHadithDto, Guid>(repository, mapper)
{
    protected override Guid GetId(HadithDto dto) => dto.Id;
}

public class SemestersController(IRepositoryBase<Semester, Guid> repository, IMapper mapper) :
    BaseController<Semester, SemesterDto, CreateSemesterDto, UpdateSemesterDto, Guid>(repository, mapper)
{
    protected override Guid GetId(SemesterDto dto) => dto.Id;
}

public class PointCategoriesController(IRepositoryBase<PointCategory, Guid> repository, IMapper mapper) :
    BaseController<PointCategory, PointCategoryDto, CreatePointCategoryDto, UpdatePointCategoryDto, Guid>(repository, mapper)
{
    protected override Guid GetId(PointCategoryDto dto) => dto.Id;
}

public class PointsController(IRepositoryBase<Point, Guid> repository, IMapper mapper) :
    BaseController<Point, PointDto, CreatePointDto, UpdatePointDto, Guid>(repository, mapper)
{
    protected override Guid GetId(PointDto dto) => dto.Id;
}
