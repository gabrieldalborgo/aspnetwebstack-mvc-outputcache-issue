using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OutputCacheIssue.Helpers
{
    internal static class DataHelpers
    {
        internal static object Generate(int count)
        {
            var data = new List<string[]>();
            for (int i = 0; i < count; i++)
                data.Add(new string[] 
                {
                    "Gecko", "Firefox 1.0","Win 98+ OSX.2+","1.7","A"
                });
            return new
            {
                sEcho = 1,
                iTotalRecords = data.Count,
                iTotalDisplayRecords = data.Count,
                aaData = data
            };
        }

        internal static object Generate(int count, int start, int length)
        {
            var data = new List<string[]>();
            for (int i = 0; i < length; i++)
                data.Add(new string[] 
                {
                    "Gecko", "Firefox 1.0","Win 98+ OSX.2+","1.7","A"
                });

            return new
            {
                sEcho = Guid.NewGuid().ToString(),
                iTotalRecords = count,
                iTotalDisplayRecords = count,
                aaData = data
            };
        }
    }
}