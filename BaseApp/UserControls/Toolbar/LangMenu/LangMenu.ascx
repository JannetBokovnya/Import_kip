<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LangMenu.ascx.cs" Inherits="UserControls_Menu_LangMenu_LangMenu" %>

<style>
    .langMenu {
        float: right;
        margin: 0 3px 0 3px;
        padding-right: 2px;
    }

     .langMenu > .RadMenu > .rmRootGroup, 
     .langMenu > .RadMenu > .rmHorizontal > .rmItem > a.rmLink, 
     .langMenu > .RadMenu > .rmHorizontal > .rmItem > a.rmLink > .rmText {
         background-color: #0096db !important;
         background-image: none !important;
     }

    .langMenu > .RadMenu > .rmHorizontal > .rmItem > .rmLink, 
    .langMenu > .RadMenu > .rmHorizontal > .rmItem > a.rmLink > .rmText {
        padding: 0;
    }
</style>
<script>
    function MyFunction(sender, args) {
        OnClientItemClicking(sender, args);

        var itemChildCount = args.get_item().get_items().get_count();
        if (itemChildCount == 0) {
            var itemImageUrl = args.get_item().get_imageUrl();
            if (itemImageUrl == null) {
                closeAllWindows();
            }
        }
    }

    
</script>
<div class="langMenu" style="vertical-align: middle;">
    <telerik:RadMenu ID="rmLang" runat="server"  ClickToOpen="True" EnableEmbeddedSkins="False" 
                     EnableRoundedCorners="true" EnableShadows="true" EnableAutoScroll="true" 
                     OnItemClick="rmLang_OnItemClick" OnClientItemClicked="MyFunction" 
                     OnClientLoad="AddMenuToArray" OnClientMouseOver="OnClientMouseOverHandler" >
        <Items>
            <telerik:RadMenuItem runat="server" Text="RU"  StaysOpenOnClick="False" CssClass="langMenu" 
                                 Font-Bold="True" ForeColor="White" PostBack="False" meta:resourcekey="lblLangResource1" >
                <Items>
                    <telerik:RadMenuItem runat="server" Text="<b>RU</b> Русский" Value="ru-RU"/>
                    <telerik:RadMenuItem runat="server" Text="<b>EN</b> English" Value="en-US"/>
                </Items>
            </telerik:RadMenuItem>
        </Items>
    </telerik:RadMenu>
</div>

