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
    public class ProductsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            IEnumerable<Product> products = db.Query("products_tbl").Get<Product>();

            return Json(products, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Single(Guid id)
        {
            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            var product = db.Query("products_tbl").Where("Id", id).First<Product>();

            return Json(product, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Index(Product product)
        {
            var results = new ProductValidator().Validate(product);

            if (!results.IsValid)
            {
                return Json(Http.ValidationErrors(results.Errors));
            }

            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            db.Query("products_tbl").Insert(product);

            return Json(product);
        }

        [HttpPost]
        public ActionResult Update(Guid id, Product product)
        {
            var results = new ProductValidator(true).Validate(product);

            if (!results.IsValid)
            {
                return Json(Http.ValidationErrors(results.Errors));
            }

            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            db.Query("products_tbl").Where("Id", id).Update(product);

            return Json(product);
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            db.Query("products_tbl").Where("Id", id).Delete();

            return Json(new
            {
                message= "Deleted successfully."
            });
        }
    }
}