<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="frmEstado.aspx.cs" Inherits="ClinicaWeb.frmEstado" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <asp:Literal ID="ltlHeading" runat="server"></asp:Literal>
        <div class="col-4">
            <div class="mb-3">
                <asp:Label Text="Estado" CssClass="form-label" runat="server" />
                <asp:TextBox ID="txtEstado" placeHolder="Estado..." CssClass="form-control" runat="server" />
            </div>
            <div class="mb-3"></div>
            <div class="mb-3">
                <asp:Button CssClass="btn btn-primary" ID="btnAgregarEstado" OnClick="btnAgregarEstado_Click" Text="Guardar" runat="server" />
            </div>
            
        </div>
        <asp:Label ID="lblEstadoConfirmacion" Text="" ForeColor="Green"  runat="server" />
    </div>

</asp:Content>
