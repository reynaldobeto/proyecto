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
        [Authorize(Roles = "usuario")]
        public ActionResult reditarcurso(int id)
        {
            claseslinqDataContext db = new claseslinqDataContext();
            var perfil = (from a in db.publicacions
                          join b in db.cursos on a.id equals b.id_conte
                          where a.id == id
                          select new
                          {
                              titulo = a.titulo,
                              descripcion = a.descripcion,
                              portada = a.portada,
                              contenido = a.contenido,

                          }
                         );

            ViewBag.observaciones = (from i in db.publicacions where i.id == id select i).ToArray()[0].observacion;
            ViewBag.id = id;
            ViewBag.titulo = perfil.ToArray()[0].titulo;
            ViewBag.descripcion = perfil.ToArray()[0].descripcion;
            ViewBag.portada = perfil.ToArray()[0].portada;
            ViewBag.contenido = perfil.ToArray()[0].contenido;
            return View();
        }
        [HttpPost, Authorize(Roles = "usuario")]
        public ActionResult reditarcurso(FormCollection f, HttpPostedFileBase portada)
        {
            claseslinqDataContext db = new claseslinqDataContext();
            Guid UserId = (from dt in db.aspnet_Users where dt.UserName == User.Identity.Name select dt.UserId).ToArray()[0];
            ViewBag.lista = db.categorias;
            int ID = Convert.ToInt32(f["ID"]);
            publicacion c = db.publicacions.Single(u => u.id == ID);

            if (f["titulo"] != null && f["titulo"] != "")
                c.titulo = f["titulo"];
            if (f["descripcion"] != null && f["descripcion"] != "")
                c.descripcion = f["descripcion"];

            if (portada != null)
            {
                var data2 = new byte[portada.ContentLength];
                portada.InputStream.Read(data2, 0, portada.ContentLength);
                var path = ControllerContext.HttpContext.Server.MapPath("/Content/imagenes2/");
                var filename = Path.Combine(path, Path.GetFileName(portada.FileName));
                System.IO.File.WriteAllBytes(Path.Combine(path, filename), data2);
                c.portada = (portada.FileName).ToString();
            }
            if (f["ckeditor"] != null && f["ckeditor"] != "")
                c.contenido = f["ckeditor"];


            c.estado = "false";

            db.SubmitChanges();
            return Redirect("/Home/log/");

        }
        public ActionResult Observar(FormCollection f)
        {
            claseslinqDataContext db = new claseslinqDataContext();
            int id = Convert.ToInt32(f["idcont"]);
            publicacion c = db.publicacions.Single(u => u.id == id);
            c.observacion = f["observacion"];
            c.estado = "observado";
            db.SubmitChanges();
            return RedirectToAction("moderarcurso", "moderacioncontenidos");
        }
        public ActionResult nomegusta(int id)
        {
            claseslinqDataContext ca = new claseslinqDataContext();

            Guid UserId = (from dt in ca.aspnet_Users where dt.UserName == User.Identity.Name select dt.UserId).ToArray()[0];
            var lista = (from i in ca.karmas where i.id_cont == id && i.UserId == UserId && i.tipo == "me gusta" select i);
            foreach (var i in lista)
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


            return Redirect("/cursos/detallecurso/" + id);
        }
        [Authorize(Roles = "usuario")]
        [HttpPost]
        public ActionResult comentariocurso(string contenido, int id_conte)
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
            return Redirect("/cursos/detallecurso/" + id);



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


            return Redirect("/cursos/detallecurso/" + id);
        }
        public ActionResult mostrarcursos() 
        {
            claseslinqDataContext x = new claseslinqDataContext();
            ViewBag.lista1 = from i in x.vista6s orderby i.fecha_publicacion descending where i.estado=="true" select i;
            return View();

        }
        [Authorize(Roles = "usuario")]
        public ActionResult detallecurso( int id)
        {
            claseslinqDataContext x = new claseslinqDataContext();
            Guid idusuario = (from a in x.aspnet_Users where a.UserName == User.Identity.Name select a.UserId).ToArray()[0];
            ViewBag.con = (from i in x.karmas where i.id_cont == id && i.UserId == idusuario && i.tipo=="me gusta" select i).Count();

            ViewBag.lista6 = from i in x.vista6s where i.id == id select i;
            int flag = (from i in x.conteos where i.tipo == "me gusta" && i.id_cont == id select i).Count();
            if (flag == 0)
                ViewBag.contador = 0;
            else
                ViewBag.contador = (from i in x.conteos where i.tipo == "me gusta" && i.id_cont == id select i).ToArray()[0].cantidad;
            flag = (from i in x.conteos where i.tipo == "comentario" && i.id_cont == id select i).Count();
            if (flag == 0)
                ViewBag.contadorcomen = 0;
            else
                ViewBag.contadorcomen = (from i in x.conteos where i.tipo == "comentario" && i.id_cont == id select i).ToArray()[0].cantidad;


            ViewBag.comentarios = (from i in x.comentars where i.id_cont == id orderby i.fecha descending select i).ToList();

            return View();
            
          
        }

        [Authorize(Roles = "usuario")]
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
                        categoria a = new categoria() { tipo = item };

                        c.categorias.InsertOnSubmit(a);     //listacategoria.Add(new categoria() { tipo = item, });
                        c.SubmitChanges();
                    }
                    int id = (from i in c.categorias where i.tipo == item select i).ToArray()[0].idcat;
                    int idcont = c.publicacions.Where(m => m.titulo == cur.titulo && cur.fecha_publicacion == cur.fecha_publicacion).Select(m => m.id).ToArray()[0];
                    //int idcont = c.publicacions.Where(m => m.titulo == arti.titulo).Select(m => m.id).ToArray()[0];
                    relcategoria r = new relcategoria() { id_cat = id, id_conte = idcont };
                    c.relcategorias.InsertOnSubmit(r);
                    c.SubmitChanges();
                }
                //string[] arraycategorias = cur.categoria.ToLower().Split(',');
                //List<categoria> listacategoria = new List<categoria>();

                //foreach (var items in arraycategorias)
                //{
                //    string item = items.Trim();
                //    if (c.categorias.Where(a => a.tipo == item).Count() == 0)
                //    {
                //        listacategoria.Add(new categoria() { tipo = item, });
                //    }
                //}
                //if (listacategoria.ToList().Count > 0)
                //{
                //    c.categorias.InsertAllOnSubmit(listacategoria);
                //    c.SubmitChanges();
                //}

                //int idpub = c.publicacions.Where(m => m.titulo == cur.titulo && cur.fecha_publicacion == cur.fecha_publicacion).Select(m => m.id).ToArray()[0];
                //int idcate = c.categorias.Where(m => m.tipo == cur.categoria).Select(m => m.idcat).ToArray()[0];
                //cur.regcategor(cur, idpub, idcate);

            }
            return RedirectToAction("mostrarcursos");

        }
    }
}
