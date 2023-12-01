<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="ClinicaWeb.Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-2"></div>
        <div class="col-2">
                <%--<div class="error">--%>
                <div class="error">
                    <h1>Opssss !!! Algo ocurrio</h1>
                    <p><%: errorInformado.ToString() %></p>
                    <%--<asp:Button ID="btnError" runat="server" Text="Volver al inicio" CssClass="boton" />--%>
                </div>

        </div>
        <div class="col-2"></div>
    </div>

</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>--%>
