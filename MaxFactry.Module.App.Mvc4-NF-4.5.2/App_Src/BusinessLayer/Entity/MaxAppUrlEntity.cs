// <copyright file="MaxAppUrlEntity.cs" company="Lakstins Family, LLC">
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
// <change date="7/20/2014" author="Brian A. Lakstins" description="Initial Release">
// <change date="8/13/2014" author="Brian A. Lakstins" description="Moved MaxAppLogic method here to be able to use some caching.  Updated URL match logic.">
// <change date="9/30/2014" author="Brian A. Lakstins" description="Integrate caching.">
// <change date="12/18/2014" author="Brian A. Lakstins" description="Updates to follow core data access patterns.">
// <change date="6/10/2014" author="Brian A. Lakstins" description="Updated to get storage key from cache better.">
// <change date="4/20/2016" author="Brian A. Lakstins" description="Updated to use centralized caching.">
// <change date="11/8/2017" author="Brian A. Lakstins" description="Remove unnecessary setlist calls">
// <change date="9/24/2019" author="Brian A. Lakstins" description="Add sending of email when there is not a match.">
// <change date="10/1/2018" author="Brian A. Lakstins" description="Add RedirectUrl support.  Update matching logic to use bitflags to indicate match type.">
// <change date="10/8/2018" author="Brian A. Lakstins" description="Update matching priority.">
// <change date="10/19/2018" author="Brian A. Lakstins" description="Update weight logic for matching.">
// <change date="8/7/2019" author="Brian A. Lakstins" description="Updated match reporting to use logging instead of email.">
// <change date="5/21/2020" author="Brian A. Lakstins" description="Add some logging for when there is not a match.">
// <change date="5/26/2020" author="Brian A. Lakstins" description="Report lack of Url match only once.">
// <change date="1/16/2021" author="Brian A. Lakstins" description="Update definition of cache keys.">
// <change date="6/2/2021" author="Brian A. Lakstins" description="Move to Mvc4 namespace.">
// <change date="6/2/2021" author="Brian A. Lakstins" description="Add Querystring handling">
// <change date="3/31/2024" author="Brian A. Lakstins" description="Updated for changes to dependency classes.">
// <change date="4/6/2025" author="Brian A. Lakstins" description="Use methods from base class.">
// <change date="6/17/2025" author="Brian A. Lakstins" description="Update logging.">
// </changelog>
#endregion

namespace MaxFactry.Module.App.Mvc4.BusinessLayer
{
    using System;
    using MaxFactry.Core;
    using MaxFactry.Base.BusinessLayer;
    using MaxFactry.Base.DataLayer;
    using MaxFactry.Base.DataLayer.Library;
    using MaxFactry.Module.App.DataLayer;
    using MaxFactry.Module.App.Mvc4.DataLayer;
    using System.Collections.Specialized;

    /// <summary>
    /// Entity that allows interaction with information about Uri related to an App.
    /// </summary>
    public class MaxAppUrlEntity : MaxFactry.Base.BusinessLayer.MaxBaseGuidKeyEntity
    {
        /// <summary>
        /// Matches Server exactly
        /// </summary>
        public const int MatchServerExactFlag = 24;

        /// <summary>
        /// Matches server
        /// </summary>
        public const int MatchServerFlag = 22;

        /// <summary>
        /// Matches script exactly
        /// </summary>
        public const int MatchScriptExactFlag = 28;

        /// <summary>
        /// Matches script in some way
        /// </summary>
        public const int MatchScriptFlag = 26;

        /// <summary>
        /// Matches script in some way
        /// </summary>
        public const int MatchScriptWildOnlyFlag = 20;

