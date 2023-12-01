<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Reportes.aspx.cs" Inherits="ClinicaWeb.Reportes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .miTabla {
            width: 95%; /* Ancho del 95% de la pantalla */
            margin: 0 auto; /* Centrar horizontalmente */
            border-collapse: collapse;
        }

            .miTabla tr {
                border-bottom: 1px solid #000; /* Línea horizontal debajo de cada fila */
            }

            .miTabla th, .miTabla td {
                padding: 15px; /* Mayor espacio entre el contenido y el borde */
                text-align: center;
                margin-bottom: 10px; /* Espacio adicional entre las filas */
            }

                .miTabla th:first-child {
                    border-top: none; /* Elimina el borde superior de la primera fila (títulos de columna) */
                }

        .green-background {
            background-color: #ABEBC6;
        }

        .yellow-background {
            background-color: #F9E79F;
        }

        .red-background {
            background-color: #F5B7B1;
        }

        .verde {
            color: green;
        }
    </style>
    <div class="row">
        <div class="col-3">
            <asp:DropDownList runat="server" ID="ddlMedicos" CssClass="form-select" OnDataBound="ddlMedicos_DataBound">
            </asp:DropDownList>
        </div>
        <div class="col-3 text-end">
            <asp:Button Text="Aplicar" class="btn btn-secondary" ID="btnAplicarFiltro" OnClick="btnAplicarFiltro_Click" runat="server" />
        </div>
    </div>
    <br />
    <div class="row">

        <asp:Table ID="tablaDinamica" runat="server" CssClass="miTabla">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell>Estado</asp:TableHeaderCell>
                <asp:TableHeaderCell>Cantidad</asp:TableHeaderCell>
                <asp:TableHeaderCell>Porcentaje</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>

    </div>
    <br />
    <div class="row">
        <div class="col-8"></div>
        <div class="col-4">
            <div style="float: right;">
                <asp:Label Text="" ID="lblCantidadTurnos" CssClass="verde" runat="server" />
            </div>
        </div>
    </div>




</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>--%>
