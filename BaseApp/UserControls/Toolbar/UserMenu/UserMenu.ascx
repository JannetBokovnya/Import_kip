<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserMenu.ascx.cs" Inherits="UserControls_Menu_UserMenu_UserMenu" %>

<telerik:RadMenu ID="rmUser" ClientIDMode="Static" runat="server"  EnableEmbeddedSkins="False" 
    EnableRoundedCorners="true" EnableShadows="true" EnableImageSprites="true" EnableAutoScroll="true"
    ClickToOpen="true" OnClientLoad="AddMenuToArray" OnClientItemClicked="OnClientItemClicking" 
    OnClientMouseOver="OnClientMouseOverHandler" >
    <Items>
        <telerik:RadMenuItem ImageUrl="../../../Images/empty_foreground.png" CssClass="imgRightHelp"
            ToolTip="Помощь" meta:resourcekey="rmiHelpResource1" />
        <telerik:RadMenuItem ImageUrl="../../../Images/empty_foreground.png" CssClass="imgRightUser"
            ToolTip="Пользовательские настройки" meta:resourcekey="rmiSettingResource1" />
    </Items>
</telerik:RadMenu>
