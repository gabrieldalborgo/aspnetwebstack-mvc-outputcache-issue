using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OutputCacheIssue.Attributes
{
    public class DebugOutputCacheAttribute : OutputCacheAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            System.Diagnostics.Debug.Write(this);
            System.Diagnostics.Debug.WriteLine(string.Format(" /// '{0}' '{1}' '{2}'",
                filterContext.ActionParameters["iDisplayStart"],
                filterContext.ActionParameters["iDisplayLength"],
                filterContext.ActionParameters["sSearch"]));
            base.OnActionExecuting(filterContext);
        }
        public override string ToString()
        {
            return string.Format("CacheProfile={0}|Duration={1}|Location={2}|VaryByParam={3}", base.CacheProfile, base.Duration, base.Location, base.VaryByParam);
        }
    }
}