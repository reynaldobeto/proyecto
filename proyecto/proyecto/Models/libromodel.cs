using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;


namespace proyecto.Models
{
    public class libromodel
    {
        public string titulo { get; set; }
        public string fecha_publicacion { get; set; }
        public string descripcion { get; set; }
        public string contenido { get; set; }
        public string portada { get; set; }
        public string estado { get; set; }
       public string autor { get; set; }
       public int año { get; set; }
       public string categoria { get; set; }
       public string idioma { get; set; }
       public string tamaño { get; set; }
       public Guid idusers { get; set; }
       claseslinqDataContext db = new claseslinqDataContext();
       public void publicacion(libromodel modelo)
       {
           publicacion pub = new publicacion()
           {

               titulo = modelo.titulo,
               fecha_publicacion = DateTime.Now,
               descripcion = modelo.descripcion,
               contenido = modelo.contenido,
               portada = modelo.portada,
               estado = "false",
               UserId = modelo.idusers,
               observacion = "ninguno",

           };
           db.publicacions.InsertOnSubmit(pub);
           db.SubmitChanges();
       
       }
       publicacion pub = new publicacion();
       public void libro(libromodel modelo, int pub)
       {
           

           libro lib = new libro()
           {

               autor = modelo.autor,
               año = modelo.año,
               idioma_tamaño = modelo.idioma,
               tamaño = modelo.tamaño + "bytes",
               id_conte = pub,
           };
           db.libros.InsertOnSubmit(lib);


           db.SubmitChanges();
       }
       //public void rcategoria(libromodel modelo)
       //{
       //    categoria cat = new categoria()
       //    {
       //        tipo = modelo.categoria,
       //    };
       //    db.categorias.InsertOnSubmit(cat);
       //    db.SubmitChanges();
       //}
       //relcategoria rcar = new relcategoria();
       //public void regcategoria(libromodel modelo, int idpub, int idcate)
       //{
       //    relcategoria rcar = new relcategoria() 
       //    { 
       //        id_cat= idcate,
       //        id_conte=idpub,
       //    };
       //    db.relcategorias.InsertOnSubmit(rcar);
       //    db.SubmitChanges();
        
       //}

    }
     
}