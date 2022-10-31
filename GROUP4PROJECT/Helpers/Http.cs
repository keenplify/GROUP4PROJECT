using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GROUP4PROJECT.Helpers
{
	public class HttpHelpers
	{
		public static object JsonError(int code, string message)
        {
            HttpContext.Current.Response.StatusCode = code;
            return new
            {
                message
            };
        }
	}
}