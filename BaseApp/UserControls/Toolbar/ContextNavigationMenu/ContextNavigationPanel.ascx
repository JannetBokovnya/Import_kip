<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ContextNavigationPanel.ascx.cs"
    Inherits="UserControls_Panel_ContextNavigation" %>
<%--<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2012.1.215.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>--%>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<asp:UpdatePanel runat="server" ID="upnlContextNavigation" UpdateMode="Conditional">
    <contenttemplate>
            <table class="contMenu">
                <tr>
                    <td>
                        <telerik:RadComboBox runat="server" ID="ddlMG" OnSelectedIndexChanged="ddlMG_SelectedIndexChanged" 
                                             AutoPostBack="True" Width="250px" EmptyMessage="Выберите газопровод..." 
                                             meta:resourcekey="ddlMGResource1" />
                    </td>
                    <td>
                        <telerik:RadComboBox runat="server" ID="ddlThread" OnSelectedIndexChanged="ddlThread_SelectedIndexChanged"
                                             AutoPostBack="True" Width="250px" EmptyMessage="Выберите протяженный объект..." 
                                             meta:resourcekey="ddlThreadResource1"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" enableclientscript="True"
                                                    ControlToValidate="ddlThread" runat="server" ValidationGroup="Val" />
                    </td>
                    <td>
                        <telerik:RadNumericTextBox runat="server" ID="txtKmStart" Width="70px" Culture="en-US" MinValue="0">
                            <NumberFormat GroupSeparator=" "/>
                            <ClientEvents OnValueChanged="SetWhiteBackGround" OnFocus="SetWhiteBackGround" />
                        </telerik:RadNumericTextBox>
                    </td>
                    <td>
                        <telerik:RadNumericTextBox runat="server" ID="txtKmEnd" Width="70px" Culture="en-US" MinValue="0">
                            <NumberFormat GroupSeparator=" "/>
                            <ClientEvents OnValueChanged="SetWhiteBackGround" OnFocus="SetWhiteBackGround" />
                        </telerik:RadNumericTextBox>
                    </td>
                    <td style="padding: 0;">
                        <asp:Panel class="imgOK" runat="server" ID="pnlSetConNav" ToolTip="Установить" meta:resourcekey="pnlSetConNavResource1">
                             <asp:Button runat="server" ID="btnSetConNav" BackColor="Transparent" BorderStyle="None"
                                         ForeColor="Transparent" Height="22px" Width="22px" ValidationGroup="Val" 
                                         OnClientClick="setUserCN();return false;"/>
                        </asp:Panel>
                        <asp:Panel class="imgClose" runat="server" ID="pnlCancelConNav" ToolTip="Отмена" meta:resourcekey="pnlCancelConNavResource1">
                            <asp:Button runat="server" ID="btnCancelConNav" BackColor="Transparent" BorderStyle="None"
                                ForeColor="Transparent" Height="22px" Width="22px" OnClientClick="cancelContextMenu()" />
                        </asp:Panel>
                    </td>
                </tr>
            </table>
    </contenttemplate>
</asp:UpdatePanel>
<script type="text/javascript">
    var _idThread = "NoThread";
    var _idMG = "NoMG";

    //Установка Кн пользователем ко нажатию на кнопку
    function setUserCN() {
        var isValid = isSetCnValid();

        if (isValid) {
            setCookieCN();
            setContextNavigation();
        }
    }

    // Проверка валидности данных. Невалидные выделить красным
    function isSetCnValid() {
        var errorColor = "#E57F7F";
        var ddlThread = $find("<%= ddlThread.ClientID %>");
        var txtKmStartElm = $find("<%= txtKmStart.ClientID %>");
            var txtKmEndElm = $find("<%= txtKmEnd.ClientID %>");

            var ddlThreadElm = ddlThread.get_inputDomElement();
            var v1 = document.getElementById("<%=RequiredFieldValidator1.ClientID%>");
        ValidatorValidate(v1);
        if (!v1.isvalid) {
            ddlThreadElm.style.backgroundColor = errorColor;
        }

        if (txtKmStartElm.get_value() == '') {
            txtKmStartElm.get_styles().EnabledStyle[0] += "background-color: " + errorColor + ";";
            txtKmStartElm.get_styles().HoveredStyle[0] += "background-color: " + errorColor + ";";
            txtKmStartElm.updateCssClass();
        }

        if (txtKmEndElm.get_value() == '') {
            txtKmEndElm.get_styles().EnabledStyle[0] += "background-color: " + errorColor + ";";
            txtKmEndElm.get_styles().HoveredStyle[0] += "background-color: " + errorColor + ";";
            txtKmEndElm.updateCssClass();
        }

        if (!v1.isvalid || txtKmStartElm.get_value() == '' || txtKmEndElm.get_value() == '') {
            return false;
        }

        return true;
    }

    // Установка КН в куки из панели
    function setCookieCN() {
        var combobox = $find("<%= ddlMG.ClientID %>");
        var idMg = combobox.get_value();
        var nameMg = combobox.get_text().replace(new RegExp('"', 'g'), '\\"');
        combobox = $find("<%= ddlThread.ClientID %>");
        var valueThread = combobox.get_value().replace(new RegExp('"', 'g'), '\\"');
        var arrThread = valueThread.split(";");
        var kmStart = document.getElementById('<%=txtKmStart.ClientID %>').value;
        var kmEnd = document.getElementById('<%=txtKmEnd.ClientID %>').value;
        if ((idMg != _idMG) && (arrThread[0] != _idThread)) {
            var valueCN = '{"IdMg":"' + idMg + '","NameMg":"' + nameMg + '","IdThread":"' + arrThread[0]
                + '","NameThread":"' + arrThread[1] + '","NameThreadShorten":"' + arrThread[1]
                + '","KmStart":"' + kmStart + '","KmEnd":"' + kmEnd + '"}';
            if (_isSynch) {
                //Если синхронизация установлена записать куки, тем самым отправить события во все открытые модули
                setCookie('ContextNavigation', escape(valueCN));
            } else {
                //Если синхронизация выключена передать события на прямую в модуль не используя кук.
                var strCn = '{"SenderWindow":"' + GetWindowName() + '", "Destination":["broadcast"], "EventParam": ' + valueCN + ' }';
                SendEventToFlashModules('NAVIGATION_CONTEXT_CHANGED', strCn);
            }
            changeLabelCN(valueCN);
        }
    }

    //Заполнить панель Кн из кук
    function SetKmToCnPanel(strCn) {
        var ddlThread = $find('<%= ddlThread.ClientID %>');
        ddlThread.set_text(strCn.NameThreadShorten);
        ddlThread.set_value(strCn.IdThread + ';' + strCn.NameThreadShorten);

        var txtKmStart = $find("<%= txtKmStart.ClientID %>");
        txtKmStart.set_value(strCn.KmStart);

        var txtKmEnd = $find("<%= txtKmEnd.ClientID %>");
        txtKmEnd.set_value(strCn.KmEnd);
    }
 
    //Установка цвета контрола в белый (дефолтный) цвет
    function SetWhiteBackGround(sender, args) {
        sender.get_styles().EnabledStyle[0] += "background-color: White;";
        sender.get_styles().HoveredStyle[0] += "background-color: White;";
        sender.updateCssClass();
    }
</script>
