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
    public partial class Reportes : System.Web.UI.Page
    {
        public List<Estado> listEstados { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogueado"] == null) Response.Redirect("Login.aspx", false);
            if (!IsPostBack)
            {
                if (((Dominio.Persona)Session["usuarioLogueado"]).usuario.perfil.Tipo != "Medico")
                {
                    EstadoNegocio estadoNegocio = new EstadoNegocio();
                    listEstados = estadoNegocio.listar();
                    var med = Session["medicoSeleccionado"];
                    if (Session["ddlMedicoSeleccionado"] == null) cargarGrillaSinFiltro(listEstados);
                    else cargarGrillaXmedico(listEstados, (string)Session["ddlMedicoSeleccionado"]);
                    MedicoNegocio medicoNegocio = new MedicoNegocio();
                    ddlMedicos.DataSource = medicoNegocio.Listar();
                    ddlMedicos.DataTextField = "datos";
                    ddlMedicos.DataValueField = "IdPersona";
                    ddlMedicos.DataBind();
                }
                else
                {
                    ddlMedicos.Visible = false;
                    btnAplicarFiltro.Visible = false;
                    EstadoNegocio estadoNegocio = new EstadoNegocio();
                    listEstados = estadoNegocio.listar();
                    int IdPersona = ((Dominio.Persona)Session["usuarioLogueado"]).IdPersona;
                    cargarGrillaXmedico(listEstados, IdPersona.ToString());
                }
            }

        }

        private string ObtenerBackground(int porcentaje)
        {
            if (porcentaje > 50) return "green-background";
            else if (porcentaje >= 30) return "yellow-background";
            else return "red-background";
        }

        protected void ddlMedicos_DataBound(object sender, EventArgs e)
        {
            ddlMedicos.Items.Insert(0, "--Todos los Medicos--");
        }

        protected void btnAplicarFiltro_Click(object sender, EventArgs e)
        {
            EstadoNegocio esNegocio = new EstadoNegocio();
            if (ddlMedicos.SelectedValue != "--Todos los Medicos--")
            {
                Session.Add("ddlMedicoSeleccionado", ddlMedicos.SelectedValue);

                cargarGrillaXmedico(esNegocio.listar(), (string)Session["ddlMedicoSeleccionado"]);
            }
            else cargarGrillaSinFiltro(esNegocio.listar());



        }

        public void cargarGrillaSinFiltro(List<Dominio.Estado> listEstados)
        {
            TurnoNegocio turnoNegocio = new TurnoNegocio();
            int totalTurnos = turnoNegocio.Listar().Count();
            foreach (Estado estado in listEstados)
            {
                TableRow fila = new TableRow();

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = estado.estado;

                TableCell celCantidad = new TableCell();
                celCantidad.Text = turnoNegocio.Listar().Where(x => x.estado.Id == estado.Id).Count().ToString();

                TableCell celPorcentaje = new TableCell();
                int cantTurnoXestado = (turnoNegocio.Listar().Where(x => x.estado.Id == estado.Id).Count());
                celPorcentaje.Text = ((cantTurnoXestado * 100) / totalTurnos).ToString() + " %";

                fila.Cells.Add(celDescripcion);
                fila.Cells.Add(celCantidad);
                fila.Cells.Add(celPorcentaje);

                tablaDinamica.Rows.Add(fila);
            }
            lblCantidadTurnos.Text = "Total de turnos: " + turnoNegocio.Listar().Count().ToString();

        }

        public void cargarGrillaXmedico(List<Dominio.Estado> lisEstados, string IdPersona)
        {
            TurnoNegocio turnoNegocio = new TurnoNegocio();
            int totalTurnos = turnoNegocio.Listar().Where(x => x.medico.IdPersona == int.Parse(IdPersona)).Count();
            foreach (Estado estado in lisEstados)
            {
                TableRow fila = new TableRow();

                TableCell celDescripcion = new TableCell();
                celDescripcion.Text = estado.estado;

                TableCell celCantidad = new TableCell();
                celCantidad.Text = turnoNegocio.Listar().Where(x => x.estado.Id == estado.Id && x.medico.IdPersona == int.Parse(IdPersona)).Count().ToString();

                TableCell celPorcentaje = new TableCell();
                int cantTurnoXestado = (turnoNegocio.Listar().Where(x => x.estado.Id == estado.Id && x.medico.IdPersona == int.Parse(IdPersona)).Count());
                if (totalTurnos == 0)
                {
                    celPorcentaje.Text = "0%";
                    celPorcentaje.CssClass = ObtenerBackground(0);
                }
                else
                {
                    celPorcentaje.Text = ((cantTurnoXestado * 100) / totalTurnos).ToString() + " %";
                    celPorcentaje.CssClass = ObtenerBackground(int.Parse(((cantTurnoXestado * 100) / totalTurnos).ToString()));
                }

                fila.Cells.Add(celDescripcion);
                fila.Cells.Add(celCantidad);
                fila.Cells.Add(celPorcentaje);

                tablaDinamica.Rows.Add(fila);
            }

            lblCantidadTurnos.Text = "Total de turnos: " + totalTurnos.ToString();
        }
    }
}