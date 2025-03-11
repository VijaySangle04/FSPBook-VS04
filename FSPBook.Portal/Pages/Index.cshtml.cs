using FSPBook.Data.DTOs;
using FSPBook.Services.News;
using FSPBook.Services.Posts;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FSPBook.Pages
{
    public class IndexModel : PageModel
    {
        private readonly INewsService _newsService;
        private readonly GetPostsService _getPostsService;

        public IndexModel(INewsService newsService, GetPostsService getPostsService)
        {
            _newsService = newsService;
            _getPostsService = getPostsService;
        }

        public List<PostDto> Posts { get; set; }
        public IEnumerable<NewsArticle> NewsArticles { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }

        private Timer timer;

        public async Task OnGetAsync(int? currentPage = 1)
        {
            // Fetch posts
            CurrentPage = currentPage ?? 1;
            Console.WriteLine($"Current Page: {CurrentPage}");

            var result = await _getPostsService.GetPostsAsync(CurrentPage, 10);
            Posts = result.Posts;
            TotalPages = result.TotalPages;

            Console.WriteLine($"Total Pages: {TotalPages}, Posts Fetched: {Posts.Count}");

            NewsArticles = await _newsService.GetTopHeadlinesAsync(5);
            timer = new Timer(async _ => await LoadNewsAsync(), null, TimeSpan.Zero, TimeSpan.FromSeconds(20));
        }

        private async Task LoadNewsAsync()
        {
            NewsArticles = await _newsService.GetTopHeadlinesAsync(5, true);
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}