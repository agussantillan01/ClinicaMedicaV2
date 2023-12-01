<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="frmPaciente.aspx.cs" Inherits="ClinicaWeb.frmPaciente" %>

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
    <div class="row">
        <asp:Literal ID="ltlHeading" runat="server"></asp:Literal>
        <div class="col-2">
        </div>
        <div class="col-4">

            <%-- Aca arranca la carga de la persona --%>
            <div class="mb-3">
                <label for="tbxDNI" class="form-label">Documento Nacional de Identidad</label>
                <asp:TextBox ID="txtDni" runat="server" CssClass="form-control" AutoPostBack="true"></asp:TextBox>
            </div>
            <div class="mb-3">
                <label for="tbxNombre" class="form-label">Nombre</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>

            </div>
            <div class="mb-3">
                <label for="tbxApellido" class="form-label">Apellido</label>
                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>

            </div>
            <div class="mb-3">
                <label for="tbxEmail" class="form-label">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
            <div class="mb-3">
                <label for="chkSexo" class="form-label">Sexo</label>
                <br />
                <asp:RadioButton ID="chkHombre" runat="server" GroupName="chkSexo" Text="Hombre" Checked="true" />
                <asp:RadioButton ID="chkMujer" runat="server" GroupName="chkSexo" Text="Mujer" />
            </div>
            <div class="mb-3">
                <asp:Button Text="Guardar" CssClass="btn btn-primary" ID="btnAgregar" OnClick="btnAgregar_Click" runat="server" />
            </div>

        </div>
        <div class="col-4">
            <%-- aca arranca la carga del paciente --%>
            <div class="mb-3">
                <label for="tbxFechaNacimiento" class="form-label">Fecha de Nacimiento</label>
                <asp:TextBox ID="txtFechaNacimiento" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                <asp:Label ID="lblFechaNacimiento" runat="server" Text="" ClientIDMode="Static"></asp:Label>
            </div>
            <div class="mb-3">
                <label for="tbxTelefono" class="form-label">Telefono</label>
                <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                <asp:Label ID="lblTelefono" runat="server" Text="" ClientIDMode="Static"></asp:Label>
            </div>
            <div class="mb-3">
                <label for="tbxDireccion" class="form-label">Direccion</label>
                <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                <asp:Label ID="lblDireccion" runat="server" Text="" ClientIDMode="Static"></asp:Label>
            </div>
        </div>
        <div class="col-2">
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
    <asp:Label ID="lblPacienteConfirmacion" Text="" ForeColor="Green" runat="server" />

</asp:Content>
