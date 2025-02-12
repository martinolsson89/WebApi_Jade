using System.Text.Json.Serialization;
using Models;

public class AttractionCuDto
{
    public virtual Guid? AttractionId { get; set; }
    public string AttractionTitle { get; set; }
    public string Description { get; set; }
    public virtual Guid CategoryId { get; set; }
    public virtual Guid AddressId { get; set; }
    public virtual List<Guid> CommentsId { get; set; } = null;
    [JsonIgnore]
    public FinancialRisk? Risk { get; set; }
    [JsonIgnore]
    public decimal? Revenue { get; set; }

    public AttractionCuDto() { }
    public AttractionCuDto(IAttraction org)
    {
        AttractionId = org.AttractionId;
        AttractionTitle = org.AttractionTitle;
        Description = org.Description;
        CategoryId = org.Category.CategoryId;
        AddressId = org.Address.AddressId;
        CommentsId = org.Comments?.Select(i => i.CommentId).ToList();
        Risk = org.Risk;
        Revenue = org.Revenue;
    }
}