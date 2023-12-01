<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="MisDatos.aspx.cs" Inherits="ClinicaWeb.MisDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .round-button {
            width: 40px;
            height: 40px;
            background-color: #ccc;
            border-radius: 50%; /* Esto hace que el botón sea redondo */
            color: #fff; /* Color del texto (flecha) */
            font-size: 24px;
            border: none;
            cursor: pointer;
        }

            /* Estilo cuando se pasa el mouse por encima */
            .round-button:hover {
                background-color: #aaa; /* Cambia el color de fondo en el hover */
            }
    </style>
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

            .rojo{
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
        <%--<asp:Literal ID="ltlHeading" runat="server"></asp:Literal>--%>
        <h2>Mis Datos</h2>

        <div class="col-2">
        </div>
        <div class="col-4">
            <div class="mb-3">
                <label for="tbxNombre" class="form-label">Nombre</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>

            </div>
            <div class="mb-3">
                <label for="tbxApellido" class="form-label">Apellido</label>
                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control"></asp:TextBox>

            </div>
            <div class="mb-3">
                <label for="tbxDNI" class="form-label">Documento Nacional de Identidad</label>
                <asp:TextBox ID="txtDni" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="mb-3">
                <label for="tbxEmail" class="form-label">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="mb-3">
                <label for="chkSexo" class="form-label">Sexo</label>
                <br />
                <asp:RadioButton ID="chkHombre" runat="server" GroupName="chkSexo" Text="Hombre" Checked="true" />
                <asp:RadioButton ID="chkMujer" runat="server" GroupName="chkSexo" Text="Mujer" />
            </div>
            <div class="mb-3">
                <asp:Button Text="Modificar" CssClass="btn btn-primary" ID="btnModificarDatosPersonales" OnClick="btnModificarDatosPersonales_Click" runat="server" />
            </div>

        </div>
        <div class="col-2"></div>
        <div class="col-2">
            <div style="float: right;">
                <asp:RadioButton ID="chkView" runat="server" GroupName="chkVista" Text="Viw" Checked="true" AutoPostBack="true" />
                <asp:RadioButton ID="chkEdit" runat="server" GroupName="chkVista" Text="Edit" AutoPostBack="true" />
            </div>
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
    <hr />
    <div class="row">
        <div class="col-6"></div>
        <div class="col-4">
            <asp:Button ID="btnRound" runat="server" Text="&#9660;" OnClick="btnRound_Click" CssClass="round-button" />
        </div>
        <div class="col-6"></div>
    </div>

    <div class="row">
        <div class="col-4"></div>
        <div class="col-4">
            <div id="visibilidadUsuario" runat="server" style="display: none;">
                <div class="mb-3">
                    <label for="tbxNombreUsuario" class="form-label">Nombre usuario</label>
                    <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="mb-3">
                    <label for="txtPasswordActual" class="form-label">Contraseña Actual</label>
                    <asp:TextBox ID="txtPasswordActual" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="mb-3">
                    <label for="txtPasswordNueva1" class="form-label">Contraseña Nueva</label>
                    <asp:TextBox ID="txtPasswordNueva1" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="mb-3">
                    <label for="txtPasswordNueva2" class="form-label">Repita la nueva contraseña</label>
                    <asp:TextBox ID="txtPasswordNueva2" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="mb-3">
                    <asp:Button Text="Modificar" CssClass="btn btn-primary" ID="btnModificarUsuario" OnClick="btnModificarUsuario_Click" runat="server" />
                </div>
                <div class="mb-3">
                    <asp:Label Text="" CssClass="rojo" ID="lblErrores" runat="server" />
                </div>
            </div>

        </div>
        <div class="col-4"></div>

    </div>



</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>--%>
