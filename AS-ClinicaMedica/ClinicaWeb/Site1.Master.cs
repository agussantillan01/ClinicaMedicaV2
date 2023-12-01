using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClinicaWeb
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Dominio.Persona)Session["UsuarioLogueado"] != null)
            {
                PersonaNegocio pneg = new PersonaNegocio();
                Persona persona = pneg.Listar().Where(x => x.usuario != null).FirstOrDefault(x => x.usuario.Id == ((Dominio.Persona)Session["UsuarioLogueado"]).usuario.Id);
                lblUsuarioLogueado.InnerText = "Usuario : " + persona.datos;
                perfilLogueado.InnerText = "Perfil : " + persona.usuario.perfil.Tipo;

            }

        }

        public bool ValidarPerfilSuperior()
        {
            UsuarioNegocio usNeg = new UsuarioNegocio();
            var usuario = (Persona)Session["UsuarioLogueado"];

            if (usuario != null)
            {
                if (usuario.usuario.perfil.Tipo != "Medico" ) return true;
            }
            return false;
        }
        public bool ValidarPerfilAdmin()
        {
            UsuarioNegocio usNeg = new UsuarioNegocio();
            var usuario = (Persona)Session["UsuarioLogueado"];

            if (usuario != null)
            {
                if (usuario.usuario.perfil.Tipo == "Administrador") return true;
            }
            return false;
        }


        protected void btnCerrarSesion_Click1(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx", false);
            Session.Add("UsuarioLogueado", null);
            Session.Clear();
        }
    }
}