using FSPBook.Data.DTOs;

namespace FSPBook.Services.Profiles
{
    public interface IGetProfilesService
    {
        Task<List<ProfileDto>> GetProfilesAsync();
    }
}