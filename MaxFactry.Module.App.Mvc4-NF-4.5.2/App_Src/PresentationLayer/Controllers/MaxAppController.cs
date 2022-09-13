// <copyright file="AppManagementController.cs" company="Lakstins Family, LLC">
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
// <change date="6/10/2014" author="Brian A. Lakstins" description="Redirect to / instead of home controller.">
// <change date="6/19/2014" author="Brian A. Lakstins" description="Add a model and move code to model.">
// <change date="5/22/2020" author="Brian A. Lakstins" description="Fix reference to StorageKey.">
// </changelog>
#endregion

namespace MaxFactry.Module.App.Mvc4.PresentationLayer
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using MaxFactry.Core;
    using MaxFactry.Module.App.BusinessLayer;
    using MaxFactry.Module.App.PresentationLayer;
    using MaxFactry.General.AspNet.IIS.Mvc4.PresentationLayer;

    [MaxAuthorize(Order = 2)]
    public class MaxAppController : MaxBaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            MaxAppViewModel loModel = new MaxAppViewModel(MaxFactry.Base.DataLayer.MaxDataLibrary.GetStorageKey(null));

            return View(loModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(MaxAppViewModel loModel)
        {
            Guid loId = new Guid(loModel.Id);
            if (!Guid.Empty.Equals(loId))
            {
                return RedirectToRoute("MaxAppHandlerRoute", routeValues: new { MaxAppId = loModel.Id });
            }

            return View(loModel);
        }
    }
}
