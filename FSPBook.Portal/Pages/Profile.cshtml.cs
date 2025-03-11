using FSPBook.Data.DTOs;
using FSPBook.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSPBook.Portal.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IPostRepository _postRepository;

        public ProfileModel(IProfileRepository profileRepository, IPostRepository postRepository)
        {
            _profileRepository = profileRepository;
            _postRepository = postRepository;
        }

        public ProfileDto Profile { get; set; }
        public List<PostDto> Posts { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Fetch the profile
            var profile = await _profileRepository.GetProfileByIdAsync(id);
            if (profile == null)
            {
                return NotFound();
            }
            else
            {
                Profile = ProfileDto.FromProfile(profile);
            }

            // Fetch the latest posts for the profile
            var posts = await _postRepository.GetPostsByAuthorIdAsync(id);
            Posts = posts.Select(PostDto.FromPost).ToList();

            return Page();
        }
    }
}
