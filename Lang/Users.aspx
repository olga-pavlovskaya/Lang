<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="Lang.Users" 
MasterPageFile="~/Site.Master"%>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <table class="lang-table" style="width:100%; border:none;">
        <thead>
            <tr>
                <td>Имя пользователя</td>
                <td>Задать пароль</td>
                <td>Администратор</td>
                <td></td>
                <td></td>
            </tr>
        </thead>
        <tbody>
            <% foreach (Data.User user in users)
               { %>
            <tr>
                <td>
                    <input type="text" value="<%= user.name %>" style="width:200px"/>
                </td>
                <td>
                    <input type="password" value="<%= user.password %>" style="width:200px"/>
                </td>
                <td>
                    <input type="checkbox" />
                </td>
                <td>
                    <a href="#">Сохранить изменения</a>
                </td>
                <td>
                    <a href="#">Удалить пользователя</a>
                </td>
            </tr>
               <% } %>
            <tr>
                <td>
                    <input type="text" value="Olga" style="width:200px"/>
                </td>
                <td>
                    <input type="password" value="Olga" style="width:200px"/>
                </td>
                <td>
                    <input type="checkbox" />
                </td>
                <td>
                    <a href="#">Сохранить изменения</a>
                </td>
                <td>
                    <a href="#">Удалить пользователя</a>
                </td>
            </tr>
                        <tr>
                <td>
                    <input type="text" value="Guest" style="width:200px"/>
                </td>
                <td>
                    <input type="password" value="Guest" style="width:200px"/>
                </td>
                <td>
                    <input type="checkbox" />
                </td>
                <td>
                    <a href="#">Сохранить изменения</a>
                </td>
                <td>
                    <a href="#">Удалить пользователя</a>
                </td>
            </tr>
                        <tr>
                <td>
                    <input type="text" value="Test User" style="width:200px"/>
                </td>
                <td>
                    <input type="password" value="11111111" style="width:200px"/>
                </td>
                <td>
                    <input type="checkbox" />
                </td>
                <td>
                    <a href="#">Сохранить изменения</a>
                </td>
                <td>
                    <a href="#">Удалить пользователя</a>
                </td>
            </tr>
        </tbody>
    </table>
    <script language="javascript" type="text/javascript">
    </script>
</asp:Content>