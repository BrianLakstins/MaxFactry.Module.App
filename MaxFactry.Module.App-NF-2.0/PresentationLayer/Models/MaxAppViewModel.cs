// <copyright file="MaxAppViewModel.cs" company="Lakstins Family, LLC">
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
// <change date="5/27/2020" author="Brian A. Lakstins" description="Fix GTM tag getting cleared when updated">
// </changelog>
#endregion

namespace MaxFactry.Module.App.PresentationLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MaxFactry.Core;
    using MaxFactry.Base.BusinessLayer;
    using MaxFactry.Module.App.BusinessLayer;

    /// <summary>
    /// View model for content.
    /// </summary>
    public class MaxAppViewModel : MaxFactry.Base.PresentationLayer.MaxBaseIdViewModel
    {
        private List<MaxAppViewModel> _oSortedList = null;

        /// <summary>
        /// Initializes a new instance of the MaxAppViewModel class
        /// </summary>
        public MaxAppViewModel()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxAppViewModel class
        /// </summary>
        /// <param name="loEntity">Entity to use as data.</param>
        public MaxAppViewModel(MaxEntity loEntity)
            : base(loEntity)
        {
        }

        public MaxAppViewModel(string lsId) : base(lsId)
        {
        }

        protected override void CreateEntity()
        {
            this.Entity = MaxAppEntity.Create();
        }

        /// <summary>
        /// Gets or sets the name property to be stored.
        /// </summary>
        [Display(Name = "Name")]
        [Required]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name property to be stored.
        /// </summary>
        [Display(Name = "Analytics Id")]
        public string AnalyticsId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name property to be stored.
        /// </summary>
        [Display(Name = "GTM Id")]
        public string GTMId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name property to be stored.
        /// </summary>
        [Display(Name = "Theme")]
        public string ThemeName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name property to be stored.
        /// </summary>
        [Display(Name = "Title")]
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name property to be stored.
        /// </summary>
        [Display(Name = "Config")]
        public string Config
        {
            get;
            set;
        }

        public List<MaxAppViewModel> GetSortedList()
        {
            if (null == this._oSortedList)
            {
                this._oSortedList = new List<MaxAppViewModel>();
                string[] laKey = this.EntityIndex.GetSortedKeyList();
                for (int lnK = 0; lnK < laKey.Length; lnK++)
                {
                    MaxAppViewModel loViewModel = new MaxAppViewModel(this.EntityIndex[laKey[lnK]] as MaxAppEntity);
                    loViewModel.Load();
                    this._oSortedList.Add(loViewModel);
                }
            }

            return this._oSortedList;
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
                MaxAppEntity loEntity = this.Entity as MaxAppEntity;
                if (null != loEntity)
                {
                    loEntity.Name = this.Name;
                    loEntity.AnalyticsId = this.AnalyticsId;
                    loEntity.GTMId = this.GTMId;
                    loEntity.ThemeName = this.ThemeName;
                    loEntity.Title = this.Title;
                    if (!string.IsNullOrEmpty(this.Config))
                    {
                        string[] laConfig = this.Config.Split('\n');
                        loEntity.Config.Clear();
                        foreach (string lsConfig in laConfig)
                        {
                            if (lsConfig.IndexOf('=') > 0)
                            {
                                string lsKey = lsConfig.Substring(0, lsConfig.IndexOf('='));
                                string lsValue = lsConfig.Substring(lsConfig.IndexOf('=') + 1);
                                loEntity.Config.Add(lsKey.Trim(), lsValue.Trim());
                            }
                        }
                    }

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
                MaxAppEntity loEntity = this.Entity as MaxAppEntity;
                if (null != loEntity)
                {
                    this.Name = loEntity.Name;
                    this.ThemeName = loEntity.ThemeName;
                    this.AnalyticsId = loEntity.AnalyticsId;
                    this.Title = loEntity.Title;
                    this.GTMId = loEntity.GTMId;
                    this.Config = string.Empty;
                    string[] laKey = loEntity.Config.GetSortedKeyList();
                    foreach (string lsKey in laKey)
                    {
                        this.Config += lsKey + "=" + loEntity.Config[lsKey] + "\r\n";
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
