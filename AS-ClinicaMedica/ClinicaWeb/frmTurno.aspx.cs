using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.util;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace ClinicaWeb
{
    public partial class frmTurno : System.Web.UI.Page
    {
        public List<string> listErrores = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogueado"] == null) Response.Redirect("Login.aspx", false);
            string headingText;
            PacienteNegocio pacNeg = new PacienteNegocio();
            EspecialidadNegocio espNeg = new EspecialidadNegocio();
            MedicoNegocio medNeg = new MedicoNegocio();
            EstadoNegocio estadoNeg = new EstadoNegocio();
            if (!IsPostBack)
            {
                ddlPaciente.DataSource = pacNeg.ListarPacientes();
                ddlPaciente.DataTextField = "datos";
                ddlPaciente.DataValueField = "IdPaciente";
                ddlPaciente.DataBind();

                ddlEspecialidad.DataSource = espNeg.Listar(); ;
                ddlEspecialidad.DataTextField = "Especialidad";
                ddlEspecialidad.DataValueField = "Id";
                ddlEspecialidad.DataBind();

                ddlMedico.DataSource = medNeg.Listar();
                ddlMedico.DataTextField = "datos";
                ddlMedico.DataValueField = "Id";
                ddlMedico.DataBind();


                ddlEstado.DataSource = estadoNeg.listar().Where(x => x.estado.Trim() == "Programado");
                ddlEstado.DataTextField = "Estado";
                ddlEstado.DataValueField = "Id";
                ddlEstado.DataBind();

                if (Request.QueryString["Id"] != null)
                {
                    headingText = "<h1>Modificacion Turno</h1>";

                        TurnoNegocio turNeg = new TurnoNegocio();
                        Turno turno = turNeg.Listar().FirstOrDefault(x => x.Id == int.Parse(Request.QueryString["Id"]));

                        ddlEstado.DataSource = estadoNeg.listar();
                        ddlEstado.DataTextField = "Estado";
                        ddlEstado.DataValueField = "Id";
                        ddlEstado.DataBind();

                        ddlPaciente.SelectedValue = turno.paciente.IdPaciente.ToString();
                        ddlMedico.SelectedValue = turno.medico.Id.ToString();
                        ddlEspecialidad.SelectedValue = turno.especialidad.Id.ToString();
                        ddlEstado.SelectedValue = turno.estado.Id.ToString();

                        txtFecha.Text = turno.fecha.ToString("yyyy-MM-dd");
                        txtHora.Text = turno.hora.ToString();

                        txtObservacion.Text = turno.Observacion.ToString();
                    
                    //if (((Dominio.Persona)Session["UsuarioLogueado"]).usuario.perfil.Tipo == "Recepcionista")
                    //{
                    //    ddlEstado.Enabled = false;
                    //    ddlEspecialidad.Enabled = false;
                    //    ddlPaciente.Enabled = false;
                    //    ddlMedico.Enabled = false;
                    //    txtFecha.ReadOnly = true;
                    //    txtHora.ReadOnly = true;
                    //    txtObservacion.ReadOnly = true;
                    //}


                }
                else
                {
                    if ((Persona)Session["UsuarioLogueado"] != null)
                    {
                        if (((Persona)Session["UsuarioLogueado"]).usuario.perfil.Tipo == "Medico")
                        {
                            lblMedico.Visible = false;
                            ddlMedico.Visible = false;

                            ddlEspecialidad.DataSource = (medNeg.Listar().FirstOrDefault(x => x.IdPersona == ((Persona)Session["UsuarioLogueado"]).IdPersona)).listEspecialidades;
                            ddlEspecialidad.DataTextField = "Especialidad";
                            ddlEspecialidad.DataValueField = "Id";
                            ddlEspecialidad.DataBind();
                        }
                    }
                    headingText = "<h1>Nuevo Turno</h1>";
                }
                ltlHeading.Text = headingText;
            }
        }

        protected void ddlPaciente_DataBound(object sender, EventArgs e)
        {
            ddlPaciente.Items.Insert(0, new ListItem("--Seleccione un Paciente--", "-1"));
            ddlEstado.Items.Insert(0, new ListItem("--Seleccione una Estado--", "-1"));
        }

        protected void ddlMedico_DataBound(object sender, EventArgs e)
        {
            ddlMedico.Items.Insert(0, "--Seleccione un Médico--");

        }

        protected void ddlEspecialidad_DataBound(object sender, EventArgs e)
        {
            ddlEspecialidad.Items.Insert(0, new ListItem("--Seleccione una Especialidad--", "-1"));
        }

        protected void ddlEspecialidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int idEspecialidadSeleccionada = int.Parse(ddlEspecialidad.SelectedValue);
                MedicoNegocio medNeg = new MedicoNegocio();
                List<Dominio.Medico> listMedicos = new List<Medico>();
                foreach (var item in medNeg.Listar())
                {
                    if (item.listEspecialidades.Any(x => x.Id == idEspecialidadSeleccionada))
                    {
                        listMedicos.Add(item);
                    }
                }
                ddlMedico.DataSource = listMedicos;
                ddlMedico.DataTextField = "datos";
                ddlMedico.DataValueField = "Id";
                ddlMedico.DataBind();
            }
            catch (Exception ex)
            {


                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }


        }

        protected void ddlMedico_SelectedIndexChanged(object sender, EventArgs e)
        {
            MedicoNegocio medNeg = new MedicoNegocio();
            Medico medicoSeleccionado = new Medico();
            Session.Add("medicoSeleccionado", medicoSeleccionado = medNeg.Listar().FirstOrDefault(x => x.Id == int.Parse(ddlMedico.SelectedValue)));

            lblHorarioMedicoSeleccionado.Text = string.Join("<br />", medicoSeleccionado.listHorarios.Select(hora => hora.diaHora));

        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                listErrores = new List<string>();
                //if ((Medico)Session["medicoSeleccionado"] != null || ((Persona)Session["UsuarioLogueado"]).usuario.perfil.Tipo == "Medico")
                //{
                if (Request.QueryString["Id"] == null)
                {

                    Medico medico = new Medico();
                    MedicoNegocio medNeg = new MedicoNegocio();
                    if ((Medico)Session["medicoSeleccionado"] == null) medico = medNeg.Listar().FirstOrDefault(x => x.IdPersona == ((Persona)Session["UsuarioLogueado"]).IdPersona);
                    else medico = (Medico)Session["medicoSeleccionado"];



                    Validar(medico, int.Parse(ddlPaciente.SelectedValue), int.Parse(ddlEspecialidad.SelectedValue), txtFecha.Text, txtHora.Text, int.Parse(ddlEstado.SelectedValue));

                }

                else
                {
                    TurnoNegocio turNeg = new TurnoNegocio();
                    Turno tur = turNeg.Listar().FirstOrDefault(x => x.Id == int.Parse(Request.QueryString["Id"]));
                    Validar(tur.medico, int.Parse(ddlPaciente.SelectedValue), int.Parse(ddlEspecialidad.SelectedValue), txtFecha.Text, txtHora.Text, int.Parse(ddlEstado.SelectedValue));
                }

                if (listErrores.Count == 0)
                {
                    MedicoNegocio medNeg = new MedicoNegocio();
                    PacienteNegocio pacNeg = new PacienteNegocio();
                    EspecialidadNegocio espeNeg = new EspecialidadNegocio();
                    EstadoNegocio estadoNegocio = new EstadoNegocio();

                    Turno turno = new Turno();
                    if ((Persona)Session["UsuarioLogueado"] != null)
                    {
                        if (((Persona)Session["UsuarioLogueado"]).usuario.perfil.Tipo == "Medico")
                        {

                            turno.medico = medNeg.Listar().FirstOrDefault(x => x.IdPersona == ((Dominio.Persona)Session["usuarioLogueado"]).IdPersona);
                            turno.paciente = pacNeg.ListarPacientes().FirstOrDefault(x => x.IdPaciente == int.Parse(ddlPaciente.SelectedValue));
                            turno.especialidad = espeNeg.Listar().FirstOrDefault(x => x.Id == int.Parse(ddlEspecialidad.SelectedValue));
                            turno.fecha = DateTime.Parse(txtFecha.Text);
                            turno.hora = txtHora.Text;
                            turno.estado = estadoNegocio.listar().FirstOrDefault(x => x.Id == int.Parse(ddlEstado.SelectedValue));
                            turno.Observacion = txtObservacion.Text.Trim();


                        }
                        else
                        {

                            turno.medico = medNeg.Listar().FirstOrDefault(x => x.Id == int.Parse(ddlMedico.SelectedValue));  //(Medico)Session["medicoSeleccionado"],
                            turno.paciente = pacNeg.ListarPacientes().FirstOrDefault(x => x.IdPaciente == int.Parse(ddlPaciente.SelectedValue));
                            turno.especialidad = espeNeg.Listar().FirstOrDefault(x => x.Id == int.Parse(ddlEspecialidad.SelectedValue));
                            turno.fecha = DateTime.Parse(txtFecha.Text);
                            turno.hora = txtHora.Text;
                            turno.estado = estadoNegocio.listar().FirstOrDefault(x => x.Id == int.Parse(ddlEstado.SelectedValue));
                            turno.Observacion = txtObservacion.Text.Trim();
                        }
                    }



                    TurnoNegocio turNeg = new TurnoNegocio();
                    if (Request.QueryString["Id"] == null)
                    {
                        turNeg.Agregar(turno);
                        EnviarMails(turno);
                        string script = "alert('Agregado correctamente'); window.location.href = 'Turnos.aspx';";
                        ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", script, true);
                    }

                    else
                    {

                        turno.Id = int.Parse(Request.QueryString["Id"]);
                        turNeg.Actualizar(turno);
                        string script = "alert('Actualizado correctamente'); window.location.href = 'Turnos.aspx';";
                        ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", script, true);
                    }
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
            catch (Exception ex)
            {

                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }

        }
        private void EnviarMails(Turno turno)
        {
            EmailServices emailPaciente = new EmailServices();
            string htmlCuerpoPaciente = generaHtmlPaciente(turno);
            emailPaciente.EnvioTurnos(turno.paciente.Email.Trim(), "Generación de Turno", htmlCuerpoPaciente);
            emailPaciente.enviarEmail();

            EmailServices emailMedico = new EmailServices();
            string htmlCuerpoMedico = generaHtmlMedico(turno);
            emailPaciente.EnvioTurnos(turno.medico.Email.Trim(), "Generación de Turno", htmlCuerpoMedico);
            emailPaciente.enviarEmail();


        }
        private string generaHtmlMedico(Turno turno)
        {
            return "    <table width=\"100%\" cellpadding=\"10\">       <tr>           <td align=\"center\" style=\"background-color: #007BFF; color: #fff;\">                <h1>Recordatorio de Turno Médico</h1>            </td>        </tr>        <tr>            <td>                <p>Estimado Dr. " + turno.medico.Nombre + " " + turno.medico.Apellido + ",</p>                <p>Le recordamos que tiene un turno médico pendiente con el paciente " + turno.paciente.datos + ". A continuación, se detallan los datos del turno:</p>                <ul>                    <li><strong>Fecha:</strong>" + turno.fecha.ToString("dd-MM-yyyy") + "</li>                     <li><strong>Horario:</strong>" + turno.hora + "</li>               </ul>               <p>Por favor, asegúrese de estar disponible y preparado para atender al paciente en la fecha y hora programada. Si tiene alguna pregunta o necesita reprogramar el turno, no dude en ponerse en contacto con nosotros.</p>\r\n                <p>Gracias por su compromiso con la atención médica de nuestros pacientes. Esperamos que esta información le sea útil.</p>              <p>Saludos cordiales,</p>               <p>Clínica Médica</p>           </td>      </tr>   </table>";
        }
        private string generaHtmlPaciente(Turno turno)
        {
            return "<table width=\"100%\" cellpadding=\"10\">        <tr>            <td align=\"center\" style=\"background-color: #007BFF; color: #fff;\">              <h1>Confirmación de Turno Médico</h1>          </td>        </tr>        <tr>            <td>                <p>Estimado/a " + turno.paciente.Nombre + ",</p>                <p>Le informamos que se ha programado un turno médico para usted con los siguientes detalles:</p>                <ul>                    <li><strong>Fecha:</strong>" + turno.fecha.ToString("dd-MM-yyyy") + "</li>                <li><strong>Horario:</strong>" + turno.hora + "</li>                    <li><strong>Médico:</strong>" + turno.medico.Nombre + " " + turno.medico.Apellido + "</li>               </ul>                <p>Por favor, asegúrese de estar presente en la clínica a tiempo para su consulta con el Dr. " + turno.medico.Nombre + ". Si necesita reprogramar el turno o tiene alguna pregunta, no dude en ponerse en contacto con nosotros.</p>                <p>Gracias por elegir nuestros servicios médicos. Le esperamos en su cita.</p>               <p>Saludos cordiales,</p>            <p>Su Clínica Médica</p>           </td>        </tr>    </table>";
        }
        private void Validar(Medico med, int? idPaciente, int? idEspecialidad, string fecha, string hora, int? idestado)
        {
            try
            {
                if (string.IsNullOrEmpty(idPaciente.ToString()) || idPaciente == -1) listErrores.Add("Debe ingresaar un paciente");
                if (string.IsNullOrEmpty(idEspecialidad.ToString()) || idEspecialidad == -1) listErrores.Add("Debe ingresaar una especialidad");
                if (string.IsNullOrEmpty(fecha.ToString())) listErrores.Add("Debe ingresaar un fecha");
                if (string.IsNullOrEmpty(hora.ToString())) listErrores.Add("Debe ingresaar una hora");
                if (string.IsNullOrEmpty(idestado.ToString()) || idestado == -1) listErrores.Add("Debe ingresaar un estado");
                if (listErrores.Count == 0)
                {


                    TurnoNegocio turNeg = new TurnoNegocio();
                    var fechaDateTime = DateTime.Parse(fecha);
                    if (Request.QueryString["Id"] == null)
                    {


                        var existeTurnosXmedico = (Turno)turNeg.Listar().FirstOrDefault(x => x.medico.Id == med.Id && x.fecha == fechaDateTime && x.hora == hora);
                        if (existeTurnosXmedico != null && (existeTurnosXmedico.estado.estado == "Programado" || existeTurnosXmedico.estado.estado == "En curso")) listErrores.Add("El Dr. " + med.Nombre + " " + med.Apellido + " ya tiene un turno asignado en ese horario.");

                        var existeTurnosXpaciente = (Turno)turNeg.Listar().FirstOrDefault(x => x.paciente.IdPaciente == idPaciente && x.fecha == fechaDateTime && x.hora == hora);
                        if (existeTurnosXpaciente != null && (existeTurnosXmedico.estado.estado == "Programado" || existeTurnosXmedico.estado.estado == "En curso"))
                        {
                            PacienteNegocio pacNeg = new PacienteNegocio();
                            var datosPaciente = (pacNeg.ListarPacientes().FirstOrDefault(x => x.IdPaciente == idPaciente)).datos;
                            listErrores.Add("El paciente. " + datosPaciente + " ya tiene un turno asignado en ese horario.");
                        }

                        DateTime fechaIngresada = DateTime.ParseExact(fecha.Trim(), "yyyy-MM-dd", null);
                        DateTime fechaActual = DateTime.Today;
                        if (fechaIngresada < fechaActual)
                        {
                            listErrores.Add("La fecha no puede ser menor al dia de hoy.");
                        }

                    }


                    bool medicoContieneEspecialidad = false;
                    foreach (var item in med.listEspecialidades)
                    {
                        if (item.Id == idEspecialidad)
                        {
                            medicoContieneEspecialidad = true;
                            break;

                        }
                    }
                    if (!medicoContieneEspecialidad) listErrores.Add("El Dr " + med.datos + " no se dedica a la especializacion seleccionada");
                    bool medicoContieneHorario = false;
                    foreach (var item in med.listHorarios)
                    {
                        if (item.Dia == ObtenerDiaDeLaSemana(fechaDateTime))
                        {
                            if (TurnoEstaEnHorario(hora, item.HoraInicio, item.HoraFin))
                            {
                                medicoContieneHorario = true;
                                break;
                            }
                        }
                    }
                    if (!medicoContieneHorario) listErrores.Add("El medico no trabaja en el horario seleccionado");
                    //if (((Dominio.Persona)Session["UsuarioLogueado"]).usuario.perfil.Tipo == "Recepcionista") listErrores.Add("Usted no tiene permiso para modificar el turno");
                }
            }
            catch (Exception ex)
            {

                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }





        }

        private bool TurnoEstaEnHorario(string horaTurno, string horaInicio, string horaFin)
        {
            if (DateTime.TryParse(horaInicio, out DateTime inicio) && DateTime.TryParse(horaFin, out DateTime fin) && DateTime.TryParse(horaTurno, out DateTime turno))
            {
                TimeSpan ParseHoraInicio = inicio.TimeOfDay;
                TimeSpan ParseHoraFin = fin.TimeOfDay;
                TimeSpan ParseHoraTurno = turno.TimeOfDay;

                return ParseHoraTurno >= ParseHoraInicio && ParseHoraTurno <= ParseHoraFin;
            }

            return false; // Devuelve false en caso de que no se puedan parsear las horas correctamente.
        }
        private string ObtenerDiaDeLaSemana(DateTime fecha)
        {
            // Utilizamos el método DayOfWeek para obtener el día de la semana como un enumerador.
            DayOfWeek diaDeLaSemana = fecha.DayOfWeek;

            // Luego, utilizamos un switch para convertir el enumerador en el nombre del día.
            switch (diaDeLaSemana)
            {
                case DayOfWeek.Sunday:
                    return "Domingo";
                case DayOfWeek.Monday:
                    return "Lunes";
                case DayOfWeek.Tuesday:
                    return "Martes";
                case DayOfWeek.Wednesday:
                    return "Miercoles";
                case DayOfWeek.Thursday:
                    return "Jueves";
                case DayOfWeek.Friday:
                    return "Viernes";
                case DayOfWeek.Saturday:
                    return "Sábado";
                default:
                    return "Día no válido";
            }
        }
        protected void ddlEstado_DataBound(object sender, EventArgs e)
        {
            ddlEstado.Items.Insert(0, new ListItem("--Seleccione una Estado--", "-1"));
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void txtFecha_TextChanged(object sender, EventArgs e)
        {
            TurnoNegocio turNeg = new TurnoNegocio();

            if (((Persona)Session["usuarioLogueado"]).usuario.perfil.Tipo == "Medico")
            {
                List<Turno> listTurnos = turNeg.Listar().Where(x => x.medico.IdPersona == ((Persona)Session["usuarioLogueado"]).IdPersona).ToList();
                listTurnos = listTurnos.Where(x => x.fecha == DateTime.Parse(txtFecha.Text)).ToList();
                List<string> listHorario = listTurnos.Select(x => x.hora).ToList();
                lblHorarioOcupado.Text = "Horarios Ocupados:<br />" + string.Join("<br />", listHorario);
            }
            else
            {
                if (((Medico)Session["medicoSeleccionado"]) != null)
                {

                    List<Turno> listTurnos = turNeg.Listar().Where(x => x.medico.Id == ((Medico)Session["medicoSeleccionado"]).Id).ToList();
                    listTurnos = listTurnos.Where(x => x.fecha == DateTime.Parse(txtFecha.Text)).ToList();
                    List<string> listHorario = listTurnos.Select(x => x.hora).ToList();
                    lblHorarioOcupado.Text = "Horarios Ocupados:<br />" + string.Join("<br />", listHorario);
                }
            }

        }
    }
}