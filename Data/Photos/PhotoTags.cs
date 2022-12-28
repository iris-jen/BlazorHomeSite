namespace BlazorHomeSite.Data;

public class PhotoTags
{
    public int Id { get; set; }
    public string TagName { get; set; }
    public List<Photo> Photos { get; set; }
}