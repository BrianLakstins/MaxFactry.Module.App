// <copyright file="MaxAppUrlViewModel.cs" company="Lakstins Family, LLC">
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
// <change date="6/26/2014" author="Brian A. Lakstins" description="Initial Release">
// <change date="10/1/2018" author="Brian A. Lakstins" description="Add RedirectUrl support.">
// <change date="5/22/2020" author="Brian A. Lakstins" description="Fix reference to StorageKey.">
// <change date="6/2/2021" author="Brian A. Lakstins" description="Move to Mvc4 namespace.">
// <change date="3/31/2024" author="Brian A. Lakstins" description="Updated for changes to dependency classes.">
// </changelog>
#endregion

namespace MaxFactry.Module.App.Mvc4.PresentationLayer
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MaxFactry.Core;
    using MaxFactry.Base.BusinessLayer;
    using MaxFactry.Base.DataLayer.Library;
    using MaxFactry.Module.App.Mvc4.BusinessLayer;
    using MaxFactry.Module.App.PresentationLayer;

    /// <summary>
    /// View model for content.
    /// </summary>
    public class MaxAppUrlViewModel : MaxFactry.Base.PresentationLayer.MaxBaseIdViewModel
    {
        private List<MaxAppUrlViewModel> _oSortedList = null;

        private List<MaxAppUrlViewModel> _oSortedListByAppId = null;

        /// <summary>
        /// Initializes a new instance of the MaxAppUrlViewModel class
        /// </summary>
        public MaxAppUrlViewModel()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxAppUrlViewModel class
        /// </summary>
        /// <param name="loEntity">Entity to use as data.</param>
        public MaxAppUrlViewModel(MaxEntity loEntity)
            : base(loEntity)
        {
        }

        public MaxAppUrlViewModel(string lsId) : base(lsId)
        {
        }

        protected override void CreateEntity()
        {
            this.Entity = MaxAppUrlEntity.Create();
        }

        /// <summary>
        /// Gets or sets the server name property to be stored.
        /// </summary>
        [Display(Name = "Server Name")]
        [Required]
        public string ServerName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the script name property to be stored.
        /// </summary>
        [Display(Name = "Script Name")]
        public string Script
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the URL to redirect this script to
        /// </summary>
        [Display(Name = "Redirect URL")]
        public string RedirectUrl
        {
            get;
            set;
        }

        public List<MaxAppUrlViewModel> GetSortedList()
        {
            if (null == this._oSortedList)
            {
                this._oSortedList = new List<MaxAppUrlViewModel>();
                string[] laKey = this.EntityIndex.GetSortedKeyList();
                for (int lnK = 0; lnK < laKey.Length; lnK++)
                {
                    MaxAppUrlViewModel loViewModel = new MaxAppUrlViewModel(this.EntityIndex[laKey[lnK]] as MaxAppUrlEntity);
                    loViewModel.Load();
                    this._oSortedList.Add(loViewModel);
                }
            }

            return this._oSortedList;
        }

        public List<MaxAppUrlViewModel> GetSortedListByCurrentAppId()
        {
            if (null == this._oSortedListByAppId)
            {
                MaxAppViewModel loAppViewModel = new MaxAppViewModel(MaxDataLibrary.GetStorageKey(null));
                this._oSortedListByAppId = new List<MaxAppUrlViewModel>();
                string[] laKey = this.EntityIndex.GetSortedKeyList();
                for (int lnK = 0; lnK < laKey.Length; lnK++)
                {
                    MaxAppUrlEntity loEntity = this.EntityIndex[laKey[lnK]] as MaxAppUrlEntity;
                    if (loEntity.AppId.ToString() == loAppViewModel.Id)
                    {
                        MaxAppUrlViewModel loViewModel = new MaxAppUrlViewModel(loEntity);
                        loViewModel.Load();
                        this._oSortedListByAppId.Add(loViewModel);
                    }
                }
            }

            return this._oSortedListByAppId;
        }

        /// <summary>
        /// Loads the entity based on the Id property.
        /// Maps the current values of properties in the ViewModel to the Entity.
        /// </summary>
        /// <returns>True if successful. False if it cannot be mapped.</returns>
        protected override bool MapToEntity()
        {
            if (base.MapToEntity())
            {
                MaxAppUrlEntity loEntity = this.Entity as MaxAppUrlEntity;
                if (null != loEntity)
                {
                    loEntity.ServerName = this.ServerName;
                    loEntity.Script = string.Empty;
                    if (!string.IsNullOrEmpty(this.Script))
                    {
                        loEntity.Script = this.Script.ToLowerInvariant();
                    }

                    loEntity.RedirectUrl = string.Empty;
                    if (!string.IsNullOrEmpty(this.RedirectUrl))
                    {
                        loEntity.RedirectUrl = this.RedirectUrl;
                    }

                    loEntity.AppId = MaxConvertLibrary.ConvertToGuid(typeof(object), MaxDataLibrary.GetStorageKey(null));
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Maps the properties of the Entity to the properties of the ViewModel.
        /// </summary>
        /// <returns>True if the entity exists.</returns>
        protected override bool MapFromEntity()
        {
            if (base.MapFromEntity())
            {
                MaxAppUrlEntity loEntity = this.Entity as MaxAppUrlEntity;
                if (null != loEntity)
                {
                    this.ServerName = loEntity.ServerName;
                    this.Script = loEntity.Script;
                    this.RedirectUrl = loEntity.RedirectUrl;
                    return true;
                }
            }

            return false;
        }
    }
}
