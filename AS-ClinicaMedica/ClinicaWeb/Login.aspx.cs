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
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                PersonaNegocio usNeg = new PersonaNegocio();


                Persona persona = usNeg.Listar().Where(x => x.usuario != null).FirstOrDefault(x => x.usuario.Nombre.Trim() == txtUsuario.Text.Trim() && x.usuario.Contrasenia.Trim() == txtPassword.Text.Trim());

                if (persona != null)
                {
                    Session.Add("UsuarioLogueado", persona);
                    Response.Redirect("Turnos.aspx", false);
                }
                else lblError.Text = "Usuario/Contraseña incorrecta";
            }
            catch (Exception ex)
            {

                throw ex;
            }




            
        }
    }
}