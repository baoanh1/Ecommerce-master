using Ecommerce.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce
{
    public interface IApi : IDisposable
    {
        IParamService Params { get; }
        /// <summary>
        /// Gets the media service.
        /// </summary>
        IMediaService Media { get; }
    }
}
