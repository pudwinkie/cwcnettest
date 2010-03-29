using System;
using System.IO;
using System.Text;
using System.Collections;

namespace PDFGenerator {
    /// <summary>
    /// Application : Generation of PDF file from text
    /// Author : Pramod Kumar Singh
    /// Date : 25th July 2001
    ///</summary>

    public class PDFGenerator {
        static float pageWidth = 594.0f;
        static float pageDepth = 828.0f;
        static float pageMargin = 30.0f;
        static float fontSize = 10.0f;
        static float leadSize = 10.0f;

        //Create a PDF file.
        //PDF on Disk
        static StreamWriter pPDF = new StreamWriter(@"c:\myPDF.pdf");
        //PDF in Memory
        static MemoryStream mPDF = new MemoryStream();

        //Convert the Text Data to PDF format and write back to
        //Memory Stream
        static void ConvertToByteAndAddtoStream(string strMsg) {
            Byte[] buffer = null;
            buffer = ASCIIEncoding.ASCII.GetBytes(strMsg);
            mPDF.Write(buffer, 0, buffer.Length);
            buffer = null;
        }

        //Format the data length in xRef Format
        static string xRefFormatting(long xValue) {
            string strMsg = xValue.ToString();
            int iLen = strMsg.Length;
            if (iLen < 10) {
                StringBuilder s = new StringBuilder();
                //string s=null;
                int i = 10 - iLen;
                s.Append('0', i);
                strMsg = s.ToString() + strMsg;
            }
            return strMsg;
        }

        //Entry Point
        static void Main(string[] args) {
            //Create a ArrayList for xRefs of PDF Document
            ArrayList xRefs = new ArrayList();
            Byte[] buffer = null;
            float yPos = 0f;
            long streamStart = 0;
            long streamEnd = 0;
            long streamLen = 0;
            string strPDFMessage = null;
            //PDF Header Message
            strPDFMessage = "%PDF-1.1 ";
            ConvertToByteAndAddtoStream(strPDFMessage);

            //ID 1 For Containt
            //ID 2 For Length of the Stream
            //write the Text

            //1> Start a new Page
            xRefs.Add(mPDF.Length);
            strPDFMessage = "1 0 obj ";
            ConvertToByteAndAddtoStream(strPDFMessage);
            strPDFMessage = "<< /Length 2 0 R >> ";
            ConvertToByteAndAddtoStream(strPDFMessage);
            strPDFMessage = "stream ";
            ConvertToByteAndAddtoStream(strPDFMessage);

            //Get the start of the stream
            streamStart = mPDF.Length;
            strPDFMessage = "BT /F0 " + fontSize + " Tf ";
            ConvertToByteAndAddtoStream(strPDFMessage);
            yPos = pageDepth - pageMargin;
            strPDFMessage = pageMargin + " " + yPos + " Td ";
            ConvertToByteAndAddtoStream(strPDFMessage);
            strPDFMessage = leadSize + " TL ";
            ConvertToByteAndAddtoStream(strPDFMessage);

            //Add the text data to the PDF memory stream
            strPDFMessage = "(Pramod Kumar Singh)Tj ";
            ConvertToByteAndAddtoStream(strPDFMessage);
            strPDFMessage = "ET ";
            ConvertToByteAndAddtoStream(strPDFMessage);
            //Get the End of the stream
            streamEnd = mPDF.Length;
            //Get the Length of the stream
            streamLen = streamEnd - streamStart;
            strPDFMessage = "endstream endobj ";
            ConvertToByteAndAddtoStream(strPDFMessage);

            //Add 2 object to xRef
            xRefs.Add(mPDF.Length);
            strPDFMessage = "2 0 obj " + streamLen + " endobj ";
            ConvertToByteAndAddtoStream(strPDFMessage);

            //Add Page to xRefs
            xRefs.Add(mPDF.Length);
            strPDFMessage = "3 0 obj <</Type/Page/Parent 4 0 R/Contents 1 0 R>> endobj ";
            ConvertToByteAndAddtoStream(strPDFMessage);

            //Build the Pages
            xRefs.Add(mPDF.Length);
            strPDFMessage = "4 0 obj <</Type /Pages /Count 1 ";
            ConvertToByteAndAddtoStream(strPDFMessage);
            strPDFMessage = "/Kids[ 3 0 R ] ";
            ConvertToByteAndAddtoStream(strPDFMessage);
            strPDFMessage = "/Resources<</ProcSet[/PDF/Text]/Font<</F0 5 0 R>> >> ";
            ConvertToByteAndAddtoStream(strPDFMessage);
            strPDFMessage = "/MediaBox [ 0 0 " + pageWidth + " " + pageDepth + " ] >> endobj ";
            ConvertToByteAndAddtoStream(strPDFMessage);

            //Add font to xRefs
            xRefs.Add(mPDF.Length);
            strPDFMessage = "5 0 obj <</Type/Font/Subtype/Type1/BaseFont/Courier/Encoding/WinAnsiEncoding>> endobj ";
            ConvertToByteAndAddtoStream(strPDFMessage);

            //Add the catalog to xRefs
            xRefs.Add(mPDF.Length);
            strPDFMessage = "6 0 obj <</Type/Catalog/Pages 4 0 R>> endobj ";
            ConvertToByteAndAddtoStream(strPDFMessage);

            //xRefs Entry
            streamStart = mPDF.Length;
            strPDFMessage = "xref 0 7 0000000000 65535 f ";
            for (int i = 0; i < xRefs.Count; i++) {
                strPDFMessage += xRefFormatting((long)xRefs[i]) + " 00000 n ";
            }
            ConvertToByteAndAddtoStream(strPDFMessage);
            //Trailer for the PDF
            strPDFMessage = "trailer << /Size " + (xRefs.Count + 1) + " /Root 6 0 R >> ";
            ConvertToByteAndAddtoStream(strPDFMessage);
            //xRef location entry
            strPDFMessage = "startxref " + streamStart + " %%EOF ";
            ConvertToByteAndAddtoStream(strPDFMessage);
            //Write the PDF from Memory Stream to File Stream
            mPDF.WriteTo(pPDF.BaseStream);
            //Close the Stream
            mPDF.Close();
            pPDF.Close();
        }
    }
}