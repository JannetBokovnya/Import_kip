<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InfoPanel.ascx.cs" Inherits="UserControls_Panel_InfoPanel" %>
<style type="text/css">
    div.RadListBox .rlbText {
        white-space: nowrap;
        display: inline-block;
    }

    div.RadListBox .rlbGroup {
        overflow: auto;
        z-index: 9999;
    }

    div.RadListBox .rlbList {
        display: inline-block;
        min-width: 100%;
    }

    * + html div.RadListBox .rlbList {
        display: inline;
    }

    * html div.RadListBox .rlbList {
        display: inline;
    }

    tr.infoTr td div.RadListBox {
        width: 100% !important;
        height: 100% !important;
    }

    .infoPanelDiv {
        position: absolute;
        top: 50px;
        bottom: 160px;
        width: 23%;
        background-color: #eeeeee;
        padding: 3px;
        padding-bottom: 23px;
        border-right: 1px solid #a0a0a0;
        border-bottom: 1px solid #a0a0a0;
        border-radius: 5px;
        behavior: url(../../Scripts/CSS3Converter/PIE.htc);
    }

    #radLstBox {
        font-family: Tahoma;
    }
</style>
 <%--<ClientItemTemplate>
        <div class="customClass">
            <img src="#= ImagePath #" alt="Photo" />
        </div>
    </ClientItemTemplate>--%>
<asp:HiddenField ID="hdnBMMenuItemSelected" runat="server"></asp:HiddenField>
<div class="infoPanelDiv">
    <asp:Label ID="lblCaption" Font-Bold="True" runat="server" Style="margin-left: 20px;" meta:resourcekey="lblCaptionResource1" />
    <hr style="margin: 3px 0 0 0;" />
   
            <telerik:RadListBox runat="server" ID="radLstBox" EnableDragAndDrop="True" 
                AllowReorder="false" BorderWidth="0" BorderColor="Transparent" BackColor="Transparent" Height="100%"
                Style="width: 100%;" onclientselectedindexchanged ="OnClientItemClickNotification">
                <ButtonSettings ShowReorder="false" />
            </telerik:RadListBox>
   <%-- --%>
    <telerik:RadContextMenu ID="cmCMBookmarkPanel" runat="server" EnableEmbeddedSkins="true"
        EnableRoundedCorners="true" CssClass="cmBookmark" EnableShadows="true">
        <Items>
            <telerik:RadMenuItem meta:resourcekey="rmiDeleteResource1" />
        </Items>
    </telerik:RadContextMenu>
</div>
<script type="text/javascript">
    function showCMBookmarkPanelMenu(e) {
        var contextMenu = $find("<%= cmCMBookmarkPanel.ClientID %>");

        if ((!e.relatedTarget) || (!$telerik.isDescendantOrSelf(contextMenu.get_element(), e.relatedTarget))) {
            contextMenu.show(e);
        }

        $telerik.cancelRawEvent(e);
    }
    function setBMHiddenField(selectedRMItem) {
        document.getElementById('<%=hdnBMMenuItemSelected.ClientID %>').value = '' + selectedRMItem + '';
    }
    function deleteBookmarkCM() {
        var idBookmark = document.getElementById('<%=hdnBMMenuItemSelected.ClientID %>').value;
        DeleteBookMark(idBookmark);
    }
</script>
