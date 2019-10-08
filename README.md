# aspnetwebstack-mvc-outputcache-issue

## Issue:
Output cache not working properly using cache profiles. For example, with this configuration:
```xml
<caching>
  <outputCacheSettings>
    <outputCacheProfiles>
      <add name="CacheEvents" duration="3600" varyByParam="iDisplayStart;iDisplayLength;sSearch" />
    </outputCacheProfiles>
  </outputCacheSettings>
</caching>
```
Example:
```c#
[OutputCache(CacheProfile = "CacheEvents")]
public ActionResult Index()
{
  return View();
}
```

## Reason:
Output cache ignores the varyByParam parameter configured at the cache profiles on the web.config.
It leaves the varyByParam value as "*", so any change on any param discards the cache.

Evidence: [https://github.com/aspnetwebstack/aspnetwebstack/blob/master/src/System.Web.Mvc/OutputCacheAttribute.cs#L23](https://github.com/aspnetwebstack/aspnetwebstack/blob/master/src/System.Web.Mvc/OutputCacheAttribute.cs#L23)

## Solution:

### #1 Configure all the settings at the CacheOutputAttribute
```c#
[OutputCache(Duration = 3600, VaryByParam = "iDisplayStart;iDisplayLength;sSearch")]
public ActionResult Index()
{
  return View();
}
```

### #2 Set the cache profile CacheOutputAttribute and configure the varyByParam parameter there too.
```c#
[OutputCache(CacheProfile = "CacheEvents", VaryByParam = "iDisplayStart;iDisplayLength;sSearch")]
public ActionResult Index()
{
  return View();
}
```

### #3 Extends the CacheOutputAttribute providing the capability of taking the varyByParam value from cache profiles on web.config
```c#
[ParameterizedOutputCacheAttribute(CacheProfile = "CacheEvents")]
public ActionResult Index()
{
  return View();
}
```

Implementation:
```c#
using System.Web.Mvc;
using System.Web.Configuration;

namespace Unconnected.Mvc.Outputcache
{
    public class ParameterizedOutputCacheAttribute:OutputCacheAttribute
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
                    if (!(profile==null || string.IsNullOrWhiteSpace(profile.VaryByParam)))
                    {
                        this.VaryByParam = profile.VaryByParam;
                    }
                }
            }
        }
    }
}
```
Courtesy of [Alexander Kouznetsov](https://github.com/unconnected4/MvcOutputCacheFix)

## Activity Log
