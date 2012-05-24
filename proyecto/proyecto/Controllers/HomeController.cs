using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using proyecto.Models;

namespace proyecto.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";
            
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
        public ActionResult log() {
            perfil p = new perfil();
            claseslinqDataContext f = new claseslinqDataContext();
            Guid id = (from dt in f.aspnet_Users where dt.UserName == User.Identity.Name select dt.UserId).ToArray()[0];
            p = (from c in f.perfils where c.UserId == id  select c).ToArray()[0];
            ViewBag.list = p; 

            return View();
        }
        
    }
}
