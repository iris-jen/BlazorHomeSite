using System.ComponentModel.DataAnnotations;

namespace BlazorHomeSite.Data;

public class Photo
{
    public int Id { get; private set; }
    
    
    public DateTime CaptureTime { get; private set; }
        
        
    [StringLength(100, ErrorMessage = $"Description Cannot Exceed 100")]
    public string? Description { get; private set; }
}