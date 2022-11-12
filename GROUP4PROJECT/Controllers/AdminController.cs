﻿using System;
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
                
                return Redirect("LoginAdmin?error=incomplete");
                //return Json(Http.JsonError(422, "The parameters are incomplete."));
                // TODO - Handle incomplete parameters
            }

            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            var admin = db.Query("administrators_tbl").Where("Username", username).FirstOrDefault<Administrator>();

            if (string.IsNullOrEmpty(username))
                {
                return Redirect("LoginAdmin?error=wrongusername");
            }


            if (!BCrypt.Net.BCrypt.Verify(password, admin.Password))
            {

                return Redirect("LoginAdmin?error=wrongpassword");
                
            }

            Session["Admin"] = admin;
            return Redirect("Product");
            // TODO - Handle login redirection
        }
    }
}