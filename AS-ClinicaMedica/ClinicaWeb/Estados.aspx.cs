using DocumentFormat.OpenXml.Office2010.Excel;
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
    public partial class Estados : System.Web.UI.Page
    {
        public List<Dominio.Estado> lstEstado { get; set; }
        public List<string> listErroes { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogueado"] == null) Response.Redirect("Login.aspx", false);
            EstadoNegocio estadoNegocio = new EstadoNegocio();
            try
            {
                if (!IsPostBack)
                {
                    lstEstado = estadoNegocio.listar();
                    Session["listaEstados"] = lstEstado;
                    dgvEstados.DataSource = lstEstado;
                    dgvEstados.DataBind();
                }
                



            }
            catch (Exception ex)
            {

                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void dgvEstados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                listErroes = new List<string>();
                int id = -1;
                if (dgvEstados.PageIndex > 0 && e.CommandName == "Eliminar")
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    int adjustedIndex = index - (dgvEstados.PageIndex * dgvEstados.PageSize);
                    GridViewRow selectedRow = dgvEstados.Rows[adjustedIndex];
                    TableCell contactName = selectedRow.Cells[0];
                    id = Convert.ToInt32(contactName.Text);
                }
                else
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow selectedRow = dgvEstados.Rows[index];
                    TableCell contactName = selectedRow.Cells[0];
                    id = Convert.ToInt32(contactName.Text);
                }
                if (e.CommandName == "Modificar")
                {
                    Response.Redirect("frmEstado.aspx?Id=" + id, false);
                }
                else if (e.CommandName == "Eliminar")
                {
                    EstadoNegocio esNeg = new EstadoNegocio();
                    Dominio.Estado estado = esNeg.listar().FirstOrDefault(x => x.Id == id);
                    Validar(estado);
                    if (listErroes.Count == 0) esNeg.Eliminar(estado);

                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowErrorModal", "showErrorModal();", true);
                        foreach (string error in listErroes)
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

        protected void dgvEstados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                EstadoNegocio estadoNeg = new EstadoNegocio();


                List<Dominio.Estado> lstEstados = estadoNeg.listar();
                //List<Dominio.Estado> listaEstadosFiltrada;

                //listaHorariosFiltrada = (List<Modelo.Horario>)Session["listaHorariosFiltrada"];
                //if (listaHorariosFiltrada is null)
                //{
                dgvEstados.PageIndex = e.NewPageIndex;
                dgvEstados.DataSource = lstEstados;
                //}
                //else
                //{
                //    dgvHorarios.PageIndex = e.NewPageIndex;
                //    dgvHorarios.DataSource = listaHorariosFiltrada;
                //}
                dgvEstados.DataBind();
            }
            catch (Exception ex)
            {

                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }

        }


        protected void tbxFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Estado> listaEstados;
            List<Estado> listaEstadosFiltrada;
            try
            {
                listaEstados = (List<Estado>)Session["listaEstados"];
                listaEstadosFiltrada = listaEstados.FindAll(estado => estado.estado.ToUpper().Contains(tbxFiltro.Text.ToUpper()));
                Session["listaEstadosFiltrada"] = listaEstadosFiltrada;
                dgvEstados.DataSource = listaEstadosFiltrada;
                dgvEstados.DataBind();
            }
            catch (Exception excepcion)
            {
                Session.Add("Error", excepcion.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        private void Validar(Estado es)
        {

            TurnoNegocio turNeg = new TurnoNegocio();
            List<Dominio.Turno> tur = turNeg.Listar();
            bool existe = false;
            foreach (var item in turNeg.Listar())
            {
                if (item.estado.Id == es.Id) existe = true;
            }
            if (existe) listErroes.Add("El estado seleccionado se utiliza en un turno");

        }
    }
}