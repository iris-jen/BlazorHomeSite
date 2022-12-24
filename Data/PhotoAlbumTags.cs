namespace BlazorHomeSite.Data;

public class PhotoAlbumTags
{
    public int Id { get; set; }
    public string TagName { get; set; }

    public List<PhotoAlbum> Albums { get; set; }
}