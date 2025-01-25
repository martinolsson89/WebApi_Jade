namespace Models;

public interface IAttraction 
{
    public Guid AttractionId { get; set; }
    public string Description { get; set; }
}