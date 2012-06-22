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
    public class cursosController : Controller
    {
        //
        // GET: /cursos/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult cursos()
        {
            return View();
        }
        public ActionResult mostrarcursos() 
        {
            claseslinqDataContext x = new claseslinqDataContext();
            ViewBag.lista1 = from i in x.vista6s select i;
            return View();

        }

        [HttpPost,ValidateInput(false)]
        public ActionResult insertar(string titulo, string descripcion, HttpPostedFileBase portada, string contenido, string categoria, curso1 cur)
        {
            if (ModelState.IsValid)
            {
                claseslinqDataContext c = new claseslinqDataContext();

                cur.titulo = titulo;
                cur.descripcion = descripcion;
                var data2 = new byte[portada.ContentLength];
                portada.InputStream.Read(data2, 0, portada.ContentLength);
                var path = ControllerContext.HttpContext.Server.MapPath("/Content/imagenes2/");
                var filename = Path.Combine(path, Path.GetFileName(portada.FileName));
                System.IO.File.WriteAllBytes(Path.Combine(path, filename), data2);
                cur.portada = (portada.FileName).ToString();

                cur.contenido = contenido;
                cur.idusers = (Guid)Membership.GetUser().ProviderUserKey;
                cur.publicacion(cur);

                int pub = c.publicacions.Where(m => m.titulo == cur.titulo && cur.fecha_publicacion == cur.fecha_publicacion).Select(m => m.id).ToArray()[0];

                cur.cursos(cur, pub);

                string[] arraycategorias = cur.categoria.ToLower().Split(',');
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

                int idpub = c.publicacions.Where(m => m.titulo == cur.titulo && cur.fecha_publicacion == cur.fecha_publicacion).Select(m => m.id).ToArray()[0];
                int idcate = c.categorias.Where(m => m.tipo == cur.categoria).Select(m => m.idcat).ToArray()[0];
                cur.regcategor(cur, idpub, idcate);

            }
            return RedirectToAction("mostrarcursos", "cursos");

        }
    }
}
