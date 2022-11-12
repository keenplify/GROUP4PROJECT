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
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Product()
        {
            return View();
        }

        public ActionResult LoginAdmin()
        {
            return View();
        }
        public ActionResult EmployeeAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult HandleLogin(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "The parameters are incomplete.";
                return View("LoginAdmin");
                //return Json(Http.JsonError(422, "The parameters are incomplete."));
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
            return Redirect("Product");
            // TODO - Handle login redirection
        }
    }
}