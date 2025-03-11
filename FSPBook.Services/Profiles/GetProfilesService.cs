using FSPBook.Data.DTOs;
using FSPBook.Data.Repositories;

namespace FSPBook.Services.Profiles
{
    public class GetProfilesService : IGetProfilesService
    {
        private readonly IProfileRepository _profileRepository;

        public GetProfilesService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        /// <summary>
        /// Fetches all user profiles
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProfileDto>> GetProfilesAsync()
        {
            var profiles = await _profileRepository.GetAllProfilesAsync();
            return profiles.Select(ProfileDto.FromProfile).ToList();
        }
    }
}
