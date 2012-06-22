using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace proyecto.Models
{
    public class articulos
    {
        public string titulo { get; set; }
        public string fecha_publicacion { get; set; }
        public string descripcion { get; set; }
        public string portada { get; set; }
        public string estado { get; set; }
        //public string titulo { get; set; }
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string contenido { get; set; }
        public string categoria { get; set; }
        public Guid idusers { get; set; }

        claseslinqDataContext db = new claseslinqDataContext();
        public void publicacion(articulos arti) 
        {
            publicacion p = new publicacion()
            {

                titulo = arti.titulo,
                fecha_publicacion = DateTime.Now,
                descripcion = arti.descripcion,
                contenido = arti.contenido,
                portada = arti.portada,
                estado = "activo",
                UserId = arti.idusers,
            };
            db.publicacions.InsertOnSubmit(p);
            db.SubmitChanges();
       
        }
        public void articulo(articulos arti, int pub) 
        {
            articulo a = new articulo()
            {
                id_conte = pub,
            };
            db.articulos.InsertOnSubmit(a);
            db.SubmitChanges();
        }
     
        public void regcategor(articulos arti, int idpub, int idcate)
        {
            relcategoria rcar = new relcategoria()
            {
                id_conte = idpub,
                id_cat = idcate,
                
            };
            db.relcategorias.InsertOnSubmit(rcar);
            db.SubmitChanges();

        }


    }
}