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
// <change date="3/31/2024" author="Brian A. Lakstins" description="Updated for changes to dependency classes.">
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
    public class MaxAppManageController : MaxManageController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            MaxAppViewModel loModel = new MaxAppViewModel(MaxFactry.Base.DataLayer.Library.MaxDataLibrary.GetStorageKey(null));
            return View(loModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(MaxAppViewModel loModel)
        {
            if (null != loModel && null != loModel.Id)
            {
                Guid loId = new Guid(loModel.Id);
                if (!Guid.Empty.Equals(loId))
                {
                    return RedirectToRoute("MaxAppRoute", routeValues: new { MaxAppId = loModel.Id });
                }
            }

            return View(loModel);
        }

        [HttpGet]
        [MaxRequireHttpsAttribute(Order = 1)]
        [Authorize(Roles = "Admin")]
        public virtual ActionResult App(string m)
        {
            return this.Show(new MaxAppViewModel(), m);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MaxRequireHttpsAttribute(Order = 1)]
        [Authorize(Roles = "Admin")]
        public virtual ActionResult App(MaxAppViewModel loModel, string uoProcess)
        {
            string lsCancelAction = "App";
            string lsSuccessAction = "App";
            string lsSuccessMessage = loModel.Name + " successfully created.";
            object loResult = this.Create(loModel, uoProcess, lsCancelAction, lsSuccessAction, lsSuccessMessage);
            if (loResult is ActionResult)
            {
                return (ActionResult)loResult;
            }

            return View(loModel);
        }

        [HttpGet]
        [MaxRequireHttpsAttribute(Order = 1)]
        [Authorize(Roles = "Admin")]
        public virtual ActionResult AppEdit(string id)
        {
            MaxAppViewModel loModel = new MaxAppViewModel(id);
            return View(loModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MaxRequireHttpsAttribute(Order = 1)]
        [Authorize(Roles = "Admin")]
        public virtual ActionResult AppEdit(MaxAppViewModel loModel, string uoProcess)
        {
            string lsCancelAction = "App";
            ActionResult loResult = this.Edit(loModel, uoProcess, lsCancelAction);
            if (loResult is ViewResult)
            {
                ViewBag.Message = "Successfully saved.";
            }

            return loResult;
        }

        [HttpGet]
        [MaxRequireHttpsAttribute(Order = 1)]
        [Authorize(Roles = "Admin,Admin - App")]
        public virtual ActionResult AppUrl(string m)
        {
            return this.Show(new MaxAppUrlViewModel(), m);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MaxRequireHttpsAttribute(Order = 1)]
        [Authorize(Roles = "Admin,Admin - App")]
        public virtual ActionResult AppUrl(MaxAppUrlViewModel loModel, string uoProcess)
        {
            string lsCancelAction = "AppUrl";
            string lsSuccessAction = "AppUrl";
            string lsSuccessMessage = loModel.ServerName + " successfully created.";
            object loResult = this.Create(loModel, uoProcess, lsCancelAction, lsSuccessAction, lsSuccessMessage);
            if (loResult is ActionResult)
            {
                return (ActionResult)loResult;
            }

            return View(loModel);
        }

        [HttpGet]
        [MaxRequireHttpsAttribute(Order = 1)]
        [Authorize(Roles = "Admin,Admin - App")]
        public virtual ActionResult AppUrlEdit(string id)
        {
            MaxAppUrlViewModel loModel = new MaxAppUrlViewModel(id);
            return View(loModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MaxRequireHttpsAttribute(Order = 1)]
        [Authorize(Roles = "Admin,Admin - App")]
        public virtual ActionResult AppUrlEdit(MaxAppUrlViewModel loModel, string uoProcess)
        {
            string lsCancelAction = "AppUrl";
            ActionResult loResult = this.Edit(loModel, uoProcess, lsCancelAction);
            if (loResult is ViewResult)
            {
                ViewBag.Message = "Successfully saved.";
            }

            return loResult;
        }

    }
}
