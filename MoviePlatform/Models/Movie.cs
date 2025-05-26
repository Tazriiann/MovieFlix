public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public string? VideoPath { get; set; }
    public string? PosterPath { get; set; }

    // Parameterless constructor required by EF
    public Movie() { }

    // Optional constructor for manual use
    public Movie(string title = "Unknown Title", string description = "No description available.", string genre = "Unknown Genre", DateTime? releaseDate = null)
    {
        Title = title;
        Description = description;
        Genre = genre;
        ReleaseDate = releaseDate ?? DateTime.Now;
    }
}
