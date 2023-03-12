namespace BlazorHomeSite.Data.Domain;

public class Song : BaseEntity
{
    public MusicAlbum Album { get; set; } = new();
    public int AlbumId { get; set; }
    public string Format { get; set; } = string.Empty;

    public string Lyrics { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string SongName { get; set; } = string.Empty;
}