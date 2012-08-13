<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FileManager.aspx.cs" Inherits="Lang.FileManager" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="lang-add">
<span class="add-ico"></span>Добавить файл
<div class="add-info"><input type="file" id="fileAdd" />Название: <input type="text" id="inputAdd"/><button type="button" id="buttonAdd">Добавить</button></div>
</div>
<table class="lang-table">
<thead>
<tr>
<td width="200px">Имя</td>
<td>Название файла</td>
<td width="120px">Дата создания</td>
<td width="120px">Автор</td>
<td width="100px">Показывать всем</td>
<td width="50px"></td>
</tr>
</thead>
<tbody>
<tr>
<td><span class="filename" id="filename0">lexic 1</span><span class="icon pencil"></span></td>
<td>l.txt</td>
<td>May 14, 2012</td>
<td>Olga</td>
<td class="center-align"><input type="checkbox" checked="checked"/></td>
<td class="center-align"><span class="icon delete"></span></td>
</tr>
<tr>
<td><span class="filename" id="filename1">lexic 2</span><span class="icon pencil"></span></td>
<td>l2.txt</td>
<td>May 14, 2012</td>
<td>Olga</td>
<td class="center-align"><input type="checkbox" checked="checked"/></td>
<td class="center-align"><span class="icon delete"></span></td>
</tr>
<tr>
<td><span class="filename" id="filename2">grammar 1</span><span class="icon pencil"></span></td>
<td>gr.txt</td>
<td>May 14, 2012</td>
<td>Olga</td>
<td class="center-align"><input type="checkbox" checked="checked"/></td>
<td class="center-align"><span class="icon delete"></span></td>
</tr>
<tr>
<td><span class="filename" id="filename3">default.dll</span><span class="icon pencil"></span></td>
<td>default.dll</td>
<td>May 14, 2012</td>
<td>Admin</td>
<td class="center-align"></td>
<td class="center-align"><span class="icon delete"></span></td>
</tr>
<tr>
<td><span class="filename" id="filename4">shared</span><span class="icon pencil"></span></td>
<td>shared.dll</td>
<td>May 14, 2012</td>
<td>Ann</td>
<td class="center-align"></td>
<td class="center-align"><span class="icon delete"></span></td>
</tr>
</tbody>
</table>

<script type="text/javascript">
    var pencilClick = function() {
        var self = $(this);
        var input = $('.filename', self.parent());
        var oldText = input.text() || input.attr('value');
        if (self.hasClass('ok')) {
            $('<span class="filename" id="' + input.attr('id') + '">' + oldText + '</span>')
            .insertAfter(input);
            input.remove();
            self.removeClass('ok');
        }
        else {
            $('<input type="text" class="filename" id="' + input.attr('id') + '" value="' + oldText + '" />')
            .insertAfter(input);
            input.remove();
            self.addClass('ok');
        }
    };
    $('.pencil').click(pencilClick);

    $('.delete').click(function() {
        $(this).parents('tr').remove();
    });

    $('.add-ico').click(function() {
        $('.add-info').toggle();
    });
    var i = 6;
    $('#buttonAdd').click(function() {
        var newDiv = $('<tr><td><span class="filename" id="filename' + i++ + '">' + $('#inputAdd').attr('value') + '</span><span class="icon pencil"/></td><td>' 
            + $('#fileAdd').attr('value') + 
            '</td><td>' + new Date() + '</td><td></td><td></td><td></td></tr>').insertAfter($('.lang-table tr:first'));
            $('.pencil', newDiv).click(pencilClick);
    });
</script>
</asp:Content>
