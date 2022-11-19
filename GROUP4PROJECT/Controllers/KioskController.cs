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
    public class KioskController : Controller
    {
        // GET: Kiosk
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Menu()
        {

            var connection = Database.GetConnection();
            var compiler = new PostgresCompiler();

            var db = new QueryFactory(connection, compiler);

            IEnumerable<Category> categories = db.Query("categories_tbl").Where("IsDeleted", false).Get<Category>();

            foreach (var category in categories)
            {
                category.Products = db.Query("products_tbl").Where("CategoryId", category.Id).Where("IsDeleted", false).Get<Product>();
            }
            ViewBag.Categories = categories;
            return View();
        }

        public ActionResult Receipt()
        {
            return View();
        }

        public ActionResult Cart()
        {
            return View();
        }
    }
}