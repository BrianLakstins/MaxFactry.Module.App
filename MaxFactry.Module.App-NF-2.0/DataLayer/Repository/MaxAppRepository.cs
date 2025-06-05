// <copyright file="MaxAppRepository.cs" company="Lakstins Family, LLC">
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
// <change date="8/13/2014" author="Brian A. Lakstins" description="Added method to load all data for a server.">
// <change date="12/18/2014" author="Brian A. Lakstins" description="Updates to follow core data access patterns.">
// <change date="12/21/2016" author="Brian A. Lakstins" description="Added methods to help determine app specific code.">
// <change date="3/31/2024" author="Brian A. Lakstins" description="Updated for changes to namespaces.">
// <change date="6/4/2025" author="Brian A. Lakstins" description="Change base class.  Remove unused code.">
// </changelog>
#endregion

namespace MaxFactry.Module.App.DataLayer
{
    using System;
    using MaxFactry.Core;
    using MaxFactry.Base.DataLayer;
    using MaxFactry.Base.DataLayer.Library;

    /// <summary>
    /// Repository for managing app data storage.
    /// </summary>
    public class MaxAppRepository : MaxFactry.Base.DataLayer.MaxBaseRepository
    {
    }
}
