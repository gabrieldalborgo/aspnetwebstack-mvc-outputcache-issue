using OutputCacheIssue.Attributes;
using OutputCacheIssue.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace OutputCacheIssue.Controllers
{
    public class HomeController : Controller
    {
        private const int ROW_COUNT = 100;

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            return View();
        }

        /// <summary>
        /// ISSUE: represents the initial configuration of the output cache using CacheProfile
        /// VaryByParam parameter setted at web.config
        /// </summary>
        [OutputCache(CacheProfile = "CacheEvents")]
        public JsonResult DemoOne(int iDisplayStart, int iDisplayLength, string sSearch)
        {
            return Json(DataHelpers.Generate(ROW_COUNT, iDisplayStart, iDisplayLength), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Solution #1: moved all initial configuration from CacheProfile to OutputCacheAttribute
        /// </summary>
        [OutputCache(Duration = 3600, VaryByParam = "iDisplayStart;iDisplayLength;sSearch")]
        public JsonResult DemoTwo(int iDisplayStart, int iDisplayLength, string sSearch)
        {
            return Json(DataHelpers.Generate(ROW_COUNT, iDisplayStart, iDisplayLength), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Solution #2: represents the initial configuration of the output cache using CacheProfile
        /// VaryByParam parameter setted at web.config and at the OutputCacheAttribute
        /// </summary>
        [OutputCache(CacheProfile = "CacheEvents", VaryByParam = "iDisplayStart;iDisplayLength;sSearch")]
        public JsonResult DemoThree(int iDisplayStart, int iDisplayLength, string sSearch)
        {
            return Json(DataHelpers.Generate(ROW_COUNT, iDisplayStart, iDisplayLength), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Solution #3: represents the initial configuration of the output cache using CacheProfile
        /// VaryByParam parameter setted at web.config and using ParameterizedOutputCacheAttribute
        /// </summary>
        [ParameterizedOutputCacheAttribute(CacheProfile = "CacheEvents")]
        public JsonResult DemoFour(int iDisplayStart, int iDisplayLength, string sSearch)
        {
            return Json(DataHelpers.Generate(ROW_COUNT, iDisplayStart, iDisplayLength), JsonRequestBehavior.AllowGet);
        }
    }
}
