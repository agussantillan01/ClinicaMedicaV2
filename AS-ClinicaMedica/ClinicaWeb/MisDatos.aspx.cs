using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClinicaWeb
{
    public partial class MisDatos : System.Web.UI.Page
    {
        public bool Editable = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogueado"] == null) Response.Redirect("Login.aspx", false);
            if (chkView.Checked == true) Editable = false;
            else Editable = true;
            Session.Add("editable", Editable);
            if (!(bool)Session["editable"])
            {
                
                txtApellido.ReadOnly = true;
                txtNombre.ReadOnly = true;
                txtDni.ReadOnly = true;
                chkHombre.Enabled = false;
                chkMujer.Enabled = false;
                txtEmail.ReadOnly = true;
                txtNombreUsuario.ReadOnly = true;
                txtPasswordActual.ReadOnly = true;
                txtPasswordNueva1.ReadOnly = true;
                txtPasswordNueva2.ReadOnly = true;
                btnModificarUsuario.Visible = false;
                btnModificarDatosPersonales.Visible = false;
            }
            else
            {
                txtApellido.ReadOnly = false;
                txtNombre.ReadOnly = false;
                txtEmail.ReadOnly = false;
                txtDni.ReadOnly = false;
                chkHombre.Enabled = true;
                chkMujer.Enabled = true;
                txtNombreUsuario.ReadOnly = false;
                txtPasswordActual.ReadOnly = false;
                txtPasswordNueva1.ReadOnly = false;
                txtPasswordNueva2.ReadOnly = false;
                btnModificarUsuario.Visible = true;
                btnModificarDatosPersonales.Visible = true;
            }
            if (!IsPostBack)
            {
                Session.Add("visibilidadDatosUsuarios", true);
                var persona = (Dominio.Persona)Session["UsuarioLogueado"];
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
                txtNombreUsuario.Text = persona.usuario.Nombre;
            }
            



        }

        protected void btnModificarDatosPersonales_Click(object sender, EventArgs e)
        {
            try
            {
                Persona persona = (Persona)Session["usuarioLogueado"];
                persona.Nombre = txtNombre.Text;
                persona.Apellido = txtApellido.Text;
                persona.DNI = txtDni.Text;
                persona.Email = txtEmail.Text;
                List<string> listErrores = new List<string>();

                listErrores = validarDatosPersonales(persona, listErrores);
                if (listErrores.Count == 0)
                {
                    PersonaNegocio perNeg = new PersonaNegocio();
                    perNeg.Actualizar(persona);
                    string script = "alert('Actualizado correctamente'); window.location.href = 'Turnos.aspx';";
                    ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", script, true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowErrorModal", "showErrorModal();", true);
                    foreach (string error in listErrores)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "AddErrorToList", "addErrorToList('" + error + "');", true);
                    }
                    //var persona1 = (Dominio.Persona)Session["UsuarioLogueado"];
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
                    txtNombreUsuario.Text = persona.usuario.Nombre;
                }
            }
            catch (Exception excepcion)
            {

                Session.Add("Error", excepcion.ToString());
                Response.Redirect("Error.aspx", false);
            }



        }

        private List<string> validarDatosPersonales(Persona persona, List<string> listErrores)
        {
            if (string.IsNullOrEmpty(persona.Nombre)) listErrores.Add("Recuerde ingresar un nombre");
            if (string.IsNullOrEmpty(persona.Apellido)) listErrores.Add("Recuerde ingresar un Apellido");
            if (string.IsNullOrEmpty(persona.DNI)) listErrores.Add("Recuerde ingresar un DNI");
            if (string.IsNullOrEmpty(persona.Email)) listErrores.Add("Recuerde ingresar un Email");

            PersonaNegocio perNeg = new PersonaNegocio();
            var existeUser = perNeg.Listar().Where(x => x.IdPersona != persona.IdPersona).FirstOrDefault(x => x.DNI == persona.DNI);
            if(existeUser != null) listErrores.Add("El Dni ingresado ya existe en la base de datos....");
             existeUser = perNeg.Listar().Where(x => x.IdPersona != persona.IdPersona).FirstOrDefault(x => x.Email == persona.Email);
            if (existeUser != null) listErrores.Add("El Email ingresado ya existe en la base de datos....");
            return listErrores;
        }
        protected void btnRound_Click(object sender, EventArgs e)
        {

            if ((bool)Session["visibilidadDatosUsuarios"])
            {
                Session.Add("visibilidadDatosUsuarios", false);
            }
            else
            {
                Session.Add("visibilidadDatosUsuarios", true);
            }

            if (!(bool)Session["visibilidadDatosUsuarios"])
            {
                visibilidadUsuario.Style["display"] = "block";
            }else
            {
                visibilidadUsuario.Style["display"] = "none";
            }
           
        }

        protected void btnModificarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> listErrores = new List<string>();
                var personaLogueada = (Persona)Session["UsuarioLogueado"];
                listErrores = validarUsuario(txtNombreUsuario.Text, txtPasswordActual.Text, txtPasswordNueva1.Text, txtPasswordNueva2.Text, personaLogueada, listErrores);
                if (listErrores.Count == 0)
                {
                    Usuario usuario = new Usuario
                    {
                        Nombre = txtNombreUsuario.Text,
                        Contrasenia = txtPasswordNueva1.Text
                    };
                    UsuarioNegocio usNeg = new UsuarioNegocio();
                    usNeg.Actualizar(usuario, personaLogueada.usuario.Id);
                    string script = "alert('Actualizado correctamente'); window.location.href = 'Turnos.aspx';";
                    ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", script, true);
                }
                else
                {
                    lblErrores.Text = listErrores.First();
                }
            }
            catch (Exception excepcion)
            {


                Session.Add("Error", excepcion.ToString());
                Response.Redirect("Error.aspx", false);
            }

        
        }
        private List<string> validarUsuario(string user, string passActual, string passNueva1, string passNueva2, Persona personaLogueada, List<string> listErrores)
        {
            PersonaNegocio perNeg = new PersonaNegocio();
            var existeUser = perNeg.Listar().Where(x=> x.IdPersona != personaLogueada.IdPersona && x.usuario != null).FirstOrDefault(x=> x.usuario.Nombre == user);
            if (existeUser != null) listErrores.Add("Ya existe el nombre de usuario ingresado");

            if (passActual.Trim() != personaLogueada.usuario.Contrasenia.Trim()) listErrores.Add("La clave actual no coincide con la ingresada");

            if (!(passNueva1.Equals(passNueva2, StringComparison.Ordinal)))
            {
                listErrores.Add("Las claves nuevas no coinciden...");
            }

            return listErrores;
        }
    }
    
}