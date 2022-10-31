using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GROUP4PROJECT.Configs;
using GROUP4PROJECT.Models;
using SqlKata.Compilers;
using SqlKata.Execution;
using GROUP4PROJECT.Helpers;

namespace GROUP4PROJECT.Controllers
{
    public class AdminController : Controller
    {
        [HttpPost]
        public JsonResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return Json(Http.JsonError(422, "The parameters are incomplete."));
                // TODO - Handle incomplete parameters
            }

            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            var admin = db.Query("administrators_tbl").Where("username", username).First<Administrator>();

            if (!BCrypt.Net.BCrypt.Verify(password, admin.Password))
            {
                return Json(Http.JsonError(401, "Wrong password."));
                // TODO - Handle wrong password redirect
            }

            Session["Admin"] = admin;
            return Json(admin);
            // TODO - Handle login redirection
        }
    }
}