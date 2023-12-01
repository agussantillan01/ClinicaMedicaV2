<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="frmHorario.aspx.cs" Inherits="ClinicaWeb.frmHorario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <asp:Literal ID="ltlHeading" runat="server"></asp:Literal>
        <div class="col-4">
            <div class="mb-3">
                <asp:Label Text="Dia" runat="server" />
                <asp:DropDownList runat="server" OnDataBound="ddlDia_DataBound" AutoPostBack="true" ID="ddlDia" CssClass="form-select">
                    <asp:ListItem Text="Lunes" Value="1" />
                    <asp:ListItem Text="Martes" Value="2" />
                    <asp:ListItem Text="Miercoles" Value="3" />
                    <asp:ListItem Text="Jueves" Value="4" />
                    <asp:ListItem Text="Viernes" Value="5" />
                    <asp:ListItem Text="Sabado" Value="6" />
                    <asp:ListItem Text="Domingo" Value="7" />
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-4">
            <div class="mb-3">
                <asp:Label Text="Horario Inicio" runat="server" />
                <asp:DropDownList runat="server" OnDataBound="ddlHorarioInicio_DataBound" AutoPostBack="true" ID="ddlHorarioInicio" CssClass="form-select">
                    <asp:ListItem Text="00:00" Value="0" />
                    <asp:ListItem Text="01:00" Value="1" />
                    <asp:ListItem Text="02:00" Value="2" />
                    <asp:ListItem Text="03:00" Value="3" />
                    <asp:ListItem Text="04:00" Value="4" />
                    <asp:ListItem Text="05:00" Value="5" />
                    <asp:ListItem Text="06:00" Value="6" />
                    <asp:ListItem Text="07:00" Value="7" />
                    <asp:ListItem Text="08:00" Value="8" />
                    <asp:ListItem Text="09:00" Value="9" />
                    <asp:ListItem Text="10:00" Value="10" />
                    <asp:ListItem Text="11:00" Value="11" />
                    <asp:ListItem Text="12:00" Value="12" />
                    <asp:ListItem Text="13:00" Value="13" />
                    <asp:ListItem Text="14:00" Value="14" />
                    <asp:ListItem Text="15:00" Value="15" />
                    <asp:ListItem Text="16:00" Value="16" />
                    <asp:ListItem Text="17:00" Value="17" />
                    <asp:ListItem Text="18:00" Value="18" />
                    <asp:ListItem Text="19:00" Value="19" />
                    <asp:ListItem Text="20:00" Value="20" />
                    <asp:ListItem Text="21:00" Value="21" />
                    <asp:ListItem Text="22:00" Value="22" />
                    <asp:ListItem Text="23:00" Value="23" />
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-4">
            <div class="mb-3">
                <asp:Label Text="Horario Fin" runat="server" />
                <asp:DropDownList runat="server" AutoPostBack="true" OnDataBound="ddlHorarioFin_DataBound" ID="ddlHorarioFin" OnSelectedIndexChanged="ddlHorarioFin_SelectedIndexChanged" CssClass="form-select">
                    <asp:ListItem Text="00:00" Value="0" />
                    <asp:ListItem Text="01:00" Value="1" />
                    <asp:ListItem Text="02:00" Value="2" />
                    <asp:ListItem Text="03:00" Value="3" />
                    <asp:ListItem Text="04:00" Value="4" />
                    <asp:ListItem Text="05:00" Value="5" />
                    <asp:ListItem Text="06:00" Value="6" />
                    <asp:ListItem Text="07:00" Value="7" />
                    <asp:ListItem Text="08:00" Value="8" />
                    <asp:ListItem Text="09:00" Value="9" />
                    <asp:ListItem Text="10:00" Value="10" />
                    <asp:ListItem Text="11:00" Value="11" />
                    <asp:ListItem Text="12:00" Value="12" />
                    <asp:ListItem Text="13:00" Value="13" />
                    <asp:ListItem Text="14:00" Value="14" />
                    <asp:ListItem Text="15:00" Value="15" />
                    <asp:ListItem Text="16:00" Value="16" />
                    <asp:ListItem Text="17:00" Value="17" />
                    <asp:ListItem Text="18:00" Value="18" />
                    <asp:ListItem Text="19:00" Value="19" />
                    <asp:ListItem Text="20:00" Value="20" />
                    <asp:ListItem Text="21:00" Value="21" />
                    <asp:ListItem Text="22:00" Value="22" />
                    <asp:ListItem Text="23:00" Value="23" />
                </asp:DropDownList>
            </div>
        </div>

        <div class="col-4">
            <div class="mb-3">
                <asp:Button Text="Guardar" CssClass="btn btn-primary" ID="btnGuardar" OnClick="btnGuardar_Click" runat="server" />
            </div>
            <div class="mb-3">
                <asp:Label Text="" ForeColor="Green" ID="lblConfirmacionHorario" runat="server" />
            </div>
        </div>


    </div>

</asp:Content>
