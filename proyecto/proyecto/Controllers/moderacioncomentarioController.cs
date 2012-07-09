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
    public class moderacioncomentarioController : Controller
    {
        //
        // GET: /moderacioncomentario/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult mostrarmoderacion() 
        {
            claseslinqDataContext db = new claseslinqDataContext();
            //ViewBag.comen = (from mo in db.comentarios join l in db.aspnet_Users on mo.UserId equals l.UserId  where mo.UserId==l.UserId select mo ).ToList();
            ViewBag.comen = (from x in db.comentars orderby x.fecha descending  select x).ToList();

           
            ViewBag.cantidad = (from i in db.comentarios select i).ToList();
            ViewBag.palabra = "*";
            ViewBag.ofensivo = (from i in db.palabras select i).ToList();
            return View();

        
        }
        public ActionResult detallecomen(string id)
        {
            claseslinqDataContext db = new claseslinqDataContext();
            ViewBag.detallecome = (from i in db.comentars where i.contenido.Contains(id) select i).ToList();
            return View();
        }
        public ActionResult moderar(int id)
        {
            claseslinqDataContext c = new claseslinqDataContext();
            var id_conte = (from i in c.comentarios where i.id == id select i.id_cont).ToArray()[0];
            using (claseslinqDataContext db = new claseslinqDataContext())
            {
                var corr = (from p in db.comentarios where p.id == id select p);

                foreach (var i in corr)
                    db.comentarios.DeleteOnSubmit(i);
                db.SubmitChanges();
                var karma1 = from k in db.publicacions
                             join u in db.aspnet_Users on k.UserId equals u.UserId
                             where k.id == Convert.ToInt32(id_conte)
                             select new
                             {
                                 userid = u.UserId,
                             };

                Guid ide11 = karma1.ToArray()[0].userid;
                var kar = (from i in db.perfils where i.UserId == ide11 select i).Single();
                kar.puntaje -= 0.5;
                db.SubmitChanges();

            }
            return Redirect("/moderacioncomentario/mostrarmoderacion/");
            

        
        }
        [HttpPost]
        public ActionResult insertard(FormCollection f)
        {
            string pal = "";
            claseslinqDataContext db = new claseslinqDataContext();
            if (f["palabra"] != null && f["palabra"] != "")
            {
                pal = f["palabra"];
                if ((from i in db.palabras where i.palabra1 == pal select i).Count() == 0)
                {
                    palabra d = new palabra() { palabra1 = pal };
                    db.palabras.InsertOnSubmit(d);
                    db.SubmitChanges();
                }
            }

            return Redirect("mostrarmoderacion");
        }
        public ActionResult eliminarcomen(string id)
        {
            claseslinqDataContext db = new claseslinqDataContext();

            var v = (from i in db.comentarios where i.contenido.Contains(id) select i);
            foreach (var i in v)
            {
                db.comentarios.DeleteOnSubmit(i);
            }
            db.SubmitChanges();
            return RedirectToAction("mostrarmoderacion");
        }


    }
   



}
