
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BookmarkPanel.ascx.cs" Inherits="UserControls_Panel_Bookmark" %>
<%--<asp:UpdatePanel runat="server" ID="upnlBookmark" UpdateMode="Conditional" OnLoad="upnlBookmark_Load">
    <contenttemplate>

--%>
<table border="0" style="margin: 0; padding: 0;">
    <tr>
        <td style="width: 150px">
            <asp:Label ID="lblTitle" runat="server" Font-Bold="True" meta:resourcekey="LabelTitleResource1" ></asp:Label>
            
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtComment" Width="300px" meta:resourcekey="txtCommentResource1" />
       </td>
        <td>
            <asp:UpdatePanel runat="server" ID="upnlBookmark" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel class="imgOK" runat="server" ID="pnlAddBookmark" meta:resourcekey="pnlAddBookmarkResource1">
                        <asp:ImageButton runat="server" ID="btnAddBookmark" BackColor="Transparent" BorderStyle="None"
                            ForeColor="Transparent" Height="22px" Width="22px"
                            OnClientClick="AddBookMark(GetTitleBookmark()); toOpen=false; ShowBookmarkPannel(toOpen)" meta:resourcekey="btnAddBookmarkResource1" ImageUrl="~/Images/empty_foreground.png" />
                    </asp:Panel>

                    <asp:Panel class="imgClose" runat="server" ID="pnlCancel" ToolTip="Отмена" meta:resourcekey="pnlCancelResource1">
                        <asp:ImageButton runat="server" ID="btnCancel" BackColor="Transparent" BorderStyle="None"
                            ForeColor="Transparent" Height="22px" Width="22px" OnClientClick="toOpen=false; ShowBookmarkPannel(toOpen)" meta:resourcekey="btnCancelResource1" ImageUrl="~/Images/empty_foreground.png" />
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
<script type="text/javascript">
    function SetTitleToBookmark(title) {
        document.getElementById('<%=txtComment.ClientID %>').value = title;
    }

    function GetTitleBookmark() {
        return document.getElementById('<%=txtComment.ClientID %>').value;
    }

    
</script>
