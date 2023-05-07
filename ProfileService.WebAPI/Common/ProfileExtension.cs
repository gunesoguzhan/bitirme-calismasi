using ProfileService.WebAPI.Dtos;
using ProfileService.WebAPI.Entities;

namespace ProfileService.WebAPI.Common;

public static class ProfileExtension
{
    public static ProfileDto AsDto(this Profile profile) =>
        new ProfileDto(profile.FirstName, profile.LastName, profile.FullName);

}