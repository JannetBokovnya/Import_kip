<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="Modules_About_system_index" MasterPageFile="~/System/System.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <title>Нет доступа</title>
    <style>
        .rounded {
            width: 320px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    <div style="height: 100px; text-align: center; font-size: 10pt;">
        <p style="font-size: 12pt;"><strong>Доступ запрещен</strong></p><br />
        <p>У вас нет доступа к данному модулю.</p>
        <p>Сменить учетную запись?</p>
    </div><br/>
    <div>
        <div style="text-align: right;">
            <asp:Image ID="btnCancel"  runat="server" AlternateText="Отмена" CssClass="Button" ImageUrl="~/images/cancel.png" />
            <asp:Image ID="btnOk" runat="server" AlternateText="Подтверждение" CssClass="Button" ImageUrl="~/images/ok32.png" />
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            var countWin = countOpenedWindows();
            if (countWin > 0) {
                $('#<%=btnCancel.ClientID %>').attr('onclick', 'javascript:window.close()');
            }
            else {
                $('#<%=btnCancel.ClientID %>').attr('onclick', 'javascript:history.back()');
            }
        });
    </script>
</asp:Content>