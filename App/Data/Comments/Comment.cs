using Ardalis.GuardClauses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorHomeSite.Data.Comments
{
    public class Comment : BaseEntity
    {
        [Required]
        [StringLength(MaxCommentLength)]
        public string Content { get; private set; }

        public int UserId { get; private set; }

        [NotMapped]
        public const int MaxCommentLength = 512;

        public Comment(string content, int userId)
        {
            Content = Guard.Against.InvalidInput(content, nameof(content), InvalidContentPredicate);
            UserId = userId;
        }

        public void UpdateContent(string content) => Content = Guard.Against.InvalidInput(content, nameof(content), InvalidContentPredicate);

        private bool InvalidContentPredicate(string c) => !string.IsNullOrEmpty(c) && c.Length <= MaxCommentLength;
    }
}