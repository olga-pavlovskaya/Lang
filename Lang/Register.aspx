<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Register.aspx.cs" Inherits="Lang.Account.Register" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
                    <h2>
                        Создание учетной записи
                    </h2>
                    <p>
                        Используйте эту форму для создания новой учетной записи.
                    </p>
                    <span class="failureNotification">
                        <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
                    </span>
                    <div class="accountInfo">
                        <fieldset class="register">
                            <legend>Учетная запись</legend>
                            <p>
                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Имя пользователя:</asp:Label>
                                <asp:TextBox ID="UserName" runat="server" CssClass="textEntry"></asp:TextBox>
                            </p>
                            <p>
                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Пароль:</asp:Label>
                                <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                            </p>
                            <p>
                                <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">Подтверждение пароля:</asp:Label>
                                <asp:TextBox ID="ConfirmPassword" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
            
                            </p>
                        </fieldset>
                        <p class="submitButton">
                         <div id="btnLexic" class="button ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" role="button" onclick="$('#MainContent_CreateUserButton').click();"><span class="ui-button-text">Создать пользователя</span></div>
                                                       <asp:Button ID="CreateUserButton" runat="server" CommandName="MoveNext" Text="Create User" style="display:none;" />
                        </p>
                    </div>
</asp:Content>
