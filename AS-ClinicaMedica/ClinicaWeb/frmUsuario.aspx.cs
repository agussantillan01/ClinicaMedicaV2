using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClinicaWeb
{
    public partial class frmUsuario : System.Web.UI.Page
    {
        public List<string> listErrores { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            string headingText;
            if (Session["UsuarioLogueado"] == null) Response.Redirect("Login.aspx", false);
            if (!IsPostBack)
            {
                TipoUsuarioNegocio tUserNegocio = new TipoUsuarioNegocio();
                List<TipoUsuario> lstTipoUser = tUserNegocio.ObtenerTipos().Where(x => x.Tipo.Trim() == "Administrador" || x.Tipo.Trim() == "Recepcionista").ToList();
                ddlTipoUsuario.DataSource = lstTipoUser;
                ddlTipoUsuario.DataTextField = "Tipo";
                ddlTipoUsuario.DataValueField = "Id";
                ddlTipoUsuario.DataBind();

                if (Request.QueryString["Id"] == null)
                {
                    headingText = "<h1>Nuevo Usuario</h1>";
                }
                else
                {
                    headingText = "<h1>Modificacion Usuario</h1>";
                    PersonaNegocio perNeg = new PersonaNegocio();
                    Persona persona = perNeg.Listar().FirstOrDefault(x => x.IdPersona == int.Parse(Request.QueryString["Id"]));
                    txtNombre.Text = persona.Nombre;
                    txtApellido.Text = persona.Apellido;
                    txtDni.Text = persona.DNI;
                    txtEmail.Text = persona.Email;
                    if (persona.Sexo == "M")
                    {
                        chkMujer.Checked = true;
                        chkHombre.Checked = false;
                    }
                    else
                    {
                        chkMujer.Checked = false;
                        chkHombre.Checked = true;
                    }
                    txtUsuario.Enabled = false;
                    //txtPassword1.Enabled = false;
                    //txtPassword2.Enabled = false;
                    ddlTipoUsuario.Enabled = false;

                }
                ltlHeading.Text = headingText;
            }
        }



        private void Validar(string nombre, string apellido, string Dni, string email, string sexo, string usuario, string tipouser)
        {
            try
            {
                listErrores = new List<string>();
                if (Request.QueryString["Id"] == null)
                {
                    if (string.IsNullOrEmpty(usuario)) listErrores.Add("Ingrese un nombre de Usuario.");

                    UsuarioNegocio usNeg = new UsuarioNegocio();
                    var usu = usNeg.listar().FirstOrDefault(x => x.Nombre == usuario);
                    if (usu != null) listErrores.Add("Ya existe ese nombre de usuario.");

                    PersonaNegocio perNeg = new PersonaNegocio();
                    var per = perNeg.Listar().FirstOrDefault(x => x.DNI == Dni);
                    if (per != null) listErrores.Add("Ya existen usuarios con el dni ingresado.");
                    per = perNeg.Listar().FirstOrDefault(x => x.Email == email);
                    if (per != null) listErrores.Add("Ya existen usuarios con el email ingresado.");
                }

                if (string.IsNullOrEmpty(nombre)) listErrores.Add("Ingrese un nombre.");
                if (string.IsNullOrEmpty(apellido)) listErrores.Add("Ingrese su apellido.");
                if (string.IsNullOrEmpty(Dni)) listErrores.Add("Ingrese su DNI.");
                if (string.IsNullOrEmpty(email))  listErrores.Add("Ingrese su Email.");
                    else if (!email.Contains("@")) listErrores.Add("El mail es incorrecto...");

                if (Request.QueryString["Id"] != null)
                {
                    PersonaNegocio perNeg = new PersonaNegocio();
                    var per = perNeg.Listar().FirstOrDefault(x => x.DNI == Dni && x.IdPersona != int.Parse(Request.QueryString["Id"]));
                    if (per != null) listErrores.Add("Ya existen usuarios con el dni ingresado.");
                    per = perNeg.Listar().FirstOrDefault(x => x.Email == email && x.IdPersona != int.Parse(Request.QueryString["Id"]));
                    if (per != null) listErrores.Add("Ya existen usuarios con el email ingresado.");
                }
            }
            catch (Exception ex)
            {


                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }




        }

        private string ObtenerSexo(RadioButton chkHombre, RadioButton chkMujer)
        {
            if (chkHombre.Checked) return "H";
            else return "M";

        }
        private Usuario obtenerUsuario(string user, string pass, int IdPerfil)
        {
            Usuario usuario = new Usuario();
            usuario.Nombre = user;
            usuario.Contrasenia = pass;
            usuario.perfil = obtenerPerfil(IdPerfil);
            return usuario;

        }
        private TipoUsuario obtenerPerfil(int id)
        {
            TipoUsuarioNegocio tipoUserNeg = new TipoUsuarioNegocio();
            TipoUsuario tipo = tipoUserNeg.ObtenerTipos().FirstOrDefault(x => x.Id == id);
            return tipo;
        }

        private void GuardarUsuario(Persona persona)
        {
            UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
            usuarioNegocio.Guardar(persona.usuario);
            persona.usuario.Id = usuarioNegocio.listar().Last().Id;
            PersonaNegocio personaNegocio = new PersonaNegocio();
            personaNegocio.Agregar(persona);

        }
        protected void ddlTipoUsuario_DataBound(object sender, EventArgs e)
        {

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Validar(txtNombre.Text, txtApellido.Text, txtDni.Text, txtEmail.Text, ObtenerSexo(chkHombre, chkMujer), txtUsuario.Text,ddlTipoUsuario.SelectedValue.ToString());
                if (listErrores.Count == 0)
                {
                    string password = GenerarPassword(txtEmail.Text.Trim());
                    Persona persona = new Persona();
                    if (Request.QueryString["Id"] == null)
                    {
                        persona.Nombre = txtNombre.Text;
                        persona.Apellido = txtApellido.Text;
                        persona.DNI = txtDni.Text;
                        persona.Email = txtEmail.Text;
                        persona.Sexo = ObtenerSexo(chkHombre, chkMujer);
                        persona.usuario = obtenerUsuario(txtUsuario.Text, password, int.Parse(ddlTipoUsuario.SelectedValue));
                        GuardarUsuario(persona);
                        EmailServices emailAvisoPassword = new EmailServices();
                        emailAvisoPassword.EnvioPasswordAsignada(persona.Email.Trim(), "Password", ObtenerCuerpoMailPasswordAsignada(persona.usuario.Contrasenia));
                        emailAvisoPassword.enviarEmail();
                        string script = "alert('Agregado correctamente'); window.location.href = 'Usuarios.aspx';";
                        ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", script, true);
                    }
                    else
                    {
                        persona.Nombre = txtNombre.Text;
                        persona.Apellido = txtApellido.Text;
                        persona.DNI = txtDni.Text;
                        persona.Email = txtEmail.Text;
                        persona.Sexo = ObtenerSexo(chkHombre, chkMujer);
                        persona.IdPersona = int.Parse(Request.QueryString["Id"]);
                        PersonaNegocio personaNegocio = new PersonaNegocio();
                        personaNegocio.Actualizar(persona);
                        string script = "alert('Actualizado correctamente'); window.location.href = 'Usuarios.aspx';";
                        ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", script, true);
                    }


                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowErrorModal", "showErrorModal();", true);
                    foreach (string error in listErrores)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "AddErrorToList", "addErrorToList('" + error + "');", true);
                    }
                }
            }
            catch (Exception ex)
            {

                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }



        }

        private string ObtenerCuerpoMailPasswordAsignada(string pass)
        {
            return "<!DOCTYPE html><html><head>    <title>Clínica Médica</title>    <style>        body {            background-color: #d0e7f9; /* Celeste */            font-family: Arial, sans-serif;           margin: 0;            padding: 20px;        }        .container {            max-width: 600px;            margin: 0 auto;            background-color: #fff;            padding: 20px;            border-radius: 8px;            box-shadow: 0 0 10px rgba(0,0,0,0.1);       }        h1 {            text-align: center;            color: #333;        }        .password {            text-align: center;            font-size: 24px;            margin-top: 20px;            color: #007bff; /* Azul */        }    </style></head><body>    <div class=\"container\">        <h1>Clínica Médica</h1>        <p>Su contraseña para iniciar sesión es: " + pass + "</p>           </div></body></html>";
        }
        private string GenerarPassword(string user)
        {
            string[] partes = user.Split('@'); // Dividir el correo en dos partes usando el '@'
            if (partes.Length == 2) // Verificar si hay exactamente una parte después del '@'
            {
                string dominio = partes[0]; // Obtener la parte del dominio
                string dominioDesordenado = DesordenarString(dominio); // Desordenar el dominio
                return dominioDesordenado;
            }
            return null;
        }
        private string DesordenarString(string texto)
        {
            Random rnd = new Random();
            char[] caracteres = texto.ToCharArray();

            int n = caracteres.Length;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                var valor = caracteres[k];
                caracteres[k] = caracteres[n];
                caracteres[n] = valor;
            }

            return new string(caracteres);
        }
    }
}