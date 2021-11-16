<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilteringMenu.ascx.cs"
    Inherits="UserControls_Menu_FilterMenu_FilteringMenu" %>
<div id="pnlContextMenu" style="width:475px" align="left">
    <telerik:RadMenu ID="cmMainMenu" runat="server" Skin="WebBlue" EnableEmbeddedSkins="False" 
        EnableRoundedCorners="true" ClientIDMode="Static" EnableShadows="true" EnableImageSprites="true" 
        EnableAutoScroll="true" ClickToOpen="true" EnableImagePreloading="false" OnClientLoad="AddMenuToArray"
        OnClientItemClicked="OnClientItemClicking" OnClientMouseOver="OnClientMouseOverHandler" >
    </telerik:RadMenu>
</div>
<style type="text/css">
    div#cmMainMenu > ul.rmRootGroup > li.rmItem >a > span.rmLeftImage {
        display: none !important;
    }
    div#cmMainMenu > ul.rmRootGroup > li.rmItem >a > span.rmText {
        padding: 0px 10px 0px 10px !important;
        color: #444444;
    }
</style>
<script>
    function DeselectFilterMenu() {
        var filterMenu = $find("<%=cmMainMenu.ClientID %>"); 
        var item = filterMenu.get_selectedItem();
        if(item != null)
            item.set_selected(false);
    }
</script>
