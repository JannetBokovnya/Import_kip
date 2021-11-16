<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ContextNavigationMenu.ascx.cs"
    Inherits="UserControls_Menu_ContextNavigationMenu_ContextNavigationMenu" %>

<asp:UpdatePanel runat="server" ID="upnlConNav" UpdateMode="Conditional">
    <contenttemplate>
        <table style="margin: 0; padding: 0">
            <tr>
                <td>
                    <asp:Panel id="divCNChange" CssClass="imgCNChange" ToolTip="Изменить контекст навигации" runat="server" meta:resourcekey="divCNChangeResource1">
                        <asp:Button runat="server" ID="btnShowConNav" BackColor="Transparent" BorderStyle="None"
                            ForeColor="Transparent" Height="22px" Width="22px" OnClientClick="ChangeIsOpenedCN(); ShowContextNavigationPannel(isOpenedCN); return false;"/>
                    </asp:Panel>
                </td>
                <td>
                    <asp:Panel id="divSynch" CssClass="imgSynch" ToolTip="Синхронизация" runat="server" meta:resourcekey="divSynchResource1">
                        <asp:Button runat="server" ID="btnSynch" BackColor="Transparent" BorderStyle="None"
                            ForeColor="Transparent" Height="22px" Width="22px" OnClientClick="doSynch(); return false;"/>
                    </asp:Panel>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblConNav" CssClass="lblOrdinary"/>
                    <label id="lblTest" class="lblOrdinary"/>
                </td>
            </tr>
        </table>
    </contenttemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    function SetActiveSynch(isActive) {
        if (isActive) {
            document.getElementById('<%=divSynch.ClientID %>').className = "imgSynchActive";
        }
        else {
            document.getElementById('<%=divSynch.ClientID %>').className = "imgSynch";
        }
    }
    //doSynch
    function testCN() {
        sendEvent('NAVIGATION_CONTEXT_CHANGED', "{\"IdMg\":\"1597326802\",\"NameMg\":\"Газопровод \\\"Казахстан-Китай\\\"/\\\"Kazakhstan-China\\\" gas pipeline\",\"IdThread\":\"1597327002\",\"NameThread\":\"ККГП 1 нитка/KCGP 1 line\",\"NameThreadShorten\":\"ККГП 1 нитка/KCGP 1 line\",\"KmStart\":\"595640.00\",\"KmEnd\":\"878500.00\"}", 'PROFILE');
    }
</script>

