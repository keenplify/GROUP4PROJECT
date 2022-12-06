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
using System.Diagnostics;

namespace GROUP4PROJECT.Controllers
{
    public class OrderController : Controller
    {
        [HttpGet]
        public ActionResult Index(bool filterDone = false)
        {
            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            var query = db.Query("orders_tbl");

            if (filterDone) query.Where("Status", "!=", "Done");

            IEnumerable<Order> orders = query.Get<Order>();

            foreach (var order in orders)
            {
                order.OrderProducts = db.Query("order_products_tbl").Where("OrderId", order.Id).Get<OrderProduct>();

                foreach (var orderProduct in order.OrderProducts)
                {
                    orderProduct.Product = db.Query("products_tbl").Where("Id", orderProduct.ProductId).First<Product>();
                }
            }
            connection.Close();
            return Json(orders, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Single(Guid id)
        {
            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            var order = db.Query("orders_tbl").Where("Id", id).First<Order>();
            connection.Close();

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

            var newOrderGuid = Guid.NewGuid();

            db.Query("orders_tbl").Insert(new {
                Id = newOrderGuid,
                order.Status,
                order.CustomerName,
                order.Type,
            });

            foreach (var orderProduct in order.OrderProducts)
            {
                db.Query("order_products_tbl").Insert(new { 
                    orderProduct.Quantity,
                    orderProduct.Remarks,
                    orderProduct.ProductId,
                    OrderId = newOrderGuid,
                });
            }

            var newOrder = db.Query("orders_tbl").Where("Id", newOrderGuid).First<Order>();

            newOrder.OrderProducts = db.Query("order_products_tbl").Where("OrderId", newOrderGuid).Get<OrderProduct>();

            foreach (var orderProduct in newOrder.OrderProducts)
            {
                orderProduct.Product = db.Query("products_tbl").Where("Id", orderProduct.ProductId).First<Product>();
            }
            connection.Close();

            return Json(newOrder);
        }

        [HttpPost]
        public ActionResult UpdateStatus(Guid id, Order order)
        {
            var results = new OrderValidator(true).Validate(order);

            if (!results.IsValid)
            {
                return Json(Http.ValidationErrors(results.Errors));
            }

            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            db.Query("orders_tbl").Where("Id", id).Update(new {
                order.Status,
            });
            connection.Close();

            return Json(order);
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            db.Query("orders_tbl").Where("Id", id).Delete();
            connection.Close();

            return Json(new
            {
                message = "Deleted successfully."
            });
        }
    }
}