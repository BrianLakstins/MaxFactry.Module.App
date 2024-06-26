﻿// <copyright file="MaxAppDataModel.cs" company="Lakstins Family, LLC">
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

namespace MaxFactry.Module.App.DataLayer
{
	using System;

	/// <summary>
	/// Defines data model for an integer Id for MaxApp
	/// </summary>
    public class MaxAppIdDataModel : MaxFactry.Base.DataLayer.MaxIdIntegerDataModel
	{
        /// <summary>
        /// IndexId for the Index
        /// </summary>
        public readonly string Name = "Name";

		/// <summary>
        /// Initializes a new instance of the MaxAppDataModel class
		/// </summary>
        public MaxAppIdDataModel()
            : base()
		{
            this.SetDataStorageName("MaxAppId");
            this.RepositoryProviderType = typeof(MaxFactry.Module.App.DataLayer.Provider.MaxAppRepositoryDefaultProvider);
            this.RepositoryType = typeof(MaxAppRepository);
            this.AddType(this.Name, typeof(MaxFactry.Base.DataLayer.MaxShortString));
		}
	}
}
