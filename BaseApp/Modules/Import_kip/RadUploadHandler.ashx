<%@ WebHandler Language="C#" Class="RadUploadHandler" %>

using System;
using System.IO;
using System.Web;

public class RadUploadHandler : Telerik.Windows.RadUploadHandler
{
    public override System.Collections.Generic.Dictionary<string, object> GetAssociatedData()
    {

        System.Collections.Generic.Dictionary<string, object> dict = new System.Collections.Generic.Dictionary<string, object>();

        string fileName = this.Request.Form["0_Filename"];

        string clientParamValue = this.Request.Form["MyParam1"];
        if (clientParamValue != null)
        {

            dict.Add("MyServerParam1",
                String.Format("Server_Value\n[{0}]\nThe file name is\n[{1}]",
                clientParamValue,
                this.Request.Form[Telerik.Windows.Controls.RadUploadConstants.ParamNameFileName]));
            string fname = this.Request.Form[Telerik.Windows.Controls.RadUploadConstants.ParamNameFileName];
            System.IO.File.Copy(fname, "user_" + fname);
        }

        //string extfile = Path.GetExtension(args.FileName).ToLower();
        //string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload/UploadedImportVTD/") + "\\", fileName);

        //FileStream fileStream = new FileStream(HttpContext.Current.Server.MapPath("~/Upload/Passport/Upload") + "\\" + fileName, FileMode.Open);
        
        //using (FileStream fs = File.Create(Path.Combine(HttpContext.Current.Server.MapPath("~/Upload/UploadedImportVTD/") + "\\", fileName)))
        //{
        //    byte[] buffer = new byte[4096];
        //    int bytesRead;
        //    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
        //    {
        //        fs.Write(buffer, 0, bytesRead);
        //    }
        //    fs.Close();
        //   // SaveFile(HttpContext.Current.Request.InputStream, fs);
            
        //    //HttpContext.Current.Server.MapPath("~/Upload/Passport/Upload") + "\\" + fileName
        //}
        
        
        return dict;
    }

    private void SaveFile(Stream stream, FileStream fs)
    {
        byte[] buffer = new byte[4096];
        int bytesRead;
        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            fs.Write(buffer, 0, bytesRead);
        }
        fs.Close();
    }
}