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
    public class POSController : Controller
    {
        // GET: POS
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult QRScanner()
        {
            return View();
        }
        public ActionResult LoginPOS()
        {
            return View();
        }
        [HttpPost]
        public ActionResult HandleLogin(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {

                return Redirect("LoginCashier?error=incomplete");
                //return Json(Http.JsonError(422, "The parameters are incomplete."));
                // TODO - Handle incomplete parameters
            }

            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            var cashier = db.Query("cashiers_tbl").Where("Username", username).FirstOrDefault<Cashier>();

            if (cashier == null)
            {
                return Redirect("LoginCashier?error=wrongusername");
            }


            if (!BCrypt.Net.BCrypt.Verify(password, cashier.Password))
            {

                return Redirect("LoginCashier?error=wrongpassword");

            }

            Session["Cashier"] = cashier;
            return Redirect("Product");
            // TODO - Handle login redirection
        }
    }
}