// <copyright file="MaxAppUrlDataModel.cs" company="Lakstins Family, LLC">
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
// <change date="6/2/2021" author="Brian A. Lakstins" description="Move to Mvc4 namespace.">
// </changelog>
#endregion

namespace MaxFactry.Module.App.Mvc4.DataLayer
{
    using System;
    using MaxFactry.Module.App.DataLayer;

    /// <summary>
    /// Defines base data model for hash table like data with a unique identifier
    /// </summary>
    public class MaxAppUrlDataModel : MaxFactry.Base.DataLayer.MaxBaseGuidKeyDataModel
    {
        /// <summary>
        /// Id of the App this URL is associated with.
        /// </summary>
        public readonly string AppId = "AppId";

        /// <summary>
        /// Server name for url.
        /// </summary>
        public readonly string ServerName = "ServerName";

        /// <summary>
        /// Script for url.
        /// </summary>
        public readonly string Script = "Script";

        /// <summary>
        /// Options associated with this App Url
        /// </summary>
        public readonly string OptionList = "OptionList";

        /// <summary>
        /// URL to redirect this script to
        /// </summary>
        public readonly string RedirectUrl = "RedirectUrl";

        /// <summary>
        /// Initializes a new instance of the MaxAppUrlDataModel class
        /// </summary>
        public MaxAppUrlDataModel()
            : base()
        {
            this.RemoveType(this.StorageKey);
            this.SetDataStorageName("MaxAppUrl");
            this.RepositoryProviderType = typeof(MaxFactry.Module.App.DataLayer.Provider.MaxAppRepositoryDefaultProvider);
            this.RepositoryType = typeof(MaxAppRepository);
            this.AddStorageKey(this.AppId, typeof(Guid));
            this.AddType(this.ServerName, typeof(MaxFactry.Base.DataLayer.MaxShortString));
            this.AddType(this.Script, typeof(MaxFactry.Base.DataLayer.MaxShortString));
            this.AddNullable(this.OptionList, typeof(long));
            this.AddNullable(this.RedirectUrl, typeof(string));
        }
    }
}
