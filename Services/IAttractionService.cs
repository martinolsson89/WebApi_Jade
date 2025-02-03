using Models;
using Models.DTO;

namespace Services;

public interface IAttractionService {

    public Task<ResponsePageDto<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
    public Task<ResponseItemDto<IAttraction>> ReadAttractionAsync(Guid id, bool flat);
    public Task<ResponseItemDto<IAttraction>> UpdateAttractionAsync(AttractionCuDto item);
    public Task <ResponseItemDto<IAttraction>> DeleteAttractionAsync(Guid id);
    public Task<ResponseItemDto<IAttraction>> PostAttractionAsync(AttractionCuDto itemDto);

    public Task<ResponsePageDto<IComment>> ReadCommentsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
    public Task<ResponseItemDto<IComment>> ReadCommentAsync(Guid id, bool flat);
    public Task<ResponseItemDto<IComment>> DeleteCommentAsync(Guid id);
    public Task<ResponseItemDto<IComment>> UpdateCommentAsync(CommentCuDto item);
    public Task<ResponseItemDto<IComment>> CreateCommentAsync(CommentCuDto item);
}
