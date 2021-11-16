<%@ Register TagPrefix="uc" TagName="VerticalIPhoneMenu" Src="~/UserControls/Startpage/IPhoneMenu/VerticalMenu.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="uc" TagName="UserMenu" Src="~/UserControls/Toolbar/UserMenu/UserMenu.ascx" %>
<%@ Register TagPrefix="uc" TagName="Panel" Src="~/UserControls/Startpage/Panel/InfoPanel.ascx" %>
<%@ Register TagPrefix="uc" TagName="BookmarkMenu" Src="~/UserControls/Toolbar/BookmarksMenu/Bookmarks.ascx" %>
<%@ Register TagPrefix="uc" TagName="Toolbar" Src="~/UserControls/Toolbar/Toolbar.ascx" %>
<%@ MasterType VirtualPath="~/System/TopMasterPage.master" %>

<%@ Page Title="StartPage" Language="C#" MasterPageFile="~/System/TopMasterPage.master"
    AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="Modules_StartPage_index" culture="auto" meta:resourcekey="PageResource1" uiculture="auto"   %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title >Начальная страница</title>
    <link rel="stylesheet" href="../../UserControls/Startpage/IPhoneMenu/Menu.iPhone.css" type="text/css" />
    <style type="text/css">
        body {
            background-color: transparent;
        }
    </style>
</asp:Content>
<asp:content id="Content2" contentplaceholderid="Body" runat="Server">
    <script type="text/javascript">
        function RestartOM() {
            __doPostBack('upnlWindows', '');
        }
        function receiveAdapter(in_EventName, in_Params) {
            return 1;
        }
    </script>

    <img src="../../Images/main_background.png" class="leftBackground" />
    <img src="../../Images/line.png" class="rightBackground" />
    <table style="height: 100%; width: 100%">
        <tr>
            <td style="z-index:0;" width="250px" valign="top"> 
                <div style="height:450px; width:100%;">
                    <uc:VerticalIPhoneMenu ID="VerticalIPhoneMenu1" runat="server" style="width: 100%;height: 100%;" />
                </div>
            </td>
            <td width="*" style="vertical-align: top; z-index:1;">
                <table style="height: 100%; width: 100%;">
                    <tr>
                       
                        <td style="width: 33%; padding-left:10px;">
                             <asp:UpdatePanel runat="server" ID="upnlWindows" UpdateMode="Conditional" ClientIDMode="Static">
                                <ContentTemplate>
                                    <uc:Panel ID="pnlOpenWindows" runat="server" meta:resourcekey="pnlOpenWindowsResource1" IsCheckBox="False"/>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        
                        <td style="width: 33%; padding-left:10px;">
                            <asp:UpdatePanel runat="server" ID="upnlBookmark" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <uc:Panel ID="pnlBookMark" runat="server" meta:resourcekey="pnlBookMarkResource1" IsCheckBox="False" 
                                              style="z-index: 0; background-color: transparent;" 
                                              OnBookmarkDeleted="Modules_StartPage_index_HiddenFieldChanged" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                       
                        <td style="width: 34%; padding-left:10px;">
                            <asp:UpdatePanel runat="server" ID="upnlNotifications" UpdateMode="Conditional" ClientIDMode="Static">
                                <ContentTemplate>
                                    <uc:Panel ID="pnlNotifications" runat="server" IsCheckBox="True"
                                              meta:resourcekey="pnlNotificationsResource1"/>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        </table>
    <div style="position: absolute; bottom: 15px; width: 100%;">
        <table width="100%">
            <tr valign="bottom">
               <%-- <td>
                    <asp:Label runat="server" Text="ФИО пользователя: " ID="fio"></asp:Label>
                </td>--%>
                <td>
                    <asp:Image ID="Image1" runat="server" CssClass="imgCustomer" 
                        style="margin-left: 15px;" ImageUrl="~/Images/main_logo.png" />
                </td>
                <td align="right" style="padding-right: 15px;">
                    <asp:Panel ID="pnlImgITT" class="imgITT" meta:resourcekey="imgITTResource1" onclick="window.open('http://www.it-transit.com/');"
                               runat="server"/>
                </td>
            </tr> 
        </table>
    </div>
    
    <script type="text/javascript">
        var bindingDivWidth = "16px";
        var bindingDivHeight = "16px";
        var bindingDivMargin = "1px";
        var bindingDivBorder = "0px solid black";
        var imageBindingDiv = "../../Images/head.png";

        function changeCheckBoxStyle() {
            var listBoxIndex = 0;
            $("#upnlNotifications input[type='checkbox']").each(function () {
                $(this).css({ opacity: 0.0 });

                var bindingDivPosition = "absolute";
                var divLeft = $(this).position().left + "px";
                var divTop = $(this).position().top + "px";

                var divStyle = "width:" + bindingDivWidth
                    + ";height:" + bindingDivHeight
                    + ";border:" + bindingDivBorder
                    + ";position:" + bindingDivPosition
                    + ";margin:" + bindingDivMargin
                    + ";left:" + divLeft
                    + ";top:" + divTop
                    + ";background:" + 'url(' + imageBindingDiv + ')';

                if (!this.checked) {
                    divStyle += ";background-position:-4px -55px";
                } else {
                    divStyle += ";background-position:-31px -55px";
                }

                var div = "<div id='divCheckBox' onclick='changeCheckBox(this)' style='" + divStyle + "' listBoxIndex=" + listBoxIndex++ + "></div>";

                $(this).after(div);
            });
        }

        // Корректировка позиции байдинговых элементов при изменении размера окна
        $(window).resize(function () {
            $("input[type='checkbox']").each(function () {
                var divLeft = $(this).position().left + "px";
                var divTop = $(this).position().top + "px";

                var bindedDiv = $(this).parent().children("div#divCheckBox");
                bindedDiv.css("left", divLeft);
                bindedDiv.css("top", divTop);
            });
        });


    </script>

</asp:content>
