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
    public class moderacioncontenidosController : Controller
    {
        //
        // GET: /moderacioncontenidos/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult mostrar()
        {
            claseslinqDataContext db = new claseslinqDataContext();
            //ViewBag.mostrar=()
            //ViewBag.mostrar = (from mo in db.libros join l in db.publicacions on mo.id_conte equals l.id where mo.id_conte == l.id select new { mo.}).ToList();
           
            ViewBag.mostrar = (from mo in db.libros join l in db.publicacions on mo.id_conte equals l.id where mo.id_conte == l.id && l.estado=="false" select l).ToList();
            ViewBag.cont = (from mo in db.libros join l in db.publicacions on mo.id_conte equals l.id where mo.id_conte == l.id && l.estado == "false" select l).Count();

            return View();

        }
        public ActionResult detalle(int id)
        {
            claseslinqDataContext db = new claseslinqDataContext();
            var a = from i in db.publicacions
                    join j in db.libros on i.id equals j.id_conte
                    where i.id == id
                    select new
                    {
                        autor=j.autor,
                        titulo=i.titulo,
                        fecha=i.fecha_publicacion,
                        descripcion=i.descripcion,
                        contenido=i.contenido,
                        portada=i.portada,
                        idee=i.UserId,
                    };
            ViewBag.id = id;
            ViewBag.autor = a.ToArray()[0].autor;
            ViewBag.titulo = a.ToArray()[0].titulo;
            ViewBag.fecha = a.ToArray()[0].fecha;
            ViewBag.descripcion = a.ToArray()[0].descripcion;
            ViewBag.portada = a.ToArray()[0].portada;
            ViewBag.contenido = a.ToArray()[0].contenido;
            Guid ide1 = (Guid)a.ToArray()[0].idee;
            var y = (from i in db.perfils where i.UserId == ide1 select i).ToArray()[0];
            ViewBag.avatar = y.avatar;
            ViewBag.nombre = y.nombre;
            ViewBag.puntaje = y.puntaje;
            ViewBag.userid = ide1;


            //ViewBag.mostrar = from i in db.vista2s where i.id == id select i;
            return View();
        }
        public ActionResult corregir(int id)
        {
            using (claseslinqDataContext db = new claseslinqDataContext())
            {
                var corr = (from p in db.publicacions where p.id == id select p).Single();
                //var karma = (from k in db.karmas where k.UserId == corr.UserId select k).Single();
                var karma1 = (from k in db.perfils where k.UserId == corr.UserId select k).Single();
                corr.estado = "true";
                karma1.puntaje += 100;
                //karma.valorkarma += 100;
                db.SubmitChanges();

            
            }
            return Redirect("/moderacioncontenidos/mostrar/");
        }
        public ActionResult corregirarti(int id)
        {
            using (claseslinqDataContext db = new claseslinqDataContext())
            {
                var corr = (from p in db.publicacions where p.id == id select p).Single();
                //var karma = (from k in db.karmas where k.UserId == corr.UserId select k).Single();
                var karma1 = (from k in db.perfils where k.UserId == corr.UserId select k).Single();
                corr.estado = "true";
                karma1.puntaje += 10;
                //karma.valorkarma += 100;
                db.SubmitChanges();


            }

            return Redirect("/moderacioncontenidos/moderarticulo/");
        }
        public ActionResult corregirtuto(int id)
        {
            using (claseslinqDataContext db = new claseslinqDataContext())
            {
                var corr = (from p in db.publicacions where p.id == id select p).Single();
                //var karma = (from k in db.karmas where k.UserId == corr.UserId select k).Single();
                var karma1 = (from k in db.perfils where k.UserId == corr.UserId select k).Single();
                corr.estado = "true";
                karma1.puntaje += 50;
                //karma.valorkarma += 100;
                db.SubmitChanges();


            }

            return Redirect("/moderacioncontenidos/moderarticulo/");
        }
        public ActionResult borrar(int id)
        {

            return View();
        }
        public ActionResult moderarticulo()
        {
            claseslinqDataContext db = new claseslinqDataContext();
            ViewBag.arti = from x in db.articulos join y in db.publicacions on x.id_conte equals y.id where x.id_conte == y.id && y.estado=="false" select y;
            ViewBag.cont = (from x in db.articulos join y in db.publicacions on x.id_conte equals y.id where x.id_conte == y.id && y.estado == "false" select y).Count();
            return View();
        }
        public ActionResult detallearti(int id)
        {
            claseslinqDataContext db = new claseslinqDataContext();
            var a = from i in db.publicacions
                    join j in db.articulos on i.id equals j.id_conte
                    where i.id == id
                    select new
                    {
                       
                        titulo = i.titulo,
                        fecha = i.fecha_publicacion,
                        descripcion = i.descripcion,
                        contenido = i.contenido,
                        portada = i.portada,
                        idee = i.UserId,
                    };
            ViewBag.id = id;
            ViewBag.titulo = a.ToArray()[0].titulo;
            ViewBag.fecha = a.ToArray()[0].fecha;
            ViewBag.descripcion = a.ToArray()[0].descripcion;
            ViewBag.portada = a.ToArray()[0].portada;
            ViewBag.contenido = a.ToArray()[0].contenido;
            Guid ide1 = (Guid)a.ToArray()[0].idee;
            var y = (from i in db.perfils where i.UserId == ide1 select i).ToArray()[0];
            ViewBag.avatar = y.avatar;
            ViewBag.nombre = y.nombre;
            ViewBag.puntaje = y.puntaje;
            ViewBag.userid = ide1;

            return View();
        }
        public ActionResult moderartuto()
        {
            claseslinqDataContext db = new claseslinqDataContext();
            ViewBag.arti = from x in db.tutorials join y in db.publicacions on x.id_conte equals y.id where x.id_conte == y.id && y.estado == "false" select y;
            ViewBag.cont = (from x in db.tutorials join y in db.publicacions on x.id_conte equals y.id where x.id_conte == y.id && y.estado == "false" select y).Count();
            return View();
        }
        public ActionResult detalletuto(int id)
        {
            claseslinqDataContext db = new claseslinqDataContext();
            var a = from i in db.publicacions
                    join j in db.tutorials on i.id equals j.id_conte
                    where i.id == id
                    select new
                    {

                        titulo = i.titulo,
                        fecha = i.fecha_publicacion,
                        descripcion = i.descripcion,
                        contenido = i.contenido,
                        portada = i.portada,
                        idee = i.UserId,
                    };
            ViewBag.id = id;
            ViewBag.titulo = a.ToArray()[0].titulo;
            ViewBag.fecha = a.ToArray()[0].fecha;
            ViewBag.descripcion = a.ToArray()[0].descripcion;
            ViewBag.portada = a.ToArray()[0].portada;
            ViewBag.contenido = a.ToArray()[0].contenido;
            Guid ide1 = (Guid)a.ToArray()[0].idee;
            var y = (from i in db.perfils where i.UserId == ide1 select i).ToArray()[0];
            ViewBag.avatar = y.avatar;
            ViewBag.nombre = y.nombre;
            ViewBag.puntaje = y.puntaje;
            ViewBag.userid = ide1;
            return View();
        }
        public ActionResult moderarcurso()
        {
            claseslinqDataContext db = new claseslinqDataContext();
            ViewBag.arti = from x in db.cursos join y in db.publicacions on x.id_conte equals y.id where x.id_conte == y.id && y.estado == "false" select y;
            ViewBag.cont = (from x in db.cursos join y in db.publicacions on x.id_conte equals y.id where x.id_conte == y.id && y.estado == "false" select y).Count();
            return View();
        }
        public ActionResult detallecurso(int id)
        {
            claseslinqDataContext db = new claseslinqDataContext();
            var a = from i in db.publicacions
                    join j in db.cursos on i.id equals j.id_conte
                    where i.id == id
                    select new
                    {

                        titulo = i.titulo,
                        fecha = i.fecha_publicacion,
                        descripcion = i.descripcion,
                        contenido = i.contenido,
                        portada = i.portada,
                        idee = i.UserId,
                    };
            ViewBag.id = id;
            ViewBag.titulo = a.ToArray()[0].titulo;
            ViewBag.fecha = a.ToArray()[0].fecha;
            ViewBag.descripcion = a.ToArray()[0].descripcion;
            ViewBag.portada = a.ToArray()[0].portada;
            ViewBag.contenido = a.ToArray()[0].contenido;
            Guid ide1 = (Guid)a.ToArray()[0].idee;
            var y = (from i in db.perfils where i.UserId == ide1 select i).ToArray()[0];
            ViewBag.avatar = y.avatar;
            ViewBag.nombre = y.nombre;
            ViewBag.puntaje = y.puntaje;
            ViewBag.userid = ide1;
            return View();
        }
        public ActionResult corregircurso(int id)
        {
            using (claseslinqDataContext db = new claseslinqDataContext())
            {
                var corr = (from p in db.publicacions where p.id == id select p).Single();
                //var karma = (from k in db.karmas where k.UserId == corr.UserId select k).Single();
                var karma1 = (from k in db.perfils where k.UserId == corr.UserId select k).Single();
                corr.estado = "true";
                karma1.puntaje += 200;
                //karma.valorkarma += 100;
                db.SubmitChanges();


            }

            return Redirect("/moderacioncontenidos/moderarticulo/");
        }

    }
}
