using System.Security.Claims;
using FluentValidation;
using ProfileService.WebAPI.Dtos;

namespace ProfileService.WebAPI.Features.Validation;

public class CancelFriendshipRequestDtoValidator : AbstractValidator<CancelFriendshipRequestDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CancelFriendshipRequestDtoValidator(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

        RuleFor(x => x.friendUserId)
            .NotEmpty()
            .Must(userIdAndFriendUserIdMustNotBeEqual);
    }

    private bool userIdAndFriendUserIdMustNotBeEqual(Guid friendUserId)
    {
        var userIdString = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdString == null || friendUserId == null)
            throw new ArgumentNullException();
        Guid userId;
        try
        {
            userId = Guid.Parse(userIdString);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return userId == friendUserId;
    }
}