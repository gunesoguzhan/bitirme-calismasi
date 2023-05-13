using Meet.ProfileService.WebAPI.Dtos;
using Meet.ProfileService.WebAPI.Entities;

namespace Meet.ProfileService.WebAPI.Common;

public static class ProfileExtension
{
    public static ProfileDto AsDto(this Profile profile) =>
        new ProfileDto(profile.FirstName, profile.LastName, profile.FullName);

}