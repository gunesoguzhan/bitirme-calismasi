using System.Security.Claims;
using FluentValidation;
using ProfileService.WebAPI.Dtos;

namespace ProfileService.WebAPI.Features.Validation;

public class AcceptFriendshipRequestDtoValidator : AbstractValidator<AcceptFriendshipRequestDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AcceptFriendshipRequestDtoValidator(IHttpContextAccessor httpContextAccessor)
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