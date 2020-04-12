using Ecommerce.Repositories;
using Ecommerce.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce
{
    public sealed class Api : IApi, IDisposable
    {
        private readonly ICache _cache;
        public IParamService Params { get; }
        public IMediaService Media { get; }
        public Api(IMediaRepository mediaRepository, ICache cache = null, IStorage storage = null, IImageProcessor processor = null)
        {
            // Store the cache
            _cache = cache;
            Media = new MediaService(mediaRepository, Params, storage, processor, cache);
        }
        public void Dispose()
        {
        }
    }
}
