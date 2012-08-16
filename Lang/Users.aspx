<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="Lang.Users" 
MasterPageFile="~/Site.Master"%>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <table class="lang-table">
        <thead>
            <tr>
                <td>Имя пользователя</td>
                <td width="30%">Пароль</td>
                <td width="200">Администратор</td>
                <td width="200"></td>
            </tr>
        </thead>
        <tbody>
            <% foreach (Data.User user in users)
            { %>
            <tr>
                <td data="UpdateUserName">
                    <span class="value" ><%= user.name %></span>
                    <input type="text" class="notcheck" value="<%= user.name %>" style="width:200px"/>
                    <span class="icon pencil" ></span>
                    <span class="icon cancel" ></span>
                    <span class="icon ok" ></span>
                    <span class="icon preloader" ></span>
                </td>
                <td data="UpdateUserPassword">
                    <span class="value"><%= user.password.Select(c => "*").Aggregate((c1, c2) => c1 + c2) %></span>
                    <input type="password" class="notcheck" value="<%= user.password %>" style="width:200px"/>
                    <span class="icon pencil" ></span>
                    <span class="icon cancel" ></span>
                    <span class="icon ok" ></span>
                    <span class="icon preloader" ></span>
                </td>
                <td data="UpdateUserRole">
                    <input type="checkbox" />
                    <span class="icon preloader" ></span>
                </td>
                <td data="DeleteUser">
                    <span class="icon delete" ></span>
                    <input type="hidden" class="userid" name="userid" value="<%= user.id %>" />
                </td>
            </tr>
            <% } %>
        </tbody>
    </table>
    <script language="javascript" type="text/javascript">
        $('.pencil').click(function() {
            var self = $(this);
            var td = self.parent('td');
            td.find('input').val(td.find('.value').html());
            td.addClass('edit');
        });
        $('.cancel').click(function() {
            var self = $(this);
            var td = self.parent('td');
            td.removeClass('edit');
        });
        $('.ok, input[type="checkbox"]').click(function() {
            var self = $(this);
            var td = self.parent('td');
            td.removeClass('edit');
            var input = td.find('input'),
                value = input.val();
            if (input.attr('type') == 'password')
                td.find('.value').html(passwordToStars(value));
            else
                td.find('.value').html(value);
            
            var method = td.attr('data');
            if (method) {
                td.addClass('saving');
                var userid = td.parent('tr').find('.userid').val();
                if (self.attr('type') == 'checkbox')
                    value = self.attr('checked') ? true : false;
                $.ajax({
                    type: "POST",
                    url: "Users.aspx/" + method,
                    data: '{ userid:"' + userid + '", value:"' + value + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        td.removeClass('saving');
                    },
                    error: function() {
                        td.removeClass('saving');
                    }
                });
            }
        });

        $('.delete').click(function() {
            var self = $(this);
            var td = self.parent('td');
            td.removeClass('edit');
            var method = td.attr('data');
            if (method) {
                td.addClass('saving');
                var userid = td.parent('tr').find('.userid').val();
                $.ajax({
                    type: "POST",
                    url: "Users.aspx/" + method,
                    data: '{ userid:"' + userid + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        td.removeClass('saving');
                        td.parent('tr').remove();
                    },
                    error: function() {
                        td.removeClass('saving');
                    }
                });
            }
        });
        function passwordToStars(pass) {
            var ret = '';
            for (var i = 0; i < pass.length; i++) {
                ret = ret + '*';
            }
            return ret;
        }
    </script>
</asp:Content>