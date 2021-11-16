<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OpenedModulesMenu.ascx.cs"
    Inherits="UserControls_Menu_OpenedModulesMenu_OpenedModulesMenu" %>

<telerik:RadMenu ID="rmOpenedModulesMain" runat="server" EnableRoundedCorners="true"
    Height="30" EnableShadows="true" EnableEmbeddedSkins="False" EnableImageSprites="true" ClickToOpen="true" 
    OnClientMouseOver="OnClientMouseOverHandler" OnClientItemClicking="OnClientItemClicking" OnClientLoad="AddMenuToArray"/>