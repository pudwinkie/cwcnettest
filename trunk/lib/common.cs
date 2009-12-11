///
/// XML Тр DataTable
///
public void Service(string Xmlstr)
{

    XmlDocument doc = new XmlDocument();
    doc.LoadXml(Xmlstr);
    XmlReader XR = XmlReader.Create(new System.IO.StringReader(doc.OuterXml));
    DataSet ds = new DataSet();
    ds.ReadXml(XR);
    DataTable dt = ds.Tables[0];      
}