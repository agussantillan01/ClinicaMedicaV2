using Dominio;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Negocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.InteropServices;

namespace ClinicaWeb
{
    public partial class Pacientes : System.Web.UI.Page
    {
        public List<Dominio.Paciente> lstPacientes { get; set; }
        public List<string> listErrores { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogueado"] == null) Response.Redirect("Login.aspx", false);
            if (!IsPostBack)
            {
                PacienteNegocio pacNeg = new PacienteNegocio();
                lstPacientes = pacNeg.ListarPacientes();
                Session["listaPacientes"] = lstPacientes;
                dgvPacientes.DataSource = lstPacientes;
                dgvPacientes.DataBind();
            }

        }

        protected void dgvPacientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                listErrores = new List<string>();
                int id = -1;
                if (dgvPacientes.PageIndex > 0 && e.CommandName == "Eliminar")
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    int adjustedIndex = index - (dgvPacientes.PageIndex * dgvPacientes.PageSize);
                    GridViewRow selectedRow = dgvPacientes.Rows[adjustedIndex];
                    TableCell contactName = selectedRow.Cells[0];
                    id = Convert.ToInt32(contactName.Text);
                }
                else
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow selectedRow = dgvPacientes.Rows[index];
                    TableCell contactName = selectedRow.Cells[0];
                     id = Convert.ToInt32(contactName.Text);
                }

                if (e.CommandName == "Modificar")
                {
                    Response.Redirect("frmPaciente.aspx?Id=" + id, false);
                }
                else if (e.CommandName == "Eliminar")
                {


                    PacienteNegocio pacNeg = new PacienteNegocio();
                    Paciente paciente = pacNeg.ListarPacientes().FirstOrDefault(x => x.IdPaciente == id);
                    Validar(paciente);
                    if (listErrores.Count == 0)
                    {
                        pacNeg.Eliminar(paciente);
                        PersonaNegocio personaneg = new PersonaNegocio();
                        personaneg.Eliminar(paciente);
                        lstPacientes = pacNeg.ListarPacientes();
                        Session["listaPacientes"] = lstPacientes;
                        dgvPacientes.DataSource = lstPacientes;
                        dgvPacientes.DataBind();


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
                else if (e.CommandName == "VerMas")
                {
                    Document doc = new Document();
                    MemoryStream ms = new MemoryStream();
                    PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                    doc.Open();

                    // Configurar una fuente y tamaño para el texto del título
                    BaseFont titleBaseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    Font titleFont = new Font(titleBaseFont, 14, Font.BOLD, BaseColor.BLACK); // Establecer un estilo de fuente negrita

                    DataTable dtMedicos = ObtenerPaciente(id);

                    foreach (DataRow row in dtMedicos.Rows)
                    {
                        // Agregar título para cada fila
                        string tituloFila = "Clinica Medica"; // Personaliza el título según tus necesidades
                        Paragraph titleParagraph = new Paragraph(tituloFila, titleFont);
                        doc.Add(titleParagraph);

                        foreach (DataColumn col in dtMedicos.Columns)
                        {
                            string cellText = row[col].ToString();
                            Paragraph cellParagraph = new Paragraph(cellText);
                            doc.Add(cellParagraph);
                        }
                    }

                    doc.Close();

                    byte[] pdfData = ms.ToArray();
                    //Response.Clear();
                    //Response.ContentType = "application/pdf";
                    //Response.AddHeader("content-disposition", "attachment; filename=Datos_Paciente.pdf");
                    //Response.BinaryWrite(pdfData);
                    //Response.End();

                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment; filename=Datos_Paciente.pdf");
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
        private DataTable ObtenerPaciente(int id)
        {
            try
            {
                PacienteNegocio pacienteNegocio = new PacienteNegocio();
                Dominio.Paciente paciente = pacienteNegocio.ListarPacientes().FirstOrDefault(x => x.IdPaciente == id);
                DataTable dataTable = new DataTable();

                dataTable.Columns.Add("Paciente.Id", typeof(string));
                dataTable.Columns.Add("Separador1", typeof(string));
                dataTable.Columns.Add("Titulo.datos", typeof(string));
                dataTable.Columns.Add("Paciente.Nombre", typeof(string));
                dataTable.Columns.Add("Paciente.Apellido", typeof(string));
                dataTable.Columns.Add("Paciente.Dni", typeof(string));
                dataTable.Columns.Add("Paciente.Email", typeof(string));
                dataTable.Columns.Add("Paciente.Sexo", typeof(string));
                dataTable.Columns.Add("Paciente.FechaNacimiento", typeof(string));
                dataTable.Columns.Add("Paciente.Direccion", typeof(string));
                dataTable.Columns.Add("Paciente.Contacto", typeof(string));
                dataTable.Columns.Add("Separador2", typeof(string));

                DataRow row = dataTable.NewRow();
                row["Paciente.Id"] = "#" + paciente.IdPaciente.ToString();
                row["separador1"] = "-----------------------------------------------------------------------------------------------------------------------";
                row["Titulo.datos"] = "Datos Personales: ";
                row["Paciente.Nombre"] = "Nombre : " + paciente.Nombre;
                row["Paciente.Apellido"] = "Apellido: " + paciente.Apellido;
                row["Paciente.Dni"] = "Dni: " + paciente.DNI.Trim();
                row["Paciente.Email"] = "Email: " + paciente.Email.Trim();
                if (paciente.Sexo.Trim() == "H") row["Paciente.Sexo"] = "Sexo: Hombre";
                else row["Paciente.Sexo"] = "Sexo: Mujer";
                row["Paciente.FechaNacimiento"] = "Fecha de nacimiento: " + paciente.FechaNacimiento.ToString("dd-MM-yyyy");
                row["Paciente.Direccion"] = "Direccion: " + paciente.Direccion;
                row["Paciente.Contacto"] = "Contacto: " + paciente.Contacto;
                row["Separador2"] = "-----------------------------------------------------------------------------------------------------------------------";


                dataTable.Rows.Add(row);

                return dataTable;
            }
            catch (Exception excepcion)
            {

                throw;
            }

        }

        private void Validar(Paciente paciente)
        {
            TurnoNegocio turNeg = new TurnoNegocio();
            foreach (var item in turNeg.Listar())
            {
                if (item.paciente.IdPaciente == paciente.IdPaciente && (item.estado.estado == "Programado" || item.estado.estado == "En curso")) listErrores.Add("El paciente contiene un turno pendiente");
                break;
            }
        }

        protected void tbxFiltroDNI_TextChanged(object sender, EventArgs e)
        {
            List<Dominio.Paciente> listaPacientes;
            List<Dominio.Paciente> listaPacientesFiltrada;
            try
            {
                listaPacientes = (List<Dominio.Paciente>)Session["listaPacientes"];
                listaPacientesFiltrada = filtrarDNI(listaPacientes);
                if (tbxFiltroNombre.Text.Length > 0)
                {
                    listaPacientesFiltrada = filtrarNombre(listaPacientesFiltrada);
                }
                if (tbxFiltroApellido.Text.Length > 0)
                {
                    listaPacientesFiltrada = filtrarApellido(listaPacientesFiltrada);
                }
                Session["listaPacientesFiltrada"] = listaPacientesFiltrada;
                dgvPacientes.DataSource = listaPacientesFiltrada;
                dgvPacientes.DataBind();
            }
            catch (Exception excepcion)
            {

            }
        }

        private List<Dominio.Paciente> filtrarDNI(List<Dominio.Paciente> listaPacientes)
        {
            List<Dominio.Paciente> listaDNIs;
            try
            {
                listaDNIs = listaPacientes.FindAll(paciente => paciente.DNI.ToUpper().Contains(tbxFiltroDNI.Text.ToUpper()));
                return listaDNIs;
            }
            catch
            {
                return null;
            }
        }

        private List<Dominio.Paciente> filtrarNombre(List<Dominio.Paciente> listaPacientes)
        {
            List<Dominio.Paciente> listaNombres;
            try
            {
                listaNombres = listaPacientes.FindAll(paciente => paciente.Nombre.ToUpper().Contains(tbxFiltroNombre.Text.ToUpper()));
                return listaNombres;
            }
            catch
            {
                return null;
            }
        }

        private List<Dominio.Paciente> filtrarApellido(List<Dominio.Paciente> listaPacientes)
        {
            List<Dominio.Paciente> listaApellidos;
            try
            {
                listaApellidos = listaPacientes.FindAll(paciente => paciente.Apellido.ToUpper().Contains(tbxFiltroApellido.Text.ToUpper()));
                return listaApellidos;
            }
            catch
            {
                return null;
            }
        }


        protected void dgvPacientes_PageIndexChanging1(object sender, GridViewPageEventArgs e)
        {
            try
            {
                PacienteNegocio pacNegocio = new PacienteNegocio();


                List<Dominio.Paciente> lstPacientes = pacNegocio.ListarPacientes();
                //List<Dominio.Especialidad> listaHorariosFiltrada;

                //listaHorariosFiltrada = (List<Modelo.Horario>)Session["listaHorariosFiltrada"];
                //if (listaHorariosFiltrada is null)
                //{
                dgvPacientes.PageIndex = e.NewPageIndex;
                dgvPacientes.DataSource = lstPacientes;
                //}
                //else
                //{
                //    dgvHorarios.PageIndex = e.NewPageIndex;
                //    dgvHorarios.DataSource = listaHorariosFiltrada;
                //}
                dgvPacientes.DataBind();
            }
            catch (Exception ex)
            {

                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }
    }
}