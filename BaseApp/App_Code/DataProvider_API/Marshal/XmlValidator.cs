using System;
using System.IO;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using log4net;

namespace DataProvider_API.Marshal
{
    /// <summary>
    /// Summary description for XmlValidator
    /// </summary>
    public class XmlValidator
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataProvider).Name);
        private static readonly string FolderPath = AppDomain.CurrentDomain.BaseDirectory + "Modules\\";
        private string WebPath { get; set; }
        private string ErrMsg { get; set; }

        public XmlValidator(OraWCI oraWciParams)
        {
            string query = oraWciParams.InSQL;
            if(oraWciParams.InSQL.IndexOf("(") > 0)
                query = query.Remove(query.IndexOf("("));
            query = query.Substring(query.LastIndexOf(".") + 1);

            if (oraWciParams.ModuleName != null)
                WebPath = FolderPath + oraWciParams.ModuleName + "\\ClientBin\\XSD\\" + query + ".xsd";
            else
            {
                Log.Info("В запросе к OraWCI отсутствует inModuleName");
            }
        }

        public string CheckValid(string xml)
        {
            string xsdInRes = GetXsdFromFile();

            if (!String.IsNullOrWhiteSpace(xsdInRes))
            {
                bool result = IsValidXmlToXsd(xml, xsdInRes);   
            }

            return ErrMsg;
        }

        private string GetXsdFromFile()
        {
            //read resx file
            //return HttpContext.GetLocalResourceObject(WebPath, Query);
            
            //read xsd file
            string line = String.Empty;
            if (File.Exists(WebPath))
                using (StreamReader sr = new StreamReader(WebPath))
                {
                    line = sr.ReadToEnd();
                }
            return line;
        }

        private bool IsValidXmlToXsd(string xml, string xsd)
        {
            bool result = true;

            XDocument xdoc = XDocument.Parse(xml);
            XmlSchemaSet schemas = new XmlSchemaSet();
            XmlSchema schema = XmlSchema.Read(new StringReader(xsd), (sender, args) => { });
            schemas.Add(schema);

            xdoc.Validate(schemas, (sender, e) => { result = false; ErrMsg = e.Message; });

            return result;
        }

        private string GenerateXsdFromXml(string xml)
        {
            string xsd = "";

            XmlReader xmlReader = XmlReader.Create(new StringReader(xml));

            XmlSchemaInference schemaInference = new XmlSchemaInference();
            XmlSchemaSet schemaSet = schemaInference.InferSchema(xmlReader);

            foreach (XmlSchema xmlSchema in schemaSet.Schemas())
            {
                StringWriter sw = new StringWriter();
                xmlSchema.Write(sw);
                xsd = sw.ToString();
            }

            return xsd;
        }

        private void WriteXsdToRes(string value)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(FolderPath);

            XmlElement dataElement = doc.CreateElement("data");
            dataElement.SetAttribute("name", "ИмяЗначение");
            dataElement.SetAttribute("xml:space", "preserve");
            XmlElement velueElement = doc.CreateElement("value");
            velueElement.InnerText = value;

            dataElement.AppendChild(velueElement);

            if (doc.DocumentElement != null)
            {
                doc.DocumentElement.AppendChild(dataElement);
            }
            else
            {
                throw new NullReferenceException("doc.DocumentElement is null");
            }

            doc.Save(FolderPath);
        }
    }
}