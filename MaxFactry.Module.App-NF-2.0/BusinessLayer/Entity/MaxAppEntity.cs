// <copyright file="MaxAppEntity.cs" company="Lakstins Family, LLC">
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
// <change date="12/18/2014" author="Brian A. Lakstins" description="Updates to follow core data access patterns.">
// <change date="4/28/2020" author="Brian A. Lakstins" description="Add Javascript tag code.">
// <change date="4/28/2020" author="Brian A. Lakstins" description="Use replace instead of string.format because it caused an exception.">
// <change date="5/19/2020" author="Brian A. Lakstins" description="Fix getting current application so works when StorageKey is not saved in ScopeProcess.">
// <change date="5/22/2020" author="Brian A. Lakstins" description="Fix reference to StorageKey.">
// <change date="3/31/2024" author="Brian A. Lakstins" description="Updated for changes to dependency classes.">
// <change date="4/9/2025" author="Brian A. Lakstins" description="Override SetProperties for setting properties.">
// <change date="6/4/2025" author="Brian A. Lakstins" description="Change base class.  Remove integration with AppId entity">
// </changelog>
#endregion

namespace MaxFactry.Module.App.BusinessLayer
{
    using System;
    using System.Collections.Generic;
    using MaxFactry.Core;
    using MaxFactry.Base.DataLayer.Library;
    using MaxFactry.Base.BusinessLayer;
    using MaxFactry.Module.App.DataLayer;

    /// <summary>
    /// Entity that allows interaction with App information
    /// </summary>
    public class MaxAppEntity : MaxFactry.Base.BusinessLayer.MaxBaseGuidKeyEntity
    {
        private MaxIndex _oConfig = null;

        private static Dictionary<string, string> _oTrackingCodeIndex = new Dictionary<string, string>();

        private static object _oLock = new object();

        private static string _sGoogleUniversalAnalyticsCodeFormat = "<script async src=\"https://www.googletagmanager.com/gtag/js?id=[GID]\"></script>" +
                    "<script>window.dataLayer = window.dataLayer || [];" +
                    "function gtag(){dataLayer.push(arguments);}" +
                    "gtag('js', new Date());" +
                    "gtag('config', '[GID]');</script>";

        private static string _sGoogleTagManagerHeadCodeFormat = "<script>(function(w,d,s,l,i){w[l]=w[l]||[];w[l].push({'gtm.start':" +
                    "new Date().getTime(),event:'gtm.js'});var f = d.getElementsByTagName(s)[0]," +
                    "j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async=true;j.src=" +
                    "'https://www.googletagmanager.com/gtm.js?id='+i+dl;f.parentNode.insertBefore(j, f);" +
                    "})(window, document,'script','dataLayer','[GTMID]');</script>";

        private static string _sGoogleTagManagerBodyCodeFormat = "<noscript><iframe src=\"https://www.googletagmanager.com/ns.html?id=[GTMID]\" " +
                    "height=\"0\" width=\"0\" style=\"display:none;visibility:hidden\"></iframe></noscript>";

        private static string _sMicrosoftApplicationInsightsHeadCodeFormat = "<script type='text/javascript'>var sdkInstance='appInsightsSDK';window[sdkInstance]='appInsights';var aiName=window[sdkInstance],aisdk=window[aiName]||" +
                        "function(n){var o={config:n,initialize:!0},t=document,e=window,i='script';" +
                        "setTimeout(function(){var e=t.createElement(i);e.src=n.url||'https://az416426.vo.msecnd.net/scripts/b/ai.2.min.js',t.getElementsByTagName(i)[0].parentNode.appendChild(e)});" +
                        "try{o.cookie=t.cookie}catch(e){}" +
                        "function a(n){o[n]=function(){var e=arguments;o.queue.push(function(){o[n].apply(o,e)})}}o.queue=[],o.version=2;" +
                        "for(var s=['Event','PageView','Exception','Trace','DependencyData','Metric','PageViewPerformance'];s.length;)a('track'+s.pop());" +
                        "var r='Track',c=r+'Page';a('start'+c),a('stop'+c);var u=r+'Event';" +
                        "if(a('start'+u),a('stop'+u),a('addTelemetryInitializer'),a('setAuthenticatedUserContext'),a('clearAuthenticatedUserContext'),a('flush')," +
                        "o.SeverityLevel={Verbose:0,Information:1,Warning:2,Error:3,Critical:4},!(!0===n.disableExceptionTracking||n.extensionConfig&&" +
                        "n.extensionConfig.ApplicationInsightsAnalytics&&!0===n.extensionConfig.ApplicationInsightsAnalytics.disableExceptionTracking)){a('_'+(s='onerror'));" +
                        "var p=e[s];e[s]=function(e,n,t,i,a){var r=p&&p(e,n,t,i,a);return!0!==r&&o['_'+s]({message:e,url:n,lineNumber:t,columnNumber:i,error:a}),r},n.autoExceptionInstrumented=!0}return o}" +
                        "({ instrumentationKey:'[INSTRUMENTATIONKEY]' });" +
                        "(window[aiName]=aisdk).queue&&0===aisdk.queue.length&&aisdk.trackPageView({});</script>";

