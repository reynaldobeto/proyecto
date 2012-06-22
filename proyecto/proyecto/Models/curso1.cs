using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace proyecto.Models
{
    public class curso1
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
        public void publicacion(curso1 cur)
        {
            publicacion p = new publicacion()
            {

                titulo = cur.titulo,
                fecha_publicacion = DateTime.Now,
                descripcion = cur.descripcion,
                contenido = cur.contenido,
                portada = cur.portada,
                estado = "activo",
                UserId = cur.idusers,
            };
            db.publicacions.InsertOnSubmit(p);
            db.SubmitChanges();

        }
        public void cursos(curso1 cur, int pub)
        {
            curso a = new curso()
            {
                id_conte = pub,
            };
            db.cursos.InsertOnSubmit(a);
            db.SubmitChanges();
        }

        public void regcategor(curso1 cur, int idpub, int idcate)
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