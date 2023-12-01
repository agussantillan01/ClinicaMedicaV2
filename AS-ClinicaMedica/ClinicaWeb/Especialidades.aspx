<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Especialidades.aspx.cs" Inherits="ClinicaWeb.Especialidades" %>

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
        function confirmarEliminacion(rowIndex) {
            if (confirm('¿Estás seguro que deseas eliminar?')) {
                __doPostBack('<%= dgvEspecialidades.ClientID %>', 'Eliminar$' + rowIndex);
                return true;
            }
            return false;
        }

    </script>


    <div class="row">
        <div class="col-4">
            <asp:TextBox ID="tbxFiltro" runat="server" placeholder="Filtro especialidades..." CssClass="form-control" OnTextChanged="tbxFiltro_TextChanged" AutoPostBack="true"></asp:TextBox>
        </div>
        <div class="col-4">
        </div>
        <div class="col-4 text-end">
            <a href="frmEspecialidad.aspx" class="btn btn-secondary" role="button">Nueva Especialidad</a>
        </div>
    </div>
    <br />
    <br />
    <div class="row">
        <div class="col">
            <asp:GridView runat="server" ID="dgvEspecialidades" DataKeyNames="Id" CssClass="table table-dark table-striped" AutoGenerateColumns="false" AllowPaging="true" PageSize="6" OnRowCommand="dgvEspecialidades_RowCommand" OnPageIndexChanging="dgvEspecialidades_PageIndexChanging">
                <Columns>
                    <asp:BoundField HeaderText="Id." DataField="Id" HeaderStyle-CssClass="oculto" ItemStyle-CssClass="oculto" />
                    <asp:BoundField HeaderText="Especialidad" DataField="Especialidad" />
                    <asp:ButtonField ButtonType="Button" CommandName="Modificar" HeaderText="Modificar" Text="Modificar" ControlStyle-CssClass="btn btn-outline-light" />
                    <%--                    <asp:ButtonField ButtonType="Button" CommandName="Eliminar" HeaderText="Eliminar" Text="Eliminar" ControlStyle-CssClass="btn btn-outline-light" />--%>
                    <asp:TemplateField HeaderText="Eliminar">
                        <ItemTemplate>
                            <asp:Button runat="server" CommandName="Eliminar" Text="Eliminar" CssClass="btn btn-outline-light"
                                OnClientClick='<%# "return confirmarEliminacion(" + Container.DataItemIndex + ");" %>'
                                CommandArgument='<%# Container.DataItemIndex %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
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
