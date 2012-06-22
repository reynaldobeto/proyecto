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
    public class libroController : Controller
    {
        //
        // GET: /libro/return

        public ActionResult Index()
        {
           return   View();
        }
        public ActionResult mostrarlibro()
        {
           claseslinqDataContext x = new claseslinqDataContext();
            ViewBag.lista1 = from i in x.vista2s select i;



            return View();
          
        }
        public ActionResult detalle(int ide)
        {
            //int a = ide;
            //ViewBag.hola = ide;
            claseslinqDataContext x = new claseslinqDataContext();
            ViewBag.lista2 = from i in x.vista2s where i.id == ide select i ;

            return View();
        
        }
        public ActionResult insertarlibro()
        {
            /*perfil p = new perfil();
            claseslinqDataContext f = new claseslinqDataContext();
            Guid id = (from dt in f.aspnet_Users where dt.UserName == User.Identity.Name select dt.UserId).ToArray()[0];
            p = (from c in f.perfils where c.UserId == id select c).ToArray()[0];
            ViewBag.list = p;*/
            return View();
            //return redirect("Detalle/id")
           
        }
        //public ActionResult Detalle(int id) {
                
        //    return View();
        //}
       /* public string titulo { get; set; }
        public string fecha_publicacion { get; set; }
        public string descripcion { get; set; }
        public string contenido { get; set; }
        public string portada { get; set; }
        public string estado { get; set; }
        public string autor { get; set; }
        public string año { get; set; }
        public string idioma { get; set; }
        public string tamaño { get; set; }
        */


        [HttpPost]
        public ActionResult insertarlibro(string titulo, string descripcion, HttpPostedFileBase contenido, HttpPostedFileBase portada, string autor, int año, string idioma, string tamaño, string categoria, libromodel model) 
        {
            if (ModelState.IsValid)
            {
                
                claseslinqDataContext b = new claseslinqDataContext();
                model.titulo = titulo;
                model.descripcion = descripcion;
                var data1 = new byte[contenido.ContentLength];
                contenido.InputStream.Read(data1, 0, contenido.ContentLength);
                var path1 = ControllerContext.HttpContext.Server.MapPath("/Content/libros/");
                var filename1 = Path.Combine(path1, Path.GetFileName(contenido.FileName));
                System.IO.File.WriteAllBytes(Path.Combine(path1, filename1), data1);
                model.contenido = (contenido.FileName).ToString();

                
                var data2 = new byte[portada.ContentLength];
                portada.InputStream.Read(data2, 0, portada.ContentLength);
                var path = ControllerContext.HttpContext.Server.MapPath("/Content/imagenes/");
                var filename = Path.Combine(path, Path.GetFileName(portada.FileName));
                System.IO.File.WriteAllBytes(Path.Combine(path, filename), data2);
                model.portada = (portada.FileName).ToString();

                model.autor = autor;
                model.año = año;
                model.idioma = idioma;
                model.tamaño = (contenido.ContentLength).ToString();
                model.idusers = (Guid)Membership.GetUser().ProviderUserKey;
                model.publicacion(model);
            
                int pub = b.publicacions.Where(m => m.titulo == model.titulo).Select(m => m.id).ToArray()[0];
                
                model.libro(model, pub);

                //model.rcategoria(model);
                string[] arraycategorias = model.categoria.ToLower().Split(',');
                List<categoria> listacategoria = new List<categoria>();
                
                foreach (var items in arraycategorias) 
                {
                    string item = items.Trim();
                    if (b.categorias.Where(a => a.tipo==item).Count()==0)
                    {
                        listacategoria.Add(new categoria() { tipo = item, });
                    }
                }
                if (listacategoria.ToList().Count>0)
                {
                    b.categorias.InsertAllOnSubmit(listacategoria);
                    b.SubmitChanges();
                }


                int idpub = b.publicacions.Where(m => m.titulo == model.titulo).Select(m => m.id).ToArray()[0];
                int idcate = b.categorias.Where(m => m.tipo == model.categoria).Select(m => m.idcat).ToArray()[0];
                model.regcategoria(model, idpub, idcate);

                



            }

            ViewBag.mensaje = "El libro se ha registrado correctamente";

            return RedirectToAction("mostrarlibro");
           
 


            /*var data = new byte[avatar.ContentLength];
            avatar.InputStream.Read(data, 0, avatar.ContentLength);
            var path = ControllerContext.HttpContext.Server.MapPath("/imagenes");
            var filename = Path.Combine(path, Path.GetFileName(avatar.FileName));
            System.IO.File.WriteAllBytes(Path.Combine(path, filename), data);
            //FormsAuthentication.SetAuthCookie(User.Identity.Name, false);
            //return RedirectToAction("Index", "Home");
            claseslinqDataContext regis = new claseslinqDataContext();
            Guid idusuario = (from a in regis.aspnet_Users where a.UserName == User.Identity.Name select a.UserId).ToArray()[0];
            
            publicacion pub = new publicacion()
            {

                titulo = modelo.titulo,
                fecha_publicacion =DateTime.Now,
                descripcion = modelo.descripcion,
                contenido = "modelo.contenido",
                portada = "modelo.portada",
                estado = "activo",
                UserId = idusuario,
               
            };
            regis.publicacions.InsertOnSubmit(pub);
            regis.SubmitChanges();
            int idpublicacion = (from a in regis.publicacions where a.titulo == modelo.titulo && a.fecha_publicacion == pub.fecha_publicacion select a.id).ToArray()[0];
            
            libro lib = new libro()
            {

                autor = modelo.autor,
                año = modelo.año,
                idioma_tamaño = modelo.idioma,
                tamaño = "modelo.tamaño",
                id_conte = idpublicacion,
            };
            regis.libros.InsertOnSubmit(lib);

            
            regis.SubmitChanges();
            return View();*/
        }
        
    }
}
