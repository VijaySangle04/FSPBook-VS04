using FSPBook.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FSPBook.Data.Repositories.Tests
{
    [TestClass]
    public class PostRepositoryTests
    {
        private Context _mockContext;
        private PostRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var _dbContextOptions = new DbContextOptionsBuilder<Context>()
                                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique database
                                    .Options;
            _mockContext = new Context(_dbContextOptions);
            _repository = new PostRepository(_mockContext);
        }

        [TestMethod]
        public async Task AddPostAsync_AddsPostToContext()
        {
            // Arrange
            var post = new Post { Id = 1, Content = "Test Post" };

            // Act
            await _repository.AddPostAsync(post);

            // Assert
            Assert.AreEqual(1, _mockContext.Post.Count());
            Assert.AreEqual(post.Id, _mockContext.Post.First().Id);
        }

        [TestMethod]
        public async Task GetAllPostsAsync_ReturnsAllPosts()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post { Id = 1, Content = "Post 1", Author = new Profile{ Id=1, FirstName="Test", LastName="User1"} },
                new Post { Id = 2, Content = "Post 2", Author = new Profile{ Id=2, FirstName="Test", LastName="User2"} }
            }.AsQueryable();

            _mockContext.Post.AddRange(posts);
            _mockContext.SaveChanges();
            // Act
            var result = await _repository.GetAllPostsAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task GetPostByIdAsync_ReturnsNull_WhenPostDoesNotExist()
        {
            // Arrange
            var posts = new List<Post>().AsQueryable();
            _mockContext.Post.AddRange(posts);
            _mockContext.SaveChanges();

            // Act
            var result = await _repository.GetPostByIdAsync(1);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetPaginatedPostsAsync_ReturnsPaginatedPosts()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post { Id = 1, Content = "Post 1", Author = new Profile{ Id=1, FirstName="Test", LastName="User1"} },
                new Post { Id = 2, Content = "Post 2", Author = new Profile{ Id=2, FirstName="Test", LastName="User2"} },
                new Post { Id = 3, Content = "Post 3", Author = new Profile{ Id=3, FirstName="Test", LastName="User3"} },
                new Post { Id = 4, Content = "Post 4", Author = new Profile{ Id=4, FirstName="Test", LastName="User4"} },
                new Post { Id = 5, Content = "Post 5", Author = new Profile{ Id=5, FirstName="Test", LastName="User5"} }
            };
            _mockContext.Post.AddRange(posts);
            _mockContext.SaveChanges();

            // Act
            var result = await _repository.GetPaginatedPostsAsync(1, 2);

            // Assert
            Assert.AreEqual(5, result.TotalCount);
            Assert.AreEqual(2, result.Posts.Count());
        }
    }

}
