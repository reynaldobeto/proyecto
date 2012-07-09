using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace proyecto.Models
{
    public class tutoriales
    {
        public string titulo { get; set; }
        public string fecha_publicacion { get; set; }
        public string descripcion { get; set; }
        public string portada { get; set; }
        public string estado { get; set; }
        //public string titulo { get; set; }
        //[UIHint("tinymce_jquery_full"), AllowHtml]
        public string contenido { get; set; }
        public string categoria { get; set; }
        public Guid idusers { get; set; }
        claseslinqDataContext db = new claseslinqDataContext();
        public void publicacion(tutoriales tuto)
        {
            publicacion p = new publicacion()
            {

                titulo = tuto.titulo,
                fecha_publicacion = DateTime.Now,
                descripcion = tuto.descripcion,
                contenido = tuto.contenido,
                portada = tuto.portada,
                estado = "false",
                UserId = tuto.idusers,
                observacion="ninguno",
            };
            db.publicacions.InsertOnSubmit(p);
            db.SubmitChanges();

        }
        public void tutorial(tutoriales tuto, int pub)
        {
            tutorial a = new tutorial()
            {
                id_conte = pub,
            };
            db.tutorials.InsertOnSubmit(a);
            db.SubmitChanges();
        }

        //public void regcategor(tutoriales tuto, int idpub, int idcate)
        //{
        //    relcategoria rcar = new relcategoria()
        //    {
        //        id_conte = idpub,
        //        id_cat = idcate,

        //    };
        //    db.relcategorias.InsertOnSubmit(rcar);
        //    db.SubmitChanges();

        //}

    }
}