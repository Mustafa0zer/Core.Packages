using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Application.Pipelines.Caching;

public class CacheRemovingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ICacheRemoverRequest
{
    private readonly IDistributedCache _cache;

    public CacheRemovingBehavior(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if(request.BypassCache) 
            return await next();

        TResponse response = await next();

        if(request.CacheGroupKey != null)
        {
            byte[]? cachedGroup = await _cache.GetAsync(request.CacheGroupKey, cancellationToken);
            if (cachedGroup != null)
            {
                HashSet<string> keysInGroup=JsonSerializer.Deserialize<HashSet<string>>(Encoding.Default.GetString(cachedGroup));
                foreach (var key in keysInGroup)
                {
                    await _cache.RemoveAsync(key,cancellationToken);
                    //_logger.LogInformation($"Removed Cache -> {key}");
                }
                await _cache.RemoveAsync(request.CacheGroupKey, cancellationToken);
                //_logger.LogInformation($"Removed Cache -> {request.CacheGroupKey}");
                await  _cache.RemoveAsync(key: $"{request.CacheGroupKey}SlidingExpiration",cancellationToken);
            }
        }
        
        if(request.CachingKey != null)
        {
            await _cache.RemoveAsync(request.CachingKey,cancellationToken);
        }
        return response; 
    }
}
