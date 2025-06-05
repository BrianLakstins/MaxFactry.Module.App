// <copyright file="MaxHtmlHelperAppExtension.cs" company="Lakstins Family, LLC">
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
// <change date="4/28/2020" author="Brian A. Lakstins" description="Initial creation">
// <change date="6/4/2025" author="Brian A. Lakstins" description="Arrange code so debugger can be used to test speed.">
// </changelog>
#endregion

namespace MaxFactry.General.AspNet.IIS.Mvc4.PresentationLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using MaxFactry.Core;
    using MaxFactry.General.AspNet.BusinessLayer;
    using MaxFactry.General.AspNet.PresentationLayer;
    using MaxFactry.Base.BusinessLayer;
    using MaxFactry.Base.PresentationLayer;
    using MaxFactry.General.BusinessLayer;
    using MaxFactry.General.PresentationLayer;
    using MaxFactry.Module.App.BusinessLayer;

    /// <summary>
    /// Helper library for producing HTML
    /// </summary>
    public static class MaxHtmlHelperAppExtension
    {
        public static IHtmlString MaxGetGoogleAnalytics<T>(this HtmlHelper<T> helper)
        {
            IHtmlString loR = new HtmlString(MaxAppEntity.GetGoogleAnalyticsJavascriptTag());
            return loR;
        }

        public static IHtmlString MaxGetGoogleTagManagerHead<T>(this HtmlHelper<T> helper)
        {
            IHtmlString loR = new HtmlString(MaxAppEntity.GetGoogleTagManagerJavascriptHead());
            return loR;
        }

        public static IHtmlString MaxGetGoogleTagManagerBody<T>(this HtmlHelper<T> helper)
        {
            IHtmlString loR = new HtmlString(MaxAppEntity.GetGoogleTagManagerJavascriptBody());
            return loR;
        }

        public static IHtmlString MaxGetMicrosoftApplicationInsightsHead<T>(this HtmlHelper<T> helper)
        {
            IHtmlString loR = new HtmlString(MaxAppEntity.GetMicrosoftApplicationInsightsJavascript());
            return loR;
        }
    }
}