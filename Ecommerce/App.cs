/*
 * Copyright (c) 2016-2020 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 *
 * https://github.com/piranhacms/piranha.core
 *
 */

using System;
using System.Reflection;
using Ecommerce.Runtime;
using Newtonsoft.Json;

namespace Ecommerce
{
    /// <summary>
    /// The main application object.
    /// </summary>
    public sealed class App
    {
        /// <summary>
        /// The singleton app instance.
        /// </summary>
        private static readonly App Instance;

        /// <summary>
        /// Mutex for thread safe initialization.
        /// </summary>
        private static readonly object _mutex = new object();

        /// <summary>
        /// If the app has been initialized.
        /// </summary>
        private static volatile bool _isInitialized;
        /// <summary>
        /// The currently registered media types.
        /// </summary>
        private readonly MediaManager _mediaTypes;
        //private readonly AppModuleList _modules;
        /// <summary>
        /// The currently registered hooks.
        /// </summary>
        private readonly HookManager _hooks;
        //public static AppModuleList Modules => Instance._modules;
        public static HookManager Hooks => Instance._hooks;
        /// <summary>
        /// Gets the currently registered media types.
        /// </summary>
        public static MediaManager MediaTypes => Instance._mediaTypes;
        private Cache.CacheLevel _cacheLevel = Cache.CacheLevel.Full;
        /// <summary>
        /// Gets/sets the current cache level.
        /// </summary>
        public static Cache.CacheLevel CacheLevel
        {
            get => Instance._cacheLevel;
            set => Instance._cacheLevel = value;
        }
        static App()
        {
            Instance = new App();

            // Setup media types
            Instance._mediaTypes.Documents.Add(".pdf", "application/pdf");
            Instance._mediaTypes.Images.Add(".jpg", "image/jpeg");
            Instance._mediaTypes.Images.Add(".jpeg", "image/jpeg");
            Instance._mediaTypes.Images.Add(".png", "image/png");
            Instance._mediaTypes.Videos.Add(".mp4", "video/mp4");
            Instance._mediaTypes.Audio.Add(".mp3", "audio/mpeg");
            Instance._mediaTypes.Audio.Add(".wav", "audio/wav");

        


        }

        /// <summary>
        /// Default private constructor.
        /// </summary>
        private App()
        {
            
            _mediaTypes = new MediaManager();
        }

        /// <summary>
        /// Initializes the application.
        /// </summary>
        public static void Init(IApi api)
        {
            Instance.InitApp(api);
        }

      
        /// <summary>
        /// Initializes the application object.
        /// </summary>
        /// <param name="api">The current api</param>
        private void InitApp(IApi api)
        {
            
        }
    }
}
