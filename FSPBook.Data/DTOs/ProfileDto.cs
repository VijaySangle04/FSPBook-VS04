using FSPBook.Data.Entities;

namespace FSPBook.Data.DTOs
{
    public class ProfileDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }

        public static ProfileDto FromProfile(Profile profile)
        {
            return new ProfileDto
            {
                Id = profile.Id,
                FullName = profile.FullName,
                JobTitle = profile.JobTitle
            };
        }
    }
}
