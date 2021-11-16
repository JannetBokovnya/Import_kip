<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RedmineMenu.ascx.cs" Inherits="UserControls_Toolbar_RedmineMenu_RedmineMenu" %>

<asp:UpdatePanel ID="upnlRedmineMenu" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>

        <telerik:RadMenu ID="rmRedmine" ClientIDMode="Static" runat="server">
            <Items>
                <telerik:RadMenuItem ImageUrl="../../../Images/empty_foreground.png" CssClass="imgRightRedmine"
                                     meta:resourcekey="rmiRedmine" ToolTip="Ссылка на Redmine"  />
            </Items>
        </telerik:RadMenu>

    </ContentTemplate>

</asp:UpdatePanel>
<script type="text/javascript">
    function openRedmine() {
        window.open('<%=RedmineUrl%>');
        return false;
    }
</script>
    