using FSPBook.Data.DTOs;
using FSPBook.Data.Repositories;

namespace FSPBook.Services.Profiles
{
    public class GetProfilesService
    {
        private readonly IProfileRepository _profileRepository;

        public GetProfilesService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<List<ProfileDto>> GetProfilesAsync()
        {
            var profiles = await _profileRepository.GetAllAsync();
            return profiles.Select(ProfileDto.FromProfile).ToList();
        }
    }
}
