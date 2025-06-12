// <copyright file="MaxConfigurationLibraryAppProvider.cs" company="Lakstins Family, LLC">
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
// <change date="2/19/2017" author="Brian A. Lakstins" description="Initial creation">
// <change date="11/30/2018" author="Brian A. Lakstins" description="Updated to use AttributeIndex or Config">
// <change date="4/28/2020" author="Brian A. Lakstins" description="Try to use to override application scope, but causes stack overflow.">
// <change date="5/19/2020" author="Brian A. Lakstins" description="Add some logging code.">
// <change date="6/5/2020" author="Brian A. Lakstins" description="Updated to allow fall back to base if key does not exist in Scope24">
// <change date="6/12/2025" author="Brian A. Lakstins" description="Update for MaxApp maybe returning a null">
// </changelog>
#endregion

namespace MaxFactry.Core.Provider
{
    using System;
    using System.Configuration;
    using System.Web;
    using System.Web.Security;
    using MaxFactry.Core;
    using MaxFactry.Core.Provider;
    using MaxFactry.Base.DataLayer;
    using MaxFactry.Base.BusinessLayer;
    using MaxFactry.Module.App.BusinessLayer;

    /// <summary>
    /// MaxFactory Provider for use in app based web applications.
    /// </summary>
    public class MaxConfigurationLibraryAppProvider : MaxConfigurationLibraryAspNetIISProvider
    {
        public override object GetValue(MaxEnumGroup loScope, string lsKey)
        {
            if (loScope == MaxEnumGroup.Scope24)
            {
                MaxAppEntity loCurrent = MaxAppEntity.GetCurrent();
                if (null != loCurrent && loCurrent.AttributeIndex.Contains(lsKey) || loCurrent.Config.Contains(lsKey))
                {
                    object loR = loCurrent.AttributeIndex[lsKey];
                    if (null == loR)
                    {
                        loR = loCurrent.Config[lsKey];
                    }

                    return loR;
                }
            }

            return base.GetValue(loScope, lsKey);
        }
    }
}