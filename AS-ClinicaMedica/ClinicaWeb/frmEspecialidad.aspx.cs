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
    public partial class frmEspecialidad : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogueado"] == null) Response.Redirect("Login.aspx", false);
            string headingText;
            if (!IsPostBack)
            {
                if (Request.QueryString["Id"] != null)
                {
                    EspecialidadNegocio espeNeg = new EspecialidadNegocio();
                    Especialidad espe = espeNeg.Listar().FirstOrDefault(x => x.Id == int.Parse(Request.QueryString["Id"]));
                    txtEspecialidad.Text = espe.especialidad;
                    headingText = "<h1>Modificacion Especialidad</h1>";
                }
                else
                {
                    headingText = "<h1>Nueva Especialidad</h1>";

                }
                ltlHeading.Text = headingText;
            }
        }

        protected void btnAgregarEspecialidad_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["Id"] == null)
                {
                    if (validarEspecialidadAgregada(txtEspecialidad.Text) && txtEspecialidad.Text != "")
                    {
                        EspecialidadNegocio espNegocio = new EspecialidadNegocio();
                        Especialidad especialidad = new Especialidad
                        {
                            especialidad = txtEspecialidad.Text,
                        };
                        espNegocio.Agregar(especialidad);
                        string script = "alert('Agregado correctamente'); window.location.href = 'Especialidades.aspx';";
                        ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", script, true);

                    }
                    else lblEspecialidadConfirmacion.Text = "Por favor,revise la especialidad agregada";
                }
                else
                {
                    Especialidad especialidad = new Especialidad
                    {
                        Id = int.Parse(Request.QueryString["Id"]),
                        especialidad = txtEspecialidad.Text,
                    };
                    if (validarEspecialidadModificada(especialidad))
                    {
                        EspecialidadNegocio espeNeg = new EspecialidadNegocio();
                        espeNeg.Actualizar(especialidad);
                        string script = "alert('Actualizado correctamente'); window.location.href = 'Especialidades.aspx';";
                        ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", script, true);
                    }


                }
            }
            catch (Exception ex)
            {

                Session.Add("Error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }


        }

        private bool validarEspecialidadModificada (Especialidad es)
        {
            EspecialidadNegocio espeNeg = new EspecialidadNegocio();
            Especialidad espe = espeNeg.Listar().FirstOrDefault(x => x.especialidad.ToUpper() == es.especialidad.ToUpper() && x.Id != es.Id);
            if (espe != null) return false;
            return true;
        }
        private bool validarEspecialidadAgregada (string espe)
        {
            EspecialidadNegocio especialidadNegocio = new EspecialidadNegocio();
            Especialidad especialidad = especialidadNegocio.Listar().FirstOrDefault(x => x.especialidad.ToUpper() == espe.ToUpper());
            if (especialidad != null) return false;
            return true;
        }
    }
}