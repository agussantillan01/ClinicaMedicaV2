<%@ Page Title="" Language="C#" MasterPageFile="~/webFormLogin.Master" AutoEventWireup="true" CodeBehind="forgotPassword.aspx.cs" Inherits="ClinicaWeb.forgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .red {
            color: red;
        }
    </style>
    <div class="row">
        <div class="col-4"></div>
        <div class="col-4">
            <div style="padding-top: 30%;">
                <h3 style="color: black; font-family: Arial;">Recuperar Contraseña</h3>
                <asp:Label Text="Email" CssClass="form-label" runat="server" />
                <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" />
                <br />
                <asp:Button ID="btnEnviarMail" runat="server" Text="Enviar Mail" CssClass="btn btn-primary" OnClick="btnEnviarMail_Click" />
                <br />
                <asp:Label Text="Password" ID="tituloPassword1" CssClass="form-label" runat="server" />
                <asp:TextBox ID="txtPassword1" CssClass="form-control" TextMode="Password" runat="server" />
                <br />
                <asp:Label Text="Escriba nuevamente la clave" ID="tituloPassword2" CssClass="form-label" runat="server" />
                <asp:TextBox ID="txtPassword2" CssClass="form-control" TextMode="Password" runat="server" />
                <br />
                <br />
                <asp:Button ID="btnRenovarClave" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnRenovarClave_Click" />
                <br />

            </div>

            <div>
                <asp:Label Text="" ID="lblErrores" CssClass="red" runat="server" />
            </div>
        </div>
        <div class="col-4"></div>
    </div>
</asp:Content>
