using LearningPlan.DomainModel;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace LearningPlan.ObjectServices.Implementation.Mongo;

public class UserActivationCodeObjectService : ObjectService, IUserActivationCodeObjectService
{
    private readonly IMongoDatabase _database;

    public UserActivationCodeObjectService(IMongoDatabase database) : base(database)
    {
        _database = database;
    }

    public override string CollectionName => "usersActiveCodes";

    public async Task<UserActivationCode> GetCodeByUserAsync(User user)
    {
        return await _database.GetCollection<UserActivationCode>(CollectionName)
            .Find(Builders<UserActivationCode>.Filter.Eq(code => code.UserId, user.Id)).SingleOrDefaultAsync();
    }

    public async Task<UserActivationCode> GetCodeAsync(Guid code)
    {
        return await _database.GetCollection<UserActivationCode>(CollectionName)
            .Find(Builders<UserActivationCode>.Filter.Eq(activationCode => activationCode.Code, code)).SingleOrDefaultAsync();
    }

    public async Task<UserActivationCode> CreateCodeAsync(User user)
    {
        var code = await GetCodeByUserAsync(user);

        if (code != null)
        {
            throw new InvalidOperationException("User already exist.");
        }

        UserActivationCode userActivationCode = new UserActivationCode
        {
            Code = Guid.NewGuid(),
            UserId = user.Id,
        };

        await CreateAsync(userActivationCode);

        return userActivationCode;
    }
}