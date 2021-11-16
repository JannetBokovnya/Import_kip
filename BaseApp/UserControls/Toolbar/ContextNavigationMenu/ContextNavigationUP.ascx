<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ContextNavigationUP.ascx.cs" Inherits="ContextNavigavionUP" %>

<%@ Register TagPrefix="uc" TagName="ContextNavigationMenu" Src="~/UserControls/Toolbar/ContextNavigationMenu/ContextNavigationMenu.ascx" %>

<asp:UpdatePanel runat="server" ID="upnlContextNavigation" UpdateMode="Conditional" Visible="True">
    <ContentTemplate>
        <uc:ContextNavigationMenu ID="contextNavigationMenu" runat="server" Visible="True" />
    </ContentTemplate>
</asp:UpdatePanel>
