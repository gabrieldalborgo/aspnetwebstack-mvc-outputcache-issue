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

## Demo

Technical specifications:
- Developed using Visual Studio 2013
- MVC 4 Web Application (.NET Framework 4)
- DataTables jQuery plug-in (version 1.9.4) with server-side mode.

[Source code](https://github.com/gabrieldalborgo/aspnetwebstack-mvc-outputcache-issue/tree/master/src)

## Activity Log

- Create application environment
  - Install visual studio 2013 for working with the same IDE at the time of the issue
  - Create MVC 4 web application
    - Configure cache profiles with the given configuration
    - Download DataTables jQuery plug-in (version 1.9.4)
    - Implement DataTables with server-side configuration
- Identifie the issue
  - Deeply understand the purpose and function of the output cache
  - Analize possible configurations of the output cache
  - Implement output cache in all possible ways
- Find the solution
  - Investigate viable options
  - Implement solutions in environment created in step 1
  - Document and communicate
