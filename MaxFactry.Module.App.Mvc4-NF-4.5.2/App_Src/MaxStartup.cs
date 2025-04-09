// <copyright file="MaxStartup.cs" company="Lakstins Family, LLC">
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
// <change date="6/3/2014" author="Brian A. Lakstins" description="Initial Release">
// <change date="6/20/2014" author="Brian A. Lakstins" description="Moved encryption provider to MaxFactry_System.Web namespace.">
// <change date="6/23/2014" author="Brian A. Lakstins" description="Updates for testing.">
// <change date="6/27/2014" author="Brian A. Lakstins" description="Remove dependency on AppId.">
// <change date="4/9/2025" author="Brian A. Lakstins" description="Use SetId instead of Insert(Guid)">
// </changelog>
#endregion

namespace MaxFactry.Module.App.Mvc4
{
    using System;
    using System.Configuration;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;
    using MaxFactry.Core;
    using MaxFactry.Base.DataLayer.Library;
    using MaxFactry.Module.App.Mvc4.PresentationLayer;
    using MaxFactry.Base.BusinessLayer;
    using MaxFactry.Module.App.BusinessLayer;

    public class MaxStartup : MaxFactry.Base.MaxStartup
    {
        /// <summary>
        /// Internal storage of single object
        /// </summary>
        private static object _oInstance = null;

        /// <summary>
        /// Gets the single instance of this class.
        /// </summary>
        public new static MaxStartup Instance
        {
            get
            {
                _oInstance = CreateInstance(typeof(MaxStartup), _oInstance);
                return _oInstance as MaxStartup;
            }
        }

        public override void SetProviderConfiguration(MaxIndex loConfig)
        {
            MaxFactry.Module.App.MaxStartup.Instance.SetProviderConfiguration(loConfig);
        }

        public override void RegisterProviders()
        {
            MaxFactry.Module.App.MaxStartup.Instance.RegisterProviders();
            //// Override the Owin library provider
            MaxFactry.General.AspNet.PresentationLayer.MaxOwinLibrary.Instance.ProviderSet(
                typeof(MaxFactry.General.AspNet.PresentationLayer.Provider.MaxOwinLibraryAppProvider));
            //// Override the Configuration library provider
            MaxConfigurationLibrary.Instance.ProviderSet(
                typeof(MaxFactry.Core.Provider.MaxConfigurationLibraryAppProvider));
            //// Override the Data library provider
            MaxDataLibrary.Instance.ProviderSet(typeof(MaxFactry.Base.DataLayer.Library.Provider.MaxDataLibraryAppProvider));
        }

        public override void ApplicationStartup()
        {
            MaxFactry.Module.App.MaxStartup.Instance.ApplicationStartup();
            RouteTable.Routes.MapRoute(
                name: "MaxAppRoute",
                url: "MaxApp/{action}/{id}",
                defaults: new { controller = "MaxApp", action = "Index", id = UrlParameter.Optional }
            );

            RouteTable.Routes.MapRoute(
                name: "MaxAppManageRoute",
                url: "MaxAppManage/{action}/{id}",
                defaults: new { controller = "MaxAppManage", action = "Index", id = UrlParameter.Optional }
            );

            RouteTable.Routes.MapRoute(
                name: "MaxAppManagePartialRoute",
                url: "MaxAppManagePartial/{action}/{id}",
                defaults: new { controller = "MaxAppManagePartial", action = "Index", id = UrlParameter.Optional }
            );

            RouteTable.Routes.Add("MaxAppHandlerRoute", new Route("a/{MaxAppId}", new MaxAppHandler()));

            string lsDefaultId = MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopeApplication, MaxFactryLibrary.MaxStorageKeyName) as string;
            MaxFactry.General.AspNet.IIS.Mvc4.MaxAppLibrary.AddValidStorageKey(lsDefaultId);
            Guid loId = new Guid(lsDefaultId);
            MaxEntityList loList = MaxAppEntity.Create().LoadAllCache();
            bool lbFound = false;
            for (int lnL = 0; lnL < loList.Count; lnL++)
            {
                MaxAppEntity loAppEntity = loList[lnL] as MaxAppEntity;
                MaxFactry.General.AspNet.IIS.Mvc4.MaxAppLibrary.AddValidStorageKey(loAppEntity.Id.ToString());
                if (loAppEntity.Id.Equals(loId))
                {
                    lbFound = true;
                }
            }

            if (!lbFound)
            {
                MaxAppEntity loAppEntity = MaxAppEntity.Create();
                loAppEntity.Name = "Primary";
                loAppEntity.IsActive = true;
                loAppEntity.SetId(loId);
                loAppEntity.Insert();
            }
        }
    }
}