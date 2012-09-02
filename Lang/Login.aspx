<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="Lang.Login" Debug="true" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Вход в систему
    </h2>
    <p>
        Пожалуйста, введите имя пользователя и пароль или 
        <asp:HyperLink ID="RegisterHyperLink" runat="server" EnableViewState="false">зарегистрируйтесь</asp:HyperLink>.
    </p>
    <asp:Login ID="LoginUser" runat="server" EnableViewState="false" RenderOuterTable="false">
        <LayoutTemplate>
            <span class="failureNotification">
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
            </span>
            <div class="accountInfo">
                <fieldset class="login">
                    <legend>Учетная запись</legend>
                    <p>
                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Имя пользователя:</asp:Label>
                        <asp:TextBox ID="UserName" runat="server" CssClass="textEntry"></asp:TextBox>
                    </p>
                    <p>
                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Пароль:</asp:Label>
                        <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                    </p>
                </fieldset>
                <p class="submitButton">
                    <asp:Button ID="LoginButton" runat="server" OnClick="btnLogin_Click" Text="Log In" style="display:none;"/>
                    <div id="btnLexic" class="button ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" role="button" onclick="$('#MainContent_LoginUser_LoginButton').click(); "><span class="ui-button-text">Войти</span></div>
                </p>
            </div>
        </LayoutTemplate>
    </asp:Login>
</asp:Content>
