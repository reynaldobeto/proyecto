using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using proyecto.Models;
using System.IO;
using System.Web.Security;

namespace proyecto.Controllers
{
    public class comentarioController : Controller
    {
        //
        // GET: /comentario/

        public ActionResult Index()
        {
            return View();
        }
     
        [HttpPost, Authorize(Roles = "usuario")]
        public ActionResult comentario(string contenido, int id_conte)
        {
            claseslinqDataContext ca = new claseslinqDataContext();
            Guid ide = (from dt in ca.aspnet_Users where dt.UserName == User.Identity.Name select dt.UserId).ToArray()[0];
            comentario co = new comentario()
            { 
                contenido = contenido,
                fecha = DateTime.Now,
                UserId = ide,
                id_cont = id_conte,
            };
            ca.comentarios.InsertOnSubmit(co);
            ca.SubmitChanges();
            //var karma1 = (from k in ca.perfils where k.id == id_conte select k).Single();
            var karma1 = from k in ca.publicacions
                         join u in ca.aspnet_Users on k.UserId equals u.UserId
                         where k.id == id_conte
                         select new
                         {
                             userid = u.UserId,
                         };

             Guid ide11 = karma1.ToArray()[0].userid;
             var kar = (from i in ca.perfils where i.UserId == ide11 select i).Single();
            kar.puntaje += 0.5;
            ca.SubmitChanges();
            int id = id_conte;
            return Redirect("/libro/detalle/" + id);


        }
    }
}
