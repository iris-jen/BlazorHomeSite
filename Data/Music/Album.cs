namespace BlazorHomeSite.Data.Music;

public class Album
{
    public int Id { get; set; }
    public string AlbumName { get; set; }
    public string Description { get; set; }
    public string AlbumCover { get; set; }
    public DateTime DateRecorded { get; set; }
    public List<Song> Songs { get; set; }
}