<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="ForbiddenPage"  MasterPageFile="~/System/System.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
    <title>Сайт недоступен</title>
    <style>
        .rounded {
            width: 320px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    <div style="height: 100px; text-align: center; font-size: 10pt;">
        <p style="font-size: 12pt;"><strong>Сайт временно недоступен</strong></p><br />
        
        <p> Ведутся технические работы (обновление системы).</p>
    </div>
    <font size="2pt">
        <div align="right">Разработчик: ООО&nbsp;&laquo;ИТ-Транзит&raquo;
            <br/>
            <a href="http://www.it-transit.com">http://www.it-transit.com</a>
        </div>
    </font>
</asp:Content>   