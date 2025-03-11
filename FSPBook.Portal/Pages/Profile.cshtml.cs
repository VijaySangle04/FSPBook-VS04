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
        public List<PostDto> Posts { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Fetch user profile by Id
            var userProfile = await _profileRepository.GetProfileByIdAsync(id);
            if (userProfile == null)
            {
                return NotFound();
            }

            Profile = ProfileDto.FromProfile(userProfile);

            // Fetch all posts by user in descending order
            var posts = await _postRepository.GetPostsByAuthorIdAsync(id);
            Posts = posts.Select(PostDto.FromPost).ToList();

            return Page();
        }
    }
}
