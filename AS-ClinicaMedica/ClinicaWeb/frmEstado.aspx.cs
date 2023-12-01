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
    public partial class frmEstado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogueado"] == null) Response.Redirect("Login.aspx", false);
            string headingText;
            if (!IsPostBack)
            {
                
                if (Request.QueryString["Id"] != null)
                {
                    headingText = "<h1>Modificacion Estado</h1>";
                    EstadoNegocio esNeg = new EstadoNegocio();
                    Estado es = esNeg.listar().FirstOrDefault(x => x.Id == int.Parse(Request.QueryString["Id"]));
                    txtEstado.Text = es.estado.ToString();
                }
                else
                {
                     headingText = "<h1>Nuevo Estado</h1>";
                    
                }
                ltlHeading.Text = headingText;
            }
            
        }

        protected void btnAgregarEstado_Click(object sender, EventArgs e)
        {
            try
            {
                Dominio.Estado es;
                if (txtEstado.Text != "")
                {
                    if (validarEstado(txtEstado.Text.ToUpper()))
                    {
                        if (Request.QueryString.ToString() == "")
                        {
                            es = new Estado
                            {
                                estado = txtEstado.Text.ToString(),
                            };
                            lblEstadoConfirmacion.Text = "Estado agregado correctamente...";
                            Negocio.EstadoNegocio neg = new Negocio.EstadoNegocio();
                            neg.Agregar(es);
                            string script = "alert('Agregado correctamente'); window.location.href = 'Estados.aspx';";
                            ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", script, true);
                        }
                        else
                        {
                            es = new Estado
                            {
                                Id = int.Parse(Request.QueryString["Id"]),
                                estado = txtEstado.Text.ToString()
                            };
                            lblEstadoConfirmacion.Text = "Estado actualizado correctamente...";
                            Negocio.EstadoNegocio neg = new Negocio.EstadoNegocio();
                            neg.Actualizar(es);
                            string script = "alert('Agregado correctamente'); window.location.href = 'Estados.aspx';";
                            ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", script, true);
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

        private bool validarEstado(string estado)
        {
                EstadoNegocio estadoNegocio = new EstadoNegocio();
                List<Estado> lstEstados = estadoNegocio.listar();

            Estado es = lstEstados.FirstOrDefault(x=> x.estado.ToUpper() == estado);
                if (es == null) return true;
                return false;
         


        }
    }
}