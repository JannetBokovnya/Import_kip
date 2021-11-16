<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ResetPassword.aspx.cs" Inherits="Modules_Admin_ResetPassword" 
    MasterPageFile="../System/Admin.master" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    <style type="text/css">
        input {
            margin-top: 5px;
        }
        td {
            text-align: right;
            vertical-align: baseline;
        }
    </style>

    <asp:changepassword Style="width:auto" ID="chepa" OnChangingPassword="chepa_OnChangingPassword"
    runat="server" PasswordLabelText="Старый пароль:&nbsp;" 
     Font-Size="13px" Font-Names="Tahoma" ChangePasswordButtonType="Image" ChangePasswordButtonImageUrl="../Images/ok32.png"
     NewPasswordLabelText="Новый пароль:&nbsp;" ConfirmNewPasswordLabelText="Подтвердите пароль:&nbsp;" CancelButtonType="Link" CancelButtonText=""
     ConfirmPasswordCompareErrorMessage="Пароли не совпадают" SuccessText="Пароль обновлен" ChangePasswordFailureText="" meta:resourcekey="chepaResource1" >
        <ChangePasswordTemplate>
            <table cellpadding="1" cellspacing="0" style="border-collapse:collapse; margin: 10px;">
                <tr>
                    <td>
                        <table cellpadding="0" class="table">
                            <tr>
                                <td colspan="2" align="center" style="font-size: 14px; text-align:center; padding: 0px;">
                                    <asp:Label runat="server" ID="lblSetNewPassword"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label Text="Старый пароль" runat="server" CssClass="label" meta:resourcekey="LabelResource1" />
                                </td>
                                <td>
                                    <asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password" Width="175px" meta:resourcekey="CurrentPasswordResource1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label Text="Новый пароль" runat="server" CssClass="label" meta:resourcekey="LabelResource2" />
                                </td>
                                <td>
                                    <asp:TextBox ID="NewPassword" runat="server" TextMode="Password" Width="175px" meta:resourcekey="NewPasswordResource1"></asp:TextBox>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" Text="Подтверждение" runat="server" CssClass="label" meta:resourcekey="Label1Resource1" />
                                </td>
                                <td>
                                    <asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password" Width="175px" meta:resourcekey="ConfirmNewPasswordResource1"></asp:TextBox>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left" style="padding: 0px;">
                                    <asp:CompareValidator ID="NewPasswordCompare" runat="server" ForeColor="Red" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword" Display="Dynamic" ErrorMessage="Пароли не совпадают" ValidationGroup="chepa" meta:resourcekey="NewPasswordCompareResource1"></asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left" style="color:Red; width:100%; padding:0px;">
                                    <asp:Literal ID="FailureText" runat="server" />
                                    <asp:Literal ID="ChangePasswordFailureText" runat="server" EnableViewState="False"></asp:Literal>
                                    <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword" ValidationGroup="chepa" Display="None" meta:resourcekey="CurrentPasswordRequiredResource1">*</asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword" ValidationGroup="chepa" Display="None" meta:resourcekey="NewPasswordRequiredResource1">*</asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword" ValidationGroup="chepa" Display="None" meta:resourcekey="ConfirmNewPasswordRequiredResource1">*</asp:RequiredFieldValidator>
                                    <asp:ValidationSummary id="ValidationSummary1" runat="server" ValidationGroup="chepa" HeaderText="Ошибка при задании пароля" BorderStyle="None" DisplayMode="List" meta:resourcekey="ValidationSummary1Resource1"></asp:ValidationSummary>
                                    
                                    <asp:Literal ID="FailureText2" runat="server" EnableViewState="True" meta:resourcekey="FailureTextResource1"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:ImageButton ID="btnCancel" CssClass="Button" runat="server" OnClientClick="javascript: window.close();" AlternateText="Cancel" ImageUrl="../Images/cancel.png" meta:resourcekey="ChangePasswordImageButtonResource1" />
                                    <asp:ImageButton ID="ChangePasswordImageButton" CssClass="Button" runat="server" AlternateText="Change Password" CommandName="ChangePassword" ImageUrl="../Images/ok32.png" ValidationGroup="chepa" meta:resourcekey="ChangePasswordImageButtonResource1" />
                                </td>
                            </tr>                   
                        </table>
                        
                    </td>
                </tr>
            </table>
        </ChangePasswordTemplate>
    </asp:changepassword>
    <asp:Label ID="Message1" Runat="server"  ForeColor="Red" meta:resourcekey="Message1Resource1" />
</asp:Content>


