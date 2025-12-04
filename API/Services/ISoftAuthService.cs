using API.Extensions;
using Common.Services;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace API.Services;

public interface ISoftAuthService
{

    public Task<string?> Authenticate(string groupSlug);

    public bool IsAuthenticated();

    public Task<Group?> GetAuthenticatedGroup();

}
public class SoftAuthService(IMemoryCache cache, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment environment, IGroupService groupService): ISoftAuthService
{
    private const string Separator = "|";

    // Returns the groupKey
    public async Task<string?> Authenticate(string groupSlug)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) return null;

        var sessionId = httpContext.GetSessionId();
        if (sessionId == null) return null;

        var group = await groupService.GetGroupBySlug(groupSlug, includePeople: false);
        if (group == null) return null;
        var cachedSessionKey = cache.Get<string>(group.Id);
        if (cachedSessionKey != null)
        {
            var (_, groupKey) = SeparateSessionKey(cachedSessionKey);
            return groupKey;
        }

        var newGroupKey = "key-" + Guid.NewGuid();
        var sessionKey = CreateSessionKey(sessionId, newGroupKey);
        cache.Set(sessionKey, group.Id, DateTimeOffset.Now.AddHours(1));

        return newGroupKey;
    }

    public bool IsAuthenticated()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) return false;

        var sessionId = httpContext.GetSessionId();
        var groupKey = httpContext.GetGroupKeyCookie();
        if (sessionId == null || groupKey == null) return false;

        var sessionKey = CreateSessionKey(sessionId, groupKey);
        var groupId = cache.Get<Guid?>(sessionKey);
        Console.WriteLine("GroupId" + groupId);
        if (groupId == null) return false;
        return true;
    }



    public async Task<Group?> GetAuthenticatedGroup()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) return null;

        var sessionId = httpContext.GetSessionId();
        var groupKey = httpContext.GetGroupKeyCookie();
        if (sessionId == null || groupKey == null) return null;

        // Get the groupId for the groupKey
        var sessionKey = CreateSessionKey(sessionId, groupKey);
        var groupId = cache.Get<Guid?>(sessionKey);
        if (groupId is null) return null;

        var group = await groupService.GetGroupById(groupId.Value);
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