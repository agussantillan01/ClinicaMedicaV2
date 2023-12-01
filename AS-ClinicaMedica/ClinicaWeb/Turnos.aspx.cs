using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Runtime.InteropServices;

namespace ClinicaWeb
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public List<Dominio.Turno> lstTurnos { get; set; }
        public List<string> listErrores { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogueado"] == null) Response.Redirect("Login.aspx", false);
            if (!IsPostBack)
            {
                TurnoNegocio turnoNegocio = new TurnoNegocio();
                if (((Dominio.Persona)Session["UsuarioLogueado"]).usuario.perfil.Tipo == "Medico")
                {
                    var idPer = ((Dominio.Persona)Session["UsuarioLogueado"]).IdPersona;
                    lstTurnos = turnoNegocio.Listar().Where(x=> x.medico.IdPersona == idPer).ToList();
                }else
                {
                    lstTurnos = turnoNegocio.Listar();
                }


                Session["listaTurnos"] = lstTurnos;
                dgvTurnos.DataSource = lstTurnos;
                dgvTurnos.DataBind();

            }
        }

        protected void dgvTurnos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                listErrores = new List<string>();
                int id = -1;
                if (dgvTurnos.PageIndex > 0 && e.CommandName == "Eliminar")
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    int adjustedIndex = index - (dgvTurnos.PageIndex * dgvTurnos.PageSize);
                    GridViewRow selectedRow = dgvTurnos.Rows[adjustedIndex];
                    TableCell contactName = selectedRow.Cells[0];
                    id = Convert.ToInt32(contactName.Text);
                }
                else
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow selectedRow = dgvTurnos.Rows[index];
                    TableCell contactName = selectedRow.Cells[0];
                    id = Convert.ToInt32(contactName.Text);
                }
                if (e.CommandName == "Modificar")
                {
                    Response.Redirect("frmTurno.aspx?Id=" + id, false);
                }
                //else if (e.CommandName == "Eliminar")
                //{
                //    TurnoNegocio turNeg = new TurnoNegocio();
                //    Dominio.Turno turno = turNeg.Listar().FirstOrDefault(x => x.Id == id);
                //    Validar(turno);
                //    if (listErrores.Count == 0)
                //    {
                //        turNeg.Eliminar(turno);
                //        if (((Dominio.Persona)Session["UsuarioLogueado"]).usuario.perfil.Tipo == "Medico")
                //        {
                //            var idPer = ((Dominio.Persona)Session["UsuarioLogueado"]).IdPersona;
                //            lstTurnos = turNeg.Listar().Where(x => x.medico.IdPersona == idPer).ToList();
                //        }
                //        else
                //        {
                //            lstTurnos = turNeg.Listar();
                //        }

                //        Session["listaTurnos"] = lstTurnos;
                //        dgvTurnos.DataSource = lstTurnos;
                //        dgvTurnos.DataBind();

                //    }
                       
                //    else
                //    {
                //        ClientScript.RegisterStartupScript(this.GetType(), "ShowErrorModal", "showErrorModal();", true);
                //        foreach (string error in listErrores)
                //        {
                //            ClientScript.RegisterStartupScript(this.GetType(), "AddErrorToList", "addErrorToList('" + error + "');", true);
                //        }
                //    }
                //}
                else if (e.CommandName == "VerMas")
                {

                    Document doc = new Document();
                    MemoryStream ms = new MemoryStream();
                    PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                    doc.Open();

                    // Configurar una fuente y tamaño para el texto del título
                    BaseFont titleBaseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    Font titleFont = new Font(titleBaseFont, 14, Font.BOLD, BaseColor.BLACK); // Establecer un estilo de fuente negrita

                    DataTable dtTurno = ObtenerDTTurno(id);

                    foreach (DataRow row in dtTurno.Rows)
                    {
                        // Agregar título para cada fila
                        string tituloFila = "Clinica Medica"; // Personaliza el título según tus necesidades
                        Paragraph titleParagraph = new Paragraph(tituloFila, titleFont);
                        doc.Add(titleParagraph);

                        foreach (DataColumn col in dtTurno.Columns)
                        {
                            string cellText = row[col].ToString();
                            Paragraph cellParagraph = new Paragraph(cellText);
                            doc.Add(cellParagraph);
                        }
                    }

                    doc.Close();

                    byte[] pdfData = ms.ToArray();

                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment; filename=Datos_Turno.pdf");
                    Response.BinaryWrite(pdfData);
                    Response.Flush();  // Usar Flush en lugar de End
                    HttpContext.Current.ApplicationInstance.CompleteRequest();


                }
            }
            catch (Exception excepcion)
            {

                Session.Add("Error", excepcion.ToString());
                Response.Redirect("Error.aspx", false);
            }

        }

        private void Validar(Dominio.Turno turno)
        {
            if (turno.estado.estado == "Programado" || turno.estado.estado == "En curso") listErrores.Add("Si quiere eliminar el turno, recuerde que no debe estar programado o en curso");
        }

        private DataTable ObtenerDTTurno(int id)
        {
            TurnoNegocio turNeg = new TurnoNegocio();
            Dominio.Turno turno = turNeg.Listar().FirstOrDefault(x => x.Id == id);
            DataTable dataTable = new DataTable();
          
            dataTable.Columns.Add("Turno.Id", typeof(string));
            dataTable.Columns.Add("Separador", typeof(string));
            dataTable.Columns.Add("datosPaciente", typeof(string));
            dataTable.Columns.Add("Paciente.Nombre", typeof(string));
            dataTable.Columns.Add("Paciente.Apellido", typeof(string));
            dataTable.Columns.Add("Paciente.Dni", typeof(string));
            dataTable.Columns.Add("Paciente.Email", typeof(string));
            dataTable.Columns.Add("Separador2", typeof(string));
            dataTable.Columns.Add("datosMedico", typeof(string));
            dataTable.Columns.Add("Medico.Nombre", typeof(string));
            dataTable.Columns.Add("Medico.Apellido", typeof(string));
            dataTable.Columns.Add("Medico.Dni", typeof(string));
            dataTable.Columns.Add("Medico.Email", typeof(string));
            dataTable.Columns.Add("Separador3", typeof(string));
            dataTable.Columns.Add("datosTurno", typeof(string));
            dataTable.Columns.Add("Turno.Fecha", typeof(string));
            dataTable.Columns.Add("Turno.Hora", typeof(string));
            dataTable.Columns.Add("Turno.Observacion", typeof(string));
            DataRow row = dataTable.NewRow();
            row["Turno.Id"] = "#"+turno.Id.ToString();
            row["Separador"] = "-----------------------------------------------------------------------------------------------------------------------";
            row["datosPaciente"] = "Datos del paciente ";
            row["Paciente.Nombre"] = "Nombre del paciente: "+ turno.paciente.Nombre.Trim();
            row["Paciente.Apellido"] = "Apellido del paciente: "+turno.paciente.Apellido.Trim();
            row["Paciente.Dni"] = "Dni del paciente: "+ turno.paciente.DNI.Trim();
            row["Paciente.Email"] = "Email: "+turno.paciente.Email.Trim();
            row["Separador2"] = "-----------------------------------------------------------------------------------------------------------------------";
            row["datosMedico"] = "Datos del Medico ";
            row["Medico.Nombre"] = "Nombre del medico: "+ turno.medico.Nombre.Trim();
            row["Medico.Apellido"] = "Apellido del medico: "+turno.medico.Apellido.Trim();
            row["Medico.Dni"] = "Dni del medico: "+turno.medico.DNI.Trim();
            row["Medico.Email"] = "Email: "+turno.medico.Email.Trim();
            row["Separador3"] = "-----------------------------------------------------------------------------------------------------------------------";
            row["datosTurno"] = "Datos del turno ";
            row["Turno.Fecha"] = "Fecha del turno: "+turno.fecha.ToString("yyyy-MM-dd");
            row["Turno.Hora"] = "Horario del turno: "+turno.hora.ToString() +" hs";
            row["Turno.Observacion"] = "Observacion: " + turno.Observacion.ToString();
            dataTable.Rows.Add(row);

            return dataTable;     
        }

        protected void txtFiltroTurno_TextChanged(object sender, EventArgs e)
        {
            List<Dominio.Turno> listaTurnosFiltrada;
            try
            {

                listaTurnosFiltrada = (List<Dominio.Turno>)Session["listaTurnos"];

                if (txtFiltroTurno.Text.Length > 0)
                {
                    listaTurnosFiltrada = filtrarNumero(listaTurnosFiltrada);
                }
                if (txtFiltroPaciente.Text.Length > 0)
                {
                    listaTurnosFiltrada = filtrarPaciente(listaTurnosFiltrada);
                }
                if (txtFiltroEspecialidad.Text.Length > 0)
                {
                    listaTurnosFiltrada = filtrarEspecialidad(listaTurnosFiltrada);
                }
                if (txtFiltroMedico.Text.Length > 0)
                {
                    listaTurnosFiltrada = filtrarMedico(listaTurnosFiltrada);
                }
                Session["listaTurnosFiltrada"] = listaTurnosFiltrada;
                dgvTurnos.DataSource = listaTurnosFiltrada;
                dgvTurnos.DataBind();
            }
            catch(Exception ex)
            {

            }

        }

        private List<Dominio.Turno> filtrarNumero(List<Dominio.Turno> listaTurnos)
        {
            List<Dominio.Turno> listaFiltradaNumeros = null;
            try
            {
                listaFiltradaNumeros = listaTurnos.FindAll(turno => turno.Id.ToString().ToUpper().Contains(txtFiltroTurno.Text.ToUpper()));
                return listaFiltradaNumeros;
            }
            catch
            {
                return listaFiltradaNumeros;
            }
        }

        private List<Dominio.Turno> filtrarPaciente(List<Dominio.Turno> listaTurnos)
        {
            List<Dominio.Turno> listaFiltradaPacientes = null;
            try
            {
                listaFiltradaPacientes = listaTurnos.FindAll(turno => turno.paciente.denominacion.ToUpper().Contains(txtFiltroPaciente.Text.ToUpper()));
                return listaFiltradaPacientes;
            }
            catch
            {
                return listaFiltradaPacientes;
            }
        }

        private List<Dominio.Turno> filtrarEspecialidad(List<Dominio.Turno> listaTurnos)
        {
            List<Dominio.Turno> listaFiltradaEspecialidades = null;
            try
            {
                listaFiltradaEspecialidades = listaTurnos.FindAll(turno => turno.especialidad.especialidad.ToUpper().Contains(txtFiltroEspecialidad.Text.ToUpper()));
                return listaFiltradaEspecialidades;
            }
            catch
            {
                return listaFiltradaEspecialidades;
            }
        }

        private List<Dominio.Turno> filtrarMedico(List<Dominio.Turno> listaTurnos)
        {
            List<Dominio.Turno> listaFiltradaMedicos = null;
            try
            {
                listaFiltradaMedicos = listaTurnos.FindAll(turno => turno.medico.datos.ToUpper().Contains(txtFiltroMedico.Text.ToUpper()));
                return listaFiltradaMedicos;
            }
            catch
            {
                return listaFiltradaMedicos;
            }
        }

        protected void dgvTurnos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                

                

                
                TurnoNegocio turNeg = new TurnoNegocio();
                List<Dominio.Turno> listaTurnos = turNeg.Listar();
                if (((Dominio.Persona)Session["UsuarioLogueado"]).usuario.perfil.Tipo == "Medico")
                {
                    var idPer = ((Dominio.Persona)Session["UsuarioLogueado"]).IdPersona;
                    listaTurnos = turNeg.Listar().Where(x => x.medico.IdPersona == idPer).ToList();
                }
                   
                //listaHorariosFiltrada = (List<Modelo.Horario>)Session["listaHorariosFiltrada"];
                //if (listaHorariosFiltrada is null)
                //{
                dgvTurnos.PageIndex = e.NewPageIndex;
                dgvTurnos.DataSource = listaTurnos;
                //}
                //else
                //{
                //    dgvHorarios.PageIndex = e.NewPageIndex;
                //    dgvHorarios.DataSource = listaHorariosFiltrada;
                //}
                dgvTurnos.DataBind();
            }
            catch (Exception ex)
            {

                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }
    }
}