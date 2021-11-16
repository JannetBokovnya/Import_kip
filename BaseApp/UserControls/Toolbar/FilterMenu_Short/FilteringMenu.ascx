<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilteringMenu.ascx.cs"
    Inherits="UserControls_Menu_FilterMenu_FilteringMenu" %>
<div id="pnlContextMenu">
    <telerik:RadMenu ID="cmMainMenu" runat="server" EnableRoundedCorners="true"
        EnableShadows="true" EnableImageSprites="true" EnableAutoScroll="true" ClickToOpen="true" 
        OnClientItemClicked="OnClientItemClicking" OnClientLoad="AddMenuToArray" OnClientMouseOver="OnClientMouseOverHandler">
        <Items>
            <telerik:RadMenuItem ImageUrl="../../../Images/m_blue.png" CssClass="imgMainMenu" />
        </Items>
    </telerik:RadMenu>
</div>
<script>
    function DeselectFilterMenu() {
    }
</script>
