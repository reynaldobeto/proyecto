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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
           //claseslinqDataContext x=new claseslinqDataContext();
           //ViewBag.lista = from i in x.vistas select i;
           
               
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            claseslinqDataContext x = new claseslinqDataContext();
            ViewBag.lista4 = from i in x.vista6s orderby i.fecha_publicacion descending where i.estado=="true" select i;
            ViewBag.lista3 = from i in x.vista5s orderby i.fecha_publicacion descending where i.estado == "true" select i;
            ViewBag.lista2 = from i in x.vista2s orderby i.fecha_publicacion descending where i.estado == "true" select i;
            ViewBag.lista1 = from i in x.vista4s orderby i.fecha_publicacion descending where i.estado == "true" select i;
            return View();

        }

        public ActionResult About()
        {
            return View();
        }
       
        [Authorize(Roles = "usuario , administrador")]
        public ActionResult log()
        {
            claseslinqDataContext db = new claseslinqDataContext();
            Guid id = (from dt in db.aspnet_Users where dt.UserName == User.Identity.Name select dt.UserId).ToArray()[0];
            var per = from x in db.aspnet_Memberships
                      join y in db.perfils on x.UserId equals y.UserId
                      where x.UserId == id
                      select new
                      {
                          nick = x.Email,
                          avatar = y.avatar,
                          nombre = y.nombre,
                          apellido = y.apellido,
                          intereses = y.intereses,
                          sexo = y.sexo,
                          pais = y.pais,
                          id = y.id,
                          puntaje = y.puntaje,


                      };
            ViewBag.avatar = per.ToArray()[0].avatar;
            ViewBag.nic = per.ToArray()[0].nick;
            ViewBag.nombre = per.ToArray()[0].nombre;
            ViewBag.apellido = per.ToArray()[0].apellido;
            ViewBag.intereses = per.ToArray()[0].intereses;
            ViewBag.sexo = per.ToArray()[0].sexo;
            ViewBag.pais = per.ToArray()[0].pais;
            ViewBag.id = per.ToArray()[0].id;
            ViewBag.puntaje = per.ToArray()[0].puntaje;
            ViewBag.clibro = (from x in db.publicacions join y in db.libros on x.id equals y.id_conte where x.UserId == id && x.estado == "true" select x).Count();
           // ViewBag.clibrop = (from x in db.publicacions join y in db.libros on x.id equals y.id_conte where x.UserId == id && x.estado == "observado" select x).Count();
            ViewBag.libro = (from x in db.publicacions join y in db.libros on x.id equals y.id_conte where x.UserId == id && x.estado=="true" select x).ToList();
            ViewBag.libro1 = (from x in db.publicacions join y in db.libros on x.id equals y.id_conte where x.UserId == id && x.estado == "observado" select x).ToList();
            ViewBag.libro2 = (from x in db.publicacions join y in db.libros on x.id equals y.id_conte where x.UserId == id && x.estado == "false" select x).ToList();

            ViewBag.carti = (from x in db.publicacions join y in db.articulos on x.id equals y.id_conte where x.UserId == id && x.estado == "true" select x).Count();
            // ViewBag.clibrop = (from x in db.publicacions join y in db.libros on x.id equals y.id_conte where x.UserId == id && x.estado == "observado" select x).Count();
            ViewBag.arti = (from x in db.publicacions join y in db.articulos on x.id equals y.id_conte where x.UserId == id && x.estado == "true" select x).ToList();
            ViewBag.arti1 = (from x in db.publicacions join y in db.articulos on x.id equals y.id_conte where x.UserId == id && x.estado == "observado" select x).ToList();
            ViewBag.arti2 = (from x in db.publicacions join y in db.articulos on x.id equals y.id_conte where x.UserId == id && x.estado == "false" select x).ToList();


            ViewBag.ctuto = (from x in db.publicacions join y in db.tutorials on x.id equals y.id_conte where x.UserId == id && x.estado == "true" select x).Count();
            // ViewBag.clibrop = (from x in db.publicacions join y in db.libros on x.id equals y.id_conte where x.UserId == id && x.estado == "observado" select x).Count();
            ViewBag.tuto = (from x in db.publicacions join y in db.tutorials on x.id equals y.id_conte where x.UserId == id && x.estado == "true" select x).ToList();
            ViewBag.tuto1 = (from x in db.publicacions join y in db.tutorials on x.id equals y.id_conte where x.UserId == id && x.estado == "observado" select x).ToList();
            ViewBag.tuto2 = (from x in db.publicacions join y in db.tutorials on x.id equals y.id_conte where x.UserId == id && x.estado == "false" select x).ToList();

            ViewBag.ccurso = (from x in db.publicacions join y in db.cursos on x.id equals y.id_conte where x.UserId == id && x.estado == "true" select x).Count();
            // ViewBag.clibrop = (from x in db.publicacions join y in db.libros on x.id equals y.id_conte where x.UserId == id && x.estado == "observado" select x).Count();
            ViewBag.curso = (from x in db.publicacions join y in db.cursos on x.id equals y.id_conte where x.UserId == id && x.estado == "true" select x).ToList();
            ViewBag.curso1 = (from x in db.publicacions join y in db.cursos on x.id equals y.id_conte where x.UserId == id && x.estado == "observado" select x).ToList();
            ViewBag.curso2 = (from x in db.publicacions join y in db.cursos on x.id equals y.id_conte where x.UserId == id && x.estado == "false" select x).ToList();
            
            // ViewBag.libro = (from x in db.publicacions where x.UserId==id && x.estado=="false" select x).ToList();
            //perfil p = new perfil();
            //claseslinqDataContext f = new claseslinqDataContext();
            //Guid id = (from dt in f.aspnet_Users where dt.UserName == User.Identity.Name select dt.UserId).ToArray()[0];
            //p = (from c in f.perfils where c.UserId == id select c).ToArray()[0];
            //ViewBag.list = p;
            ViewBag.publica = (from pu in db.publicacions where pu.UserId==id select pu).Count();
            ViewBag.cate = (from ca in db.categorias select ca).ToList();
            return View();    

        }
        [Authorize(Roles = "usuario")]
        public ActionResult buscar(FormCollection d)
        {
            perfil p = new perfil();
            claseslinqDataContext f = new claseslinqDataContext();
            Guid id = (from dt in f.aspnet_Users where dt.UserName == User.Identity.Name select dt.UserId).ToArray()[0];
            p = (from c in f.perfils where c.UserId == id select c).ToArray()[0];
            ViewBag.list = p;

            string criterio = d["criterio"];
            string filtro = d["porque"];
            ViewBag.dato = criterio;
            if (filtro == "*")
            {
                ViewBag.lista = (from a in f.publicacions where a.titulo.Contains(criterio) select a).ToList();
                //ViewBag.lista = (from x in f.publicacions where x.contenido.Contains(criterio) select x).ToList();
                ViewBag.con = (from x in f.publicacions where x.titulo.Contains(criterio) select x).Count();
            }


            return View();

        }
         [Authorize(Roles = "usuario")]
        public ActionResult actualizar( int ide) {
            //ViewBag.idee = ide;
            claseslinqDataContext a = new claseslinqDataContext();
            ViewBag.act = from act in a.perfils where act.id == ide select act;
            return View();
        
        }

         [HttpPost, Authorize(Roles = "usuario")]
         public ActionResult actualizar(FormCollection f, HttpPostedFileBase avatar)
         {
             claseslinqDataContext db = new claseslinqDataContext();
             int ID = Convert.ToInt32(f["ID"]);
             perfil p = db.perfils.Single(u => u.id == ID);
             if (f["nombre"] != null && f["nombre"] != "")
                 p.nombre = f["nombre"];
             if (f["apellido"] != null && f["apellido"] != "")
                 p.apellido = f["apellido"];
             if (f["pais"] != null && f["pais"] != "")
                 p.pais = f["pais"];
             if (f["intereses"] != null && f["intereses"] != "")
                 p.intereses = f["intereses"];
             if (avatar != null)
             {
                 var data = new byte[avatar.ContentLength];
                 avatar.InputStream.Read(data, 0, avatar.ContentLength);
                 var path = ControllerContext.HttpContext.Server.MapPath("/imagenes");
                 var filename = Path.Combine(path, Path.GetFileName(avatar.FileName));
                 System.IO.File.WriteAllBytes(Path.Combine(path, filename), data);
                 p.avatar = (avatar.FileName).ToString();
             }

             db.SubmitChanges();
             return Redirect("log");
            
         }

    }
}
