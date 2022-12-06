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
    public class CategoryController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            IEnumerable<Category> categories = db.Query("categories_tbl").Where("IsDeleted", false).Get<Category>();

            foreach (var category in categories)
            {
                category.Products = db.Query("products_tbl").Where("CategoryId", category.Id).Get<Product>();
            }

            connection.Close();


            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Single(Guid id)
        {
            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            var category = db.Query("categories_tbl").Where("Id", id).Where("IsDeleted", false).First<Category>();
            category.Products = db.Query("categories_tbl").Where("CategoryId", category.Id).Get<Product>();
            connection.Close();

            return Json(category, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Index(Category category)
        {
            var results = new CategoryValidator().Validate(category);

            if (!results.IsValid)
            {
                return Json(Http.ValidationErrors(results.Errors));
            }

            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            category.Id = Guid.NewGuid();
            db.Query("categories_tbl").Insert(new { 
                category.Name,
                category.ImageUrl,
            });
            connection.Close();

            return Json(category);
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
            connection.Close();

            return Json(product);
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);
            db.Query("categories_tbl").Where("Id", id).Update(new { IsDeleted = true });
            connection.Close();

            return Json(new
            {
                message= "Deleted successfully."
            });
        }
    }
}