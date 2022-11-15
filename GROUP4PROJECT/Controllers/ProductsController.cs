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

            IEnumerable<Product> products = db.Query("products_tbl").Where("IsDeleted", false).Get<Product>();

            foreach (var product in products)
            {
                product.Category = db.Query("categories_tbl").Where("Id", product.CategoryId).First<Category>();
            }

            return Json(products, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Single(Guid id)
        {
            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            var product = db.Query("products_tbl").Where("Id", id).Where("IsDeleted", false).First<Product>();
            product.Category = db.Query("categories_tbl").Where("Id", product.CategoryId).First<Category>();

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

            product.Id = Guid.NewGuid();
            db.Query("products_tbl").Insert(new {
                product.Name,
                product.Price,
                product.Description,
                product.CategoryId,
                product.ImageUrl
            });

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

            db.Query("products_tbl").Where("Id", id).Update(new
            {
                product.Name,
                product.Price,
                product.Description,
                product.CategoryId,
                product.ImageUrl
            });

            return Json(product);
        }

        [HttpPost]
        public ActionResult Delete(Guid id, string redirect)
        {
            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            db.Query("products_tbl").Where("Id", id).Update(new { IsDeleted = true });

            if (!string.IsNullOrEmpty(redirect))
            {
                return Redirect(redirect);
            }

            return Json(new
            {
                message= "Deleted successfully."
            });
        }
    }
}