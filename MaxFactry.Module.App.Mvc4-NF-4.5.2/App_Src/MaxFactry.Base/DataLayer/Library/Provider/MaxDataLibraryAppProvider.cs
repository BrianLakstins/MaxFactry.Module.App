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
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Library.Provider
{
    using System;
    using System.Diagnostics;
    using System.Web;
    using MaxFactry.Core;
    using MaxFactry.Module.App.Mvc4.BusinessLayer;

    /// <summary>
    /// Provides methods to manipulate storage of data
    /// </summary>
    public class MaxDataLibraryAppProvider : MaxDataLibraryGeneralAspNetProvider
    {
        /// <summary>
        /// Gets the storage key used to separate the storage of data
        /// </summary>
        /// <param name="loData">The data to be stored using the storage key.</param>
        /// <returns>string used for the storage key</returns>
        public override string GetStorageKey(MaxData loData)
        {
            string lsLog = string.Empty;
            Stopwatch loWatch = Stopwatch.StartNew();
            string lsR = this.GetStorageKeyFromProcess();
            lsLog += "|GetStorageKeyFromProcess|" + loWatch.ElapsedTicks;
            if (null == lsR || lsR.Length.Equals(0))
            {
                lsR = this.GetStorageKeyFromUrl();
                lsLog += "|GetStorageKeyFromUrl|" + loWatch.ElapsedTicks;
                if (null == lsR || lsR.Length.Equals(0))
                {
                    lsR = this.GetStorageKeyFromQueryString();
                    lsLog += "|GetStorageKeyFromQueryString|" + loWatch.ElapsedTicks;
                    if (null == lsR || lsR.Length.Equals(0))
                    {
                        lsR = this.GetStorageKeyFromCookie();
                        lsLog += "|GetStorageKeyFromCookie|" + loWatch.ElapsedTicks;
                        if (null == lsR || lsR.Length.Equals(0))
                        {
                            lsR = this.GetStorageKeyFromConfiguration();
                            lsLog += "|GetStorageKeyFromConfiguration|" + loWatch.ElapsedTicks;
                        }
                    }
                }
            }

            if (lsR.Length == 0)
            {
                MaxFactry.Core.MaxLogLibrary.Log(new MaxLogEntryStructure("GetStorageKeyProvider", MaxEnumGroup.LogError, "GetStorageKey(MaxData loData) ended with blank storagekey."));
            }
            else
            {
                long lnDuration = loWatch.ElapsedMilliseconds;
                if (lnDuration > 500)
                {
                    MaxFactry.Core.MaxLogLibrary.Log(new MaxLogEntryStructure("GetStorageKeyProvider", MaxEnumGroup.LogWarning, "GetStorageKey took {lnDuration} ms for storage key {lsStorageKey} with log {lsLog}.", lnDuration, lsR, lsLog));
                }
                else
                {
                    MaxFactry.Core.MaxLogLibrary.Log(new MaxLogEntryStructure("GetStorageKeyProvider", MaxEnumGroup.LogDebug, "GetStorageKey took {lnDuration} ms for storage key {lsStorageKey} with log {lsLog}.", lnDuration, lsR, lsLog));
                }
            }

            return lsR;
        }

        protected virtual string GetStorageKeyFromUrl()
        {
            string lsR = string.Empty;
            if (null != this.Request)
            {
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
            }

            return lsR;
        }
    }
}
