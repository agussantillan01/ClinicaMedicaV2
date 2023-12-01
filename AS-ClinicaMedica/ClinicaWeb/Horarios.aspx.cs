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
    public partial class Horarios : System.Web.UI.Page
    {
        public List<Dominio.Horario> lstHorarios { get; set; }
        public List<string> listErrores { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogueado"] == null) Response.Redirect("Login.aspx", false);
            HorarioNegocio hsNegocio = new HorarioNegocio(); 
            if (!IsPostBack)
            {
                lstHorarios = hsNegocio.Listar();
                Session["listaHorarios"] = lstHorarios;
                dgvHorarios.DataSource = lstHorarios;
                dgvHorarios.DataBind();
                
            }
        }

        protected void dgvHorarios_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {

        }

        protected void dgvHorarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                listErrores = new List<string>();
                int id = -1;
                if (dgvHorarios.PageIndex > 0 && e.CommandName == "Eliminar")
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    int adjustedIndex = index - (dgvHorarios.PageIndex * dgvHorarios.PageSize);
                    GridViewRow selectedRow = dgvHorarios.Rows[adjustedIndex];
                    TableCell contactName = selectedRow.Cells[0];
                    id = Convert.ToInt32(contactName.Text);
                }
                else
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow selectedRow = dgvHorarios.Rows[index];
                    TableCell contactName = selectedRow.Cells[0];
                    id = Convert.ToInt32(contactName.Text);
                }
                if (e.CommandName == "Modificar")
                {
                    Response.Redirect("frmHorario.aspx?Id=" + id, false);
                }
                else if (e.CommandName == "Eliminar")
                {
                    HorarioNegocio horaNegocio = new HorarioNegocio();
                    Dominio.Horario hora = horaNegocio.Listar().FirstOrDefault(x => x.Id == id);
                    Validar(hora);

                    if (listErrores.Count == 0)
                    {
                        horaNegocio.Eliminar(hora);
                        Session["listaHorarios"] = null;
                        Session["listaHorarios"] = horaNegocio.Listar();
                        dgvHorarios.DataSource = lstHorarios;
                        dgvHorarios.DataBind();
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
            catch (Exception ex)
            {

                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }

        }

        protected void dgvHorarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                HorarioNegocio hsNeg = new HorarioNegocio();


                List<Dominio.Horario> lstHorario = hsNeg.Listar();
                //List<Dominio.Especialidad> listaHorariosFiltrada;

                //listaHorariosFiltrada = (List<Modelo.Horario>)Session["listaHorariosFiltrada"];
                //if (listaHorariosFiltrada is null)
                //{
                dgvHorarios.PageIndex = e.NewPageIndex;
                dgvHorarios.DataSource = lstHorario;
                //}
                //else
                //{
                //    dgvHorarios.PageIndex = e.NewPageIndex;
                //    dgvHorarios.DataSource = listaHorariosFiltrada;
                //}
                dgvHorarios.DataBind();
            }
            catch (Exception ex)
            {

                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }

        }

        private void Validar (Horario hs)
        {
            TurnoNegocio turNeg = new TurnoNegocio();
            MedicoNegocio medNeg = new MedicoNegocio();

            bool existe = false;
            foreach (var item in medNeg.Listar())
            {
                if (item.listHorarios.Any(x => x.Id == hs.Id)) existe = true;
            }


            if (existe) listErrores.Add("El horario Seleccionado, contiene un medico trabajando...");

        }

        protected void tbxFiltroDia_TextChanged(object sender, EventArgs e)
        {
            List<Dominio.Horario> listaHorarios;
            List<Dominio.Horario> listaHorariosFiltrada;
            try
            {
                listaHorarios = (List<Dominio.Horario>)Session["listaHorarios"];
                listaHorariosFiltrada = filtrarDia(listaHorarios);
                if (tbxFiltroInicio.Text.Length > 0)
                {
                    listaHorariosFiltrada = filtrarInicio(listaHorariosFiltrada);
                }
                if (tbxFiltroFin.Text.Length > 0)
                {
                    listaHorariosFiltrada = filtrarFin(listaHorariosFiltrada);
                }
                Session["listaHorariosFiltrada"] = listaHorariosFiltrada;
                dgvHorarios.DataSource = listaHorariosFiltrada;
                dgvHorarios.DataBind();
            }
            catch (Exception excepcion)
            {
                Session.Add("pagOrigen", "Horarios.aspx");
                Session.Add("excepcion", excepcion);
                Response.Redirect("Error.aspx", false);
            }
        }

        private List<Dominio.Horario> filtrarDia(List<Dominio.Horario> listaHorarios)
        {
            List<Dominio.Horario> listaDias;
            try
            {
                listaDias = listaHorarios.FindAll(horario => horario.Dia.ToUpper().Contains(tbxFiltroDia.Text.ToUpper()));
                return listaDias;
            }
            catch
            {
                return null;
            }
        }

        private List<Dominio.Horario> filtrarInicio(List<Dominio.Horario> listaHorarios)
        {
            List<Dominio.Horario> listaInicios;
            try
            {
                listaInicios = listaHorarios.FindAll(horario => horario.HoraInicio.ToString().ToUpper().Contains(tbxFiltroInicio.Text.ToUpper()));
                return listaInicios;
            }
            catch
            {
                return null;
            }
        }

        private List<Dominio.Horario> filtrarFin(List<Dominio.Horario> listaHorarios)
        {
            List<Dominio.Horario> listaFines;
            try
            {
                listaFines = listaHorarios.FindAll(horario => horario.HoraFin.ToString().ToUpper().Contains(tbxFiltroFin.Text.ToUpper()));
                return listaFines;
            }
            catch
            {
                return null;
            }
        }
    }
}