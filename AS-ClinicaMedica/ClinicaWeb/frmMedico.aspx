<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="frmMedico.aspx.cs" Inherits="ClinicaWeb.frmMedico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        <style >
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
        <div class="col-4">
            <div class="mb-3">
                <asp:Label Text="Nombre" CssClass="form-label" runat="server" />
                <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server" />
            </div>
            <div class="mb-3">
                <asp:Label Text="Apellido" CssClass="form-label" runat="server" />
                <asp:TextBox ID="txtApellido" CssClass="form-control" runat="server" />
            </div>
            <div class="mb-3">
                <asp:Label Text="DNI" CssClass="form-label" runat="server" />
                <asp:TextBox ID="txtDni" CssClass="form-control" runat="server" />
            </div>
            <div class="mb-3">
                <asp:Label Text="Email" CssClass="form-label" runat="server" />
                <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" />
            </div>
            <div class="mb-3">
                <asp:Label Text="Sexo" CssClass="form-label" runat="server" />
                <br />
                <asp:RadioButton ID="chkHombre" runat="server" GroupName="chkSexo" Text="Hombre" Checked="true" />
                <asp:RadioButton ID="chkMujer" runat="server" GroupName="chkSexo" Text="Mujer" />
            </div>
        </div>
        <div class="col-3"></div>
        <div class="col-4">
            <div class="mb-3">
                <asp:Label Text="Usuario" CssClass="form-label" runat="server" />
                <asp:TextBox ID="txtUsuario" CssClass="form-control" runat="server" />
            </div>
<%--            <div class="mb-3">
                <asp:Label Text="Password" CssClass="form-label" runat="server" />
                <asp:TextBox ID="txtPassword1" TextMode="Password" CssClass="form-control" runat="server" />
            </div>
            <div class="mb-3">
                <asp:Label Text="Escriba nuevamente la clave" CssClass="form-label" runat="server" />
                <asp:TextBox ID="txtPassword2" CssClass="form-control" TextMode="Password" runat="server" />
            </div>--%>
            <div class="mb-3">
                <asp:Label Text="Tipo Usuario:" CssClass="form-label" runat="server" />
                <asp:DropDownList runat="server" OnDataBound="ddlTipoUsuario_DataBound" CssClass="form-select" ID="ddlTipoUsuario"></asp:DropDownList>
            </div>

        </div>

    </div>
    <hr />
    <div class="row">
        <div class="col-4">
            <div class="mb-3">
                <asp:Label Text="Horarios" CssClass="form-label" runat="server" />
                <asp:DropDownList runat="server" ID="ddlHorarios" OnSelectedIndexChanged="ddlHorarios_SelectedIndexChanged" AutoPostBack="True" CssClass="form-select"></asp:DropDownList>
            </div>
            <div class="mb-3">
                <table class="table">
                    <asp:Repeater runat="server" ID="repetidorHorarios">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#Eval("diaHora")%>

                                </td>
                                <td>
                                    <asp:Button Text="-" CssClass="btn btn-danger" ID="btnEliminarHorario" AutoPostBack="true" OnClick="btnEliminarHorario_Click" CommandArgument='<%#Eval("Id")%>' runat="server" />
                                </td>
                            </tr>

                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
        <div class="col-3"></div>
        <div class="col-4">
            <div class="mb-3">
                <asp:Label Text="Especialidad/es" CssClass="form-label" runat="server" />
                <asp:DropDownList runat="server" OnDataBound="ddlEspecialidades_DataBound" OnSelectedIndexChanged="ddlEspecialidades_SelectedIndexChanged1" ID="ddlEspecialidades" CssClass="form-select" AutoPostBack="true"></asp:DropDownList>
            </div>
            <div class="mb-3">
                <table class="table">
                    <asp:Repeater runat="server" ID="repetidorEspecialidades">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#Eval("especialidad")%>

                                </td>
                                <td>
                                    <asp:Button Text="-" CssClass="btn btn-danger" AutoPostBack="true" ID="btnEliminarEspecialidad" OnClick="btnEliminarEspecialidad_Click" CommandArgument='<%#Eval("Id")%>' runat="server" />
                                </td>
                            </tr>

                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-4">
            <div class="mb-3">
                <asp:Button Text="Guardar" CssClass="btn btn-primary" ID="btnGuardarMedico" OnClick="btnGuardarMedico_Click" runat="server" />
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

</asp:Content>
