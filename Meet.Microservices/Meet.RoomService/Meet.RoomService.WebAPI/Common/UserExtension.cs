using Meet.RoomService.WebAPI.Dtos;
using Meet.RoomService.WebAPI.Entities;

namespace Meet.RoomService.WebAPI.Common;

public static class UserExtension
{
    public static UserDto AsDto(this User user)
        => new UserDto(user.Id, user.FirstName, user.LastName);
}