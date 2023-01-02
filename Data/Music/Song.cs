namespace BlazorHomeSite.Data.Music;

public class Song
{
    public int Id { get; set; }

    public string SongName { get; set; } = string.Empty;
    public string Lyrics { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string Format { get; set; } = string.Empty;
    public Album Album { get; set; } = new Album();
    public int AlbumId { get; set; }
}