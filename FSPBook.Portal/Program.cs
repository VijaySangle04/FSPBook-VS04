using FSPBook.Data;
using FSPBook.Data.Repositories;
using FSPBook.Data.Utilities;
using FSPBook.Services.News;
using FSPBook.Services.Posts;
using FSPBook.Services.Profiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<Context>(options => 
                                        options.UseInMemoryDatabase("FSPBookDataBase"));
builder.Services.AddRazorPages();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<INewsApiClient, NewsApiClient>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<INewsService, TechnologyNewsService>();
builder.Services.AddScoped<ICreatePostService, CreatePostService>();
builder.Services.AddScoped<IGetPostsService, GetPostsService>();
builder.Services.AddScoped<IGetProfilesService, GetProfilesService>();
builder.Services.AddMemoryCache();

var app = builder.Build();

SeedDatabase(app);

//Seed Data
void SeedDatabase(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            SeedData.Seed(services);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred seeding the DB.");
        }
    }
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

app.MapRazorPages();

app.Run();