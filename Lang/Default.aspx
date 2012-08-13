<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Lang._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="tabs">
        <ul>
		    <li><a href="#lexic">1. Лексика</a>
            <div class="combo-div">
                <select>
                    <option value="1" text="mylexic.lexic">lexic 1</option>
                    <option value="1" text="mylexic.lexic">lexic 2</option>
                </select>
            </div>
            </li>
		    <li><a href="#grammar">2. Грамматика</a>
            <div class="combo-div">
                <select>
                    <option value="1" text="mylexic.lexic">grammar 1</option>
                    <option value="1" text="mylexic.lexic">grammar 2</option>
                </select>
            </div></li>
            <li><a href="#">3. Генератор кода</a>
            <div class="combo-div">
                <select>
                    <option value="1" text="mylexic.lexic">default</option>
                    <option value="1" text="mylexic.lexic">mary.dll</option>
                </select>
            </div></li>
		    <li style="height:40px;"><a href="#function" style="line-height:32px;">4. Текст программы</a></li>
            <li><a href="#result">Результат</a></li>
	    </ul>
        
	    <div id="lexic">
            <textarea id="taLexic" class="ta" onkeypress="$('#btnLexic').button({disabled:false});" >
                <%= t1 %>
            </textarea>
            <div style="width:95%; float:left;padding-top:10px;margin-right:50%;">
                <div id="btnLexic" class="button">Сохранить</div>
                <span style="color:#8c2a1c;font-size:12px;display:none;margin-left:10px;" class="err">Ошибка при разборе</span>
		    </div>
            <div id="machine-table" style="width:100%; height:170px; overflow:auto;"></div>
	    </div>
	    <div class="grammar" id="grammar">
            <div class="grammar-ok">
                <textarea id="taGrammar" class="ta" onkeypress="$('#btnGrammar').button({disabled:false});" >
                    <%= t2 %>
                </textarea>
	            
                <div style="width:95%; float:left;padding-top:10px;margin-right:50%;">
                    <div id="btnGrammar" class="button">Сохранить</div>
                    <span style="color:#8c2a1c;font-size:12px;display:none;margin-left:10px;" class="err">Ошибка при разборе</span>
		        </div>
            </div>
	    </div>
        
	    <div class="function" id="function">
            <div class="grammar-ok">
                <textarea id="taFunction" class="ta" onkeypress="$('#btnFunction').button({disabled:false});" >
                    <%= t3 %>
                </textarea>
                <div style="width:95%; float:left;padding-top:10px;margin-right:50%;">
                    Задайте параметры через пробел: <asp:TextBox runat="server" ID="tb3"></asp:TextBox><br /><div id="btnFunction" class="button">Вычислить</div>
                    <span style="color:#8c2a1c;font-size:12px;display:none;margin-left:10px;" class="err">Ошибка при разборе</span>
		        </div>
            </div>
	    </div>
	    <div class="function" id="result">
            <div style="border: 1px solid black; width:200px; height: 100px; " >
            1<br />2<br />3<br />4<br />5
            </div>
            <div class="result-ok">
                <textarea id="Textarea1" class="ta" onkeypress="$('#btnFunction').button({disabled:false});" >
                    <%= t3 %>
                </textarea>
            </div>
	    </div>
    </div>
    <script language="javascript" type="text/javascript">
        $(function () {
            $('select').combobox();
            $.studyObj = {isLexic:false, isGrammar:false};
            $(".tabs").tabs();
            $('.ui-corner-top').removeClass('ui-corner-top');
            $('#btnLexic').click(function () {
                $.ajax({
                    type: "POST",
                    url: "Default.aspx/SetLexic",
                    data: '{text:"' + $('#taLexic')[0].value + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        setParsedLexic(result);
                    }
                });
            }).button();
            $('#btnGrammar').click(function () {
                $.ajax({
                    type: "POST",
                    url: "Default.aspx/SetGrammar",
                    data: '{text:"' + $('#taGrammar')[0].value + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        setParsedGrammar(result);
                    }
                });
            }).button();

            $('#btnFunction').click(function () {
                $.ajax({
                    type: "POST",
                    url: "Default.aspx/DoFunction",
                    data: '{text:"' + $('#taFunction')[0].value + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        doFunction(result);
                    }
                });
            }).button();

            var doFunction = function(result){
                if (result.d != null) {
                    $('#btnFunction').button({ disabled: true });
                    var table=$('<table class="result-table"></table>');
                    table.append('<tr><td class="result-table-header">Класс</td><td class="result-table-header">Значение</td>');
                    for (var i in result.d.dict) {
                        var obj = result.d.dict[i];
                        table.append('<tr><td class="result-table-cell">' + obj.type + '</td><td class="result-table-cell">' + obj.value + '</td></tr>');
                    }
                    $('#parsedLexic').empty().append(table);
                    $('#taIl')[0].value = result.d.il;
                
                }
                else {
                    $('#btnFunction').next().show();
                }
            };
            
            var setParsedGrammar = function(result){
                if (result.d != null) {
                    $.studyObj.isGrammar = true;
                    $('.function .grammar-ok').show();
                    $('.function .error').hide();
                    $('#btnGrammar').button({ disabled: true });
                }
                else {
                    $.studyObj.isGrammar = false;
                    $('.function .grammar-ok').hide();
                    $('.function .error').show();
                    $('#btnGrammar').next().show();
                }
            };

            var setParsedLexic = function(result){
                $('#machine-table').html(result.d);
                $('#machine-table tr:first td').addClass('header');
                $('td:first', $('#machine-table tr')).addClass('header');
                /*
                var table=$('<table class="result-table"></table>');
                table.append('<tr><td class="result-table-header">Класс</td><td class="result-table-header">Значение</td>');
                $('#parsedLexic').empty().append(table);
                if (result.d != null) {
                    $.studyObj.isLexic = true;
                    $('.grammar .grammar-ok, .function .grammar-ok').show();
                    $('.grammar .error, .function .error').hide();

                    $('#btnLexic').button({ disabled: true });
                }
                else {
                    $.studyObj.isLexic = false;
                    $('.grammar .grammar-ok, .function .grammar-ok').hide();
                    $('.grammar .error, .function .error').show();
                    $('#btnLexic').next().show();
                }*/
            };
        });
    </script>
</asp:Content>
