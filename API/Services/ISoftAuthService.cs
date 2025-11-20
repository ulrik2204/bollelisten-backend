using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace API.Services;

public interface ISoftAuthService
{

    public Task<string?> Authenticate(string sessionId, string groupSlug);

    public bool IsAuthenticated(string sessionId, string groupKey);

    public Task<Group?> GetAuthenticatedGroup(string sessionId, string groupKey);

}
public class SoftAuthService(AppDbContext context, IMemoryCache cache)
{
    private const string Separator = "|";

    // Returns the groupKey
    public async Task<string?> Authenticate(string sessionId, string groupSlug)
    {
        var group = await context.Groups.SingleOrDefaultAsync(group => group.Slug == groupSlug);
        if (group == null) return null;
        var cachedSessionKey = cache.Get<string>(group.Id);
        if (cachedSessionKey != null)
        {
            var (_, groupKey) = SeparateSessionKey(cachedSessionKey);
            return groupKey;
        }

        var newGroupKey = "key-" + Guid.NewGuid();
        var sessionKey = CreateSessionKey(sessionId, newGroupKey);
        cache.Set(group.Id, sessionKey, DateTimeOffset.Now.AddHours(1));
        cache.Set(sessionKey, group.Id, DateTimeOffset.Now.AddHours(1));

        return newGroupKey;
    }

    public bool IsAuthenticated(string sessionId, string groupKey)
    {
        var sessionKey = CreateSessionKey(sessionId, groupKey);
        var groupId = cache.Get<string>(sessionKey);
        if (groupId == null) return false;
        return true;
    }



    public async Task<Group?> GetAuthenticatedGroup(string sessionId, string groupKey)
    {
        // Get the groupId for the groupKey
        var sessionKey = CreateSessionKey(sessionId, groupKey);
        var groupId = cache.Get<string>(sessionKey);
        if (groupId == null) return null;

        var group = await context.Groups.SingleOrDefaultAsync(g => g.Id.ToString() == groupId);
        return group;
    }


    private string CreateSessionKey(string sessionId, string groupKey)
    {
        return $"{sessionId}{Separator}{groupKey}";
    }

    private (string, string) SeparateSessionKey(string sessionKey)
    {
        var list = sessionKey.Split(Separator);
        var sessionId = list[0];
        var groupKey = list[1];
        return (sessionId, groupKey);

    }

}