namespace Models;



public interface IAttraction 
{
    public Guid AttractionId { get; set; }
    public string AttractionTitle { get; set; }
    public string Description { get; set; }
    
    public IFinancial? Financial { get; set; }

    public ICategory Category { get; set; }
    public List<IComment> Comments { get; set;}
    public IAddress Address{ get; set; }
}


