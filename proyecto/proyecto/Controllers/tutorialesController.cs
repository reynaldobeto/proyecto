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
    public class tutorialesController : Controller
    {
        //
        // GET: /tutoriales/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult tutorial() {
            return View();
        }
        public ActionResult mostrartutorial() 
        {
            claseslinqDataContext x = new claseslinqDataContext();
            ViewBag.lista1 = from i in x.vista5s select i;
            return View();
           
        }
        [HttpPost,ValidateInput(false)]
        public ActionResult insertar(string titulo, string descripcion, HttpPostedFileBase portada, string contenido, string categoria, tutoriales tuto)
        {
            if (ModelState.IsValid)
            {
                claseslinqDataContext c = new claseslinqDataContext();

                tuto.titulo = titulo;
                tuto.descripcion = descripcion;
                var data2 = new byte[portada.ContentLength];
                portada.InputStream.Read(data2, 0, portada.ContentLength);
                var path = ControllerContext.HttpContext.Server.MapPath("/Content/imagenes1/");
                var filename = Path.Combine(path, Path.GetFileName(portada.FileName));
                System.IO.File.WriteAllBytes(Path.Combine(path, filename), data2);
                tuto.portada = (portada.FileName).ToString();

                tuto.contenido = contenido;
                tuto.idusers = (Guid)Membership.GetUser().ProviderUserKey;
                tuto.publicacion(tuto);

                int pub = c.publicacions.Where(m => m.titulo == tuto.titulo && tuto.fecha_publicacion == tuto.fecha_publicacion).Select(m => m.id).ToArray()[0];

                tuto.tutorial(tuto, pub);

                string[] arraycategorias = tuto.categoria.ToLower().Split(',');
                List<categoria> listacategoria = new List<categoria>();

                foreach (var items in arraycategorias)
                {
                    string item = items.Trim();
                    if (c.categorias.Where(a => a.tipo == item).Count() == 0)
                    {
                        listacategoria.Add(new categoria() { tipo = item, });
                    }
                }
                if (listacategoria.ToList().Count > 0)
                {
                    c.categorias.InsertAllOnSubmit(listacategoria);
                    c.SubmitChanges();
                }

                int idpub = c.publicacions.Where(m => m.titulo == tuto.titulo && tuto.fecha_publicacion == tuto.fecha_publicacion).Select(m => m.id).ToArray()[0];
                int idcate = c.categorias.Where(m => m.tipo == tuto.categoria).Select(m => m.idcat).ToArray()[0];
                tuto.regcategor(tuto, idpub, idcate);

            }
            return RedirectToAction("mostrarlibro", "libro");

        }

    }
}
