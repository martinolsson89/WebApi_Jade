using Models;
using Models.DTO;

namespace Services;

public interface ICategoryService {

    public Task<ResponsePageDto<ICategory>> ReadCategoriesAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
    public Task<ResponseItemDto<ICategory>> ReadCategoryAsync(Guid id, bool flat);

    //public Task<ResponseItemDto<ICategory>> UpdateAttractionAsync(Category item);

    //public Task <ResponseItemDto<ICategory>> DeleteAttractionAsync(Guid id);

    //public Task<ResponseItemDto<ICategory>> PostAttractionAsync(AttractionCuDto itemDto);

    }