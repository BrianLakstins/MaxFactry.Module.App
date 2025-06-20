﻿// <copyright file="MaxDataLibraryAppProvider.cs" company="Lakstins Family, LLC">
// Copyright (c) Brian A. Lakstins (http://www.lakstins.com/brian/)
// </copyright>

#region License
// <license>
// This software is provided 'as-is', without any express or implied warranty. In no 
// event will the author be held liable for any damages arising from the use of this 
// software.
//  
// Permission is granted to anyone to use this software for any purpose, including 
// commercial applications, and to alter it and redistribute it freely, subject to the 
// following restrictions:
// 
// 1. The origin of this software must not be misrepresented; you must not claim that 
// you wrote the original software. If you use this software in a product, an 
// acknowledgment (see the following) in the product documentation is required.
// 
// Portions Copyright (c) Brian A. Lakstins (http://www.lakstins.com/brian/)
// 
// 2. Altered source versions must be plainly marked as such, and must not be 
// misrepresented as being the original software.
// 
// 3. This notice may not be removed or altered from any source distribution.
// </license>
#endregion

#region Change Log
// <changelog>
// <change date="5/22/2020" author="Brian A. Lakstins" description="Initial creation">
// <change date="5/22/2020" author="Brian A. Lakstins" description="Update logging.  Removing caching because MaxAppUrlEntity already caches.">
// <change date="3/31/2024" author="Brian A. Lakstins" description="Updated for changes to dependency classes.">
// <change date="6/12/2025" author="Brian A. Lakstins" description="Update for ApplicationKey.  Try to prevent Stack Overflow">
// <change date="6/17/2025" author="Brian A. Lakstins" description="Add caching for ApplicationKey based on Url.">
// <change date="6/21/2025" author="Brian A. Lakstins" description="Add seting process configuration for results of application key.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Library.Provider
{
    using System;
    using System.Diagnostics;
    using System.Web;
    using System.Web.UI;
    using MaxFactry.Core;
    using MaxFactry.Module.App.Mvc4.BusinessLayer;

    /// <summary>
    /// Provides methods to manipulate storage of data
    /// </summary>
    public class MaxDataLibraryAppProvider : MaxDataLibraryGeneralAspNetProvider
    {
        private static readonly string ApplicationKeyForUrl = "_MaxDataLibraryAppProvider_ApplicationKeyForUrl";

        private static readonly string ApplicationKeyFromUrl = "_MaxDataLibraryAppProvider_ApplicationKeyFromUrl";

        private static readonly string ApplicationKey = "_MaxDataLibraryAppProvider_ApplicationKey";

        /// <summary>
        /// Gets the storage key used to separate the storage of data
        /// </summary>
        /// <param name="loData">The data to be stored using the storage key.</param>
        /// <returns>string used for the storage key</returns>
        public override string GetApplicationKey()
        {
            string lsR = MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopeProcess, ApplicationKey) as string;
            if (null == lsR || lsR.Length == 0)
            {
                lsR = base.GetApplicationKey();
                string lsApplicationKey = MaxConvertLibrary.ConvertToString(typeof(object), MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopeProcess, ApplicationKeyForUrl));
                if (null == lsApplicationKey || lsApplicationKey.Length == 0)
                {
                    lsApplicationKey = MaxConvertLibrary.ConvertToString(typeof(object), MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopeProcess, ApplicationKeyFromUrl));
                    if (string.IsNullOrEmpty(lsApplicationKey))
                    {
                        lsApplicationKey = this.GetStorageKeyFromUrl(lsR);
                        if (!string.IsNullOrEmpty(lsApplicationKey))
                        {
                            MaxConfigurationLibrary.SetValue(MaxEnumGroup.ScopeProcess, ApplicationKeyFromUrl, lsApplicationKey);
                        }
                    }
                }

                if (null != lsApplicationKey && lsApplicationKey.Length > 0)
                {
                    lsR = lsApplicationKey;
                }

                if (lsR.Length == 0)
                {
                    MaxFactry.Core.MaxLogLibrary.Log(new MaxLogEntryStructure(this.GetType(), "GetApplicationKey", MaxEnumGroup.LogError, "GetApplicationKey() ended with blank key."));
                }
                else
                {
                    MaxConfigurationLibrary.SetValue(MaxEnumGroup.ScopeProcess, ApplicationKey, lsR);
                }
            }

            return lsR;
        }

        protected virtual string GetStorageKeyFromUrl(string lsApplicationKey)
        {            
            string lsR = string.Empty;
            if (null != this.Request)
            {
                MaxConfigurationLibrary.SetValue(MaxEnumGroup.ScopeProcess, ApplicationKeyForUrl, lsApplicationKey);
                MaxAppUrlEntity loEntity = MaxAppUrlEntity.Create();
                int lnMatchLevel = loEntity.LoadByUrl(this.Request.Url);
                if (lnMatchLevel > 0)
                {
                    Guid loUrlStorageKey = loEntity.AppId;
                    if (!Guid.Empty.Equals(loUrlStorageKey))
                    {
                        MaxLogLibrary.Log(new MaxLogEntryStructure("GetStorageKeyFromUrl", MaxEnumGroup.LogInfo, "GetStorageKey from Url {loUrlStorageKey}{this.Request.Url} ", loUrlStorageKey, this.Request.Url));
                        lsR = loUrlStorageKey.ToString();
                        MaxConfigurationLibrary.SetValue(MaxEnumGroup.ScopeProcess, MaxFactryLibrary.MaxStorageKeyName, lsR);
                    }
                }

                MaxConfigurationLibrary.SetValue(MaxEnumGroup.ScopeProcess, ApplicationKeyForUrl, lsR);
            }
            else
            {                
                StackTrace loStackTrace = new StackTrace(true);
                if (!loStackTrace.ToString().Contains("System.Web.Hosting.PipelineRuntime.InitializeApplication(IntPtr appContext)") &&
                    !loStackTrace.ToString().Contains("System.Threading.ThreadHelper.ThreadStart()"))
                {
                    MaxException loException = new MaxException("GetStorageKeyFromUrl called with null Request.");
                    MaxLogLibrary.Log(new MaxLogEntryStructure(this.GetType(), "GetStorageKeyFromUrl", MaxEnumGroup.LogError, "GetStorageKeyFromUrl() called with null Request from {StackTrace}", loException, loStackTrace.ToString()));
                }
            }

            return lsR;
        }
    }
}
