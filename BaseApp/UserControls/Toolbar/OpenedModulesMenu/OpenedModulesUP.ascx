<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OpenedModulesUP.ascx.cs" Inherits="OpenedModulesUP" %>
<%@ Register TagPrefix="uc" TagName="OpenedModulesMenu" Src="~/UserControls/Toolbar/OpenedModulesMenu/OpenedModulesMenu.ascx" %>

<asp:UpdatePanel runat="server" id="upnlOpenModules" ClientIDMode="Static"  UpdateMode="Conditional" OnLoad="upnlOpenModules_Load">
    <ContentTemplate>
        <uc:OpenedModulesMenu ID="openedModulesMenu" runat="server" Visible="True" />
    </ContentTemplate>
</asp:UpdatePanel>
