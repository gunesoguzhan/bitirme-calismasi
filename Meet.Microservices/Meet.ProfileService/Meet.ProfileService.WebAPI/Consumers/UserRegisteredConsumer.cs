using MassTransit;
using Meet.IdentityService.Contracts;
using Meet.ProfileService.WebAPI.Data;

namespace Meet.ProfileService.WebAPI.Consumers;

public class UserRegisteredConsumer : IConsumer<UserRegistered>
{
    private readonly ApplicationDbContext _dbContext;

    public UserRegisteredConsumer(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<UserRegistered> context)
    {
        var message = context.Message;
        var item = _dbContext.Profiles.FirstOrDefault(x => x.UserId == message.Id);
        if (item != null)
            return;
        await _dbContext.Profiles.AddAsync(new()
        {
            UserId = message.Id,
            FirstName = message.firstName,
            LastName = message.lastName
        });
        await _dbContext.SaveChangesAsync();
    }
}
