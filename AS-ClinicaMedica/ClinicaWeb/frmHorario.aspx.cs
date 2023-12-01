using Dominio;
using Negocio;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace ClinicaWeb
{
    public partial class frmHorario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogueado"] == null) Response.Redirect("Login.aspx", false);
            string headingText;
            if (!IsPostBack)
            {
                if (Request.QueryString["Id"] != null)
                {
                    headingText = "<h1>Modificacion Horario</h1>";
                    HorarioNegocio hsNeg = new HorarioNegocio();
                    Horario horario = hsNeg.Listar().FirstOrDefault(x => x.Id == int.Parse(Request.QueryString["Id"]));
                    DataTable dt = ObtenerIndicesPorFecha(horario);
                    ddlDia.SelectedValue = dt.Rows[0]["DIA"].ToString(); 
                    ddlHorarioInicio.SelectedValue = dt.Rows[0]["INICIO"].ToString();
                    ddlHorarioFin.SelectedValue = dt.Rows[0]["FIN"].ToString(); 


                }
                else
                {
                    headingText = "<h1>Nuevo Horario</h1>";
                }
                ltlHeading.Text = headingText;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)

        {
            try
            {
                if (validarHorario(ddlHorarioInicio.SelectedItem, ddlHorarioFin.SelectedItem))
                {
                    HorarioNegocio hsNeg = new HorarioNegocio();
                    Horario hs = new Horario
                    {
                        Dia = ddlDia.SelectedItem.Text,
                        HoraInicio = ddlHorarioInicio.SelectedItem.Text,
                        HoraFin = ddlHorarioFin.SelectedItem.Text
                    };
                    if (Request.QueryString["Id"] == null)
                    {
                        hsNeg.Agregar(hs);
                        string script = "alert('Agregado correctamente'); window.location.href = 'Horarios.aspx';";
                        ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", script, true);
                    }
                    else
                    {
                        hs.Id = int.Parse(Request.QueryString["Id"].ToString());
                        hsNeg.Actualizar(hs);
                        string script = "alert('Actualizado correctamente'); window.location.href = 'Horarios.aspx';";
                        ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", script, true);
                    }
                }
                else lblConfirmacionHorario.Text = "Por favor, revice el horario cargado...";
            }
            catch (Exception ex)
            {


                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }

        }

        private bool validarHorario(ListItem hsInicio, ListItem hsFin)
        {
            bool horariosCorrectos = validarHorarios(hsInicio.Text, hsFin.Text);
            if (!horariosCorrectos) return false;

            return true;
        }

        private bool validarHorarios(string inicio, string fin)
        {
            string timeFormat = "HH:mm";
            DateTime hsInicio = DateTime.ParseExact(inicio, timeFormat, null);
            DateTime hsFin = DateTime.ParseExact(fin, timeFormat, null);

            if (hsInicio.Hour < hsFin.Hour) return true;
            return false;

        }
        protected void ddlHorarioFin_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = ddlHorarioFin.SelectedValue;
            var DaySelected = ddlHorarioFin.SelectedItem;
        }
        private DataTable ObtenerIndicesPorFecha(Horario hs)
        {
            DataTable result = new DataTable();
            result.Columns.Add("DIA", typeof(int));
            result.Columns.Add("INICIO", typeof(int));
            result.Columns.Add("FIN", typeof(int));
            DataRow newRow1 = result.NewRow();
            result = SeteoDia(newRow1, hs, result);
            result = seteoHorario(newRow1, hs, result);

            return result;

        }


        private DataTable seteoHorario(DataRow dt, Horario hs, DataTable dtable)
        {

            switch (hs.HoraInicio)
            {
                case "00:00":
                    dtable.Rows[0]["INICIO"] = 0;
                    break;
                case "01:00":
                    dtable.Rows[0]["INICIO"] = 1;
                    break;
                case "02:00":
                    dtable.Rows[0]["INICIO"] = 2;
                    break;
                case "03:00":
                    dtable.Rows[0]["INICIO"] = 3;
                    break;
                case "04:00":
                    dtable.Rows[0]["INICIO"] = 4;
                    break;
                case "05:00":
                    dtable.Rows[0]["INICIO"] = 5;
                    break;
                case "06:00":
                    dtable.Rows[0]["INICIO"] = 6;
                    break;
                case "07:00":
                    dtable.Rows[0]["INICIO"] = 7;
                    break;
                case "08:00":
                    dtable.Rows[0]["INICIO"] = 8;
                    break;
                case "09:00":
                    dtable.Rows[0]["INICIO"] = 9;
                    break;
                case "10:00":
                    dtable.Rows[0]["INICIO"] = 10;
                    break;
                case "11:00":
                    dtable.Rows[0]["INICIO"] = 11;
                    break;
                case "12:00":
                    dtable.Rows[0]["INICIO"] = 12;
                    break;
                case "13:00":
                    dtable.Rows[0]["INICIO"] = 13;
                    break;
                case "14:00":
                    dtable.Rows[0]["INICIO"] = 14;
                    break;
                case "15:00":
                    dtable.Rows[0]["INICIO"] = 15;
                    break;
                case "16:00":
                    dtable.Rows[0]["INICIO"] = 16;
                    break;
                case "17:00":
                    dtable.Rows[0]["INICIO"] = 17;
                    break;
                case "18:00":
                    dtable.Rows[0]["INICIO"] = 18;
                    break;
                case "19:00":
                    dtable.Rows[0]["INICIO"] = 19;
                    break;
                case "20:00":
                    dtable.Rows[0]["INICIO"] = 20;
                    break;
                case "21:00":
                    dtable.Rows[0]["INICIO"] = 21;
                    break;
                case "22:00":
                    dtable.Rows[0]["INICIO"] = 22;
                    break;
                case "23:00":
                    dtable.Rows[0]["INICIO"] =  23;
                    break;
            }
           

            switch (hs.HoraFin)
            {
                case "00:00":
                    dtable.Rows[0]["FIN"] = 0;
                    break;
                case "01:00":
                    dtable.Rows[0]["FIN"] = 1;
                    break;
                case "02:00":
                    dtable.Rows[0]["FIN"] = 2;
                    break;
                case "03:00":
                    dtable.Rows[0]["FIN"] = 3;
                    break;
                case "04:00":
                    dtable.Rows[0]["FIN"] = 4;
                    break;
                case "05:00":
                    dtable.Rows[0]["FIN"] = 5;
                    break;
                case "06:00":
                    dtable.Rows[0]["FIN"] = 6;
                    break;
                case "07:00":
                    dtable.Rows[0]["FIN"] = 7;
                    break;
                case "08:00":
                    dtable.Rows[0]["FIN"] = 8;
                    break;
                case "09:00":
                    dtable.Rows[0]["FIN"] = 9;
                    break;
                case "10:00":
                    dtable.Rows[0]["FIN"] = 10;
                    break;
                case "11:00":
                    dtable.Rows[0]["FIN"] = 11;
                    break;
                case "12:00":
                    dtable.Rows[0]["FIN"] = 12;
                    break;
                case "13:00":
                    dtable.Rows[0]["FIN"] = 13;
                    break;
                case "14:00":
                    dtable.Rows[0]["FIN"] = 14;
                    break;
                case "15:00":
                    dtable.Rows[0]["FIN"] = 15;
                    break;
                case "16:00":
                    dtable.Rows[0]["FIN"] = 16;
                    break;
                case "17:00":
                    dtable.Rows[0]["FIN"] = 17;
                    break;
                case "18:00":
                    dtable.Rows[0]["FIN"] = 18;
                    break;
                case "19:00":
                    dtable.Rows[0]["FIN"] = 19;
                    break;
                case "20:00":
                    dtable.Rows[0]["FIN"] = 20;
                    break;
                case "21:00":
                    dtable.Rows[0]["FIN"] = 21;
                    break;
                case "22:00":
                    dtable.Rows[0]["FIN"] = 22;
                    break;
                case "23:00":
                    dtable.Rows[0]["FIN"] = 23;
                    break;
            }
            
            return dtable;
        }
        private DataTable SeteoDia(DataRow dt, Horario hs, DataTable dtable)
        {
            switch (hs.Dia)
            {
                case "Lunes":
                    dt["DIA"] = 1;
                    break;
                case "Martes":
                    dt["DIA"] = 2;
                    break;
                case "Miercoles":
                    dt["DIA"] = 3;
                    break;
                case "Jueves":
                    dt["DIA"] = 4;
                    break;
                case "Viernes":
                    dt["DIA"] = 5;
                    break;
                case "Sabado":
                    dt["DIA"] = 6;
                    break;
                case "Domingo":
                    dt["DIA"] = 7;
                    break;
            }
            dtable.Rows.Add(dt);
            return dtable;
        }

        protected void ddlDia_DataBound(object sender, EventArgs e)
        {
            ddlDia.Items.Insert(0, "--Seleccione un Dia--");
        }

        protected void ddlHorarioInicio_DataBound(object sender, EventArgs e)
        {
            ddlHorarioInicio.Items.Insert(0, "--Seleccione un Horario--");
        }

        protected void ddlHorarioFin_DataBound(object sender, EventArgs e)
        {
            ddlHorarioFin.Items.Insert(0, "--Seleccione un Horario--");
        }
    }
}