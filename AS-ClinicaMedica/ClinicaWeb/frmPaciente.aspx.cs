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
    public partial class frmPaciente : System.Web.UI.Page
    {

        #region METODOS ASPX
        public List<string> lstErrores = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogueado"] == null) Response.Redirect("Login.aspx", false);
            PacienteNegocio pacNeg = new PacienteNegocio();
            string headingText;
            if (!IsPostBack)
            {
                if (Request.QueryString["Id"] != null)
                {
                    headingText = "<h1>Modificacion Paciente</h1>";
                    Paciente pac = pacNeg.ListarPacientes().FirstOrDefault(x => x.IdPaciente == int.Parse(Request.QueryString["Id"]));
                    txtApellido.Text = pac.Apellido;
                    txtNombre.Text = pac.Nombre;
                    txtDni.Text = pac.DNI;
                    txtEmail.Text = pac.Email;
                    txtFechaNacimiento.Text = (pac.FechaNacimiento).ToString("yyyy-MM-dd");
                    txtTelefono.Text = pac.Contacto;
                    txtDireccion.Text = pac.Direccion;



                }
                else
                {
                    headingText = "<h1>Nuevo Paciente</h1>";

                }
                ltlHeading.Text = headingText;
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            lstErrores = new List<string>();
            try
            {
                PersonaNegocio pNeg = new PersonaNegocio();
                PacienteNegocio pacNeg = new PacienteNegocio();
                Persona persona;
                Paciente paciente;

                if (Request.QueryString["Id"] == null)
                {
                    
                    persona = new Persona();
                    persona = cargaObjetoPersona(txtNombre.Text, txtApellido.Text, txtDni.Text, ObtenerSexo(chkHombre, chkMujer), txtEmail.Text);
                    ValidarPersona(persona);
                    if (persona != null && lstErrores.Count == 0)
                    {
                        pNeg.Agregar(persona);
                        paciente = cargarObjetoPaciente(ObtenerPorDni(persona.DNI), txtFechaNacimiento.Text, txtDireccion.Text, txtTelefono.Text);
                        if (paciente != null) pacNeg.Agregar(paciente);
                        string script = "alert('Agregado correctamente'); window.location.href = 'Pacientes.aspx';";
                        ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", script, true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowErrorModal", "showErrorModal();", true);
                        foreach (string error in lstErrores)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "AddErrorToList", "addErrorToList('" + error + "');", true);
                        }
                    }

                }
                else
                {
                    paciente = pacNeg.ListarPacientes().FirstOrDefault(x => x.IdPaciente == int.Parse(Request.QueryString["Id"]));
                    paciente.FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text);
                    paciente.Direccion = txtDireccion.Text;
                    paciente.Contacto = txtTelefono.Text;

                    paciente.Nombre = txtNombre.Text;
                    paciente.Apellido = txtApellido.Text;
                    paciente.DNI = txtDni.Text;
                    paciente.Email = txtEmail.Text;
                    paciente.Sexo = ObtenerSexo(chkHombre, chkMujer);

                    ValidarPaciente(paciente);

                    if (lstErrores.Count == 0)
                    {

                        pacNeg.Actualizar(paciente);
                        string script = "alert('Actualizado correctamente'); window.location.href = 'Pacientes.aspx';";
                        ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", script, true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowErrorModal", "showErrorModal();", true);
                        foreach (string error in lstErrores)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "AddErrorToList", "addErrorToList('" + error + "');", true);
                        }
                    }




                }
            }
            catch (Exception ex)
            {

                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }

        }
    

        #endregion

        #region FUNCIONES PRIVADAS

        private void ValidarPaciente (Paciente pac)
        {
            //validar 
            //lstErrores.Add("Hubo un error che....");
        }
        private void ValidarPersona(Persona per)
        {
            if (string.IsNullOrEmpty(per.Nombre)) lstErrores.Add("Debe ingresar un nombre");
            if (string.IsNullOrEmpty(per.Apellido)) lstErrores.Add("Debe ingresar un Apellido");
            if (string.IsNullOrEmpty(per.DNI)) lstErrores.Add("Debe ingresar un dni");
            if (string.IsNullOrEmpty(per.Email)) lstErrores.Add("Debe ingresar un email");

            PersonaNegocio perNeg = new PersonaNegocio();
            var persona = perNeg.Listar().FirstOrDefault(x=> x.DNI == per.DNI);
            if (persona != null) lstErrores.Add("Ya existe una persona con ese Dni");

        }
        private Persona cargaObjetoPersona(string nombre, string apellido, string dni, string sexo, string email)
        {
            
            Persona pe = new Persona
            {
                Nombre = nombre,
                Apellido = apellido,
                DNI = dni,
                Sexo = sexo,
                Email = email.Trim()
            };

            return pe;

        }
        private Persona ObtenerPorDni(string dni)
        {
            PersonaNegocio pNeg = new PersonaNegocio();
            return pNeg.Listar().FirstOrDefault(x => x.DNI.Trim() == dni.Trim());
        }

        private Paciente cargarObjetoPaciente(Persona per, string fechaNacimiento, string direccion, string telefono)
        {

            //validar
            Paciente paciente = new Paciente
            {
                FechaNacimiento = DateTime.Parse(fechaNacimiento),
                Direccion = direccion,
                Contacto = telefono,
                IdPersona = per.IdPersona

            };
            return paciente;
        }
        private string ObtenerSexo(RadioButton chkHombre, RadioButton chkMujer)
        {
            if (chkHombre.Checked) return "H";
            else return "M";

        }
        #endregion

    }
}