// <copyright file="MaxAppIdEntity.cs" company="Lakstins Family, LLC">
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
// <change date="6/18/2015" author="Brian A. Lakstins" description="Initial creation">
// <change date="3/31/2024" author="Brian A. Lakstins" description="Updated for changes to dependency classes.">
// </changelog>
#endregion

namespace MaxFactry.Module.App.BusinessLayer
{
    using System;
    using MaxFactry.Core;
    using MaxFactry.Base.DataLayer.Library;
    using MaxFactry.Base.BusinessLayer;
    using MaxFactry.Module.App.DataLayer;

    /// <summary>
    /// Entity that allows interaction with App information
    /// </summary>
    public class MaxAppIdEntity : MaxFactry.Base.BusinessLayer.MaxIdIntegerEntity
    {
		/// <summary>
        /// Initializes a new instance of the MaxAppEntity class
		/// </summary>
		/// <param name="loData">object to hold data</param>
        public MaxAppIdEntity(MaxFactry.Base.DataLayer.MaxData loData)
            : base(loData)
		{
		}

        /// <summary>
        /// Initializes a new instance of the MaxAppEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxAppIdEntity(Type loDataModelType)
            : base(loDataModelType)
        {
        }

        /// <summary>
        /// Gets or sets the name of the App
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetString(this.DataModel.Name);
            }

            set
            {
                this.Set(this.DataModel.Name, value);
            }
        }

        /// <summary>
        /// Gets the Data Model for this entity
        /// </summary>
        protected MaxAppIdDataModel DataModel
        {
            get
            {
                return (MaxAppIdDataModel)MaxDataLibrary.GetDataModel(this.DataModelType);
            }
        }

        /// <summary>
        /// Dynamically creates a new instance of this entity.
        /// </summary>
        /// <returns>A new instance of this entity.</returns>
        public static MaxAppIdEntity Create()
        {
            MaxAppIdEntity loEntity = MaxBusinessLibrary.GetEntity(
                typeof(MaxAppIdEntity),
                typeof(MaxAppIdDataModel)) as MaxAppIdEntity;
            loEntity.IdLength = 9;
            return loEntity;
        }
    }
}
