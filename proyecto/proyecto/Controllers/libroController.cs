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
        public ActionResult nomegusta(int id)
        {
            claseslinqDataContext ca = new claseslinqDataContext();
            
            Guid UserId = (from dt in ca.aspnet_Users where dt.UserName == User.Identity.Name select dt.UserId).ToArray()[0];
            var lista = (from i in ca.karmas where i.id_cont == id && i.UserId == UserId && i.tipo == "me gusta" select i);
            foreach(var i in lista)
                ca.karmas.DeleteOnSubmit(i);
            ca.SubmitChanges();
            
            var karma1 = from ka in ca.publicacions
                         join u in ca.aspnet_Users on ka.UserId equals u.UserId
                         where ka.id == id
                         select new
                         {
                             userid = u.UserId,
                         };

            Guid ide11 = karma1.ToArray()[0].userid;
            var kar = (from i in ca.perfils where i.UserId == ide11 select i).Single();
            kar.puntaje -= 1;
            ca.SubmitChanges();
        



            return Redirect("/libro/detalle/" + id);
        }
        public ActionResult Observar(FormCollection f)
        {
            claseslinqDataContext db = new claseslinqDataContext();
            int id = Convert.ToInt32(f["idcont"]);
            publicacion c = db.publicacions.Single(u => u.id == id);
            c.observacion = f["observacion"];
            c.estado = "observado" ;
            db.SubmitChanges();
            return RedirectToAction("mostrar", "moderacioncontenidos");
        }

        public ActionResult mostrarlibro()
        {
           claseslinqDataContext x = new claseslinqDataContext();
            ViewBag.lista1 = from i in x.vista2s orderby i.id descending where i.estado=="true" select i;



            return View();
          
        }

        [Authorize(Roles = "usuario")]
        public ActionResult detalle(int id)
        {
            //int a = ide;
            //ViewBag.hola = ide;

            claseslinqDataContext x = new claseslinqDataContext();
            Guid idusuario = (from a in x.aspnet_Users where a.UserName == User.Identity.Name select a.UserId).ToArray()[0];
             ViewBag.con = (from i in x.karmas where i.id_cont == id && i.UserId == idusuario && i.tipo=="me gusta" select i).Count();
           
            ViewBag.lista2 = from i in x.vista2s where i.id == id select i;
            int flag = (from i in x.conteos where i.tipo == "me gusta" && i.id_cont == id select i).Count();
            if (flag == 0)
                ViewBag.contador = 0;
            else
                ViewBag.contador=(from i in  x.conteos where i.tipo=="me gusta" && i.id_cont==id select i).ToArray()[0].cantidad;
            flag = (from i in x.conteos where i.tipo == "comentario" && i.id_cont == id select i).Count();
            if (flag == 0)
                ViewBag.contadorcomen = 0;
            else
                ViewBag.contadorcomen = (from i in x.conteos where i.tipo == "comentario" && i.id_cont == id select i).ToArray()[0].cantidad;

            ViewBag.comentarios = (from i in x.comentars where i.id_cont == id orderby i.fecha descending select i).ToList();
            //ViewBag.comentarios =( from i in x.comentarios where i.id_cont==id orderby i.id descending select i).ToList();

            return View();

        }
        public ActionResult megusta(int id)
        {
            claseslinqDataContext ca = new claseslinqDataContext();
            karma k = new karma()
            {
                valorkarma = 1,
                tipo = "me gusta",
                UserId = (from dt in ca.aspnet_Users where dt.UserName == User.Identity.Name select dt.UserId).ToArray()[0],
                id_cont = id

            };
            ca.karmas.InsertOnSubmit(k);
            ca.SubmitChanges();
            var karma1 = from ka in ca.publicacions
                         join u in ca.aspnet_Users on ka.UserId equals u.UserId
                         where ka.id == id
                         select new
                         {
                             userid = u.UserId,
                         };

            Guid ide11 = karma1.ToArray()[0].userid;
            var kar = (from i in ca.perfils where i.UserId == ide11 select i).Single();
            kar.puntaje += 1;
            ca.SubmitChanges();
        


            return Redirect("/libro/detalle/" + id);
        }

         [Authorize(Roles = "usuario")]
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


        [HttpPost, Authorize(Roles = "usuario")]
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
                        categoria c = new categoria() { tipo = item };

                        b.categorias.InsertOnSubmit(c);     //listacategoria.Add(new categoria() { tipo = item, });
                        b.SubmitChanges();
                    }
                    int id=(from i in b.categorias where i.tipo==item select i).ToArray()[0].idcat;
                    int idcont = b.publicacions.Where(m => m.titulo == model.titulo).Select(m => m.id).ToArray()[0];
                    relcategoria r = new relcategoria() { id_cat=id,id_conte=idcont};
                    b.relcategorias.InsertOnSubmit(r);
                    b.SubmitChanges();
                }

                //if (listacategoria.ToList().Count>0)
                //{
                //    b.categorias.InsertAllOnSubmit(listacategoria);
                //    b.SubmitChanges();
                //}


                //int idpub = b.publicacions.Where(m => m.titulo == model.titulo).Select(m => m.id).ToArray()[0];
                //int idcate = b.categorias.Where(m => m.tipo == model.categoria).Select(m => m.idcat).ToArray()[0];
                //model.regcategoria(model, idpub, idcate);

                



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
        public ActionResult reditarlibro(int id)
        {
            claseslinqDataContext db = new claseslinqDataContext();
            var perfil = (from a in db.publicacions
                         join b in db.libros on a.id equals b.id_conte
                         where a.id == id
                         select new
                         {
                             titulo = a.titulo,
                              descripcion = a.descripcion,
                              portada = a.portada,
                              autor = b.autor,
                              año = b.año,
                              idioma = b.idioma_tamaño,
                              contenido = a.contenido,
                              
                         }
                         );

            ViewBag.observaciones = (from i in db.publicacions where i.id == id select i).ToArray()[0].observacion;
            ViewBag.id = id;
            ViewBag.titulo = perfil.ToArray()[0].titulo;
            ViewBag.descripcion = perfil.ToArray()[0].descripcion;
            ViewBag.portada = perfil.ToArray()[0].portada;
            ViewBag.autor = perfil.ToArray()[0].autor;
            ViewBag.año = perfil.ToArray()[0].año;
            ViewBag.idioma = perfil.ToArray()[0].idioma;
            ViewBag.contenido = perfil.ToArray()[0].contenido;



            return View();

        }
      [HttpPost, Authorize(Roles = "usuario")]
        public ActionResult reditarlibro(FormCollection f, HttpPostedFileBase portada, HttpPostedFileBase contenido)
        {
            claseslinqDataContext db = new claseslinqDataContext();
            Guid UserId = (from dt in db.aspnet_Users where dt.UserName == User.Identity.Name select dt.UserId).ToArray()[0];
            ViewBag.lista = db.categorias;
            int ID = Convert.ToInt32(f["ID"]);
            publicacion c = db.publicacions.Single(u => u.id == ID);
            libro l = db.libros.Single(u => u.id_conte == ID);
            if (f["titulo"] != null && f["titulo"] != "")
                c.titulo = f["titulo"];
            if (f["descripcion"] != null && f["descripcion"] != "")
                c.descripcion = f["descripcion"];

            if (portada != null)
            {
                var data2 = new byte[portada.ContentLength];
                portada.InputStream.Read(data2, 0, portada.ContentLength);
                var path = ControllerContext.HttpContext.Server.MapPath("/Content/imagenes/");
                var filename = Path.Combine(path, Path.GetFileName(portada.FileName));
                System.IO.File.WriteAllBytes(Path.Combine(path, filename), data2);
                c.portada = (portada.FileName).ToString();
            }
            if (f["autor"] != null && f["autor"] != "")
                l.autor = f["autor"];
            if (f["año"] != null && f["año"] != "")
                l.año = Convert.ToInt32(f["año"]);
            if (f["idioma"] != null && f["idioma"] != "")
                l.idioma_tamaño = f["idioma"];
            if (contenido != null)
            {
                var data1 = new byte[contenido.ContentLength];
                contenido.InputStream.Read(data1, 0, contenido.ContentLength);
                var path1 = ControllerContext.HttpContext.Server.MapPath("/Content/libros/");
                var filename1 = Path.Combine(path1, Path.GetFileName(contenido.FileName));
                System.IO.File.WriteAllBytes(Path.Combine(path1, filename1), data1);
                c.contenido = (contenido.FileName).ToString();
            }
            c.estado = "false";
            db.SubmitChanges();
            return Redirect("/Home/log/");

        }
        
    }
}
