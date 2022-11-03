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

namespace GROUP4PROJECT.Controllers
{
    public class OrderController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            IEnumerable<Order> orders = db.Query("orders_tbl").Get<Order>();

            return Json(orders, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Single(Guid id)
        {
            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            var order = db.Query("orders_tbl").Where("Id", id).First<Order>();

            return Json(order, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Index(Order order)
        {
            var results = new OrderValidator().Validate(order);

            if (!results.IsValid)
            {
                return Json(Http.ValidationErrors(results.Errors));
            }

            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            db.Query("orders_tbl").Insert(order);

            return Json(order);
        }

        [HttpPost]
        public ActionResult Update(Guid id, Order order)
        {
            var results = new OrderValidator(true).Validate(order);

            if (!results.IsValid)
            {
                return Json(Http.ValidationErrors(results.Errors));
            }

            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            db.Query("orders_tbl").Where("Id", id).Update(order);

            return Json(order);
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            db.Query("orders_tbl").Where("Id", id).Delete();

            return Json(new
            {
                message = "Deleted successfully."
            });
        }
    }
}