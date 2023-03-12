namespace BlazorHomeSite.Data.Domain
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public int UserId { get; set; }
    }
}