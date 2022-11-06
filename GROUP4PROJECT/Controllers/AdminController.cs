using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}