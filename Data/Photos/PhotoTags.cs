namespace BlazorHomeSite.Data;

public class PhotoTags
{
    public int Id { get; set; }
    public string TagName { get; set; } = string.Empty;
    public List<Photo> Photos { get; set; } = new();
}