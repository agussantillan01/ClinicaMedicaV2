<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Turnos.aspx.cs" Inherits="ClinicaWeb.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
        .oculto {
            display: none;
        }

        .modal {
            display: none;
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            background-color: transparent;
            padding: 20px;
            z-index: 1000;
        }

        .overlay {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.7);
            z-index: 999;
        }

        .close {
            color: red; /* Cambia el color de la "x" a rojo */
            font-size: 20px;
            position: absolute;
            top: 0;
            right: 0;
            padding: 5px;
            cursor: pointer;
        }

            .close:hover {
                background-color: red; /* Agrega un fondo rojo cuando el cursor está sobre la "x" */
                color: white; /* Cambia el color del texto a blanco */
            }
    </style>

    <script>
        function showErrorModal() {
            document.getElementById('errorModal').style.display = 'block';
            document.getElementById('overlay').style.display = 'block';
        }

        function closeErrorModal() {
            document.getElementById('errorModal').style.display = 'none';
            document.getElementById('overlay').style.display = 'none';
        }

        function addErrorToList(errorText) {
            var errorList = document.getElementById('errorList');
            var errorItem = document.createElement('li');
            errorItem.textContent = errorText;
            errorList.appendChild(errorItem);
        }


    </script>

    <div>
        <h3>Administración de Turnos</h3>
    </div>

    <div class="row">
        <div class="col-2">
            <asp:TextBox ID="txtFiltroTurno" placeholder="Filtro numero..."  OnTextChanged="txtFiltroTurno_TextChanged" CssClass="form-control" AutoPostBack="true" runat="server" />
        </div>
        <div class="col-2">
            <asp:TextBox ID="txtFiltroPaciente" placeholder="Filtro paciente..." OnTextChanged="txtFiltroTurno_TextChanged" CssClass="form-control" AutoPostBack="true" runat="server" />
        </div>
        <div class="col-2">
            <asp:TextBox ID="txtFiltroEspecialidad" placeholder="Filtro especialidad..." OnTextChanged="txtFiltroTurno_TextChanged" CssClass="form-control" AutoPostBack="true" runat="server" />
        </div>
        <div class="col-2">
            <asp:TextBox ID="txtFiltroMedico" placeholder="Filtro Medico..." OnTextChanged="txtFiltroTurno_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server" />
        </div>
        <div class="col-2">
            <a class="btn btn-secondary" href="frmTurno.aspx" role="button">Nuevo Turnos</a>
        </div>
    </div>
    <br />
    <br />
    <div class="row">
        <div class="col">
            <asp:GridView runat="server" ID="dgvTurnos" DataKeyNames="Id" CssClass="table table-dark table-striped" AutoGenerateColumns="false" OnRowCommand="dgvTurnos_RowCommand" AllowPaging="true" PageSize="6" OnPageIndexChanging="dgvTurnos_PageIndexChanging" >
                <Columns>
                    <asp:BoundField HeaderText="Id" DataField="Id" HeaderStyle-CssClass="oculto" ItemStyle-CssClass="oculto" />
                    <asp:BoundField HeaderText="Paciente" DataField="paciente.Nombre" />
                    <asp:BoundField HeaderText="Medico" DataField="medico.Nombre" />
                    <asp:BoundField HeaderText="Fecha" DataField="Fecha" DataFormatString="{0:yyyy-MM-dd}"/>
                    <asp:BoundField HeaderText="Hora" DataField="Hora" />
                    <asp:BoundField HeaderText="Estado" DataField="Estado.estado" />
                    <asp:ButtonField ButtonType="Button" CommandName="Modificar" HeaderText="Modificar" Text="Modificar" ControlStyle-CssClass="btn btn-outline-light" />
<%--                    <asp:ButtonField ButtonType="Button" CommandName="Eliminar" HeaderText="Eliminar" Text="Eliminar" ControlStyle-CssClass="btn btn-outline-light" />--%>
                    <asp:ButtonField ButtonType="Button" CommandName="VerMas" HeaderText="Ver Todo" Text="Descargar PDF" ControlStyle-CssClass="btn btn-outline-dark" runat="server" />

                </Columns>
                <PagerSettings Mode="NumericFirstLast"
                    Position="Bottom"
                    PageButtonCount="10" />
                <PagerStyle BackColor="LightBlue"
                    Height="30px"
                    VerticalAlign="Bottom"
                    HorizontalAlign="Center" />
            </asp:GridView>
        </div>


        <div id="errorModal" class="modal">
            <div class="modal-content">
                <span class="close" onclick="closeErrorModal()">&times;</span>
                <p>Se ha producido un error.</p>
                <ul id="errorList"></ul>
            </div>
        </div>
        <div id="overlay" class="overlay"></div>


    </div>


</asp:Content>
