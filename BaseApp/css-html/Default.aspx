<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Modules_Transparent_Default" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Controls</title>
    <link rel="stylesheet" type="text/css" href="FlashStyle.css" />
    <link rel="stylesheet" type="text/css" href="ComboBox.Style.css" />
    <link rel="stylesheet" type="text/css" href="Button.Style.css" />
    <link rel="stylesheet" type="text/css" href="Input.Style.css" />
</head>
<body>
    <form id="form1" runat="server" style="width: 300px; margin: 5px; padding: 5px;">
        <div style="display: none;">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        </div>
        <div style="width: 300px; margin: 15px; background: pink;">
            <asp:Button runat="server" Text="I am Asp:Button" CssClass="FlashButton" 
                        Width="100%" Height="29px"/>
            <br/><br/>
            <asp:Button ID="Button1" runat="server" Text="I am Asp:Button" CssClass="FlashButton" 
                        Width="100%" Height="29px" Enabled="False"/>
        </div>
        <div style="width: 300px; margin: 15px; background: pink;">
            <telerik:RadComboBox runat="server" Skin="Flash" EnableEmbeddedSkins="False" 
                                 EmptyMessage="Пусто" Width="100%">
                <Items>
                    <telerik:RadComboBoxItem Text="Winter"/>
                    <telerik:RadComboBoxItem Text="Fall"/>
                    <telerik:RadComboBoxItem Text="Spring"/>
                    <telerik:RadComboBoxItem Text="Summer"/>
                </Items>
            </telerik:RadComboBox>
            <br/><br/>
            <telerik:RadComboBox runat="server" Skin="Flash" EnableEmbeddedSkins="False" 
                                 EmptyMessage="Пусто" Width="100%" Enabled="False">  
                <Items>
                    <telerik:RadComboBoxItem Text="Winter"/>
                    <telerik:RadComboBoxItem Text="Fall"/>
                    <telerik:RadComboBoxItem Text="Spring"/>
                    <telerik:RadComboBoxItem Text="Summer"/>
                </Items>
            </telerik:RadComboBox>
        </div>
        <div style="width: 300px; margin: 15px; background: pink;">
            <telerik:RadComboBox runat="server" Width="100%" EmptyMessage="Type an E-mail" 
                                 Skin="Flash" EnableEmbeddedSkins="False" 
                                 Filter="Contains" MarkFirstMatch="True" >
                <Items>
                    <telerik:RadComboBoxItem Text="Fall"/>
                    <telerik:RadComboBoxItem Text="Spring"/>
                    <telerik:RadComboBoxItem Text="Summer"/>
                    <telerik:RadComboBoxItem Text="Winter"/>
                    <telerik:RadComboBoxItem Text="Winter1"/>
                    <telerik:RadComboBoxItem Text="Winter2"/>
                    <telerik:RadComboBoxItem Text="Winter3"/>
                    <telerik:RadComboBoxItem Text="Fall1"/>
                    <telerik:RadComboBoxItem Text="Spring1"/>
                    <telerik:RadComboBoxItem Text="Summer1"/>
                    <telerik:RadComboBoxItem Text="Fall2"/>
                    <telerik:RadComboBoxItem Text="Spring2"/>
                    <telerik:RadComboBoxItem Text="Summer2"/>
                    <telerik:RadComboBoxItem Text="Fall3"/>
                    <telerik:RadComboBoxItem Text="Spring3"/>
                    <telerik:RadComboBoxItem Text="Summer3"/>
                </Items>
            </telerik:RadComboBox>
            <br/><br/>
            <telerik:RadComboBox runat="server" Width="100%" EmptyMessage="Type an E-mail" 
                                 Skin="Flash" EnableEmbeddedSkins="False" 
                                 Filter="Contains" MarkFirstMatch="True" Enabled="False" >
                <Items>
                    <telerik:RadComboBoxItem Text="Fall"/>
                    <telerik:RadComboBoxItem Text="Spring"/>
                    <telerik:RadComboBoxItem Text="Summer"/>
                    <telerik:RadComboBoxItem Text="Winter"/>
                    <telerik:RadComboBoxItem Text="Winter1"/>
                    <telerik:RadComboBoxItem Text="Winter2"/>
                    <telerik:RadComboBoxItem Text="Winter3"/>
                    <telerik:RadComboBoxItem Text="Fall1"/>
                    <telerik:RadComboBoxItem Text="Spring1"/>
                    <telerik:RadComboBoxItem Text="Summer1"/>
                    <telerik:RadComboBoxItem Text="Fall2"/>
                    <telerik:RadComboBoxItem Text="Spring2"/>
                    <telerik:RadComboBoxItem Text="Summer2"/>
                    <telerik:RadComboBoxItem Text="Fall3"/>
                    <telerik:RadComboBoxItem Text="Spring3"/>
                    <telerik:RadComboBoxItem Text="Summer3"/>
                </Items>
            </telerik:RadComboBox>
        </div>
        <div style="width: 300px; margin: 15px; background: pink;">
            <telerik:RadButton runat="server" Text="I am telerik Button" Skin="Flash" 
                               EnableEmbeddedSkins="False"  Width="100%" Height="29px">
            </telerik:RadButton>
            <br/><br/>
            <telerik:RadButton runat="server" Text="I am telerik Button" Skin="Flash" 
                               EnableEmbeddedSkins="False" Width="100%"  Height="29px"
                               Enabled="False">
            </telerik:RadButton>
        </div>
        <div style="width: 300px; margin: 15px; background: pink;">
            <telerik:RadTextBox runat="server" Skin="Flash" EnableEmbeddedSkins="False" 
                                Width="100%" EmptyMessage="Пустота" Height="35" />
            <br/><br/>
            <telerik:RadTextBox runat="server" Skin="Flash" EnableEmbeddedSkins="False" 
                                Width="100%" EmptyMessage="Пустота" Height="25" 
                                Enabled="False"/>
        </div>
        <div>
            <span style="height: 50px;background: pink; display: table-cell; vertical-align: middle;">Hello world</span>
        </div>
    </form>
</body>
</html>
