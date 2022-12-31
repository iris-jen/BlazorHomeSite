namespace BlazorHomeSite.Data.Music;

public class Song
{
    public int Id { get; set; }
    
    public string SongName { get; set; }
    public string Lyrics { get; set; }
    public double PlayTimeSeconds { get; set; }
    public string Format { get; set; }
    public Album Album { get; set; }
    public int AlbumId { get; set; }
}