        /// <summary>
        /// Initializes a new instance of the MaxAppEntity class
        /// </summary>
        /// <param name="loData">object to hold data</param>
        public MaxAppEntity(MaxFactry.Base.DataLayer.MaxData loData)
            : base(loData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxAppEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxAppEntity(Type loDataModelType)
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
        /// Gets or sets the Theme of the App
        /// </summary>
        public string ThemeName
        {
            get
            {
                return this.GetString(this.DataModel.ThemeName);
            }

            set
            {
                this.Set(this.DataModel.ThemeName, value);
            }
        }

        /// <summary>
        /// Gets or sets the AnalyticsId of the App
        /// </summary>
        public string AnalyticsId
        {
            get
            {
                return this.GetString(this.DataModel.AnalyticsId);
            }

            set
            {
                this.Set(this.DataModel.AnalyticsId, value);
            }
        }

        /// <summary>
        /// Gets or sets the Google Tag Manager Id of the App
        /// </summary>
        public string GTMId
        {
            get
            {
                return this.GetString(this.DataModel.GTMId);
            }

            set
            {
                this.Set(this.DataModel.GTMId, value);
            }
        }

        /// <summary>
        /// Gets or sets the AnalyticsId of the App
        /// </summary>
        public string Title
        {
            get
            {
                return this.GetString(this.DataModel.Title);
            }

            set
            {
                this.Set(this.DataModel.Title, value);
            }
        }

        public string InstrumentationKey
        {
            get
            {
                string lsR = this.Config["InstrumentationKey"] as string;
                if (null == lsR)
                {
                    lsR = MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopeProcess, "InstrumentationKey") as string;
                    if (null == lsR)
                    {
                        lsR = MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopeApplication, "InstrumentationKey") as string;
                        if (null != lsR && lsR.Length > 0)
                        {
                            MaxConfigurationLibrary.SetValue(MaxEnumGroup.ScopeProcess, "InstrumentationKey", lsR);
                        }
                    }
                }

                return lsR;
            }
        }

        /// <summary>
        /// Gets or sets the AnalyticsId of the App
        /// </summary>
        public MaxIndex Config
        {
            get
            {
                if (null == this._oConfig)
                {
                    this._oConfig = new MaxIndex();
                    string lsConfig = this.GetString(this.DataModel.Config);
                    if (null != lsConfig && lsConfig.Length > 0)
                    {
                        this._oConfig = MaxConvertLibrary.DeserializeObject(lsConfig, typeof(MaxIndex)) as MaxIndex;
                    }
                }

                return this._oConfig;
            }

            set
            {
                this._oConfig = value;
            }
        }

        /// <summary>
        /// Gets the Data Model for this entity
        /// </summary>
        protected MaxAppDataModel DataModel
        {
            get
            {
                return (MaxAppDataModel)MaxDataLibrary.GetDataModel(this.DataModelType);
            }
        }

        /// <summary>
        /// Dynamically creates a new instance of this entity.
        /// </summary>
        /// <returns>A new instance of this entity.</returns>
        public static MaxAppEntity Create()
        {
            return MaxBusinessLibrary.GetEntity(
                typeof(MaxAppEntity),
                typeof(MaxAppDataModel)) as MaxAppEntity;
        }

        public static MaxAppEntity GetCurrent()
        {
            MaxAppEntity loEntity = MaxAppEntity.Create();
            MaxEntityList loList = loEntity.LoadAllCache();
            if (loList.Count == 1)
            {
                MaxAppEntity loR = loList[0] as MaxAppEntity;
                loR.IsActive = false;
                loR.IsActive = true;
                loR.Update();
                return loR;
            }

            string lsId = MaxDataLibrary.GetStorageKey(null);
            if (null != lsId && lsId.Length > 0)
            {
                Guid loId = MaxConvertLibrary.ConvertToGuid(typeof(object), lsId);
                if (!Guid.Empty.Equals(loId))
                {
                    for (int lnE = 0; lnE < loList.Count; lnE++)
                    {
                        loEntity = loList[lnE] as MaxAppEntity;
                        if (loEntity.Id.Equals(loId))
                        {
                            return loEntity;
                        }
                    }
                }
            }

            return MaxAppEntity.Create();
        }

