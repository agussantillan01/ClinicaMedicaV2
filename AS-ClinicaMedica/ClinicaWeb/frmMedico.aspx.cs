using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ClinicaWeb
{
    public partial class frmMedico : System.Web.UI.Page
    {
        public List<Horario> horariosSeleccionados;
        public List<Especialidad> especialidadesSeleccionadas;
        public List<string> listErrores;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogueado"] == null) Response.Redirect("Login.aspx", false);
            string headingText;
            horariosSeleccionados = (List<Horario>)Session["listaHorarios"];
            if (horariosSeleccionados == null) horariosSeleccionados = new List<Horario>();

            especialidadesSeleccionadas = (List<Especialidad>)Session["listaEspecialidades"];
            if (especialidadesSeleccionadas == null) especialidadesSeleccionadas = new List<Especialidad>();
            if (!IsPostBack)
            {
                TipoUsuarioNegocio tUserNegocio = new TipoUsuarioNegocio();
                List<TipoUsuario> lstTipoUser = tUserNegocio.ObtenerTipos().Where(x => x.Tipo.Trim() == "Medico").ToList();
                ddlTipoUsuario.DataSource = lstTipoUser;
                ddlTipoUsuario.DataTextField = "Tipo";
                ddlTipoUsuario.DataValueField = "Id";
                ddlTipoUsuario.DataBind();

                EspecialidadNegocio espNegocio = new EspecialidadNegocio();
                List<Especialidad> lstEspecialidades = espNegocio.Listar();
                ddlEspecialidades.DataSource = lstEspecialidades;
                ddlEspecialidades.DataTextField = "especialidad";
                ddlEspecialidades.DataValueField = "Id";
                ddlEspecialidades.DataBind();


                HorarioNegocio hsNegocio = new HorarioNegocio();
                List<Horario> listHorarios = hsNegocio.Listar();
                ddlHorarios.DataSource = listHorarios;
                ddlHorarios.DataTextField = "diaHora";
                ddlHorarios.DataValueField = "Id";
                ddlHorarios.DataBind();

                if (Request.QueryString["Id"] != null)
                {
                    headingText = "<h1>Modificacion Medico</h1>";

                    MedicoNegocio medNeg = new MedicoNegocio();
                    Medico med = medNeg.Listar().FirstOrDefault(x => x.Id == int.Parse(Request.QueryString["Id"]));

                    txtNombre.Text = med.Nombre;
                    txtApellido.Text = med.Apellido;
                    txtDni.Text = med.DNI;
                    txtEmail.Text = med.Email;
                    txtUsuario.Text = med.usuario.Nombre;
                    txtUsuario.Enabled = false;
                    //txtPassword1.Text = med.usuario.Contrasenia;
                    //txtPassword1.Enabled = false;
                    //txtPassword2.Text = med.usuario.Contrasenia;
                    //txtPassword2.Enabled = false;
                    if (med.Sexo == "M")
                    {
                        chkMujer.Checked = true;
                        chkHombre.Checked = false;
                    }
                    else
                    {
                        chkMujer.Checked = false;
                        chkHombre.Checked = true;
                    }
                    ddlTipoUsuario.SelectedValue = med.usuario.perfil.Id.ToString();

                    horariosSeleccionados = med.listHorarios;
                    Session.Add("listaHorarios", horariosSeleccionados);
                    repetidorHorarios.DataSource = horariosSeleccionados;
                    repetidorHorarios.DataBind();

                    especialidadesSeleccionadas = med.listEspecialidades;
                    Session.Add("listaEspecialidades", especialidadesSeleccionadas);
                    repetidorEspecialidades.DataSource = especialidadesSeleccionadas;
                    repetidorEspecialidades.DataBind();

                }
                else
                {
                    headingText = "<h1>Nuevo Medico</h1>";


                }

                ltlHeading.Text = headingText;
            }
        }

        protected void ddlHorarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                HorarioNegocio hsNeg = new HorarioNegocio();
                Horario hs = hsNeg.Listar().FirstOrDefault(x => x.Id == int.Parse(ddlHorarios.SelectedValue));
                bool estaEnLaLista = horariosSeleccionados.Any(h => h.Id == hs.Id);

                if (!estaEnLaLista)
                {
                    horariosSeleccionados.Add(hs);
                    Session.Add("listaHorarios", horariosSeleccionados);

                    repetidorHorarios.DataSource = null;
                    repetidorHorarios.DataSource = horariosSeleccionados;
                    repetidorHorarios.DataBind();
                }
            }
            catch (Exception ex)
            {

                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }


        }

        protected void btnEliminarHorario_Click(object sender, EventArgs e)
        {
            try
            {
                HorarioNegocio hsNeg = new HorarioNegocio();
                var argument = ((Button)sender).CommandArgument;
                horariosSeleccionados.RemoveAll(x => x.Id == int.Parse(argument));


                repetidorHorarios.DataSource = null;
                repetidorHorarios.DataSource = horariosSeleccionados;
                repetidorHorarios.DataBind();
            }
            catch (Exception ex)
            {

                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }

        }

        protected void ddlTipoUsuario_DataBound(object sender, EventArgs e)
        {
            ddlTipoUsuario.Items.Insert(0, "--Seleccione un Perfil--");
        }

        //protected void ddlHorarios_DataBound(object sender, EventArgs e)
        //{
        //    ddlHorarios.Items.Insert(0, "--Seleccione un Horario--");
        //}

        protected void ddlEspecialidades_DataBound(object sender, EventArgs e)
        {
            ddlEspecialidades.Items.Insert(0, "--Seleccione una especialidad--");
        }


        protected void btnEliminarEspecialidad_Click(object sender, EventArgs e)
        {
            try
            {
                var argument = ((Button)sender).CommandArgument;
                especialidadesSeleccionadas.RemoveAll(x => x.Id == int.Parse(argument));


                repetidorEspecialidades.DataSource = null;
                repetidorEspecialidades.DataSource = especialidadesSeleccionadas;
                repetidorEspecialidades.DataBind();
            }
            catch (Exception ex)
            {

                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }

        }

        protected void ddlEspecialidades_SelectedIndexChanged1(object sender, EventArgs e)
        {
            try
            {
                EspecialidadNegocio espeNeg = new EspecialidadNegocio();
                Especialidad espe = espeNeg.Listar().FirstOrDefault(x => x.Id == int.Parse(ddlEspecialidades.SelectedValue));
                bool estaEnLaLista = especialidadesSeleccionadas.Any(x => x.Id == espe.Id);

                if (!estaEnLaLista)
                {
                    especialidadesSeleccionadas.Add(espe);
                    Session.Add("listaEspecialidades", especialidadesSeleccionadas);
                    repetidorEspecialidades.DataSource = especialidadesSeleccionadas;
                    repetidorEspecialidades.DataBind();
                }
            }
            catch (Exception ex)
            {

                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }

        }

        protected void btnGuardarMedico_Click(object sender, EventArgs e)
        {
            try
            {
                listErrores = new List<string>();
                if (Request.QueryString["Id"] == null)
                {
                    
                    Validar(txtNombre.Text, txtApellido.Text, txtDni.Text, txtEmail.Text, ObtenerSexo(chkHombre, chkMujer), txtUsuario.Text, ddlTipoUsuario.SelectedValue.ToString());
                    if (listErrores.Count == 0)
                    {
                        string password = GenerarPassword(txtEmail.Text);
                        Medico med = new Medico
                        {
                            Nombre = txtNombre.Text,
                            Apellido = txtApellido.Text,
                            DNI = txtDni.Text,
                            Email = txtEmail.Text,
                            Sexo = ObtenerSexo(chkHombre, chkMujer),
                            usuario = obtenerUsuario(txtUsuario.Text, password, int.Parse(ddlTipoUsuario.SelectedValue)),
                            listHorarios = (List<Horario>)Session["listaHorarios"],
                            listEspecialidades = (List<Especialidad>)Session["listaEspecialidades"]
                        };

                        GuardarMedico(med);
                        EmailServices emailAvisoPassword = new EmailServices();
                        emailAvisoPassword.EnvioPasswordAsignada(med.Email.Trim(), "Password", ObtenerCuerpoMailPasswordAsignada(med.usuario.Contrasenia));
                        emailAvisoPassword.enviarEmail();
                        string script = "alert('Agregado correctamente'); window.location.href = 'Medicos.aspx';";
                        ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", script, true);
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
                else
                {
                    Validar(txtNombre.Text, txtApellido.Text, txtDni.Text, txtEmail.Text, ObtenerSexo(chkHombre, chkMujer), txtUsuario.Text, ddlTipoUsuario.SelectedValue.ToString());
                    if (listErrores.Count == 0)
                    {
                        
                        MedicoNegocio medNeg = new MedicoNegocio();
                        Medico medico = medNeg.Listar().FirstOrDefault(x => x.Id == int.Parse(Request.QueryString["Id"]));
                        List<Horario> nuevosHorarios = SeAgregaronHorarios(medico.listHorarios, (List<Horario>)Session["listaHorarios"]);
                        List<Horario> horarioEliminar = HorariosAEliminar(medico.listHorarios, (List<Horario>)Session["listaHorarios"]);

                        List<Especialidad> nuevasEspecialidades = SeAgregaronEspecialidades(medico.listEspecialidades, (List<Especialidad>)Session["listaEspecialidades"]);
                        List<Especialidad> especialidadesEliminar = EspecialidadesAEliminar(medico.listEspecialidades, (List<Especialidad>)Session["listaEspecialidades"]);
                        ValidarEspecialidadesYhorarios(medico, especialidadesEliminar, horarioEliminar);
                        if (listErrores.Count == 0)
                        {
                            medico.Nombre = txtNombre.Text;
                            medico.Apellido = txtApellido.Text;
                            medico.DNI = txtDni.Text;
                            medico.Email = txtEmail.Text;
                            medico.Sexo = ObtenerSexo(chkHombre, chkMujer);
                            medico.listHorarios = (List<Horario>)Session["listaHorarios"];
                            medico.listEspecialidades = (List<Especialidad>)Session["listaEspecialidades"];
                            //Realizar Update Persona
                            PersonaNegocio pNeg = new PersonaNegocio();
                            pNeg.Actualizar(medico);
                            //Realizar Update List Horarios 
                            if (nuevosHorarios.Count != 0 || horarioEliminar.Count !=0)
                            {
                                if (horarioEliminar.Count != 0)
                                {
                                    foreach (var item in horarioEliminar)
                                    {
                                        medNeg.EliminarHorariosXmedico(medico.Id, item);
                                    }
                                }
                                foreach (var item in nuevosHorarios)
                                {
                                    medNeg.AgregarHorario(medico.Id, item);
                                }
                            }

                            if (nuevasEspecialidades.Count != 0 || especialidadesEliminar.Count!=0)
                            {
                                if (especialidadesEliminar.Count != 0)
                                {
                                    foreach (var item in especialidadesEliminar)
                                    {
                                        medNeg.EliminarEspecilidadXmedico(medico.Id, item);
                                    }
                                }

                                foreach (var item in nuevasEspecialidades)
                                {
                                    medNeg.AgregarEspecialidad(medico.Id, item);
                                }
                            }
                            string script = "alert('Actualizado correctamente'); window.location.href = 'Medicos.aspx';";
                            ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", script, true);
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


        #region funcionesPrivadas

        private string GenerarPassword(string user)
        {
            string[] partes = user.Split('@'); // Dividir el correo en dos partes usando el '@'
            if (partes.Length == 2) // Verificar si hay exactamente una parte después del '@'
            {
                string dominio = partes[0]; // Obtener la parte del dominio
                string dominioDesordenado = DesordenarString(dominio); // Desordenar el dominio
                return dominioDesordenado;
            }
            return null;
        }
        private string DesordenarString(string texto)
        {
            Random rnd = new Random();
            char[] caracteres = texto.ToCharArray();

            int n = caracteres.Length;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                var valor = caracteres[k];
                caracteres[k] = caracteres[n];
                caracteres[n] = valor;
            }

            return new string(caracteres);
        }
        private void ValidarEspecialidadesYhorarios(Medico medico, List<Especialidad>especialidadesEliminar, List<Horario> horarioEliminar)
        {
            TurnoNegocio turNeg = new TurnoNegocio();
            foreach (var item in especialidadesEliminar)
            {
                var turnoXEspecialidad = turNeg.Listar().FirstOrDefault(x => x.medico.Id == medico.Id && x.especialidad.Id == item.Id);
                if (turnoXEspecialidad != null && (turnoXEspecialidad.estado.estado == "Programado" || turnoXEspecialidad.estado.estado == "En curso")){
                    listErrores.Add("La especialidad que desea eliminar, tiene un turno programado o en curso");
                }

            }
            List<Turno> listTurnos = (List<Turno>)turNeg.Listar().Where(x=> x.medico.Id == medico.Id).ToList();
            listTurnos.Where(x => x.estado.estado == "Programado" || x.estado.estado == "En curso").ToList();
            foreach (var item in horarioEliminar)
            {
                foreach (var turno in listTurnos)
                {
                    if (ObtenerDia(turno.fecha) == item.Dia)
                    {
                        listErrores.Add("El medico, tiene un turno pendiente para con el dia a eliminar.");
                        break;
                    }

                }
            }
        }

        private string ObtenerDia(DateTime fecha)
        {
            DayOfWeek diaSemana = fecha.DayOfWeek;

            // Convertimos el DayOfWeek a su representación en texto
            string nombreDia = "";

            switch (diaSemana)
            {
                case DayOfWeek.Sunday:
                    nombreDia = "Domingo";
                    break;
                case DayOfWeek.Monday:
                    nombreDia = "Lunes";
                    break;
                case DayOfWeek.Tuesday:
                    nombreDia = "Martes";
                    break;
                case DayOfWeek.Wednesday:
                    nombreDia = "Miercoles";
                    break;
                case DayOfWeek.Thursday:
                    nombreDia = "Jueves";
                    break;
                case DayOfWeek.Friday:
                    nombreDia = "Viernes";
                    break;
                case DayOfWeek.Saturday:
                    nombreDia = "Sabado";
                    break;
                default:
                    break;
            }

            return nombreDia;
        }
        private void GuardarMedico(Medico medico)
        {
            medico.usuario.Id = GenerarUsuario(medico.usuario);
            medico.IdPersona = GenerarPersona(medico);
            medico.Id = GenerarMedico(medico);

            GuardarHorarios(medico);
            GuardarEspecialidades(medico);
        }
        private void GuardarEspecialidades(Medico med)
        {
            MedicoNegocio medNeg = new MedicoNegocio();
            foreach (Especialidad item in med.listEspecialidades)
            {
                medNeg.AgregarEspecialidad(med.Id, item);
            }
        }
        private void GuardarHorarios(Medico med)
        {
            MedicoNegocio medNeg = new MedicoNegocio();
            foreach (Horario item in med.listHorarios)
            {
                medNeg.AgregarHorario(med.Id, item);
            }
        }
        private int GenerarMedico(Medico med)
        {
            MedicoNegocio medNeg = new MedicoNegocio();
            medNeg.Agregar(med);

            return medNeg.Listar().Last().Id;


        }
        private int GenerarPersona(Medico med)
        {

            PersonaNegocio perNeg = new PersonaNegocio();
            perNeg.Agregar(med);
            Persona per = perNeg.Listar().Last();
            return per.IdPersona;
            
        }
        private int GenerarUsuario(Usuario us)
        {
            UsuarioNegocio usNeg = new UsuarioNegocio();
            usNeg.Guardar(us);

            Usuario usuario = usNeg.listar().Last();
            return usuario.Id;
        }
        private void Validar(string nombre, string apellido, string dni, string email, string sexo, string usuario,string IDTipouser)
        {
            if (Request.QueryString["Id"] != null)
            {
                if ((string.IsNullOrEmpty(nombre) && string.IsNullOrEmpty(apellido) && string.IsNullOrEmpty(dni) && string.IsNullOrEmpty(email) && string.IsNullOrEmpty(sexo) && string.IsNullOrEmpty(IDTipouser)))
                {

                    listErrores.Add("Revise los campos a completar....");
                    
                }
                else
                {
                    if (!email.Contains("@")) listErrores.Add("El mail es incorrecto...");
                    ValidarDni(dni);
                }

            }
            else
            {
                if ((string.IsNullOrEmpty(nombre) && string.IsNullOrEmpty(apellido) && string.IsNullOrEmpty(dni) && string.IsNullOrEmpty(email) && string.IsNullOrEmpty(sexo) && string.IsNullOrEmpty(usuario) && string.IsNullOrEmpty(IDTipouser)))
                {

                    listErrores.Add("Revise los campos a completar....");
                }
                else
                {
                    ValidarDni(dni);
                    if (!email.Contains("@")) listErrores.Add("El mail es incorrecto...");
                    PersonaNegocio perNeg = new PersonaNegocio();
                    var per = perNeg.Listar().FirstOrDefault(x => x.Email.Trim().ToUpper() == email.Trim().ToUpper());
                    if (per != null) listErrores.Add("Ya existe un usuario con el mail ingresado");
                    per = null;
                    per = perNeg.Listar().Where(x=> x.usuario != null).FirstOrDefault(x => x.usuario.Nombre.Trim().Equals(usuario.Trim()));
                    if (per != null) listErrores.Add("Ya existe el nombre de usuario");


                }
            }

        }
        private Usuario obtenerUsuario(string user, string pass, int IdPerfil)
        {
            Usuario usuario = new Usuario();
            usuario.Nombre = user;
            usuario.Contrasenia = pass;
            usuario.perfil = obtenerPerfil(IdPerfil);
            return usuario;

        }
        private TipoUsuario obtenerPerfil(int id)
        {
            TipoUsuarioNegocio tipoUserNeg = new TipoUsuarioNegocio();
            TipoUsuario tipo = tipoUserNeg.ObtenerTipos().FirstOrDefault(x => x.Id == id);
            return tipo;
        }
        private string ObtenerSexo(RadioButton chkHombre, RadioButton chkMujer)
        {
            if (chkHombre.Checked) return "H";
            else return "M";

        }
        private void ValidarDni(string Dni)
        {

            if (ValidaNumeros(Dni) && Request.QueryString["Id"] == null)
            {
                PersonaNegocio pNeg = new PersonaNegocio();
                Persona personas = pNeg.Listar().FirstOrDefault(x => x.DNI == Dni);
                if (personas != null) listErrores.Add("El Dni ingresado ya existe...");
            }
        }
        private bool ValidaNumeros(string dni)
        {
            try
            {
                int num = int.Parse(dni);
                return true;
            }
            catch (Exception)
            {

                listErrores.Add("Recuerde que el DNI deben ser numeros sin puntos...");
                return false;
            }
        }


        private List<Horario> SeAgregaronHorarios(List<Horario> horariosMedico, List<Horario> horariosNuevos)
        {
            List<Horario> horarios = new List<Horario>();
            if (HuboModificacionHorarios(horariosMedico, horariosNuevos))
            {
                foreach (var item in horariosNuevos)
                {
                    if (!(horariosMedico.Any(e => e.Id == item.Id)))
                    {
                        horarios.Add(item);
                    }
                }
            }


            return horarios;
        }

        private bool HuboModificacionHorarios(List<Horario> horariosMedico, List<Horario> horariosNuevos)
        {
            foreach (var item in horariosNuevos)
            {
                if (!(horariosMedico.Any(e => e.Id == item.Id)))
                {
                    return true;
                }

            }
            return false;
        }

        private List<Horario> HorariosAEliminar(List<Horario> horariosMedico, List<Horario> horariosNuevos)
        {
            List<Horario> horariosNoEncontrados = new List<Horario>();
            foreach (var horario in horariosMedico)
            {
                bool encontrado = false;
                foreach (var nuevoHorario in horariosNuevos)
                {
                    if (horario.Id == nuevoHorario.Id)
                    {
                        encontrado = true;
                        break;
                    }
                }

                if (!encontrado)
                {
                    horariosNoEncontrados.Add(horario);
                }
            }
            return horariosNoEncontrados;
        }


        private List<Especialidad> SeAgregaronEspecialidades(List<Especialidad> EspecialidadesMedico, List<Especialidad> especialidadesNuevas)
        {
            List<Especialidad> horarios = new List<Especialidad>();
            if (HuboModificacionEspecialidades(EspecialidadesMedico, especialidadesNuevas))
            {
                foreach (var item in especialidadesNuevas)
                {
                    if (!(EspecialidadesMedico.Any(e => e.Id == item.Id)))
                    {
                        horarios.Add(item);
                    }
                }
            }


            return horarios;
        }

        private bool HuboModificacionEspecialidades(List<Especialidad> EspecialidadMedico, List<Especialidad> EspecialidadNuevos)
        {
            foreach (var item in EspecialidadNuevos)
            {
                if (!(EspecialidadMedico.Any(e => e.Id == item.Id)))
                {
                    return true;
                }

            }
            return false;
        }

        private List<Especialidad> EspecialidadesAEliminar(List<Especialidad> especialidadesMedico, List<Especialidad> especialidadesNuevas)
        {
            List<Especialidad> EspecialidadesNoEncontrados = new List<Especialidad>();
            foreach (var horario in especialidadesMedico)
            {
                bool encontrado = false;
                foreach (var nuevoHorario in especialidadesNuevas)
                {
                    if (horario.Id == nuevoHorario.Id)
                    {
                        encontrado = true;
                        break;
                    }
                }

                if (!encontrado)
                {
                    EspecialidadesNoEncontrados.Add(horario);
                }
            }
            return EspecialidadesNoEncontrados;
        }


        private string ObtenerCuerpoMailPasswordAsignada(string pass)
        {
            return "<!DOCTYPE html><html><head>    <title>Clínica Médica</title>    <style>        body {            background-color: #d0e7f9; /* Celeste */            font-family: Arial, sans-serif;           margin: 0;            padding: 20px;        }        .container {            max-width: 600px;            margin: 0 auto;            background-color: #fff;            padding: 20px;            border-radius: 8px;            box-shadow: 0 0 10px rgba(0,0,0,0.1);       }        h1 {            text-align: center;            color: #333;        }        .password {            text-align: center;            font-size: 24px;            margin-top: 20px;            color: #007bff; /* Azul */        }    </style></head><body>    <div class=\"container\">        <h1>Clínica Médica</h1>        <p>Su contraseña para iniciar sesión es: "+ pass + "</p>           </div></body></html>";
        }
        #endregion
    }
}