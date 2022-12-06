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
using Newtonsoft.Json;

namespace GROUP4PROJECT.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["Admin"] == null) return Redirect("/Admin/LoginAdmin");

            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            IEnumerable<Order> orders = db.Query("orders_tbl").Get<Order>();

            foreach (var order in orders)
            {
                order.OrderProducts = db.Query("order_products_tbl").Get<OrderProduct>();
                foreach (var orderProduct in order.OrderProducts)
                {
                    orderProduct.Product = db.Query("products_tbl").Where("Id", orderProduct.ProductId).First<Product>();
                }
            }


            ViewBag.Orders = JsonConvert.SerializeObject(orders);

            return View();
        }
        public ActionResult Product(string mode, string id)
        {
            if (Session["Admin"] == null) return Redirect("/Admin/LoginAdmin");

            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            IEnumerable<Product> products = db.Query("products_tbl").Where("IsDeleted", false).Get<Product>();

            ViewBag.Products = products;

            if (mode == "edit" && id != null)
            {
                var product = db.Query("products_tbl").Where("Id", Guid.Parse(id)).Where("IsDeleted", false).First<Product>();
                ViewBag.Product = product;

                var categories = db.Query("categories_tbl").Where("IsDeleted", false).Get<Category>();
                ViewBag.Categories = categories;
            }
            connection.Close();

            return View();
        }

        public ActionResult LoginAdmin()
        {
            Session["Admin"] = null;
            return View();
        }
        public ActionResult EmployeeAdmin(string mode, string id)
        {
            if (Session["Admin"] == null) return Redirect("/Admin/LoginAdmin");

            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            IEnumerable<Cashier> cashiers = db.Query("cashiers_tbl").Get<Cashier>();

            ViewBag.Cashiers = cashiers;

            if (mode == "edit" && id != null)
            {
                var cashier = db.Query("cashiers_tbl").Where("Id", Guid.Parse(id)).First<Cashier>();
                ViewBag.Cashier = cashier;
            }

            connection.Close();


            return View();
        }

        [HttpPost]
        public ActionResult HandleLogin(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return Redirect("LoginAdmin?error=incomplete");
            }

            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            var admin = db.Query("administrators_tbl").Where("Username", username).FirstOrDefault<Administrator>();

            if (admin == null)
            {
                return Redirect("LoginAdmin?error=wrongusername");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, admin.Password))
            {

                return Redirect("LoginAdmin?error=wrongpassword");
                
            }

            Session["Admin"] = admin;

            connection.Close();

            return Redirect("Product");
        }
    }
}