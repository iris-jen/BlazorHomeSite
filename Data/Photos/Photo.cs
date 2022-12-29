namespace BlazorHomeSite.Data;

public class Photo
{
    public int Id { get; set; }
    public bool IsAlbumCover { get; set; }
    public DateTime CaptureTime { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? LocationCoOrdinates { get; set; }
    public string? PhotoPath { get; set; }
    public string? ThumbnailPath { get; set; }
    public int ThumbnailRotation { get; set; }
    public int PhotoRotation { get; set; }
    public PhotoAlbum? Album { get; set; }
    public int AlbumId { get; set; }
    public List<PhotoTags>? Tags { get; set; }
}