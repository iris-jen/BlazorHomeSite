namespace BlazorHomeSite.Data.Domain;

public class MusicAlbum : BaseEntity
{
    public Photo? AlbumCover { get; set; }
    public string AlbumName { get; set; } = string.Empty;
    public DateTime DateRecorded { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<Song> Songs { get; set; } = new();
}