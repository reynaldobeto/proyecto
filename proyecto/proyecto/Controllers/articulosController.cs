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
    public class articulosController : Controller
    {
        //
        // GET: /articulos/

        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult articulos()
        {
            return View();
        }
        public ActionResult mostrarticulo()
        {
            claseslinqDataContext x = new claseslinqDataContext();
            ViewBag.lista1 = from i in x.vista4s select i;
            return View();
        }


        [HttpPost,ValidateInput(false)]
        public ActionResult insertar(string titulo, string descripcion, HttpPostedFileBase portada, string contenido, string categoria, articulos arti) 
        {
            if (ModelState.IsValid)
            {
                claseslinqDataContext c = new claseslinqDataContext();

                arti.titulo = titulo;
                arti.descripcion = descripcion;
                var data2 = new byte[portada.ContentLength];
                portada.InputStream.Read(data2, 0, portada.ContentLength);
                var path = ControllerContext.HttpContext.Server.MapPath("/Content/imagenes/");
                var filename = Path.Combine(path, Path.GetFileName(portada.FileName));
                System.IO.File.WriteAllBytes(Path.Combine(path, filename), data2);
                arti.portada = (portada.FileName).ToString();

                arti.contenido = contenido;
                arti.idusers = (Guid)Membership.GetUser().ProviderUserKey;
                arti.publicacion(arti);

                int pub = c.publicacions.Where(m => m.titulo == arti.titulo && arti.fecha_publicacion == arti.fecha_publicacion).Select(m => m.id).ToArray()[0];
                
                arti.articulo(arti, pub);

                string[] arraycategorias = arti.categoria.ToLower().Split(',');
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

                int idpub = c.publicacions.Where(m => m.titulo == arti.titulo && arti.fecha_publicacion == arti.fecha_publicacion).Select(m => m.id).ToArray()[0];
                int idcate = c.categorias.Where(m => m.tipo == arti.categoria).Select(m => m.idcat).ToArray()[0];
                arti.regcategor(arti,idpub,idcate);

            }
            return RedirectToAction("articulos", "mostrarticulo");
        
        }

    }
}
