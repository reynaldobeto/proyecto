using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using proyecto.Models;
using System.IO;

namespace proyecto.Controllers
{
    public class perfilController : Controller
    {
        //
        // GET: /perfil/
    
        public ActionResult Index()
        {
            claseslinqDataContext db = new claseslinqDataContext();


            return View();
        }
        public ActionResult perfil()
        {
            return View();
        }
        [HttpPost]
        public ActionResult perfil(string nombre, string apellido, string pais, string intereses, string sexo,  HttpPostedFileBase avatar)
        {
            var data = new byte[avatar.ContentLength];
            avatar.InputStream.Read(data, 0, avatar.ContentLength);
            var path = ControllerContext.HttpContext.Server.MapPath("/imagenes");
            var filename = Path.Combine(path, Path.GetFileName(avatar.FileName));
            System.IO.File.WriteAllBytes(Path.Combine(path, filename), data);
            //FormsAuthentication.SetAuthCookie(User.Identity.Name, false);
            //return RedirectToAction("Index", "Home");
            //if (intereses == null)
            //    intereses = "";
            claseslinqDataContext regis = new claseslinqDataContext();
            Guid idusuario = (from a in regis.aspnet_Users where a.UserName == User.Identity.Name select a.UserId).ToArray()[0];
            perfil registrar = new perfil()
            { 
                nombre=nombre,apellido=apellido,pais=pais,intereses=intereses, sexo=sexo, estado="activo", avatar=avatar.FileName ,UserId=idusuario,puntaje=0
            };
            regis.perfils.InsertOnSubmit(registrar);
            regis.SubmitChanges();
            return View();


        }
    }
}
