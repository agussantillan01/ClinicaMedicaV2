using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClinicaWeb
{
    public partial class Usuarios : System.Web.UI.Page
    {
        public List<Dominio.Persona> listaPersonas { get; set; }
        public List<string> listErrores { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogueado"] == null) Response.Redirect("Login.aspx", false);
            if (!IsPostBack)
            {
                PersonaNegocio pacNeg = new PersonaNegocio();
                listaPersonas = pacNeg.Listar();
                listaPersonas.RemoveAll(x => x.usuario == null);
                listaPersonas.RemoveAll(x => x.IdPersona == ((Dominio.Persona)Session["UsuarioLogueado"]).IdPersona || x.usuario.perfil.Tipo == "Medico");
                Session["listaUsuarios"] = listaPersonas;
                dgvUsuarios.DataSource = listaPersonas;
                dgvUsuarios.DataBind();
            }

        }


        protected void dgvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            listErrores = new List<string>();
            int id = -1;
            if (dgvUsuarios.PageIndex > 0 && e.CommandName == "Eliminar")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int adjustedIndex = index - (dgvUsuarios.PageIndex * dgvUsuarios.PageSize);
                GridViewRow selectedRow = dgvUsuarios.Rows[adjustedIndex];
                TableCell contactName = selectedRow.Cells[0];
                id = Convert.ToInt32(contactName.Text);
            }
            else
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = dgvUsuarios.Rows[index];
                TableCell contactName = selectedRow.Cells[0];
                id = Convert.ToInt32(contactName.Text);
            }
            if (e.CommandName == "Modificar")
            {
                Response.Redirect("frmUsuario.aspx?Id=" + id, false);
            }
            else if (e.CommandName == "Eliminar")
            {
                PersonaNegocio personaneg = new PersonaNegocio();
                Dominio.Persona persona = personaneg.Listar().FirstOrDefault(x=> x.IdPersona == id);    

                Validar(persona);
                if (listErrores.Count == 0) {
                    personaneg.Eliminar(persona);
                    UsuarioNegocio usNeg = new UsuarioNegocio();
                    usNeg.Eliminar(persona.usuario);
                    
                    Response.Redirect("Usuarios.aspx", false);
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
        }

        private void Validar(Dominio.Persona persona)
        {
            listErrores = new List<string>();
            if (persona.usuario.perfil.Tipo != "Medico")
            {

            }
            else
            {
                listErrores.Add("Para eliminar un medico, debe ir a la solapa Médicos..");
            }
        }
        protected void dgvUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                PersonaNegocio pacNeg = new PersonaNegocio();
                listaPersonas = pacNeg.Listar();
                listaPersonas.RemoveAll(x => x.usuario == null);
                listaPersonas.RemoveAll(x => x.IdPersona == ((Dominio.Persona)Session["UsuarioLogueado"]).IdPersona || x.usuario.perfil.Tipo == "Medico");
                //List<Dominio.Especialidad> listaHorariosFiltrada;

                //listaHorariosFiltrada = (List<Modelo.Horario>)Session["listaHorariosFiltrada"];
                //if (listaHorariosFiltrada is null)
                //{
                dgvUsuarios.PageIndex = e.NewPageIndex;
                dgvUsuarios.DataSource = listaPersonas;
                //}
                //else
                //{
                //    dgvHorarios.PageIndex = e.NewPageIndex;
                //    dgvHorarios.DataSource = listaHorariosFiltrada;
                //}
                dgvUsuarios.DataBind();
            }
            catch (Exception ex)
            {

                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }


        protected void txtFiltroNombre_TextChanged(object sender, EventArgs e)
        {
            List<Dominio.Persona> listaUsuarios;
            List<Dominio.Persona> listaUsuariosFiltrada;
            try
            {
                listaUsuarios = (List<Dominio.Persona>)Session["listaUsuarios"];
                listaUsuariosFiltrada = filtrarNombre(listaUsuarios);
                if (tbxFiltroPerfil.Text.Length > 0)
                {
                    listaUsuariosFiltrada = filtrarPerfil(listaUsuariosFiltrada);
                }
                Session["listaUsuariosFiltrada"] = listaUsuariosFiltrada;
                dgvUsuarios.DataSource = listaUsuariosFiltrada;
                dgvUsuarios.DataBind();
            }
            catch (Exception excepcion)
            {
                Session.Add("pagOrigen", "Usuario.aspx");
                Session.Add("excepcion", excepcion);
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void txtFiltroApellido_TextChanged(object sender, EventArgs e)
        {
            List<Dominio.Persona> listaUsuarios;
            List<Dominio.Persona> listaUsuariosFiltrada;
            try
            {
                listaUsuarios = (List<Dominio.Persona>)Session["listaUsuarios"];
                listaUsuariosFiltrada = filtrarPerfil(listaUsuarios);
                if (tbxFiltroUsuario.Text.Length > 0)
                {
                    listaUsuariosFiltrada = filtrarNombre(listaUsuariosFiltrada);
                }
                Session["listaUsuariosFiltrada"] = listaUsuariosFiltrada;
                dgvUsuarios.DataSource = listaUsuariosFiltrada;
                dgvUsuarios.DataBind();
            }
            catch (Exception excepcion)
            {
                Session.Add("pagOrigen", "Usuario.aspx");
                Session.Add("excepcion", excepcion);
                Response.Redirect("Error.aspx", false);
            }
        }

        private List<Dominio.Persona> filtrarPerfil(List<Dominio.Persona> listaUsuarios)
        {
            List<Dominio.Persona> listaPerfiles;
            try
            {
                listaPerfiles = listaUsuarios.FindAll(usuario => usuario.usuario.perfil.Tipo.ToUpper().Contains(tbxFiltroPerfil.Text.ToUpper()));
                return listaPerfiles;
            }
            catch
            {
                return null;
            }

        }
        private List<Dominio.Persona> filtrarNombre(List<Dominio.Persona> listaUsuarios)
        {
            List<Dominio.Persona> listaNombres;
            try
            {
                listaNombres = listaUsuarios.FindAll(usuario => usuario.Nombre.ToUpper().Contains(tbxFiltroUsuario.Text.ToUpper()));
                return listaNombres;
            }
            catch
            {
                return null;
            }

        }
    }
}