<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="frmTurno.aspx.cs" Inherits="ClinicaWeb.frmTurno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .verde { 
            color:green;
        }
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
            .rojo { 
                color:red;
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
    <div class="row">
        <asp:Literal ID="ltlHeading" runat="server"></asp:Literal>
        <div class="col-4">
            <div class="mb-3">
                <asp:Label Text="Paciente" CssClass="form-label" runat="server" />
                <asp:DropDownList ID="ddlPaciente" OnDataBound="ddlPaciente_DataBound" CssClass="form-control" runat="server"></asp:DropDownList>
            </div>
            <div class="mb-3">
                <asp:Label Text="Especialidad" CssClass="form-label" runat="server" />
                <asp:DropDownList runat="server" ID="ddlEspecialidad" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlEspecialidad_SelectedIndexChanged" OnDataBound="ddlEspecialidad_DataBound"></asp:DropDownList>
            </div>
            <div class="mb-3">
                <asp:Label Text="Medico" ID="lblMedico" CssClass="form-label" runat="server" />
                <asp:DropDownList runat="server" ID="ddlMedico" AutoPostBack="true" CssClass="form-control" OnDataBound="ddlMedico_DataBound" OnSelectedIndexChanged="ddlMedico_SelectedIndexChanged"></asp:DropDownList>
                <asp:Label Text="" CssClass="verde" ID="lblHorarioMedicoSeleccionado" runat="server" />
            </div>
            <div class="mb-3">
                <asp:Label Text="Fecha" CssClass="form-label" runat="server" />
                <asp:TextBox ID="txtFecha" runat="server" TextMode="Date" AutoPostBack="true" OnTextChanged="txtFecha_TextChanged" CssClass="form-control"></asp:TextBox>
                <asp:Label Text="" CssClass="rojo" ID="lblHorarioOcupado" runat="server" />
            </div>
            <div class="mb-3">
                <asp:Label Text="Horario" CssClass="form-label" runat="server" />
                <asp:TextBox ID="txtHora" CssClass="form-control" TextMode="Time" runat="server" />
            </div>
            <div class="mb-3">
                <asp:Label Text="Observacion" CssClass="form-label" runat="server" />
                <asp:TextBox ID="txtObservacion" CssClass="form-control" TextMode="MultiLine" runat="server" />
            </div>
            <div class="mb-3">
                <asp:Label Text="Estado" CssClass="form-label" runat="server" />
                <asp:DropDownList runat="server" ID="ddlEstado" AutoPostBack="true" CssClass="form-control" OnDataBound="ddlEstado_DataBound" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged"></asp:DropDownList>

            </div>
            <div class="mb-3">
                <asp:Button Text="Guardar" OnClick="btnAgregar_Click" CssClass="btn btn-primary" ID="btnAgregar" runat="server" />
            </div>
        </div>
        <div class="col-3"></div>
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
