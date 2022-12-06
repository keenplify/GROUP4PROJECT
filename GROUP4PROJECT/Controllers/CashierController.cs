using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GROUP4PROJECT.Configs;
using GROUP4PROJECT.Models;
using SqlKata.Compilers;
using SqlKata.Execution;
using GROUP4PROJECT.Validations;
using GROUP4PROJECT.Helpers;
using BCrypt.Net;
using Newtonsoft.Json;

namespace GROUP4PROJECT.Controllers
{
    public class CashierController : Controller
    {
        [HttpPost]
        public ActionResult Index(Cashier cashier, string redirect)
        {
            var results = new CashierValidator().Validate(cashier);

            if (!results.IsValid)
            {
                return Json(Http.ValidationErrors(results.Errors));
            }

            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            cashier.Id = Guid.NewGuid();

            var data = new {
                cashier.FullName,
                cashier.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(cashier.Password, 12)
            };

            db.Query("cashiers_tbl").Insert(data);

            if (!string.IsNullOrEmpty(redirect))
            {
                return Redirect(redirect);
            }

            connection.Close();


            return Json(cashier);
        }

        [HttpPost]
        public ActionResult Update(Guid id, Cashier cashier, string redirect)
        {
            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            db.Query("cashiers_tbl").Where("Id", id).Update(new { 
                cashier.Username,
                cashier.FullName
            });

            if (!string.IsNullOrEmpty(redirect))
            {
                return Redirect(redirect);
            }

            connection.Close();


            return Json(cashier);
        }

        [HttpPost]
        public ActionResult ChangePassword(Guid id, string password, string redirect)
        {
            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            db.Query("cashiers_tbl").Where("Id", id).Update(new
            {
                Password = BCrypt.Net.BCrypt.HashPassword(password)
            });

            if (!string.IsNullOrEmpty(redirect))
            {
                return Redirect(redirect);
            }

            connection.Close();


            return Json(new { message = "Successful" });
        }
    }
}