// <copyright file="MaxRequestLibrary.cs" company="Lakstins Family, LLC">
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
// <change date="6/19/2014" author="Brian A. Lakstins" description="Initial Release">
// <change date="6/27/2014" author="Brian A. Lakstins" description="Add IsAuthenticated to be able to override Request.IsAuthenticated">
// <change date="10/1/2018" author="Brian A. Lakstins" description="Add Redirect Url support.  Send current URL to get storage key.">
// <change date="10/2/2018" author="Brian A. Lakstins" description="Allow redirect to same server, but using SSL.">
// <change date="10/19/2018" author="Brian A. Lakstins" description="Update redirect logic after testing on EFS site.">
// <change date="10/20/2018" author="Brian A. Lakstins" description="Fix referencing HttpContext URL when URL is being passed as arguement.">
// <change date="5/22/2020" author="Brian A. Lakstins" description="Fix reference to StorageKey.">
// <change date="3/31/2024" author="Brian A. Lakstins" description="Updated for changes to dependency classes.">
// </changelog>
#endregion

namespace MaxFactry.General.AspNet.PresentationLayer.Provider
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using MaxFactry.Core;
    using MaxFactry.Module.App.Mvc4.BusinessLayer;
    using MaxFactry.Module.App.BusinessLayer;
    using MaxFactry.Module.App.PresentationLayer;

    /// <summary>
    /// Library used to wrap methods that interact with the HttpRequestBase.
    /// </summary>
    public class MaxOwinLibraryAppProvider : MaxOwinLibraryIISProvider
    {
        public override string GetStorageKeyFromUrl(Uri loUrl)
        {
            string lsR = string.Empty;
            MaxFactry.Core.MaxLogLibrary.Log(MaxFactry.Core.MaxEnumGroup.LogDebug, "GetStorageKey() get from url before [" + loUrl.ToString() + "]", "MaxAppWebApplicationLibraryProvider");
            MaxAppUrlEntity loEntity = MaxAppUrlEntity.Create();
            int lnMatchLevel = loEntity.LoadByUrl(loUrl);
            if (lnMatchLevel > 0)
            {
                Guid loUrlStorageKey = loEntity.AppId;
                if (!Guid.Empty.Equals(loUrlStorageKey))
                {
                    MaxLogLibrary.Log(MaxEnumGroup.LogInfo, "GetStorageKey from Url [" + loUrlStorageKey.ToString() + "][" + loUrl.ToString() + "]", "MaxAppWebApplicationLibraryProvider");
                    lsR = loUrlStorageKey.ToString();
                }
            }

            return lsR;
        }

        public override string GetTitle()
        {
            string lsR = base.GetTitle();
            if (string.IsNullOrEmpty(lsR))
            {
                MaxAppViewModel loModel = new MaxAppViewModel(MaxFactry.Base.DataLayer.Library.MaxDataLibrary.GetStorageKey(null));
                lsR = loModel.Title;
            }

            return lsR;
        }

        public override string GetRedirectUrl(Uri loUrl)
        {
            string lsR = string.Empty;
            MaxAppUrlEntity loEntity = MaxAppUrlEntity.Create();
            try
            {
                int lnMatchLevel = loEntity.LoadByUrl(loUrl);
                if (!string.IsNullOrEmpty(loEntity.RedirectUrl))
                {
                    string lsRedirectUrl = loEntity.RedirectUrl;
                    if (lsRedirectUrl.StartsWith("*") && lsRedirectUrl.Length > 1)
                    {
                        lsRedirectUrl = loUrl.Scheme + "://" + loUrl.Authority + lsRedirectUrl.Substring(1);
                    }

                    if (lsRedirectUrl.EndsWith("*"))
                    {
                        lsRedirectUrl = lsRedirectUrl.Substring(0, lsRedirectUrl.Length - 1);
                    }

                    Uri loRedirectUrl = null;
                    if (!Uri.TryCreate(lsRedirectUrl, UriKind.Absolute, out loRedirectUrl))
                    {
                        if (!Uri.TryCreate(lsRedirectUrl, UriKind.Relative, out loRedirectUrl))
                        {
                            loRedirectUrl = null;
                            MaxLogLibrary.Log(new MaxLogEntryStructure("GetRedirectUrl", MaxEnumGroup.LogError, "Cannot create for url {lsRedirectUrl} for entity {loEntity} for url {loUrl}", lsRedirectUrl, loEntity, loUrl));
                        }
                    }

                    if (null != loRedirectUrl)
                    {
                        if ((lnMatchLevel & (int)Math.Pow(2, MaxAppUrlEntity.MatchScriptExactFlag)) != 0)
                        {
                            if (!string.IsNullOrEmpty(loRedirectUrl.Host))
                            {
                                lsR = loRedirectUrl.Scheme;
                                if (lsR.Length == 0)
                                {
                                    lsR = loUrl.Scheme;
                                }

                                lsR += "://" + loRedirectUrl.Authority + loRedirectUrl.AbsolutePath;
                            }
                            else
                            {
                                lsR = loRedirectUrl.AbsolutePath;
                            }
                        }
                        else if (loEntity.RedirectUrl.EndsWith("*") && (((lnMatchLevel & (int)Math.Pow(2, MaxAppUrlEntity.MatchScriptFlag)) != 0) || ((lnMatchLevel & (int)Math.Pow(2, MaxAppUrlEntity.MatchScriptWildOnlyFlag)) != 0)))
                        {
                            if (loRedirectUrl.Host.ToLower() != loUrl.Host.ToLower() ||
                                (loRedirectUrl.Scheme == "https" && loUrl.Scheme == "http") ||
                                !loRedirectUrl.AbsolutePath.StartsWith(loUrl.AbsolutePath))
                            {
                                lsR = loRedirectUrl.Scheme;
                                if (lsR.Length == 0)
                                {
                                    lsR = loUrl.Scheme;
                                }

                                lsR += "://" + loRedirectUrl.Authority + loUrl.AbsolutePath;
                                if (loUrl.AbsolutePath.ToLower().StartsWith("/maxapp") || loUrl.AbsolutePath.ToLower().StartsWith("/maxsecurity"))
                                {
                                    lsR = string.Empty;
                                }
                            }
                        }
                        else
                        {
                            lsR = loRedirectUrl.ToString();
                        }
                    }

                    if (lsR.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(loRedirectUrl.Query) && !lsR.Contains("?"))
                        {
                            lsR += loRedirectUrl.Query;
                        }
                        else if (!string.IsNullOrEmpty(loUrl.Query))
                        {
                            string lsQry = loUrl.Query;
                            if (loEntity.Script.Contains("?"))
                            {
                                lsQry = lsQry.Replace(loEntity.Script.Split('?')[1], string.Empty).Replace("?&", "?");
                            }

                            if (lsQry.Length > 1)
                            {
                                lsR += lsQry;
                            }
                        }
                    }


                }
            }
            catch (Exception loEUrl)
            {
                MaxLogLibrary.Log(new MaxLogEntryStructure("GetRedirectUrl", MaxEnumGroup.LogError, "Error for url {loUrl} for entity {loEntity}", loEUrl, loUrl, loEntity));
            }

            return lsR;
        }
    }
}