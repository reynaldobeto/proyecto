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
        [HttpPost]
        public ActionResult comentario(string contenido, int id_conte)
        {
            claseslinqDataContext ca = new claseslinqDataContext();
            Guid id = (from dt in ca.aspnet_Users where dt.UserName == User.Identity.Name select dt.UserId).ToArray()[0];
            comentario co = new comentario()
            { 
                contenido = contenido,
                fecha = DateTime.Now,
                UserId = id,
                id_cont = id_conte,
            };
            ca.comentarios.InsertOnSubmit(co);
            ca.SubmitChanges();

            return RedirectToAction("mostrarlibro", "libro");

            //perfil registrar = new perfil()
            //{
            //    nombre = nombre,
            //    apellido = apellido,
            //    pais = pais,
            //    intereses = intereses,
            //    sexo = sexo,
            //    estado = "activo",
            //    avatar = avatar.FileName,
            //    UserId = idusuario
            //};
            //regis.perfils.InsertOnSubmit(registrar);
            //regis.SubmitChanges();
        }
    }
}
