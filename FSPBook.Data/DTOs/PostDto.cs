using FSPBook.Data.Entities;
using System;

namespace FSPBook.Data.DTOs
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public DateTimeOffset DateTimePosted { get; set; }

        public static PostDto FromPost(Post post)
        {
            return new PostDto
            {
                Id = post.Id,
                Content = post.Content,
                AuthorId = post.AuthorId,
                AuthorName = $"{post.Author.FirstName} {post.Author.LastName}",
                DateTimePosted = post.DateTimePosted
            };
        }
    }
}
