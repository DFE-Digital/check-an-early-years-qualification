using Microsoft.Extensions.Caching.Distributed;

namespace Dfe.EarlyYearsQualification.Content.Caching;

public class NoCache : IDistributedCache
{
    public byte[]? Get(string key)
    {
        return null;
    }

    public Task<byte[]?> GetAsync(string key, CancellationToken token = new())
    {
        return Task.FromResult<byte[]?>(null);
    }

    public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
    {
        // do nothing
    }

    public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options,
                         CancellationToken token = new())
    {
        return Task.CompletedTask;
    }

    public void Refresh(string key)
    {
        // do nothing
    }

    public Task RefreshAsync(string key, CancellationToken token = new())
    {
        return Task.CompletedTask;
    }

    public void Remove(string key)
    {
        // do nothing
    }

    public Task RemoveAsync(string key, CancellationToken token = new())
    {
        return Task.CompletedTask;
    }
}