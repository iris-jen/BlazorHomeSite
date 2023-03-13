using Ardalis.GuardClauses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorHomeSite.Data.Domain
{
    public class Comment : BaseEntity
    {
        [Required]
        [MaxLength(MaxCommentLength)]
        public string Content { get; private set; }

        public int UserId { get; private set; }

        [NotMapped]
        public const int MaxCommentLength = 512;

        public Comment(string content, int userId)
        {
            Content = Guard.Against.InvalidInput(content, nameof(content), InvalidContentPredicate);
            UserId = userId;
        }

        private bool InvalidContentPredicate(string c) => string.IsNullOrEmpty(c) || c.Length > MaxCommentLength;

        private void UpdateContnet(string content) => Guard.Against.InvalidInput(content, nameof(content), InvalidContentPredicate);
    }
}