using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RefBOT.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string ss = System.Configuration.ConfigurationManager.AppSettings["ConString"].ToString();
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
