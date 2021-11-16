<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Toolbar.ascx.cs" Inherits="UserControls_Toolbar_Toolbar" %>
<%@ Register TagPrefix="uc" TagName="RedmineMenu" Src="~/UserControls/Toolbar/RedmineMenu/RedmineMenu.ascx" %>
<%@ Register TagPrefix="uc" TagName="FilterMenu" Src="~/UserControls/Toolbar/FilterMenu_Long/FilteringMenu.ascx" %>
<%@ Register TagPrefix="uc" TagName="BookmarkMenu" Src="~/UserControls/Toolbar/BookmarksMenu/Bookmarks.ascx" %>
<%@ Register TagPrefix="uc" TagName="UserMenu" Src="~/UserControls/Toolbar/UserMenu/UserMenu.ascx" %>
<%@ Register TagPrefix="uc" TagName="ContextNavigation" Src="~/UserControls/Toolbar/ContextNavigationMenu/ContextNavigationMenu.ascx" %>
<%@ Register TagPrefix="uc" TagName="LangMenu" Src="~/UserControls/Toolbar/LangMenu/LangMenu.ascx"  %>


<td valign="middle" width="20">
    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/favicon.ico" onclick="sendEvent('OPEN_WINDOW', '{}', 'STARTPAGE');" Style="margin:6px 0px 4px 5px; cursor: hand" meta:resourcekey="Image1Resource1" />
</td>
<td valign="middle" style="position: relative;  width: 21%;overflow: hidden; text-overflow: ellipsis;" >
	<asp:Label runat="server" ID="lblMainPage" CssClass="lblMainPage" ClientIDMode="Static" meta:resourcekey="lblMainPageResource1" />
</td>
<td valign="middle" width="*" align="center">
    <uc:FilterMenu ID="filterMenu1" runat="server" ClientIDMode="Static"/>
   
</td>
<%--width="260"--%>
<td runat="server" id="tdCN" valign="middle"  align="left"  class="tdCN" width="260" ClientIDMode="Static">
    <uc:ContextNavigation ID="contextNavigation" runat="server" ClientIDMode="Static" />
</td> 
<%--<td width="160">
    <asp:Label ID="laUserFio"  runat="server"/>
</td>--%>
<td valign="middle" style="width:150px;">
    <asp:UpdatePanel runat="server" ID="upnlUserMenu" UpdateMode="Conditional" OnLoad="upnlUserMenu_Load" ClientIDMode="Static">
        <ContentTemplate>
            <div runat="server" style="float: right">
                <uc:RedmineMenu ID="redmineMenu" runat="server" ClientIDMode="Static" />
                <uc:BookmarkMenu ID="BookmarkMenu" runat="server" ClientIDMode="Static" />
                <uc:UserMenu ID="userMenu"  runat="server" ClientIDMode="Static"/>
                <uc:LangMenu ID="LandMenu" runat="server" ClientIDMode="Static" />
            </div>
           
            <asp:HiddenField ID="hdnAddedBookmark" runat="server" ViewStateMode="Enabled" ClientIDMode="Static" />
            
        </ContentTemplate>
    </asp:UpdatePanel>
</td>

<asp:HiddenField ID="hdnNewBookmark" runat="server" />
<asp:HiddenField ID="hdnUser" runat="server" />
<script>
    function SetInLabelModulesMenu() {
        document.getElementById('<%=lblMainPage.ClientID %>').innerText = GetWindowTitle();
    }
    function RestartOM() {
        //заглушка для страниц. Для АГП обновление на Стартпейдже
    }
</script>
