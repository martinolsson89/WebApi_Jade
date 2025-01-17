using Configuration;

namespace Models;

public class MusicGroup: IMusicGroup
{
    public virtual Guid Id { get; set;}
    public  virtual string Name { get; set;}
    public  int EstablshedYear { get; set;}
    public  MusicGenre Genre { get; set;}
}