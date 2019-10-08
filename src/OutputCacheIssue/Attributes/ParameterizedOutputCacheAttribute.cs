using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace OutputCacheIssue.Attributes
{
    public class ParameterizedOutputCacheAttribute : OutputCacheAttribute
    {
        private const string sectionPath = "system.web/caching/outputCacheSettings";

        protected static OutputCacheProfileCollection profiles
        {
            get
            {
                var section = (OutputCacheSettingsSection)WebConfigurationManager.GetSection(sectionPath);
                if (section != null)
                {
                    return section.OutputCacheProfiles;
                }
                return null;
            }
        }

        public new string CacheProfile
        {
            get
            {
                return base.CacheProfile;
            }
            set
            {
                base.CacheProfile = value;

                if (profiles != null)
                {
                    var profile = profiles[value];
                    if (!(profile == null || string.IsNullOrWhiteSpace(profile.VaryByParam)))
                    {
                        this.VaryByParam = profile.VaryByParam;
                    }
                }
            }
        }
    }

}