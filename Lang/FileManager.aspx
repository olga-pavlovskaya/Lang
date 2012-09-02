<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FileManager.aspx.cs" Inherits="Lang.FileManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="lang-add">
        <span class="add-ico"></span>Добавить файл
        <div class="add-info">
            <asp:FileUpload runat="server" ID="fileAdd" />
            Название: <asp:TextBox runat="server" ID="fileName" />
            <asp:Button runat="server" ID="btnAddFile" Text="Добавить" OnClick="btnAddFile_Click" />
        </div>
    </div>
    <table class="lang-table">
        <thead>
            <tr>
                <td width="200px">Имя</td>
                <td>Название файла</td>
                <td width="170px">Тип</td>
                <td width="120px">Дата загрузки</td>
                <td width="120px">Автор</td>
                <td width="100px">Показывать всем</td>
                <td width="50px"></td>
            </tr>
        </thead>
        <tbody>
        <% foreach (var file in Data.FileManager.GetAllFiles())
        { %>
        <tr>
            <td data="ChangeFileName">
                <span class="value"><%= file.name%></span>
                <input type="text" class="notcheck" value="<%= file.name %>" style="width:200px"/>
                <span class="icon pencil" ></span>
                <span class="icon cancel" ></span>
                <span class="icon ok" ></span>
                <span class="icon preloader" ></span>
            </td>
            <td>
                <span><%= file.filename%></span>
            </td>
            <td data="ChangeFileType">
                <select>
                    <option value="0">Лексика</option>
                    <option value="1">Грамматика</option>
                    <option value="2">dll</option>
                </select>
                <span class="icon preloader" ></span>
                <input type="hidden" value="<%= file.type %>" name="filetype" />
            </td>
            <td>
                <%= file.date_created.ToString("dd MMMM yyyy")%>
            </td>
            <td>
                <%= Data.UserManager.GetUser(file.owner).name %>
            </td>
            <td class="center-align" data="ChangeFileVisibility">
                <% if (file.is_public > 0)
                   { %>  <input type="checkbox" checked="checked" /> <% }
                   else
                   {  %> <input type="checkbox" /> <% } %>
                <span class="icon preloader" ></span>
            </td>
            <td class="center-align" data="DeleteFile">
                <span class="icon delete"></span>
                <input type="hidden" class="fileid" name="fileid" value="<%= file.id %>" />
            </td>
        </tr>
        <%} %>
        </tbody>
    </table>

    <script type="text/javascript">
        $('select').each(function() {
            var self = $(this),
                td = self.parent('td'),
                value = td.find('[name="filetype"]').val();
            self.find('option[value="' + value + '"]').attr('selected', 'selected');
        });

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

        $('.delete').click(function() {
            var self = $(this);
            var td = self.parent('td');
            td.removeClass('edit');
            var method = td.attr('data');
            if (method) {
                td.addClass('saving');
                var fileid = td.parent('tr').find('.fileid').val();
                $.ajax({
                    type: "POST",
                    url: "FileManager.aspx/" + method,
                    data: '{ fileid:"' + fileid + '"}',
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

        var onInputChange = function(e) {
            var td = $(this).parent('td');
            td.removeClass('edit');
            var fileid = td.parent('tr').find('.fileid').val();
            var select = $('select',td),
                value = td.find('input').val();
            if (select.length > 0)
                value = select.val();
            else if (td.find('input').attr('type') == 'checkbox')
                value = td.find('input').attr('checked') ? true : false;
            else
                td.find('.value').html(value);
                
            var method = td.attr('data');
            if (method) {
                td.addClass('saving');
                $.ajax({
                    type: "POST",
                    url: "FileManager.aspx/" + method,
                    data: '{ fileid:"' + fileid + '", value:"' + value + '"}',
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
        }
        $('select').change(onInputChange);
        $('.ok, input[type="checkbox"]').click(onInputChange);


    </script>
</asp:Content>
