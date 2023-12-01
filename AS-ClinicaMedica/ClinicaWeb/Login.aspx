<%@ Page Title="" Language="C#" MasterPageFile="~/webFormLogin.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ClinicaWeb.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .custom-button {
            background-color: #8E44AD;
            color: white;
            border-color: #8E44AD;
        }

            .custom-button:hover {
                background-color: #6C3483;
                border-color: #6C3483;
            }
    </style>

    <div class="row">
        <div class="col-6">
            <img src="../img/imgLogin.jpg" style="width: 100%;" alt="Alternate Text" />
        </div>
        <div class="col-4">
            <div style="padding-top: 30%;">
                <h3 style="color: grey; font-family: Arial;">Logueate</h3>
                <asp:Label Text="Usuario" CssClass="form-label" runat="server" />
                <asp:TextBox ID="txtUsuario" CssClass="form-control" runat="server" />
                <asp:Label Text="Password" CssClass="form-label" runat="server" />
                <asp:TextBox ID="txtPassword" CssClass="form-control" TextMode="Password" runat="server" />
                <div style="color: red;">
                    <asp:Label Text="" ID="lblError" runat="server" />
                </div>
                <br />
                <br />
                <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" CssClass="btn custom-button" OnClick="btnIngresar_Click" />
                <br />
                <a href="forgotPassword.aspx">Recuperar Contraseña</a>
            </div>
        </div>
    </div>
</asp:Content>
