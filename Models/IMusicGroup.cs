namespace Models;

public enum MusicGenre {Rock, Blues, Jazz, Metal} 

public interface IMusicGroup
{
    public  Guid Id { get; set;}
    public  string Name { get; set;}
    public  int EstablshedYear { get; set;}
    public  MusicGenre Genre { get; set;}
}