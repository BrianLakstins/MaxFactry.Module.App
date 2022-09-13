// <copyright file="MaxAppHandler.cs" company="Lakstins Family, LLC">
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
// <change date="7/25/2014" author="Brian A. Lakstins" description="Initial Release">
// <change date="8/13/2014" author="Brian A. Lakstins" description="Updated to return a handler to avoid a thread abort error message.">
// <change date="9/30/2014" author="Brian A. Lakstins" description="Updated cookie handling.">
// <change date="10/1/2018" author="Brian A. Lakstins" description="Send Current URL to get storage key.">
// </changelog>
#endregion

namespace MaxFactry.Module.App.Mvc4.PresentationLayer
{

    using System;
    using System.Web;
    using System.Web.Routing;
    using MaxFactry.Core;
    using MaxFactry.Module.App.PresentationLayer;
    using MaxFactry.General.AspNet.IIS;
    using MaxFactry.General.AspNet.PresentationLayer;

    public class MaxAppHandler : IRouteHandler, IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        string _sId = string.Empty;

        public MaxAppHandler()
        {

        }

        public MaxAppHandler(object loMaxAppId)
        {
            if (null != loMaxAppId && loMaxAppId is string)
            {
                this._sId = (string)loMaxAppId;
            }
            else if (null != loMaxAppId)
            {
                this._sId = loMaxAppId.ToString();
            }
        }

        public IHttpHandler GetHttpHandler(RequestContext loRequestContext)
        {
            object loMaxAppId = loRequestContext.RouteData.Values["MaxAppId"];
            return new MaxAppHandler(loMaxAppId);
        }

        // Summary:
        //     Gets a value indicating whether another request can use the System.Web.IHttpHandler
        //     instance.
        //
        // Returns:
        //     true if the System.Web.IHttpHandler instance is reusable; otherwise, false.
        public bool IsReusable { get { return false; } }

        // Summary:
        //     Enables processing of HTTP Web requests by a custom HttpHandler that implements
        //     the System.Web.IHttpHandler interface.
        //
        // Parameters:
        //   context:
        //     An System.Web.HttpContext object that provides references to the intrinsic
        //     server objects (for example, Request, Response, Session, and Server) used
        //     to service HTTP requests.
        public void ProcessRequest(HttpContext loContext)
        {
            if (!string.IsNullOrEmpty(this._sId))
            {
                MaxAppViewModel loModel = new MaxAppViewModel(this._sId);
                loContext.Response.Clear();
                Guid loId = MaxConvertLibrary.ConvertToGuid(typeof(object), loModel.Id);
                if (Guid.Empty.Equals(loId))
                {
                    loContext.Response.StatusCode = 404;
                }
                else
                {
                    MaxAppLibrary.SignOut();
                    //MaxConfigurationLibrary.SetValue(MaxEnumGroup.ScopeSession, MaxFactryLibrary.MaxStorageKeyName, loModel.Id);
                    MaxConfigurationLibrary.SetValue(MaxEnumGroup.ScopeProfile, MaxFactryLibrary.MaxStorageKeyName, loModel.Id);
                    MaxConfigurationLibrary.SetValue(MaxEnumGroup.ScopeProcess, MaxFactryLibrary.MaxStorageKeyName, loModel.Id);
                    MaxOwinLibrary.SetStorageKeyForClient(HttpContext.Current.Request.Url, true, loModel.Id.ToString());
                    loContext.Response.Redirect("/", false);
                }

                loContext.ApplicationInstance.CompleteRequest();
            }
        }
    }
}
