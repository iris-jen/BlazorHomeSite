namespace BlazorHomeSite.Data.Music;

public class Album
{
    public int Id { get; set; }
    public string AlbumName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AlbumCover { get; set; } = string.Empty;
    public DateTime DateRecorded { get; set; }
    public List<Song> Songs { get; set; } = new List<Song>();
}