        /// <summary>
        /// Initializes a new instance of the MaxAppUrlEntity class
        /// </summary>
        /// <param name="loData">object to hold data</param>
        public MaxAppUrlEntity(MaxFactry.Base.DataLayer.MaxData loData)
            : base(loData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxAppUrlEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxAppUrlEntity(Type loDataModelType)
            : base(loDataModelType)
        {
        }

        /// <summary>
        /// Gets or sets the name of the server in the Uri
        /// </summary>
        public string ServerName
        {
            get
            {
                return this.GetString(this.DataModel.ServerName);
            }

            set
            {
                this.Set(this.DataModel.ServerName, value);
            }
        }

        /// <summary>
        /// Gets or sets the direct path to a script on the server
        /// </summary>
        public string Script
        {
            get
            {
                return this.GetString(this.DataModel.Script);
            }

            set
            {
                this.Set(this.DataModel.Script, value);
            }
        }

        /// <summary>
        /// Gets or sets the URL to redirect this script to
        /// </summary>
        public string RedirectUrl
        {
            get
            {
                return this.GetString(this.DataModel.RedirectUrl);
            }

            set
            {
                this.Set(this.DataModel.RedirectUrl, value);
            }
        }

        /// <summary>
        /// Gets or sets the Id of the App to which this Uri is related.
        /// </summary>
        public Guid AppId
        {
            get
            {
                return this.GetGuid(this.DataModel.AppId);
            }

            set
            {
                this.Set(this.DataModel.AppId, value);
            }
        }

        /// <summary>
        /// Gets the Data Model for this entity
        /// </summary>
        protected MaxAppUrlDataModel DataModel
        {
            get
            {
                return (MaxAppUrlDataModel)MaxDataLibrary.GetDataModel(this.DataModelType);
            }
        }

        /// <summary>
        /// Dynamically creates a new instance of this entity.
        /// </summary>
        /// <returns>A new instance of this entity.</returns>
        public static MaxAppUrlEntity Create()
        {
            return MaxBusinessLibrary.GetEntity(
                typeof(MaxAppUrlEntity),
                typeof(MaxAppUrlDataModel)) as MaxAppUrlEntity;
        }

        /// <summary>
        /// Loads an entity based on the Id
        /// </summary>
        /// <param name="loAppId">The Id of the entity to load</param>
        /// <returns>True if data was found, loaded, and not marked as deleted.  False could be not found, or deleted.</returns>
        public MaxEntityList LoadAllByAppId(Guid loAppId)
        {
            return this.LoadAllByPropertyCache(this.DataModel.AppId, loAppId);
        }

        /// <summary>
        /// Loads an entity based on the Id
        /// </summary>
        /// <param name="lsServerName">Name of the server to match when loading.</param>
        /// <returns>True if data was found, loaded, and not marked as deleted.  False could be not found, or deleted.</returns>
        public MaxEntityList LoadAllByServerName(string lsServerName)
        {
            return this.LoadAllByPropertyCache(this.DataModel.ServerName, lsServerName);
        }

        /// <summary>
        /// Gets a string that can be used to sort a list of this entity.
        /// </summary>
        /// <returns>Lowercase version of Name passed to 100 characters.</returns>
        public override string GetDefaultSortString()
        {
            return this.ServerName.ToLowerInvariant().PadRight(100, ' ') + this.Script.ToLowerInvariant().PadRight(100, ' ') + base.GetDefaultSortString();
        }

        /// <summary>
        /// Loads the entity that best matches the URL
        /// </summary>
        /// <param name="loUrl">Uri to use to look up the entity.</param>
        /// <returns>Match Level</returns>
        public int LoadByUrl(Uri loUrl)
        {
            string lsLog = string.Empty;
            string lsCacheKey = this.GetCacheKey("LoadByUrl/" + loUrl.Host + "/" + loUrl.AbsolutePath + "/" + loUrl.Query);
            lsLog += "CacheKey=" + lsCacheKey + "\r\n";
            MaxData loData = MaxCacheRepository.Get(this.GetType(), lsCacheKey, typeof(MaxData)) as MaxData;
            int lnR = MaxConvertLibrary.ConvertToInt(typeof(object), MaxCacheRepository.Get(this.GetType(), lsCacheKey + "MatchLevel", typeof(string)));
            if (null != loData && lnR > 0)
            {
                this.Load(loData);
            }
            else if (lnR < 0)
            {
                lsLog += "ApplicationKey=" + MaxDataLibrary.GetApplicationKey() + "\r\n";
                MaxEntityList loEntityList = this.LoadAllCache();
                Guid loR = Guid.Empty;
                int lnMatchLevelCurrent = 0;
                lsLog += "Checking [" + loEntityList.Count + "] entities\r\n";
                for (int lnE = 0; lnE < loEntityList.Count; lnE++)
                {
                    MaxAppUrlEntity loEntity = (MaxAppUrlEntity)loEntityList[lnE];
                    if (loEntityList.Count < 100)
                    {
                        lsLog += "Servername: [" + loEntity.ServerName.ToLower() + "]  Host: + [" + loUrl.Host.ToLower() + "]\r\n";
                    }

                    if (loEntity.IsActive)
                    {
                        if (loEntityList.Count < 100)
                        {
                            lsLog += "Active\r\n";
                        }

                        // Rate all matches by how closely they match.
                        int lnMatchLevel = 0;
                        if (loEntity.ServerName.ToLower().Equals(loUrl.Host.ToLower()))
                        {
                            // Exact match is best.
                            lnMatchLevel += (int)Math.Pow(2, MatchServerExactFlag);
                        }
                        else if (loEntity.ServerName.StartsWith("*.") && loEntity.ServerName.Length > 2)
                        {
                            if (loUrl.Host.ToLower().EndsWith(loEntity.ServerName.Substring(2).ToLower()))
                            {
                                // Longest wildcard match is best.
                                lnMatchLevel += (int)Math.Pow(2, MatchServerFlag) + loEntity.ServerName.Length;
                            }
                        }
                        else if (loEntity.ServerName == "*")
                        {
                            lnMatchLevel += (int)Math.Pow(2, MatchServerFlag);
                        }

                        //// Only match scripts that already match the server.
                        if (lnMatchLevel > 0 && !string.IsNullOrEmpty(loEntity.Script))
                        {
                            int lnMatchLevelServer = lnMatchLevel;
                            string lsScript = loEntity.Script;
                            bool lbIsQSMatch = true;
                            if (lsScript.Contains("?"))
                            {
                                lbIsQSMatch = false;
                                lsScript = lsScript.Split('?')[0];
                                if (!string.IsNullOrEmpty(loUrl.Query))
                                {
                                    lbIsQSMatch = true;
                                    NameValueCollection loQSScript = System.Web.HttpUtility.ParseQueryString(loEntity.Script.Substring(lsScript.Length));
                                    NameValueCollection loQSUrl = System.Web.HttpUtility.ParseQueryString(loUrl.Query);
                                    foreach (string lsName in loQSScript.AllKeys)
                                    {
                                        if (loQSScript[lsName] != loQSUrl[lsName])
                                        {
                                            lbIsQSMatch = false;
                                        }
                                    }
                                }
                            }

                            if (lbIsQSMatch)
                            {
                                if (lsScript.ToLower().Equals(loUrl.AbsolutePath.ToLower()))
                                {
                                    // Exact Match is best.
                                    lnMatchLevel += (int)Math.Pow(2, MatchScriptExactFlag);
                                }
                                else if (lsScript == "*")
                                {
                                    // Longest wildcard path match is best.
                                    lnMatchLevel += (int)Math.Pow(2, MatchScriptWildOnlyFlag) + lsScript.Length;
                                }
                                else if (lsScript.EndsWith("*") && loUrl.AbsolutePath.StartsWith(lsScript.Substring(0, lsScript.Length - 1)))
                                {
                                    // Longest wildcard path match is best.
                                    lnMatchLevel += (int)Math.Pow(2, MatchScriptFlag) + lsScript.Length;
                                }
                            }

                            if (lnMatchLevel == lnMatchLevelServer)
                            {
                                lnMatchLevel = 0;
                            }
                        }
                        
                        if (loEntityList.Count < 100)
                        {
                            lsLog += "MatchLevel=" + lnMatchLevel + "  Current=" + lnMatchLevelCurrent + "\r\n";
                        }

                        if (lnMatchLevel > lnMatchLevelCurrent)
                        {
                            lnR = lnMatchLevel;
                            loData = loEntity.GetData();
                            lnMatchLevelCurrent = lnMatchLevel;
                        }
                    }
                }

                if (null != loData && lnR > 0)
                {
                    MaxCacheRepository.Set(this.GetType(), lsCacheKey, loData, this.GetCacheExpire());
                    this.Load(loData);
                    MaxCacheRepository.Set(this.GetType(), lsCacheKey + "MatchLevel", lnR, this.GetCacheExpire());
                }

                if (lnMatchLevelCurrent == 0)
                {
                    MaxCacheRepository.Set(this.GetType(), lsCacheKey + "MatchLevel", lnMatchLevelCurrent, this.GetCacheExpire());
                    string lsSentKey = "MaxAppUrlEntity.LoadFromUrlNotification-" + loUrl.Host;
                    bool lbReported = MaxConvertLibrary.ConvertToBoolean(typeof(object), MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopePersistent, lsSentKey));
                    if (!lbReported && loUrl.Host != "localhost" && !loUrl.Host.EndsWith("116vb.local"))
                    {
                        MaxLogLibrary.Log(new MaxLogEntryStructure(this.GetType(), "LoadByUrl", MaxEnumGroup.LogError, "URL Match not found. {Url} {lsLog} {Environment}", new MaxException("AppUrl Match Error"), loUrl, lsLog, MaxFactry.Core.MaxLogLibrary.GetEnvironmentInformation()));
                        MaxConfigurationLibrary.SetValue(MaxEnumGroup.ScopePersistent, lsSentKey, "true");
                    }
                }

                MaxConfigurationLibrary.SetValue(MaxEnumGroup.ScopeProcess, MaxFactryLibrary.MaxStorageKeyName, null);
            }

            return lnR;
        }
    }
}
