<%@ Page Title="" Language="C#" MasterPageFile="~/System/TopMasterPage.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index"  culture="auto" uiculture="auto" meta:resourcekey="PageResource1" %>
<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Импорт замеров КИП</title>
    <style type="text/css">
        html, body
        {
            height: 100%;
            overflow: auto;
        }
        body
        {
            padding: 0;
            margin: 0;
        }

        #silverlightControlHost
        {
            height: 100%;
            text-align: center;
        }

    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Body" Runat="Server">
 
        <div style="height:100%; width:100%; font-size:0;"> 
            <object id="SilverlightImportKIPControl" data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="100%" height="100%">
                <param name="source" value="ClientBin/ImportKIP.xap"/>
                <param name="windowless" value="true" />
                <param name="wmode" value="opaque"/>
                <param name="initParams"  value="lang= <asp:Literal id="id" runat="server"/>, context = 10, wmode = opaque" />
            </object>  
        </div> 
    <asp:HiddenField ID="HtmlText1" runat="server" /> 
</asp:Content>