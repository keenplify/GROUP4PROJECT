using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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