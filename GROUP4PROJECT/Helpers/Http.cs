using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentValidation;

namespace GROUP4PROJECT.Helpers
{
	public class Http
	{
		public static object JsonError(int code, string message)
        {
            HttpContext.Current.Response.StatusCode = code;
            return new
            {
                message
            };
        }

        public static object ValidationErrors(List<FluentValidation.Results.ValidationFailure> errors)
        {
            HttpContext.Current.Response.StatusCode = 422;
            return new
            {
                message = "We are unable to continue with your request because of validation errors",
                errors
            };
        }
	}
}