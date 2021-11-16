<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OraWCI.aspx.cs" Inherits="DataProvider_OraWCI" ValidateRequest="false"%>

<%
    string contentType =  GetContentType(); 
    Response.ContentType = contentType;

    if (!string.IsNullOrEmpty(OraWciParams.InSQL))
     {
         byte[] l_res = null;
         l_res = GetMainResBinary();
         if (l_res != null)
         {
             Response.BinaryWrite(l_res);
         } 
     
     } 
%>