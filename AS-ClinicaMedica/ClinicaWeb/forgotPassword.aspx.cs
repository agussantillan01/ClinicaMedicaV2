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
    public partial class forgotPassword : System.Web.UI.Page
    {
        public bool emailCorrecto = false;
        public List<string> listErrores { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["IdPersona"] == null)
            {
                if (!IsPostBack)
                {
                    tituloPassword1.Visible = false;
                    tituloPassword2.Visible = false;
                    txtPassword1.Visible = false;
                    txtPassword2.Visible = false;
                    btnRenovarClave.Visible = false;
                }
            }
            else
            {
                if (!IsPostBack)
                {
                    txtEmail.Text = ObtenerMail(int.Parse(Request.QueryString["IdPersona"]));
                    txtEmail.ReadOnly = true;
                    tituloPassword1.Visible = true;
                    tituloPassword2.Visible = true;
                    txtPassword1.Visible = true;
                    txtPassword2.Visible = true;
                    btnRenovarClave.Visible = true;
                    btnEnviarMail.Visible = false;
                }
            }

        }

        protected void btnRenovarClave_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarContrasenias(txtPassword1.Text, txtPassword2.Text);
                if (listErrores.Count == 0)
                {
                    PersonaNegocio perNeg = new PersonaNegocio();
                    Persona user = perNeg.Listar().FirstOrDefault(x => x.Email.Trim().ToUpper() == txtEmail.Text.Trim().ToUpper());
                    UsuarioNegocio usNeg = new UsuarioNegocio();
                    usNeg.ModificarContrasenia(user.usuario.Id, txtPassword1.Text.Trim());
                    Response.Redirect("Login.aspx", false);
                }
                else lblErrores.Text = string.Join("<br />", listErrores);
            }
            catch (Exception ex)
            {

                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }



        }

        protected void btnEnviarMail_Click(object sender, EventArgs e)
        {
            try
            {
                PersonaNegocio perNeg = new PersonaNegocio();
                listErrores = new List<string>();
                var user = perNeg.Listar().FirstOrDefault(x => x.Email.Trim().ToUpper() == txtEmail.Text.Trim().ToUpper());
                if (user == null) listErrores.Add("No se encontró el email ingresado");
                else
                {
                    if (user.usuario == null)
                    {
                        listErrores.Add("El email ingresado no tiene acceso");
                    }

                }
                if (listErrores.Count == 0)
                {
                    Response.Redirect("Login.aspx", false);
                    EmailServices emailPaciente = new EmailServices();
                    string htmlCuerpoPaciente = generaHtmlRecuperarClave(user);
                    emailPaciente.EnvioTurnos(user.Email.Trim(), "Recuperar la password", htmlCuerpoPaciente);
                    emailPaciente.enviarEmail();

                }
                else lblErrores.Text = string.Join("<br />", listErrores);
            }
            catch (Exception ex)
            {

                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false); 
            }



        }

        private string generaHtmlRecuperarClave(Persona persona)
        {
            return "    <table width=\"100%\" cellpadding=\"10\">       <tr>           <td align=\"center\" style=\"background-color: #007BFF; color: #fff;\">                <h1>Recuperar la contraseña</h1>            </td>        </tr>        <tr>            <td>                <p>Estimado " + persona.Nombre + " " + persona.Apellido + ", para recuperar la clave persione <a href=\"https://localhost:44375/forgotPassword.aspx?IdPersona="+persona.IdPersona+"\"> Aquí </a></p>                                       <p>Saludos cordiales,</p>               <p>Clínica Médica</p>           </td>      </tr>   </table>";

        }
        private string ObtenerMail(int idPersona)
        {
            PersonaNegocio perNeg = new PersonaNegocio();
            return (perNeg.Listar().FirstOrDefault(x => x.IdPersona == idPersona)).Email;
        }
        private void ValidarContrasenias(string clave1, string clave2)
        {
            
            listErrores = new List<string>();
            listErrores.Clear();
            if (!(clave1.Equals(clave2, StringComparison.Ordinal)))
            {
                listErrores.Add("Las claves no coinciden...");
            }
            if (string.IsNullOrEmpty(clave1)) listErrores.Add("La contraseña ingresada es incorrecta... recuerde llenar los campos.");
            if (string.IsNullOrEmpty(clave2)) listErrores.Add("La contraseña ingresada es incorrecta... recuerde llenar los campos.");
        }
    }
}