        public static string GetGoogleAnalyticsJavascriptTag()
        {
            string lsAnalyticsId = GetCurrent().AnalyticsId;
            if (!string.IsNullOrEmpty(lsAnalyticsId))
            {
                if (!_oTrackingCodeIndex.ContainsKey(lsAnalyticsId))
                {
                    lock (_oLock)
                    {
                        if (!_oTrackingCodeIndex.ContainsKey(lsAnalyticsId))
                        {
                            string lsJavscriptCode = _sGoogleUniversalAnalyticsCodeFormat.Replace("[GID]", lsAnalyticsId);
                            // Add function to track outbound clicks
                            lsJavscriptCode += "\r\nvar pfTrackOutboundLink = function (loLink, lsUrl) {\r\n" +
                                "ga('send', 'event', 'outbound', 'click', lsUrl, {\r\n" +
                                "'hitCallback': function () {\r\n" +
                                "if (loLink.target.toLowerCase() != '_blank') { document.location = lsUrl;}}});}\r\n";
                            // Add onclick event to all external links
                            lsJavscriptCode += "\r\njQuery(document).ready(function () {\r\n" +
                                "jQuery(\"a[href^='http:']:not([href*='\" + window.location.host + \"'])\").each(function () {\r\n" +
                                "jQuery(this).on(\"click\", function () {\r\n" +
                                "pfTrackOutboundLink(this, jQuery(this).attr('href'));});});});\r\n";

                            _oTrackingCodeIndex.Add(lsAnalyticsId, "\r\n<script>\r\n" + lsJavscriptCode + "</script>\r\n");
                        }
                    }
                }

                return _oTrackingCodeIndex[lsAnalyticsId];
            }

            return string.Empty;
        }

        public static string GetGoogleTagManagerJavascriptHead()
        {
            string lsGTMId = GetCurrent().GTMId;
            if (!string.IsNullOrEmpty(lsGTMId))
            {
                if (!_oTrackingCodeIndex.ContainsKey(lsGTMId + "HEAD"))
                {
                    lock (_oLock)
                    {
                        if (!_oTrackingCodeIndex.ContainsKey(lsGTMId + "HEAD"))
                        {
                            string lsJavscriptCode = _sGoogleTagManagerHeadCodeFormat.Replace("[GTMID]", lsGTMId);
                            _oTrackingCodeIndex.Add(lsGTMId + "HEAD", lsJavscriptCode);
                        }
                    }
                }

                return _oTrackingCodeIndex[lsGTMId + "HEAD"];
            }

            return string.Empty;
        }

        public static string GetGoogleTagManagerJavascriptBody()
        {
            string lsGTMId = MaxAppEntity.GetCurrent().GTMId;
            if (!string.IsNullOrEmpty(lsGTMId))
            {
                if (!_oTrackingCodeIndex.ContainsKey(lsGTMId + "BODY"))
                {
                    lock (_oLock)
                    {
                        if (!_oTrackingCodeIndex.ContainsKey(lsGTMId + "BODY"))
                        {
                            string lsJavscriptCode = _sGoogleTagManagerBodyCodeFormat.Replace("[GTMID]", lsGTMId);
                            _oTrackingCodeIndex.Add(lsGTMId + "BODY", lsJavscriptCode);
                        }
                    }
                }

                return _oTrackingCodeIndex[lsGTMId + "BODY"];
            }

            return string.Empty;
        }

        public static string GetMicrosoftApplicationInsightsJavascript()
        {
            string lsInstrumentationKey = MaxAppEntity.GetCurrent().InstrumentationKey;
            if (!string.IsNullOrEmpty(lsInstrumentationKey))
            {
                if (!_oTrackingCodeIndex.ContainsKey(lsInstrumentationKey))
                {
                    lock (_oLock)
                    {
                        if (!_oTrackingCodeIndex.ContainsKey(lsInstrumentationKey))
                        {
                            string lsJavscriptCode = _sMicrosoftApplicationInsightsHeadCodeFormat.Replace("[INSTRUMENTATIONKEY]", lsInstrumentationKey);
                            _oTrackingCodeIndex.Add(lsInstrumentationKey, lsJavscriptCode);
                        }
                    }
                }

                return _oTrackingCodeIndex[lsInstrumentationKey];
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets a string that can be used to sort a list of this entity.
        /// </summary>
        /// <returns>Lowercase version of Name passed to 100 characters.</returns>
        public override string GetDefaultSortString()
        {
            return this.Name.ToLowerInvariant().PadRight(100, ' ') + base.GetDefaultSortString();
        }

        protected override void SetProperties()
        {
            if (null != this._oConfig)
            {
                this.Set(this.DataModel.Config, MaxConvertLibrary.SerializeObjectToString(this._oConfig));
            }

            base.SetProperties();
        }
    }
}
