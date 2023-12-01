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
using static iTextSharp.text.pdf.AcroFields;

namespace ClinicaWeb
{
    public partial class Medicos : System.Web.UI.Page
    {
        public List<Dominio.Medico> lstMedicos { get; set; }
        public List<string> listErrores { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogueado"] == null) Response.Redirect("Login.aspx", false);
            if (!IsPostBack)
            {
                MedicoNegocio pacNeg = new MedicoNegocio();
                lstMedicos = pacNeg.Listar();
                Session["listaMedicos"] = lstMedicos;
                dgvMedico.DataSource = lstMedicos;
                dgvMedico.DataBind();
            }

        }


        protected void dgvMedico_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                listErrores = new List<string>();
                int id = -1;
                if (dgvMedico.PageIndex > 0 && e.CommandName == "Eliminar")
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    int adjustedIndex = index - (dgvMedico.PageIndex * dgvMedico.PageSize);
                    GridViewRow selectedRow = dgvMedico.Rows[adjustedIndex];
                    TableCell contactName = selectedRow.Cells[0];
                    id = Convert.ToInt32(contactName.Text);
                }
                else
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow selectedRow = dgvMedico.Rows[index];
                    TableCell contactName = selectedRow.Cells[0];
                    id = Convert.ToInt32(contactName.Text);
                }
                if (e.CommandName == "Modificar")
                {
                    Response.Redirect("frmMedico.aspx?Id=" + id, false);
                }
                else if (e.CommandName == "Eliminar")
                {

                    MedicoNegocio medNeg = new MedicoNegocio();
                    Dominio.Medico medico = medNeg.Listar().FirstOrDefault(x => x.Id == id);

                    Validar(medico);
                    if (listErrores.Count == 0)
                    {
                        medNeg.Eliminar(medico);
                        PersonaNegocio perNeg = new PersonaNegocio();
                        perNeg.Eliminar(medico);
                        UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                        usuarioNegocio.Eliminar(medico.usuario);
                        Session["listaMedicos"] = medNeg.Listar();
                        dgvMedico.DataSource = lstMedicos;
                        dgvMedico.DataBind();
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

                    DataTable dtMedicos = ObtenerMedico(id);

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

                    // Configurar la respuesta HTTP para descargar el PDF
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment; filename=Datos_Medico.pdf");
                    Response.BinaryWrite(pdfData);
                    Response.Flush();  // Usar Flush en lugar de End
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {

                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }


        }

        private DataTable ObtenerMedico (int id)
        {
            MedicoNegocio mediicoNegocio = new MedicoNegocio();
            Dominio.Medico medico = mediicoNegocio.Listar().FirstOrDefault(x=> x.Id == id);
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Medico.Id", typeof(string));
            dataTable.Columns.Add("Separador1", typeof(string));
            dataTable.Columns.Add("Titulo.datos", typeof(string));
            dataTable.Columns.Add("Medico.Nombre", typeof(string));
            dataTable.Columns.Add("Medico.Apellido", typeof(string));
            dataTable.Columns.Add("Medico.Dni", typeof(string));
            dataTable.Columns.Add("Medico.Email", typeof(string));
            dataTable.Columns.Add("Medico.Sexo", typeof(string));
            dataTable.Columns.Add("Separador2", typeof(string));
            dataTable.Columns.Add("Titulo.Especialidad/es", typeof(string));
            for (int i = 0; i < medico.listEspecialidades.Count; i++)
            {
                dataTable.Columns.Add("Medico.Especialidad"+i.ToString(), typeof(string));
            }
            dataTable.Columns.Add("Separador3", typeof(string));
            dataTable.Columns.Add("Titulo.Horario/s", typeof(string));
            for (int i = 0; i < medico.listHorarios.Count; i++)
            {
                dataTable.Columns.Add("Medico.Horarios" + i.ToString(), typeof(string));
            }
            DataRow row = dataTable.NewRow();
            row["Medico.Id"] = "#" + medico.Id.ToString();
            row["separador1"] = "-----------------------------------------------------------------------------------------------------------------------";
            row["Titulo.datos"] = "Datos Personales: ";
            row["Medico.Nombre"] = "Nombre : " + medico.Nombre;
            row["Medico.Apellido"] = "Apellido: " + medico.Apellido;
            row["Medico.Dni"] = "Dni: " + medico.DNI.Trim();
            row["Medico.Email"] = "Email: " + medico.Email.Trim();
            if (medico.Sexo.Trim() == "H") row["Medico.Sexo"] = "Sexo: Hombre";
                else row["Medico.Sexo"] = "Sexo: Mujer";
            row["Separador2"] = "-----------------------------------------------------------------------------------------------------------------------";
            row["Titulo.Especialidad/es"] = "Especialidad/es: ";
            for (int i = 0; i < medico.listEspecialidades.Count; i++)
            {
                row["Medico.Especialidad"+i.ToString()] = medico.listEspecialidades[i].especialidad.ToString();
            }
            row["Separador3"] = "-----------------------------------------------------------------------------------------------------------------------";
            row["Titulo.Horario/s"] = "Horario/s :  ";
            for (int i = 0; i < medico.listHorarios.Count; i++)
            {
                row["Medico.Horarios" + i.ToString()] = medico.listHorarios[i].diaHora.ToString();
            }
            dataTable.Rows.Add(row);

            return dataTable;
        }
        private void Validar(Medico med)
        {
            listErrores = new List<string>();
            TurnoNegocio turNeg = new TurnoNegocio();
            foreach (var item in turNeg.Listar())
            {
                if (item.medico.Id == med.Id && (item.estado.estado == "Programado" || item.estado.estado == "En curso"))
                {
                    listErrores.Add("El medico tiene un turno pendiente..");
                    break;

                }
                    
            }
        }

        protected void txtFiltroMedico_TextChanged(object sender, EventArgs e)
        {
            List<Dominio.Medico> listaMedicos;
            List<Dominio.Medico> listaMedicosFiltrada;
            try
            {
                listaMedicos = (List<Dominio.Medico>)Session["listaMedicos"];
                listaMedicosFiltrada = filtrarNombre(listaMedicos);
                if (txtFiltroApellido.Text.Length > 0)
                {
                    listaMedicosFiltrada = filtrarApellido(listaMedicosFiltrada);
                }
                Session["listaMedicosFiltrada"] = listaMedicosFiltrada;
                dgvMedico.DataSource = listaMedicosFiltrada;
                dgvMedico.DataBind();
            }
            catch (Exception excepcion)
            {
                Session.Add("Error", excepcion.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        private List<Dominio.Medico> filtrarNombre(List<Dominio.Medico> listaMedicos)
        {
            List<Dominio.Medico> listaNombres;
            try
            {
                listaNombres = listaMedicos.FindAll(medico => medico.Nombre.ToUpper().Contains(txtFiltroNombre.Text.ToUpper()));
                return listaNombres;
            }
            catch
            {
                return null;
            }
        }

        private List<Dominio.Medico> filtrarApellido(List<Dominio.Medico> listaMedicos)
        {
            List<Dominio.Medico> listaApellidos;
            try
            {
                listaApellidos = listaMedicos.FindAll(medico => medico.Apellido.ToUpper().Contains(txtFiltroApellido.Text.ToUpper()));
                return listaApellidos;
            }
            catch
            {
                return null;
            }
        }

        protected void txtFiltroApellido_TextChanged(object sender, EventArgs e)
        {
            List<Dominio.Medico> listaMedicos;
            List<Dominio.Medico> listaMedicosFiltrada;
            try
            {
                listaMedicos = (List<Dominio.Medico>)Session["listaMedicos"];
                listaMedicosFiltrada = filtrarApellido(listaMedicos);
                if (txtFiltroNombre.Text.Length > 0)
                {
                    listaMedicosFiltrada = filtrarNombre(listaMedicosFiltrada);
                }
                Session["listaMedicosFiltrada"] = listaMedicosFiltrada;
                dgvMedico.DataSource = listaMedicosFiltrada;
                dgvMedico.DataBind();
            }
            catch (Exception excepcion)
            {
                Session.Add("Error", excepcion.ToString());
                Response.Redirect("Error.aspx", false);
            }

        }

        protected void dgvMedico_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                MedicoNegocio pacNegocio = new MedicoNegocio();


                List<Dominio.Medico> lstPacientes = pacNegocio.Listar();
                //List<Dominio.Especialidad> listaHorariosFiltrada;

                //listaHorariosFiltrada = (List<Modelo.Horario>)Session["listaHorariosFiltrada"];
                //if (listaHorariosFiltrada is null)
                //{
                dgvMedico.PageIndex = e.NewPageIndex;
                dgvMedico.DataSource = lstPacientes;
                //}
                //else
                //{
                //    dgvHorarios.PageIndex = e.NewPageIndex;
                //    dgvHorarios.DataSource = listaHorariosFiltrada;
                //}
                dgvMedico.DataBind();
            }
            catch (Exception ex)
            {

                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }
    }
}