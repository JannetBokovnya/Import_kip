<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Bookmarks.ascx.cs" Inherits="UserControls_Menu_BookmarksMenu_Bookmarks" %>

<style type="text/css">
    .notificationCount {
        position: absolute;
        top: 13px;
        right: 75px;
        font-size: 8px;
        color: #ffffff;
        behavior: url(../../Scripts/CSS3Converter/PIE.htc); 
        border-radius: 50%;
        background: #f69305;
        height: 12px; 
        width: 12px; 
        line-height: 12px;
        text-align: center;  
        z-index: 9999;
        cursor: default;
    }
        
</style>

<asp:UpdatePanel runat="server" ID="upnlBookmarkMenu" UpdateMode="Conditional" OnLoad="upnlBookmarkMenu_Load" ClientIDMode="Static" >
    <ContentTemplate>
        <telerik:RadMenu id="rmBookmarks" runat="server" ClickToOpen="true" ClientIDMode="Static"
                         OnClientItemClicked="OnClientItemClicking" OnClientLoad="AddMenuToArray" 
                         OnClientMouseOver="OnClientMouseOverHandler"> 
            <Items>
                <telerik:RadMenuItem runat="server" id="btnBookmarks"  ImageUrl="../../../Images/empty_foreground.png" StaysOpenOnClick="False"
                                     CssClass="imgRightBookmark" ToolTip="Закладки" meta:resourcekey="btnBookmarksResource1"/>
            </Items>
        </telerik:RadMenu>
        <telerik:radcontextmenu id="cmCMBookmark" runat="server" EnableEmbeddedSkins="False"  
            enableroundedcorners="true" cssclass="cmBookmark" enableshadows="true" onitemclick="cmCMBookmark_ItemClick" 
            style="z-index: 9000;" >
            <Items>
                <telerik:RadMenuItem ID="rmiDelete" runat="server" Text="Удалить" PostBack="true"  
                                     meta:resourcekey="rmiDeleteResource1"/>
            </Items>
        </telerik:radcontextmenu>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel runat="server" ID="upnlNotificationMenu" UpdateMode="Conditional" ClientIDMode="Static">
    <ContentTemplate>
        <telerik:RadMenu id="rmNotification" runat="server" AutoScrollMinimumWidth="400" EnableAutoScroll="true" 
                         ClickToOpen="true" OnClientItemClicked="OnClientItemClicking" OnClientLoad="AddMenuToArray" OnClientItemClicking="OnClientItemSelectedMenu"
                         OnClientMouseOver="OnClientMouseOverHandler" ClientIDMode="Static" >
            <Items>
                <telerik:RadMenuItem runat="server" ID="itmNotification" ImageUrl="../../../Images/empty_foreground.png" CssClass="imgRightNotification" ClientIDMode="Static"
                                     ToolTip="Уведомление" meta:resourcekey="itmNotificationResource1" />
            </Items>
        </telerik:RadMenu>
        <asp:Label runat="server" CssClass="notificationCount" style="display: none;">0</asp:Label>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:hiddenfield id="hdnMenuItemSelected" value="" runat="server"></asp:hiddenfield>
<asp:HiddenField ID="hdnNewBookmark" Value="" runat="server" />
<asp:HiddenField ID="hdnAddedBookmark" Value="" runat="server" ViewStateMode="Enabled" ClientIDMode="Static" />
<asp:HiddenField ID="hdnUser" Value="" runat="server" ClientIDMode="Static" />

