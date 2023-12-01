using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClinicaWeb
{
    public partial class Especialidades : System.Web.UI.Page
    {
        public List<Dominio.Especialidad> lstEspecialidades { get; set; }
        public List<string> listErrores { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogueado"] == null) Response.Redirect("Login.aspx", false);
            if (!IsPostBack)
            {
                EspecialidadNegocio espNegocio = new EspecialidadNegocio();
                lstEspecialidades = espNegocio.Listar();
                Session["listaEspecialidades"] = lstEspecialidades;
                dgvEspecialidades.DataSource = lstEspecialidades;
                dgvEspecialidades.DataBind();

            }

        }

        protected void dgvEspecialidades_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                EspecialidadNegocio espNegocio = new EspecialidadNegocio();


                List<Dominio.Especialidad> lstEspecialidades = espNegocio.Listar();
                //List<Dominio.Especialidad> listaHorariosFiltrada;

                //listaHorariosFiltrada = (List<Modelo.Horario>)Session["listaHorariosFiltrada"];
                //if (listaHorariosFiltrada is null)
                //{
                dgvEspecialidades.PageIndex = e.NewPageIndex;
                dgvEspecialidades.DataSource = lstEspecialidades;
                //}
                //else
                //{
                //    dgvHorarios.PageIndex = e.NewPageIndex;
                //    dgvHorarios.DataSource = listaHorariosFiltrada;
                //}
                dgvEspecialidades.DataBind();
            }
            catch (Exception ex)
            {

                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }

        }

        protected void dgvEspecialidades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                listErrores = new List<string>();
                int id = -1;
                if (dgvEspecialidades.PageIndex > 0 && e.CommandName == "Eliminar")
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    int adjustedIndex = index - (dgvEspecialidades.PageIndex * dgvEspecialidades.PageSize);
                    GridViewRow selectedRow = dgvEspecialidades.Rows[adjustedIndex];
                    TableCell contactName = selectedRow.Cells[0];
                    id = Convert.ToInt32(contactName.Text);
                }
                else
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow selectedRow = dgvEspecialidades.Rows[index];
                    TableCell contactName = selectedRow.Cells[0];
                    id = Convert.ToInt32(contactName.Text);
                }
                if (e.CommandName == "Modificar")
                {
                    Response.Redirect("frmEspecialidad.aspx?Id=" + id, false);
                }
                else if (e.CommandName == "Eliminar")
                {
                    EspecialidadNegocio espeNeg = new EspecialidadNegocio();
                    Dominio.Especialidad espe = espeNeg.Listar().FirstOrDefault(x => x.Id == id);
                    Validar(espe);

                    if (listErrores.Count == 0) espeNeg.Eliminar(espe);
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
            catch (Exception ex)
            {

                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }

        }

        private void Validar(Dominio.Especialidad especialidad)
        {
            MedicoNegocio medicoNegocio = new MedicoNegocio();
            TurnoNegocio turnoNegocio = new TurnoNegocio();
            foreach (var item in medicoNegocio.Listar())
            {
                if (item.listEspecialidades.Any(x => x.Id == especialidad.Id))
                {
                    listErrores.Add("La especialidad es utilizada por medicos existentes. Por favor verifique los medicos que pertenecen a dicha especialidad.");
                    break;
                }
            }

            foreach (var item in turnoNegocio.Listar().Where(x => x.estado.estado == "Programado"))
            {
                if (item.especialidad.Id == especialidad.Id)
                {
                    listErrores.Add("La especialidad es utilizada en algunos de los turnos por atender..");
                    break;
                }
            }
        }

        protected void tbxFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Especialidad> listaEspecialidades;
            List<Especialidad> listaEspecialidadesFiltrada;
            try
            {
                listaEspecialidades = (List<Especialidad>)Session["listaEspecialidades"];
                listaEspecialidadesFiltrada = listaEspecialidades.FindAll(especilidad => especilidad.especialidad.ToUpper().Contains(tbxFiltro.Text.ToUpper()));
                Session["listaEspecialidadesFiltrada"] = listaEspecialidadesFiltrada;
                dgvEspecialidades.DataSource = listaEspecialidadesFiltrada;
                dgvEspecialidades.DataBind();
            }
            catch (Exception excepcion)
            {
                Session.Add("Error", excepcion.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }
    }
}