<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserExit.ascx.cs" Inherits="UserControls_Menu_UserMenu_UserMenu" %>

<telerik:RadMenu ID="rmExit" ClientIDMode="Static" runat="server"  EnableEmbeddedSkins="False" 
    EnableRoundedCorners="true" EnableShadows="true" EnableImageSprites="true" EnableAutoScroll="true"
    ClickToOpen="true" OnClientLoad="AddMenuToArray" OnClientItemClicked="OnClientItemClicking" 
    OnClientMouseOver="OnClientMouseOverHandler" >
    <Items>
        <telerik:RadMenuItem ImageUrl="../../../Images/empty_foreground.png" CssClass="imgRightExit" 
            ToolTip="Выход" />
    </Items>
</telerik:RadMenu>

<script type="text/javascript">

    function logOff() {
        var url = GetBaseURL() + '/Admin_module/Login.aspx';
        window.location.href = url;
    }

</script>
