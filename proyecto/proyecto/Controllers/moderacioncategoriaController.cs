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
    public class moderacioncategoriaController : Controller
    {
        //
        // GET: /moderacioncategoria/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Categorias()
        {
            claseslinqDataContext db = new claseslinqDataContext();
            ViewBag.categos = (from i in db.categorias select i).ToList();
            return View();
        }
        public ActionResult Eliminar_categorias(int id)
        {
            claseslinqDataContext db = new claseslinqDataContext();
            var x = (from i in db.categorias where i.idcat == id select i).ToList();
            foreach (var n in x)
            {
                var rel = (from j in db.relcategorias where j.id_cat == n.idcat select j).ToList();
                foreach (var m in rel)
                    db.relcategorias.DeleteOnSubmit(m);
                db.SubmitChanges();
                db.categorias.DeleteOnSubmit(n);
                db.SubmitChanges();
            }
            return RedirectToAction("Categorias");
        }

        public ActionResult Actualizar_categoria(FormCollection f)
        {
            int id = Convert.ToInt32(f["id"]);
            claseslinqDataContext db = new claseslinqDataContext();
            int con = (from i in db.categorias where i.tipo == f["nuevo"].ToLower().Trim() select i).Count();
            if (con == 0)
            {
                categoria c = db.categorias.Single(u => u.idcat == id);
                c.tipo = f["nuevo"].ToLower().Trim();
                db.SubmitChanges();
            }
            else
            {
                var relaciones = (from i in db.relcategorias where i.id_cat == id select i).ToList();
                foreach (var v in relaciones)
                {
                    db.relcategorias.DeleteOnSubmit(v);
                    db.SubmitChanges();
                }
                var catego = (from i in db.categorias where i.idcat == id select i).ToList();
                foreach (var v in catego)
                {
                    db.categorias.DeleteOnSubmit(v);
                    db.SubmitChanges();
                }
                int ID = (from i in db.categorias where i.tipo == f["nuevo"].ToLower().Trim() select i).ToArray()[0].idcat;
                foreach (var v in relaciones)
                {

                    if ((from i in db.relcategorias where i.id_cat == ID && i.id_conte == v.id_conte select i).Count() == 0)
                    {
                        relcategoria r = new relcategoria() { id_cat = ID, id_conte = v.id_conte };
                        db.relcategorias.InsertOnSubmit(r);
                        db.SubmitChanges();
                    }
                }
            }
            return RedirectToAction("Categorias");
        }

    }
}