<script type="text/javascript">
    function setHiddenField(selectedRMItem) {
        document.getElementById('<%=hdnMenuItemSelected.ClientID %>').value = '' + selectedRMItem + '';
    }

    function showCMBookmarkMenu(e) {
        var contextMenu = $find("<%= cmCMBookmark.ClientID %>");

        if ((!e.relatedTarget) || (!$telerik.isDescendantOrSelf(contextMenu.get_element(), e.relatedTarget))) {
            contextMenu.show(e);
        }

        $telerik.cancelRawEvent(e);
    }

    function ShowBookmarkPannel(toOpen) {
        topPanel = 0;  //topPanel = 40; не правильно смещает в ие8 панель
        widthPanel = 0; //widthPanel = 5;
        leftPanel = document.body.clientWidth - document.getElementById('divBookMark').clientWidth;
        if (toOpen) {
            //document.getElementById('divBookMark').style.top = topPanel;
            document.getElementById('divBookMark').style.visibility = 'visible';
            document.getElementById('divContent').style.top = topPanel;
            HidePannel('divContextNavigation');
            //написать название
        }
        else {
            HidePannel('divBookMark');
            document.getElementById('divContent').style.top = 0;
        }
        toggleDisabled(document.getElementById('divContent'), toOpen);
    }

    function setCurrentState(p_cValue) {
        var l_json = '{"AppName":"' + GetWindowNameForBookmark() + '", "StateID":' + p_cValue + ', "AppTitle": "' + GetWindowTitle() + '", "User": "' + document.getElementById('<%=hdnUser.ClientID %>').value + '" }';
        document.getElementById('<%=hdnNewBookmark.ClientID %>').value = '' + encodeURI(l_json) + '';
    }

    //функция для проверки новых уведомлений
    function AddBookMark(appTitle) {
        //пока нет этой ф-ции
        newBookMark = document.getElementById('<%=hdnNewBookmark.ClientID %>').value;
        newBookMark = decodeURI(newBookMark);
        if (newBookMark != "") {
            //разобрать по косточками эту закладку
            var stateID = JSON.parse(newBookMark).StateID;
            var strStateID = JSON.stringify(stateID);
            if (strStateID == null) {
                strStateID = '{}';
            }
                
            var appName = JSON.parse(newBookMark).AppName;
            var appTitle = appTitle;  // JSON.parse(newBookMark).AppTitle;
            var user = JSON.parse(newBookMark).User;         
            var url = GetBaseURL() + GetService() + "?inParams=&inType=2&nSeq=&isCompress=0&u=0.41930";
            //var params = "inSQL=db_api.st_state.AddNewState('" + encodeURIComponent(strStateID) + "', '" + encodeURIComponent(appName) + "', '" + encodeURIComponent(appTitle) + "', '" + user + "')";
            var params = "inSQL=db_api.WRAPPER_API.AddNewState('" + encodeURIComponent(strStateID) + "', '" + encodeURIComponent(appName) + "', '" + encodeURIComponent(appTitle) + "', '0')";
            //переделать строчку в скрытом поле
            var newTMP = '{"AppName":"STARTPAGE", "StateID":{}, "AppTitle": "' + appTitle + '", "User": "-1" }';
            document.getElementById('<%=hdnNewBookmark.ClientID %>').value = newTMP;
        }
        ajaxThis(url, ParseAddBookMark, false, 'POST', params);
    }

    function checkNewBookmarkFromCookies() {
        var res = getCookieByName('bookMark');
        if ((res != undefined)) {
            if (document.getElementById('<%=hdnNewBookmark.ClientID %>').value != '' + res + '') {
                document.getElementById('<%=hdnNewBookmark.ClientID %>').value = '' + res + '';
                __doPostBack('upnlBookmarkMenu', '');
            }
        }
        setTimeout(checkNewBookmarkFromCookies, 10000);
    }
    function ParseDeleteBookMark(req) {
        var strResponse = req.responseText;
        document.getElementById('<%=hdnNewBookmark.ClientID %>').value = 'BookmarkIsDeletedCheckPls';
        __doPostBack('upnlBookmarkMenu', '');
    }


    //функция для обработки есть ли новые уведомления
    function ParseAddBookMark(req) {
        var strResponse = req.responseText;
        if (strResponse.search('<Result>False</Result>') > 0) {
            alert('Произошла ошибка при добавлении новой закладки');
        }
        else {
            alert('Добавлена закладка');
            document.getElementById('<%=hdnAddedBookmark.ClientID %>').value = true;
            //     __doPostBack('upnlBookmarkMenu', '');
        }
    }
   
</script>

