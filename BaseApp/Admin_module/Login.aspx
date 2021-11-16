<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Admin_module_Default" MasterPageFile="~/System/Admin.master" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:content id="Content" contentplaceholderid="Content" runat="Server">
    <asp:Login ID="pnlLogin" runat="server"
        TitleText="    " PasswordLabelText="Пароль:&nbsp;" UserNameLabelText="Логин:&nbsp;"
        Font-Size="13px" Font-Names="Tahoma" LoginButtonType="Image"
        FailureText = "Имя пользователя или пароль введены неверно. Обратитесь к администратору"
        PasswordRequiredErrorMessage="пусто" DisplayRememberMe="False"
        LoginButtonImageUrl="../Images/ok32.png" onloggingin="pnlLogin_LoggingIn"
        onauthenticate="LoginControl_Authenticate" meta:resourcekey="pnlLoginResource1">
        <TitleTextStyle Font-Bold="True" ForeColor="#FFFFFF" CssClass="TopImage" Height="60" />
        <TextBoxStyle Width="150" CssClass="Text" />
        <LayoutTemplate>
            <table cellpadding="1" cellspacing="0" border="0" style="border-collapse: collapse; margin: 5px;">
                <tr>
                    <td colspan="2" class="TopImage" style="color:White;font-weight:bold;height:70px;"></td>
                </tr>
                <tr>
                    <td style="width: 60px;">
                        <asp:Label ID="Label1" Text="Логин" runat="server" CssClass="label" /> <!--  meta:resourcekey="Label1Resource1"  -->
                    </td>
                    <td style="width: 150px">
                        <asp:TextBox ID="UserName" runat="server" CssClass="Text" Width="150px" meta:resourcekey="UserNameResource1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 11px;">
                        <asp:Label ID="Label2" Text="Пароль" runat="server" CssClass="label" meta:resourcekey="Label2Resource1" />
                    </td>
                    <td>
                        <asp:TextBox ID="Password" runat="server" CssClass="Text" TextMode="Password" Width="150px" meta:resourcekey="PasswordResource1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding-top: 3px;">
                        <div style="border: solid 0px #000000; float: left; text-align: left; margin-left: 15px; padding-bottom: 8px; margin-top: 5px;">
                            <asp:RadioButtonList ID="rblLanguage" runat="server" AutoPostBack="True" RepeatLayout="Flow" Font-Size="8" CssClass="rblLanguage">
                                <asp:ListItem Value="ru-RU">Русский</asp:ListItem>
                                <asp:ListItem Value="en-US">English</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div>
                            <asp:Button ID="LoginImageButton" runat="server" AlternateText="Log In" CommandName="Login" CssClass="FlashButton"
                                               Text="Войти в систему " ValidationGroup="pnlLogin" Width="100%" Height="27px"
                                               OnClientClick="localStorage.clear();" meta:resourcekey="LoginImageButtonResource1"/>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left" style="color:#e31e24; line-height: 1.5; padding-top: 8px; font-size: 11px;" class="tdError">
                        <asp:Literal ID="FailureText" runat="server" ViewStateMode="Enabled" meta:resourcekey="FailureTextResource1"></asp:Literal>
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ValidationGroup="pnlLogin" EnableViewState="False" Display="None" BorderStyle="None" EnableTheming="True" meta:resourcekey="UserNameRequiredResource1"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ValidationGroup="pnlLogin" EnableViewState="False" Display="None" BorderStyle="None" meta:resourcekey="PasswordRequiredResource1"></asp:RequiredFieldValidator>
                        <asp:ValidationSummary id="ValidationSummary1" runat="server" ValidationGroup="pnlLogin" HeaderText="Некорректно введен Логин/Пароль" BorderStyle="None" DisplayMode="List" meta:resourcekey="ValidationSummary1Resource1"></asp:ValidationSummary>
                    </td>
                </tr>
            </table>         
        </LayoutTemplate>
        <LoginButtonStyle CssClass="Button"/>
    </asp:Login>
    <script>
        $(document).ready(closeAllWindows);
    </script>
</asp:content>
