using FSPBook.Data;
using FSPBook.Data.DTOs;
using FSPBook.Services.Posts;
using FSPBook.Services.Profiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FSPBook.Pages
{
    public class CreateModel : PageModel
    {
        public Context DbContext { get; set; }

        private readonly ICreatePostService _createPostService;
        private readonly IGetProfilesService _getProfilesService;

        public List<ProfileDto> Profiles { get; set; }

        public bool Success { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Choose a person to post on behalf of")]
        [Range(1, 10000, ErrorMessage = "Choose a person to post on behalf of")]
        public int ProfileId { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Write a post")]
        [MinLength(1, ErrorMessage = "Post needs some content")]
        public string ContentInput { get; set; }

        public CreateModel(Context context, ICreatePostService createPostService, IGetProfilesService getProfilesService)
        {
            DbContext = context;
            _createPostService = createPostService;
            _getProfilesService = getProfilesService;
        }

        public async Task OnGetAsync()
        {
            await LoadProfiles();
        }

        /// <summary>
        /// Validate and Create a post
        /// </summary>
        /// <returns></returns>
        public async Task OnPostAsync()
        {
            if (ProfileId != -1)
            {
                var postId = await _createPostService
                    .CreatePostAsync(ProfileId, ContentInput);
                Success = true;
            }

            await LoadProfiles();
        }

        /// <summary>
        /// Load profiles for dropdown
        /// </summary>
        /// <returns></returns>
        private async Task LoadProfiles()
        {
            // Call GetProfilesService
            Profiles = await _getProfilesService.GetProfilesAsync();
        }
    }
}
