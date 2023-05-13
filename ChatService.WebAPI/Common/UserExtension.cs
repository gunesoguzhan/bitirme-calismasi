using ChatService.WebAPI.Dtos;
using ChatService.WebAPI.Entities;

namespace ChatService.WebAPI.Common;

public static class UserExtension
{
    public static UserDto AsDto(this User user)
        => new UserDto(user.Id, user.Username, user.FirstName, user.LastName);
}