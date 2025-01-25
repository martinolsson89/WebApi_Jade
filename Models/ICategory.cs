

using Models;

public enum CategoryNames {Action, Horror, Drama, Thriller, Romance, Comedy, Splatter, Foreign}

public interface ICategory {

    public Guid CategoryId { get; set; }

    public CategoryNames Name { get; set; }

    public List<IAttraction> Attractions { get; set; }

}