using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using proyecto.Models;
using CaptchaMVC.Attribute;
using CaptchaMVC.HtmlHelpers;
using CaptchaMVC.Models;
namespace proyecto.Controllers
{
    public class moderacionusuariosController : Controller
    {
        //
        // GET: /moderacionusuarios/

        public ActionResult Index()
        {
            return View();
        }
         public ActionResult Usuarios() {
            claseslinqDataContext db = new claseslinqDataContext();
            ViewBag.userlist = (from i in db.perfils
                                join j in db.aspnet_Memberships on i.UserId equals j.UserId
                                select i).ToList();

 
            return View();
        }
         public ActionResult Bannear(Guid id)
         {
             claseslinqDataContext db = new claseslinqDataContext();
             perfil pu = db.perfils.Single(u => u.UserId == id);
             pu.estado = "Inactivo";
             db.SubmitChanges();
             return RedirectToAction("Usuarios");
         }
         public ActionResult Activar(Guid id)
         {
             claseslinqDataContext db = new claseslinqDataContext();
             perfil pu = db.perfils.Single(u => u.UserId == id);
             pu.estado = "activo";
             db.SubmitChanges();
             return RedirectToAction("Usuarios");
         }
         public ActionResult Disminuir_karma(FormCollection f)
         {
             Guid id = new Guid(f["userid"]);

             int i = 0;
             if (f["cantidad"] != null && f["cantidad"] != "")
                 i = Convert.ToInt32(f["cantidad"]);
             claseslinqDataContext db = new claseslinqDataContext();
             perfil pu = db.perfils.Single(u => u.UserId == id);
             pu.puntaje = pu.puntaje - i;
             db.SubmitChanges();
             return RedirectToAction("Usuarios");
         }

    }
}
