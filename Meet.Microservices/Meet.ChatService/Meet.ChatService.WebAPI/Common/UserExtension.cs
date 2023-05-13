using Meet.ChatService.WebAPI.Dtos;
using Meet.ChatService.WebAPI.Entities;

namespace Meet.ChatService.WebAPI.Common;

public static class UserExtension
{
    public static UserDto AsDto(this User user)
        => new UserDto(user.Id, user.Username, user.FirstName, user.LastName);
}